using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using WebAppCore31.Models;
using WebAppCore31.Repository;
using WebAppCore31.Helper;
using WebAppCore31.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;


namespace WebAppCore31.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    public class FileManagerDbController : Controller
    {
        private readonly IFileManager FileDb;
        private readonly IHostingEnvironment Environment;

        public FileManagerDbController(IFileManager file, IHostingEnvironment env)
        {
            this.FileDb = file;
            Environment = env;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("api/upload")]
        [AllowAnonymous]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            Guid Result;
            string Extension = Path.GetExtension(file.FileName.Trim());
            string ContentType = MimeTypes.GetContentType(file.FileName);
            Guid guid = Guid.NewGuid();
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    byte[] FileBytes = ms.ToArray();

                    Result = await FileDb.InsFile(new FileModel { ContentGuid = guid, FileName = file.FileName, Content = FileBytes, Extension = Extension, PrivacyType = true, CreateDate = DateTime.Now });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            return Ok(Result);
        }
        [HttpGet]
        public async Task<IActionResult> GetFileByFileId(int FileId)
        {
            FileModel DbResult = await FileDb.GetFileByFileId(FileId);
            if (DbResult == null)
            {
                return NotFound();
            }
            string PathSave = Environment.ContentRootPath + "\\wwwroot\\TempFile\\";
            string FileName = DbResult.FileName;
            string FinalFilePath = PathSave + FileName;
            using (FileStream fsStream = new FileStream(FinalFilePath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fsStream))

            {
                writer.Write(DbResult.Content);
            }
            return Ok("/TempFile/" + FileName);
        }

        [Route("api/get")]
        public async Task<IActionResult> GetFileByGuid(Guid FileGuid)
        {
            FileModel DbResult = await FileDb.GetFileByGuid(FileGuid);
            if (DbResult == null)
            {
                return NotFound();
            }
            string PathSave = Environment.ContentRootPath + "\\wwwroot\\TempFile\\";
            string FileName = DbResult.FileName;
            string FinalFilePath = PathSave + FileName;

            using (FileStream fsStream = new FileStream(FinalFilePath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fsStream))
            {
                writer.Write(DbResult.Content);
            }

            string contentType = MimeTypes.GetContentType(DbResult.FileName);
            var stream = new FileStream(FinalFilePath, FileMode.Open);

            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = FileName
            };
        }
        public async Task<IActionResult> GetContentByGuid(Guid FileGuid)
        {
            FileModel DbResult = await FileDb.GetFileByGuid(FileGuid);
            string PathSave = Environment.ContentRootPath + "\\wwwroot\\TempFile\\";
            string FileName = DbResult.FileName;
            string FinalFilePath = PathSave + FileName;

            using (FileStream fsStream = new FileStream(FinalFilePath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fsStream))
            {
                writer.Write(DbResult.Content);
            }

            if (System.IO.File.Exists(FinalFilePath))
            {
                System.IO.File.Delete(FinalFilePath);
            }
            return Ok(DbResult);
        }

        [Route("api/download/{id?}")]
        public async Task<IActionResult> GetFileStream(Guid id)
        {
            FileModel DbResult = await FileDb.GetFileByGuid(id);

            if (DbResult == null)
            {
                return NotFound();
            }
            string FileName = DbResult.FileName;
            string contentType = MimeTypes.GetContentType(DbResult.FileName).ToLowerInvariant();
            var stream = await FileDb.GetFileStream(id);
            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = FileName
                //enablerangeprocessing = true
            };
        }
    }
}
