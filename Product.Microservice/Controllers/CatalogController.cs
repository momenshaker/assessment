using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Microservice.Core.Domain.Result;
using Product.Microservice.Dto;
using Product.Microservice.Extensions;
using Product.Microservice.Interfaces.ServicesInterface;

namespace Product.Microservice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _CatalogService;
        public CatalogController(ICatalogService CatalogService)
        {
            _CatalogService = CatalogService;
        }

        /// <summary>
        /// Add Catalog Information
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CatalogDto catalog)
        {
            var result = await _CatalogService.AddCatalogAsync(catalog);
            result.StatusCode = Core.Enums.StatusCode.Success;
            return result.ConvertToActionResult(this);
        }

        /// <summary>
        /// Edit Catalog Information
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(CatalogDto catalog)
        {

            var result = await _CatalogService.EditCatalogAsync(catalog);
            result.StatusCode = Core.Enums.StatusCode.Success;
            return result.ConvertToActionResult(this);
        }
        /// <summary>
        /// Delete Catalog From Database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {

            var result = await _CatalogService.DeleteCatalogAsync(id);
            result.StatusCode = Core.Enums.StatusCode.Success;
            return result.ConvertToActionResult(this);
        }
        /// <summary>
        /// Get Catalog Information by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCatalog(int id)
        {

            var result = await _CatalogService.GetCatalogAsync(id);
            result.StatusCode = Core.Enums.StatusCode.Success;
            return result.ConvertToActionResult(this);
        }
        /// <summary>
        /// Get Catalogs List
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCatalogList(int pageSize, int pageNumber)
        {

            var result = await _CatalogService.GetCatalogsAsync(new PagingInfo(pageNumber, pageSize));
            result.StatusCode = Core.Enums.StatusCode.Success;
            return result.ConvertToActionResult(this);
        }

    }
}