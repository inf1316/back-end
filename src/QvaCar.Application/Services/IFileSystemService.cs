using System.Collections.Generic;

namespace QvaCar.Application.Services
{
    public interface IFileSystemService
    {
        IList<string> ResolveFileNameConflicts(string[] keepFileNames, string[] fileNameToBeRenamed);
    }
}