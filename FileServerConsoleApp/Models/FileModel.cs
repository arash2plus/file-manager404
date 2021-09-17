using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FileServerConsoleApp.Models
{
    public class FileModel
    {
        //@P_FileName,@P_Extension,@P_Content,@P_PrivacyType, @P_CreateDate
        [Key]
        public Guid ContentGuid { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public byte[] Content { get; set; }
        public bool PrivacyType { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
