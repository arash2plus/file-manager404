using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore31.Model
{
    public class FileModel
    {

        [Key]
        public Guid ContentGuid { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public byte[] Content { get; set; }
        public bool PrivacyType { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
