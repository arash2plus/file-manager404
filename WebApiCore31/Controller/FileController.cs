using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore31.Data;
using WebApiCore31.Helper;

namespace WebApiCore31.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public FileController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        //public async Task<IActionResult> GetFileStream(Guid id) {
        //    FilesEntities entities = new FilesEntities();
        //    tblFile file = entities.tblFiles.ToList().Find(p => p.id == fileId.Value);
        //    return File(file.Data, file.ContentType, file.Name);
        //}


    }
}
