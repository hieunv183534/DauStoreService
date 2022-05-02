using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DauStore.Api.Controllers
{

    [Authorize(Roles = "admin, customer")]
    [Route("api")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        #region Declare

        ICategoryService _categoryService;

        #endregion

        #region Constructor

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #endregion

        [AllowAnonymous]
        [HttpGet("getCategorys")]
        public IActionResult GetCategorys()
        {
            var serviceResult = _categoryService.GetAll();
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("addCategory")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            category.ParentCode = (category.ParentCode == "xxx") ? "" : category.ParentCode;
            var _serviceResult = _categoryService.Add(category);
            return StatusCode(_serviceResult.StatusCode, _serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("updateCategory/{categoryId}/{newName}")]
        public IActionResult UpdateCategory([FromRoute] Guid categoryId, [FromRoute] string newName)
        {
            Category category = new Category();
            category.CategoryName = newName;
            var serviceResult = _categoryService.Update(category,categoryId);
            return StatusCode(serviceResult.StatusCode,serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("deleteCategory/{categoryId}")]
        public IActionResult DeleteCategory([FromRoute] Guid categoryId)
        {
            var serviceResult = _categoryService.Delete(categoryId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }
    }
}
