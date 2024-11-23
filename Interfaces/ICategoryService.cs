using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_api.controllers;
using web_api.DTOs;
using web_api.Helpers;

namespace web_api.Interfaces
{
    public interface ICategoryService
    { 
        Task<PaginatedResults<CategoryReadDto>> GetAllCategories(QueryParameters queryParameters);
        Task<CategoryReadDto?> GetCategoryById(Guid categoryId);
        Task<CategoryReadDto> CreateCategory(CategoryCreateDTO categoryData);
        Task<CategoryReadDto?> UpdateCategory(Guid categoryId, CategoryUpdateDTO categoryData);
        Task<bool> DeleteCategory(Guid categoryId);
    }
}