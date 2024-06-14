using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ETicaretAPI.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Infrastructure.Services.Storage.Azure;

public class AzureStorage : Storage, IAzureStorage
{
    private readonly BlobServiceClient _blobServiceClient;
    private BlobContainerClient _blobContainerClient;

    public AzureStorage(IConfiguration configuration)
    {
        _blobServiceClient = new(configuration["Storage:Azure"]);
    }

    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string container,
        IFormFileCollection files)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
        await _blobContainerClient.CreateIfNotExistsAsync();
        await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
        List<(string fileName, string pathOrContainerName)> data = new();
        foreach (var file in files)
        {
            var fileNewName = await FileRenameAsync(container, file.Name, HasFile);

            var blobClient = _blobContainerClient.GetBlobClient(fileNewName);
            await blobClient.UploadAsync(file.OpenReadStream());
            data.Add((fileNewName, $"{container}/{fileNewName}"));
        }

        return data;
    }

    public async Task DeleteAsync(string container, string fileName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
        var blobClient = _blobContainerClient.GetBlobClient(fileName);
        await blobClient.DeleteAsync();
    }

    public List<string> GetFiles(string container)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
        return _blobContainerClient.GetBlobs().Select(x => x.Name).ToList();
    }

    public bool HasFile(string container, string fileName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
        return _blobContainerClient.GetBlobs().Any(x => x.Name == fileName);
    }
}