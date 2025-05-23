
using API.Data;
using API.Models;


namespace API.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly BookDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            BookDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }


        public async Task<Image> UploadAsync(Image image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (image.File == null) throw new ArgumentNullException(nameof(image.File));
            if (string.IsNullOrWhiteSpace(image.FileName) || string.IsNullOrWhiteSpace(image.FileExtension))
                throw new ArgumentException("FileName and FileExtension cannot be empty.");
            if (httpContextAccessor.HttpContext == null)
                throw new InvalidOperationException("HTTP context is not available.");

            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://" +
                              $"{httpContextAccessor.HttpContext.Request.Host}" +
                              $"{httpContextAccessor.HttpContext.Request.PathBase}/Images/" +
                              $"{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }

    }
}