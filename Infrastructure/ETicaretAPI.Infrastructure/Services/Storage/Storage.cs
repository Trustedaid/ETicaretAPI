using ETicaretAPI.Infrastructure.Operations;

namespace ETicaretAPI.Infrastructure.Services.Storage;

public class Storage
{
    protected delegate bool HasFile(string pathOrContainerName, string fileName);

    protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod, bool first = true)
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

            // if (File.Exists($"{path}\\{newFileName}"))
            if(hasFileMethod(pathOrContainerName, newFileName))
                return await FileRenameAsync(pathOrContainerName, newFileName, hasFileMethod, false);
            else
                return newFileName;
        });

        return newFileName;
    }
}


/* OTHER OPTIMIZED VERSIONS
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
            return await FileRenameAsync2(path, newFileName, false);
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
            return await FileRenameAsync3(path, newFileName, false);
        }

        return newFileName;
    }
    
    
    
      protected async Task<string> FileRenameAsync4(string pathOrContainerName, string fileName, HasFile hasFileMethod, bool first = true)
    {
        string extension = Path.GetExtension(fileName);
        string newFileName = first ? GetFirstFileName(fileName, extension) : fileName;
        
        while (await Task.Run(() => hasFileMethod(pathOrContainerName, newFileName)))
        {
            newFileName = GetNextFileName(newFileName, extension);
        }

        return newFileName;
    }

    private string GetFirstFileName(string fileName, string extension)
    {
        string oldName = Path.GetFileNameWithoutExtension(fileName);
        return $"{NameOperation.CharacterRegulatory(oldName)}{extension}";
    }

    private string GetNextFileName(string fileName, string extension)
    {
        int indexNo = fileName.LastIndexOf("-");
        if (indexNo == -1)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}-2{extension}";
        }

        int indexNo2 = fileName.IndexOf(".", indexNo);
        if (indexNo2 == -1)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}-2{extension}";
        }

        string fileNoStr = fileName.Substring(indexNo + 1, indexNo2 - indexNo - 1);
        if (int.TryParse(fileNoStr, out int fileNo))
        {
            fileNo++;
            return $"{fileName.Substring(0, indexNo + 1)}{fileNo}{extension}";
        }
        
        return $"{Path.GetFileNameWithoutExtension(fileName)}-2{extension}";
    }
*/