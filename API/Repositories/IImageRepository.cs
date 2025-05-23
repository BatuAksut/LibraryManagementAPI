
using API.Models;

namespace API.Repositories
{
    public interface IImageRepository
    {
        Task<Image> UploadAsync(Image image);

    }
}
