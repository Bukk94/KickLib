using KickLib.Models.Errors;

namespace KickLib.Extensions;

internal static class FluentResultExtensions
{
    internal static KickLibHttpResponseError? GetResponseError<TType>(this Result<TType> result)
    {
        if (result.HasError<KickLibHttpResponseError>(out var errors))
        {
            return errors.First();
        }

        return null;
    }
}