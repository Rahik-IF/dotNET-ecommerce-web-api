using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using web_api.controllers;
using web_api.data;
using web_api.DTOs;
using web_api.Enums;
using web_api.Helpers;
using web_api.Interfaces;
using web_api.Models;

namespace web_api.Services
{
    public class CategoryService : ICategoryService
    {
        //static (not intance), as need to be same/constant for all instance/context
        //private static readonly List<Category> _categories = new List<Category>();
        private readonly AppDbContext _appDbContext;
        // use automapper interface for mapping
        private readonly IMapper _mapper;

        // dependency injection(constructor injection) of Imapper Interface
        public CategoryService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<PaginatedResults<CategoryReadDto>> GetAllCategories(QueryParameters queryParameters)
        {
            // decompose queryParameters
            var pageSize = queryParameters.PageSize;
            var pageNumber = queryParameters.PageNumber;
            var search = queryParameters.Search;
            var sortOrder = queryParameters.SortOrder;
            // get query variable
            IQueryable<Category> query = _appDbContext.Categories;


            // search by name or description
            if (!string.IsNullOrWhiteSpace(search))
            {
                var formattedSearch = $"%{search.Trim()}%";
                query = query.Where(
                    c => EF.Functions.ILike(c.Name, formattedSearch)
                      || (c.Description != null && EF.Functions.ILike(c.Description, formattedSearch))
                );
            }

            //sorting operation
            if (string.IsNullOrWhiteSpace(sortOrder))
            {
                query = query.OrderBy(c => c.Name);
            }
            else
            {
                var formattedSortOder = sortOrder.Trim().ToLower();
                if (Enum.TryParse<SortOrderEnum>(formattedSortOder, true, out var parsedSortOrder))
                {
                    query = parsedSortOrder switch
                    {
                        SortOrderEnum.NameAsc => query.OrderBy(c => c.Name),
                        SortOrderEnum.NameDesc => query = query.OrderByDescending(c => c.Name),
                        SortOrderEnum.CreatedAtAsc => query.OrderBy(c => c.CreatedAt),
                        SortOrderEnum.CreatedAtDesc => query.OrderByDescending(c => c.CreatedAt),
                        _ => query.OrderBy(c => c.Name)
                    };
                }
            }

            // Get total count
            var totalCount = await query.CountAsync();
            // logic => Skip((pageNumber-1)*pageSize).Take(pageSize)
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var results = _mapper.Map<List<CategoryReadDto>>(items);

            return new PaginatedResults<CategoryReadDto>
            {
                Items = results,
                TotalCount = totalCount,
                PageSize = pageSize,
            };
        }

        public async Task<CategoryReadDto?> GetCategoryById(Guid categoryId)
        {
            var foundCategory = await _appDbContext.Categories.FindAsync(categoryId);
            return foundCategory == null ? null :
             _mapper.Map<CategoryReadDto>(foundCategory);
        }

        public async Task<CategoryReadDto> CreateCategory(CategoryCreateDTO categoryData)
        {
            var newCategory = _mapper.Map<Category>(categoryData);
            
            await _appDbContext.Categories!.AddAsync(newCategory);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<CategoryReadDto>(newCategory);
        }

        public async Task<CategoryReadDto?> UpdateCategory(Guid categoryId, CategoryUpdateDTO categoryData)
        {
            var foundCategory = await _appDbContext.Categories.FindAsync(categoryId);
            if (foundCategory == null)
            {
                return null;
            }
            _mapper.Map(categoryData, foundCategory);
            _appDbContext.Categories.Update(foundCategory);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<CategoryReadDto>(foundCategory);
        }

        public async Task<bool> DeleteCategory(Guid categoryId)
        {
            var foundCategory = await _appDbContext.Categories.FindAsync(categoryId);
            if (foundCategory == null)
            {
                return false;
            }

            _appDbContext.Categories.Remove(foundCategory);
            await _appDbContext.SaveChangesAsync();

            return true;
        }
    }
}