using ETicaretAPI.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ETicaretAPI.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }


    public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
    {
        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        List<(string fileName, string path)> datas = new();
        List<bool> results = new();
        foreach (var file in files)
        {
            var fileNewName = await FileRenameAsync(file.FileName);
            var result = await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
            datas.Add((fileNewName, $"{uploadPath}\\{fileNewName}"));
            results.Add(result);
        }

        if (results.TrueForAll(x => x.Equals(true)))
        {
            return datas;
        }
// TODO: Exception handling will be added. (If block is  not valid, it will be thrown exception.)
        return null;
    }

    public Task<string> FileRenameAsync(string fileName)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CopyFileAsync(string path, IFormFile file)
    {
        try
        {
            await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None,
                1024 * 1024, false);
            await fileStream.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            return true;
        }
        catch (Exception ex)
        {
            // TODO: Log mechanism will be added.
            throw ex;
        }
    }
}