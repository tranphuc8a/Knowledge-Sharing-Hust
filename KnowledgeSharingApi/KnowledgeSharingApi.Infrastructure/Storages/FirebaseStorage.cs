using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Storages
{
    public class FirebaseStorage : IStorage
    {
        public Task<string?> GetImageUrl(string filename)
        {
            //// Tạo bucket Firebase Storage
            //var bucket = FirebaseAdmin.Storage.Bucket("your-firebase-bucket-name");

            //// Lấy URL của file ảnh
            //var url = await bucket.GetFileDownloadUrlAsync(fileName);

            //return url;
            throw new NotImplementedException();
        }

        public Task<string> SaveImage(IFormFile image)
        {
            throw new NotImplementedException();
        }
    }
}
