using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using WebSocketSharper;

namespace KickLib.Api.Unofficial.Clients.CycleTls
{
    public class CycleTLSClient
    {
        private readonly ILogger _logger;
        private readonly object _lockRequestCount = new();
        private readonly object _lockQueue = new();
        private bool _isQueueSendRunning = false;

        private WebSocket WebSocketClient { get; set; } = null;
        private Process GoServer { get; set; } = null;
    
        private Queue<(CycleTlsRequest Request, TaskCompletionSource<CycleTlsResponse> RequestTCS)> RequestQueue
        {
            get;
            set;
        } = new ();

        private ConcurrentDictionary<string, TaskCompletionSource<CycleTlsResponse>> SentRequests { get; set; } = new();

        private int RequestCount { get; set; } = 0;

        public CycleTLSClient(ILogger logger)
        {
            _logger = logger ?? new LoggerFactory().CreateLogger(nameof(CycleTLSClient));
        }
    
        public TimeSpan DefaultTimeOut { get; set; } = TimeSpan.FromSeconds(100);

        public CycleTlsRequestOptions DefaultRequestOptions { get; } = new()
        {
            Ja3 = "771,4865-4867-4866-49195-49199-52393-52392-49196-49200-49162-49161-49171-49172-51-57-47-53-10,0-23-65281-10-11-35-16-5-51-43-13-45-28-21,29-23-24-25-256-257,0",
            UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.54 Safari/537.36",
            Body = "",
            Cookies = new List<Cookie>(),
            DisableRedirect = null,
            HeaderOrder = new List<string>(),
            Headers = new Dictionary<string, string>(),
            Method = "",
            OrderAsProvided = null,
            Proxy = "",
            Timeout = null,
            Url = ""
        };
    
        /// <summary>
        /// Creates and runs server with source CycleTLS library and WebSocket client.
        /// </summary>
        /// <param name="port">Port used by server.</param>
        /// <exception cref="InvalidOperationException">Server already initialized.</exception>
        /// <exception cref="PlatformNotSupportedException">Not supported platform.</exception>
        public void InitializeServerAndClient(int port = 9119)
        {
            if (GoServer != null || DoesServerAlreadyRun(port))
            {
                throw new InvalidOperationException("Server already initialized.");
            }

            string executableFilename = "";
            try
            {
                executableFilename = GetExecutableFilename();
                Console.WriteLine("Executable filename: " + executableFilename);
                Console.WriteLine("Exists? " + File.Exists(executableFilename));
            }
            catch (PlatformNotSupportedException)
            {
                _logger.LogError("Operating system not supported.");
                throw;
            }

            StartServer(executableFilename, port);

            StartClient(port);
        }

        private bool DoesServerAlreadyRun(int port)
        {
            IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
            try
            {
                TcpListener tcpListener = new TcpListener(ipAddress, port);
                tcpListener.Start();
                tcpListener.Stop();
            }
            catch
            {
                return true;
            }

            return false;
        }

        private static string GetExecutableFilename()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "index.exe";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (RuntimeInformation.OSArchitecture == Architecture.Arm)
                {
                    return "index-arm";
                }

                if (RuntimeInformation.OSArchitecture == Architecture.Arm64)
                {
                    return "index-arm64";
                }

                return "./index";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "index-mac";
            }

            throw new PlatformNotSupportedException("Not supported platform.");
        }

        private void StartServer(string filename, int port)
        {
            var pi = new ProcessStartInfo(filename);
            pi.EnvironmentVariables.Add("WS_PORT", port.ToString());
            //pi.UseShellExecute = true;
            pi.WindowStyle = ProcessWindowStyle.Hidden;

            GoServer = new Process();
            GoServer.StartInfo = pi;
            GoServer.ErrorDataReceived += (_, ea) =>
            {
                if (ea.Data.Contains("Request_Id_On_The_Left"))
                {
                    var splitRequestIdAndError =
                        ea.Data.Split(new string[] { "Request_Id_On_The_Left" }, StringSplitOptions.None);
                    var requestId = splitRequestIdAndError[0];
                    var error = splitRequestIdAndError[1];
                    //_logger.LogError($"Error from CycleTLSClient: requestId:{requestId} error:{error}");
                }
                else
                {
                    _logger.LogError($"Server received error data: {ea.Data}");
                
                    try
                    {
                        GoServer.Kill();
                    }
                    finally
                    {
                        GoServer.Dispose();
                    }

                    StartServer(filename, port);
                }
            };
            GoServer.Start();
        }

        private void StartClient(int port)
        {
            var ws = new WebSocket(_logger, "ws://localhost:" + port, false);

            ws.OnMessage += (_, ea) =>
            {
                CycleTlsResponse response = JsonSerializer.Deserialize<CycleTlsResponse>(ea.Data);
                if (SentRequests.TryRemove(response.RequestID, out var requestTCS))
                {
                    requestTCS.TrySetResult(response);
                }
            };

            ws.OnError += (_, ea) =>
            {
                ws.Close();

                foreach (var requestPair in SentRequests)
                {
                    requestPair.Value.TrySetException(new Exception("Error in WebSocket connection.", ea.Exception));
                }

                SentRequests.Clear();

                Task.Delay(100).ContinueWith((t) => StartClient(port));
            };

            ws.Connect();

            WebSocketClient = ws;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">A string that represents the request Url.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<CycleTlsResponse> SendAsync(HttpMethod httpMethod, string url)
        {
            return await SendAsync(httpMethod, url, DefaultTimeOut);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<CycleTlsResponse> SendAsync(HttpMethod httpMethod, string url, TimeSpan timeout)
        {
            return await SendAsync(new CycleTlsRequestOptions
            {
                Url = url,
                Method = httpMethod.Method
            }, timeout);
        }

        public async Task<CycleTlsResponse> SendAsync(CycleTlsRequestOptions CycleTlsRequestOptions)
        {
            return await SendAsync(CycleTlsRequestOptions, DefaultTimeOut);
        }

        public Task<CycleTlsResponse> SendAsync(CycleTlsRequestOptions CycleTlsRequestOptions, TimeSpan timeout)
        {
            if (WebSocketClient == null)
            {
                throw new InvalidOperationException("WebSocket client is not initialized.");
            }

            TaskCompletionSource<CycleTlsResponse> tcs = new TaskCompletionSource<CycleTlsResponse>();
            var cancelSource = new CancellationTokenSource(timeout);
            cancelSource.Token.Register(() =>
                tcs.TrySetException(new TimeoutException($"No response after {timeout.TotalSeconds} seconds.")));

            var request = CreateRequest(CycleTlsRequestOptions);

            lock (_lockQueue)
            {
                RequestQueue.Enqueue((request, tcs));
                if (!_isQueueSendRunning)
                {
                    _isQueueSendRunning = true;
                    Task.Run(() => QueueSendAsync());
                }
            }

            return tcs.Task;
        }

        private CycleTlsRequest CreateRequest(CycleTlsRequestOptions CycleTlsRequestOptions)
        {
            // There's no records in netstandard2.0, so here's copy of options
            var optionsCopy = new CycleTlsRequestOptions();
            foreach (var propertyInfo in typeof(CycleTlsRequestOptions).GetProperties())
            {
                object defaultOption = propertyInfo.GetValue(DefaultRequestOptions);
                object customOption = propertyInfo.GetValue(CycleTlsRequestOptions);
                if (customOption == null)
                    propertyInfo.SetValue(optionsCopy, defaultOption);
                else
                    propertyInfo.SetValue(optionsCopy, customOption);
            }

            foreach (var cookie in optionsCopy.Cookies.Where(c => c.Expires == default))
            {
                cookie.Expires = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
            }

            int requestIndex;
            lock (_lockRequestCount)
                requestIndex = ++RequestCount;

            var request = new CycleTlsRequest()
            {
                RequestId = $"{requestIndex}:{DateTime.Now}:{optionsCopy.Url}",
                Options = optionsCopy
            };

            return request;
        }

        private async Task QueueSendAsync()
        {
            while (true)
            {
                if (WebSocketClient == null)
                {
                    throw new InvalidOperationException(
                        "Critical error. For some reason WebSocket client is not initialized. " +
                        "Probably, you should not see this exception");
                }

                if (!(await ClientRestartCheckDelay())) return;

                CycleTlsRequest request;
                TaskCompletionSource<CycleTlsResponse> requestTcs;
                lock (_lockQueue)
                {
                    if (!RequestQueue.Any())
                    {
                        _isQueueSendRunning = false;
                        return;
                    }

                    (request, requestTcs) = RequestQueue.Dequeue();
                }

                SentRequests.TryAdd(request.RequestId, requestTcs);

                var jsonRequestData = JsonSerializer.Serialize(request);

                WebSocketClient.SendAsync(jsonRequestData, (isCompleted) =>
                {
                    if (!isCompleted)
                    {
                        requestTcs.TrySetException(new Exception("Error in WebSocket connection."));
                        SentRequests.TryRemove(request.RequestId, out _);
                    }
                });
            }
        }

        // Returns true if restart was successful and WebSocketClient is alive now, false otherwise
        private async Task<bool> ClientRestartCheckDelay()
        {
            // Wait max 5000 milliseconds while server or client restarts
            int attempts = 0;
            int maxAttempts = 50;
            int delay = 100;
            while (!WebSocketClient.IsAlive && attempts < 50)
            {
                await Task.Delay(delay);
                attempts++;
            }

            if (!WebSocketClient.IsAlive)
            {
                lock (_lockQueue)
                {
                    while (RequestQueue.Any())
                    {
                        RequestQueue.Dequeue().RequestTCS
                            .TrySetException(new Exception($"Critical error. " +
                                                           $"WebSocket connection was not established after {maxAttempts * delay} milliseconds."));
                    }

                    _isQueueSendRunning = false;
                    return false;
                }
            }

            return true;
        }
    }
}

