using API.CustomActionFilters;
using API.Models;
using API.Models.Dtos;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository repository;
        private readonly IMapper mapper;

        public CategoriesController(ICategoryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetCategories([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var categories = await repository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            var categoriesDto = mapper.Map<List<CategoryDto>>(categories);
            return Ok(categoriesDto);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await repository.GetByIdAsync(id);
            if (category == null) return NotFound();
            var categoryDto = mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryDto addCategoryDto)
        {
            if (addCategoryDto == null) return BadRequest("Category is null");
            var category = mapper.Map<Category>(addCategoryDto);
            var createdCategory = await repository.CreateAsync(category);
            var createdCategoryDto = mapper.Map<CategoryDto>(createdCategory);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategoryDto);
        }
        [HttpPut("{id}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (updateCategoryDto == null) return BadRequest("Category is null");
            var category = mapper.Map<Category>(updateCategoryDto);

            var updatedCategory = await repository.UpdateAsync(id, category);
            if (updatedCategory == null) return NotFound();

            var updatedCategoryDto = mapper.Map<CategoryDto>(updatedCategory);
            return Ok(updatedCategoryDto);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await repository.DeleteAsync(id);
            if (category == null) return NotFound();
            var categoryDto = mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }
    }
}
