using API.Models;
using API.Models.Dtos;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository repository;
        private readonly IMapper mapper;

        public BooksController(IBookRepository repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var books= await repository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            var booksDto=mapper.Map<List<BookDto>>(books);
            return Ok(booksDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await repository.GetByIdAsync(id);
            if (book == null) return NotFound();
            var bookDto = mapper.Map<BookDto>(book);
            return Ok(bookDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] AddBookDto addBookDto)
        {
            if (addBookDto == null) return BadRequest("Book is null");
            var book = mapper.Map<Book>(addBookDto);
            var createdBook = await repository.CreateAsync(book);
            var createdBookDto = mapper.Map<BookDto>(createdBook);
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBookDto);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto updateBookDto)
        {
            if (updateBookDto == null) return BadRequest("Book is null");
            var book = mapper.Map<Book>(updateBookDto);

            var updatedBook = await repository.UpdateAsync(id, book);
            if (updatedBook == null) return NotFound();

            var updatedBookDto = mapper.Map<BookDto>(updatedBook);
            return Ok(updatedBookDto);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await repository.DeleteAsync(id);
            if (book == null) return NotFound();
            var bookDto = mapper.Map<BookDto>(book);
            return Ok(bookDto);
        }
    }
}
