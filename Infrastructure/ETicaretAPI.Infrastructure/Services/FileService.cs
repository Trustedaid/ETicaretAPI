using ETicaretAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Http;

namespace ETicaretAPI.Infrastructure.Services;

public class FileService
{
    


   


    async Task<string> FileRenameAsync(string path, string fileName, bool first = true)
    {
        string newFileName = await Task.Run<string>(async () =>
        {
            string extension = Path.GetExtension(fileName);
            string newFileName = string.Empty;
            if (first)
            {
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";
            }
            else
            {
                newFileName = fileName;
                int indexNo1 = newFileName.IndexOf("-");
                if (indexNo1 == -1)
                    newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                else
                {
                    int lastIndex = 0;
                    while (true)
                    {
                        lastIndex = indexNo1;
                        indexNo1 = newFileName.IndexOf("-", indexNo1 + 1);
                        if (indexNo1 == -1)
                        {
                            indexNo1 = lastIndex;
                            break;
                        }
                    }

                    int indexNo2 = newFileName.IndexOf(".");
                    string fileNo = newFileName.Substring(indexNo1 + 1, indexNo2 - indexNo1 - 1);
                    if (int.TryParse(fileNo, out int _fileNo))
                    {
                        _fileNo++;
                        newFileName = newFileName.Remove(indexNo1 + 1, indexNo2 - indexNo1 - 1)
                            .Insert(indexNo1 + 1, _fileNo.ToString());
                    }
                    else
                        newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                }
            }

            if (File.Exists($"{path}\\{newFileName}"))
                return await FileRenameAsync(path, newFileName, false);
            else
                return newFileName;
        });

        return newFileName;
    }


    public async Task<bool> CopyFileAsync(string path, IFormFile file)
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
    
    
  async Task<string> FileRenameAsync2(string path, string fileName, bool isUnique = true)
    {
        string extension = Path.GetExtension(fileName);
        string newFileName = fileName;

        if (isUnique)
        {
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";
        }
        else
        {
            int indexNo = newFileName.LastIndexOf("-");
            if (indexNo == -1)
            {
                newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
            }
            else
            {
                int dotIndex = newFileName.LastIndexOf(".");
                string fileNo = newFileName.Substring(indexNo + 1, dotIndex - indexNo - 1);
                if (int.TryParse(fileNo, out int fileNumber))
                {
                    fileNumber++;
                    newFileName = $"{newFileName.Substring(0, indexNo + 1)}{fileNumber}{extension}";
                }
                else
                {
                    newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                }
            }
        }

        if (File.Exists(Path.Combine(path, newFileName)))
        {
            return await FileRenameAsync(path, newFileName, false);
        }

        return newFileName;
    }
 
    public async Task<string> FileRenameAsync3(string path, string fileName, bool isUnique = true)
    {
        string extension = Path.GetExtension(fileName);
        string newFileName = fileName;

        if (isUnique)
        {
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";
        }
        else
        {
            int indexNo = newFileName.LastIndexOf("-");
            int dotIndex = newFileName.LastIndexOf(".");
        
            if (indexNo == -1 || dotIndex <= indexNo)
            {
                newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
            }
            else
            {
                string fileNo = newFileName.Substring(indexNo + 1, dotIndex - indexNo - 1);
                if (int.TryParse(fileNo, out int fileNumber))
                {
                    fileNumber++;
                    newFileName = $"{newFileName.Substring(0, indexNo + 1)}{fileNumber}{extension}";
                }
                else
                {
                    newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                }
            }
        }

        string newPath = Path.Combine(path, newFileName);
        if (File.Exists(newPath))
        {
            return await FileRenameAsync(path, newFileName, false);
        }

        return newFileName;
    }
    
    
}





    