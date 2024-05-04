using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
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
        protected readonly StorageClient storageClient;
        protected readonly string BucketName;
        protected readonly string FolderName;
        private const string FirebaseStorageBaseUrl = "https://firebasestorage.googleapis.com";

        public FirebaseStorage()
        {
            // Đọc file cấu hình của Firebase từ đường dẫn
            var googleCredential = GoogleCredential.FromFile("firebase.json");

            // Sử dụng khóa để tạo StorageClient
            storageClient = StorageClient.Create(googleCredential);

            // Đặt tên cho bucket, nhớ bỏ tiền tố 'gs://'
            BucketName = "bksnet-e46a7.appspot.com";

            FolderName = "knowledgesharing";
        }

        public async Task<string?> GetImageUrl(string filename)
        {
            try
            {
                // Tạo URL trực tiếp cho file dựa vào tên bucket và tên file
                // URL dạng: https://firebasestorage.googleapis.com/v0/b/bucket_name/o/file_name?alt=media
                var url = $"{FirebaseStorageBaseUrl}/v0/b/{BucketName}/o/{Uri.EscapeDataString(filename)}?alt=media";

                return await Task.FromResult(url);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu không thể tạo được URL
                Console.WriteLine($"Error getting image URL: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }

            var filename = FolderName + "/" + Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            try
            {
                // Cố gắng tải file lên Firebase
                await storageClient.UploadObjectAsync(BucketName, filename, image.ContentType, memoryStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during image upload: {ex.Message}");
                return null; // Trả về null nếu có lỗi xảy ra trong quá trình upload
            }

            // Nếu không có lỗi, tạo và trả về URL của file ảnh
            var imageUrl = await GetImageUrl(filename);
            return imageUrl;
        }
    }
}
