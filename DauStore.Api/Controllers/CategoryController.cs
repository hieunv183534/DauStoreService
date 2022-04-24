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

        IBaseService<Category> _categoryService;

        #endregion

        #region Constructor

        public CategoryController(IBaseService<Category> categoryService)
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

        [HttpPost("addCategory")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            return Ok();
        }

        [HttpPut("updateCategory")]
        public IActionResult UpdateCategory()
        {
            return Ok();
        }


        [HttpDelete("deleteCategory/{categoryId}")]
        public IActionResult DeleteCategory([FromRoute] Guid categoryId)
        {
            return Ok();
        }
    }
}
