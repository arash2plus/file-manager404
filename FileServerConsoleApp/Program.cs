using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FileServerConsoleApp.Models;

namespace FileServerConsoleApp
{

    public class Program
    {
        public Program()//ApplicationDbContext dbContext, IHostingEnvironment env)
        {
            //_dbContext = dbContext;
            //_env = env;
            //_dir = _env.ContentRootPath;
        }

        static void Main(string[] args)
        {
            GetFiles(new Guid("5A212D0E-6F17-EB11-9152-D45D643E3F57"));
            //UploadFile().Wait();
            Console.ReadLine();
        }

        static async Task<FileModel> GetFiles(Guid guid)
        {
            var file = new FileModel();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65213");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(string.Format("api/download/?FileGuid={0}", guid));

                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    file = await response.Content.ReadAsAsync<FileModel>();
                }
            }

            return file;
        }

        //    static async Task<FileModel> UploadFile(IFormFile file)
        //    {
        //        var model = new FileModel();
        //        bool Result;
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri("http://localhost:65213");
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //          //  HttpContent httpContent = file;

        //           HttpResponseMessage response = await client.PostAsync("api/upload", );
        //            response.EnsureSuccessStatusCode();
        //            if (response.IsSuccessStatusCode)
        //            {
        //                model = await response.Content.ReadAsAsync<FileModel>();
        //            }
        //        }

        //        return model;

        //        //string Extension = Path.GetExtension(file.FileName.Trim());
        //        //string PathSave = _env.ContentRootPath + "\\wwwroot\\TempFile\\";
        //        //string FileName = file.FileName;
        //        //string FinalFilePath = PathSave + FileName;

        //        //using (MemoryStream ms = new MemoryStream())
        //        //{
        //        //    file.CopyToAsync(ms);
        //        //    byte[] FileBytes = ms.ToArray();

        //        //    file.CopyToAsync(ms);

        //        //    _dbContext.Files.Add(new Models.FileModel { FileName = file.FileName, Content = FileBytes, Extension = Extension, PrivacyType = true, CreateDate = DateTime.Now });
        //        //    _dbContext.SaveChangesAsync();//_dbContext.SaveAsAsync(fileName);
        //        //    ms.Dispose();


        //    }
        //}
    }
}

