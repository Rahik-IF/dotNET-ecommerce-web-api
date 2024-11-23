using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web_api.DTOs;
using web_api.Helpers;
using web_api.Interfaces;
using web_api.Models;
using web_api.Services;

namespace web_api.controllers
{
    [ApiController] // set this as a controller
    [Route("api/categories/")] // set default route
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        // depenedancy injection by passing parameter
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService; // pass by reference
        }
        // GET: api/categories?pageNumber=2&&pageSize=10 => read categpries
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] QueryParameters queryParameters)
        { 
            queryParameters.Validate();
            var categoryList = 
                    await _categoryService
                    .GetAllCategories(queryParameters);
            return Ok(ApiResponse<PaginatedResults<CategoryReadDto>>.SuccessResponse(categoryList, 200, "Successfully data returned")); // If no searchValue, return all categories.
        }
        // GET: api/categories => get category by id
        [HttpGet("{categoryId:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid categoryId)
        {
            var category = await _categoryService.GetCategoryById(categoryId);
            if (category == null)
            {
                return NotFound(
                    ApiResponse<object>.ErrorResponse(
                        new List<string> { "Category  with this id not exist" },
                        404,
                        "validation failed"
                    )
                );
            }

            return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(category, 200, "Successfully category returned")); // If no searchValue, return all categories.
        }
        // POST: api/categories => create a categpry
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDTO categoryData)
        {


            var categoryReaddto = await _categoryService.CreateCategory(categoryData);
            return Created(
                $"/api/categories/{nameof(GetCategoryById)}",
                ApiResponse<CategoryReadDto>.SuccessResponse(categoryReaddto, 201, "Successfully category created")
                );
        }

        // update: api/categories/{categoryId} => update a categpry
        [HttpPut("{categoryId:guid}")]
        public async Task<IActionResult> UpdateCategoryById(Guid categoryId, [FromBody] CategoryUpdateDTO categoryData)
        {
            var foundCategory =await _categoryService.UpdateCategory(categoryId, categoryData);
            if (foundCategory == null)
            {
                return NotFound(
                    ApiResponse<object>.ErrorResponse(
                        new List<string> { "Category with this id not exist" },
                        404,
                        "validation failed"
                    )
                );
            }
            return Ok(
                ApiResponse<object>.SuccessResponse(null, 200, "Successfully Category updated")
            ); // 204 No Content

        }

        // Delete: api/categories/{categoryId} => delete a categpry
        [HttpDelete("{categoryId:guid}")]
        public async Task<IActionResult> DeleteCategoryById(Guid categoryId)
        {
            var foundCategory = await _categoryService.DeleteCategory(categoryId);
            if (!foundCategory)
            {
                return NotFound(
                    ApiResponse<object>.ErrorResponse(
                        new List<string> { "Category data with this id not found" },
                        404,
                        "validation failed"
                    )
                );
            }
            return Ok(
                ApiResponse<object>.SuccessResponse(null, 200, "Successfully Category delted")
            ); // 204 No Content

        }


    }
}