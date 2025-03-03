using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Channels
{
    public class MediaResponse
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "model_type")]
        public string ModelType { get; set; }
    
        [JsonProperty(PropertyName = "model_id")]
        public int ModelId { get; set; }
    
        [JsonProperty(PropertyName = "collection_name")]
        public string CollectionName { get; set; }
    
        public string Name { get; set; }
    
        [JsonProperty(PropertyName = "file_name")]
        public string FileName { get; set; }
    
        [JsonProperty(PropertyName = "mime_type")]
        public string MimeType { get; set; }
    
        public string Disk { get; set; }
    
        public string Size { get; set; }
    
        // TODO: manipulations
        // TODO: custom_properties
        // TODO: responsive_images
    
        [JsonProperty(PropertyName = "order_column")]
        public int OrderColumn { get; set; }
    
        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }
    
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime? UpdatedAt { get; set; }
    
        public Guid Uuid { get; set; }
    
        [JsonProperty(PropertyName = "conversions_disk")]
        public string ConversionsDisk { get; set; }
    }
}