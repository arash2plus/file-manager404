using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31.Models
{
    public class FileModel
    {
        //Id,FileName,Extension,Content,PrivacyType, CreateDate
        //@P_FileName,@P_Extension,@P_Content,@P_PrivacyType, @P_CreateDate
        public Guid ContentGuid { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public byte[] Content { get; set; }
        public bool PrivacyType { get; set; }
        public DateTime CreateDate { get; set; }
    }
   
    public class ConfigurationModel {
    }
}
