﻿using ETicaretAPI.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ETicaretAPI.Infrastructure.Services.Storage.Local;

public class LocalStorage : Storage, ILocalStorage
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public LocalStorage(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path,
        IFormFileCollection files)
    {
        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        List<(string fileName, string path)> data = new();
        foreach (var file in files)
        {
            var fileNewName = await FileRenameAsync(uploadPath, file.Name, HasFile);
            
            await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
            data.Add((fileNewName, $"{path}\\{fileNewName}"));
        }

        return data;
    }

    public async Task DeleteAsync(string path, string fileName) => File.Delete($"{path}\\{fileName}");


    public List<string> GetFiles(string path)
    {
        DirectoryInfo directory = new(path);
        return directory.GetFiles().Select(x => x.Name).ToList();
    }

    public bool HasFile(string path, string fileName) => File.Exists($"{path}\\{fileName}");

    private async Task<bool> CopyFileAsync(string path, IFormFile file)
    {
        try
        {
            await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None,
                1024 * 1024, false);
            await file.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            return true;
        }
        catch (Exception)
        {
            // TODO: Log mechanism will be added.
            throw;
        }
    }
}