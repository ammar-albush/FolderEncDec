using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderEncDec
{

    [Serializable]
    internal class ImageInfo
    {

        public byte[] Matrix { get; set; }

        public byte[]  Password { get; set; }

        public byte[] Salt { get; set; }    

        internal ImageInfo(byte[] matrix,byte[] password,byte[] salt)
        { 
        
            Matrix = matrix;
            Password = password;
            Salt = salt;    
        
        }


    }
}
