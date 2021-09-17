using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore31.Helper;
using WebAppCore31.Models;

namespace WebAppCore31.Repository
{
    public interface IFileManager
    {
        Task<Guid> InsFile(FileModel model);
        Task<FileModel> GetFile(Guid ContentGuid);
        Task<FileModel> GetFileByFileId(int FileId);
        Task<FileModel> GetFileByGuid(Guid Guid);
        Task<CustomStream> GetFileStream(Guid id);
    }
}
