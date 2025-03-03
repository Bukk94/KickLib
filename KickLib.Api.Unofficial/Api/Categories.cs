using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models.Response.v1.Categories;
using KickLib.Api.Unofficial.Models.Response.v2.Categories;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial.Api
{
    /// <summary>
    ///     Get categories data.
    /// </summary>
    public class Categories : BaseApi
    {
        private const string ApiUrlPart = "categories/";

        public Categories(IApiCaller client, ILogger logger = null)
            : base(client, logger)
        {
        }
    
        /// <summary>
        ///     Gets main (root) categories of the Kick platform.
        /// </summary>
        public Task<ICollection<CategoryResponse>> GetCategoriesAsync()
        {
            // v1/categories
            return GetAsync<ICollection<CategoryResponse>>(ApiUrlPart, ApiVersion.V1);
        }
    
        /// <summary>
        ///     Gets specific root category.
        ///     Will match only categories provided by <see cref="GetCategoriesAsync"/>.
        /// </summary>
        /// <param name="categorySlug">Category slug.</param>
        public Task<CategoryResponse> GetCategoryAsync(string categorySlug)
        {
            if (string.IsNullOrWhiteSpace(categorySlug))
            {
                throw new ArgumentNullException(nameof(categorySlug));
            }

            // v1/categories/{categorySlug}
            var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(categorySlug)}";
            return GetAsync<CategoryResponse>(urlPart, ApiVersion.V1);
        }
    
        /// <summary>
        ///     Gets TOP 13 categories (represented as 'sub-categories').
        /// </summary>
        public Task<ICollection<SubCategoryResponse>> GetTopCategoriesAsync()
        {
            // v1/categories/top
            var urlPart = $"{ApiUrlPart}top";
            return GetAsync<ICollection<SubCategoryResponse>>(urlPart, ApiVersion.V1);
        }
    
        /// <summary>
        ///     Get all sub-categories with detailed information (paged).
        /// </summary>
        /// <param name="perPage">Number of sub-categories to return per page. 50 is maximum!</param>
        /// <param name="page">Allows to specify page number to navigate through the pages.</param>
        /// <returns>Returns paged object of sub-categories</returns>
        public Task<SubCategoryPagedResponse> GetSubCategoriesAsync(int perPage = 10, int? page = null)
        {
            if (perPage < 1)
            {
                throw new ArgumentException("Per Page must be positive number!");
            }
        
            if (perPage > 50)
            {
                throw new ArgumentException("You can list maximum of 50 sub-categories per page!");
            }
        
            // v1/subcategories?limit=15?page=2
            var urlPart = $"sub{ApiUrlPart}?limit={perPage}";
            if (page.HasValue)
            {
                if (page.Value < 0)
                {
                    throw new ArgumentException("Page number must be positive number!");
                }
            
                urlPart += $"&page={page}";
            }
        
            return GetAsync<SubCategoryPagedResponse>(urlPart, ApiVersion.V1);
        }
    
        /// <summary>
        ///     Get specific sub-category.
        /// </summary>
        /// <param name="subcategorySlug">Subcategory slug</param>
        public Task<SubCategoryResponse> GetSubCategoryAsync(string subcategorySlug)
        {
            if (string.IsNullOrWhiteSpace(subcategorySlug))
            {
                throw new ArgumentNullException(nameof(subcategorySlug));
            }
        
            // v1/subcategories/{slug}
            var urlPart = $"sub{ApiUrlPart}{Uri.EscapeDataString(subcategorySlug)}";
            return GetAsync<SubCategoryResponse>(urlPart, ApiVersion.V1);
        }
    
        /// <summary>
        ///     Gets all subcategories.
        ///     This endpoint returns all subcategories at once, without paging, but with limited information.
        /// </summary>
        public Task<ICollection<SimpleSubCategoryResponse>> GetAllSubCategoriesAsync()
        {
            // v1/listsubcategories
            var urlPart = $"listsub{ApiUrlPart}";
            return GetAsync<ICollection<SimpleSubCategoryResponse>>(urlPart, ApiVersion.V1);
        }
    
        /// <summary>
        ///     Get sub-category clips (paged).
        ///     By default, first 20 entries are returned. To page to more result, use <param name="nextCursor">cursor</param> value.
        /// </summary>
        /// <param name="subcategorySlug">Subcategory slug.</param>
        /// <param name="nextCursor">Next cursor value.</param>
        public Task<CategoryClipsResponse> GetSubCategoryClipsAsync(string subcategorySlug, string nextCursor = null)
        {
            if (string.IsNullOrWhiteSpace(subcategorySlug))
            {
                throw new ArgumentNullException(nameof(subcategorySlug));
            }
        
            var query = new List<KeyValuePair<string, string>>();

            if (nextCursor is not null)
            {
                // Add cursor (if any)
                query.Add(new("cursor", nextCursor));
            }
        
            // v2/categories/slots/clips
            var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(subcategorySlug)}/clips";
            return GetAsync<CategoryClipsResponse>(urlPart, ApiVersion.V2, query);
        }
    }
}