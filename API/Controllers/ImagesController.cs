
using API.Models;
using API.Models.Dtos;
using API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository repository;

        public ImagesController(IImageRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadDto uploadDto)
        {

            ValidateFileUpload(uploadDto);
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);

            }

            var image = new Image
            {
                File = uploadDto.File,
                FileName = uploadDto.FileName,
                FileDescription = uploadDto.FileDescription,
                FileExtension = Path.GetExtension(uploadDto.File.FileName),
                FileSizeInBytes = uploadDto.File.Length,
            };



            await repository.UploadAsync(image);
            return Ok(image);
        }

        private void ValidateFileUpload(ImageUploadDto uploadDto)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(uploadDto.File.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("File", "Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed.");
            }
            if (uploadDto.File.Length > 10485760) // 10 MB
            {
                ModelState.AddModelError("File", "File size exceeds the limit of 10 MB.");
            }
        }
    }
}
