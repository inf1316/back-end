using QvaCar.Application.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QvaCar.Infraestructure.BlogStorage.Services
{
    internal class FileSystemService : IFileSystemService
    {
        public IList<string> ResolveFileNameConflicts(string[] keepFileNames, string[] fileNameToBeRenamed)
        {
            List<string> newFileNames = new(fileNameToBeRenamed.Length);
            List<string> allFileNames = new(keepFileNames);

            foreach (var fileName in fileNameToBeRenamed)
            {
                string addFileName = fileName;
                if (allFileNames.Any(x => x == addFileName))
                {
                    int index = 1;
                    while (true)
                    {
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                        string extensions = Path.GetExtension(fileName);

                        string newFileName = $"{fileNameWithoutExtension}{index}{extensions}";
                        if (!allFileNames.Any(x => x == newFileName))
                        {
                            addFileName = newFileName;
                            break;
                        }
                        index++;
                    }
                }
                newFileNames.Add(addFileName);
                allFileNames.Add(addFileName);
            }
            return newFileNames;
        }
    }
}
