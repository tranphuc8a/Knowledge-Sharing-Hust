using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;
using KnowledgeSharingApi.Domains.Exceptions;

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

        public async Task<string?> SaveImage(IFormFile? image)
        {
            if (image == null || image.Length == 0)
                return null;

            // Định nghĩa kích thước tối đa của hình ảnh là 50MB
            const int MaxImageSize = 50 * 1024 * 1024;

            if (image.Length > MaxImageSize)
            {
                throw new ValidatorException($"Kích thước của hình ảnh quá lớn. Tối đa cho phép là 50MB.");
            }

            var contentType = image.ContentType;
            var compressedImageBytes = await CompressImage(image) ?? throw new Exception("Nén hình ảnh lỗi");
            var filename = FolderName + "/" + Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            try
            {
                // Cố gắng tải file lên Firebase
                await storageClient.UploadObjectAsync(BucketName, filename, contentType, new MemoryStream(compressedImageBytes));
            }
            catch (Exception ex)
            {
                // Log exception details
                Console.WriteLine($"Lỗi khi tải ảnh lên: {ex.Message}");
                throw; // Ném ra ngoại lệ trở lại
            }

            // Nếu không có lỗi, tạo và trả về URL của file ảnh
            var imageUrl = await GetImageUrl(filename);
            return imageUrl;
        }

        public async Task<byte[]?> CompressImage(IFormFile? image)
        {
            if (image == null) return null;

            // Load the image into memory
            using var imageStream = image.OpenReadStream();
            using var outputImage = await Image.LoadAsync(imageStream);

            // Set initial quality to a higher value
            var quality = 90;
            bool shouldExit = false;

            // Loop until the size is reduced enough or we reach break conditions
            while (!shouldExit)
            {
                // Encode the image to a memory stream with the given quality
                await using var memoryStream = new MemoryStream();
                var encoder = new JpegEncoder { Quality = quality };
                outputImage.SaveAsJpeg(memoryStream, encoder);

                // Check the size of the encoded image
                if (memoryStream.Length < 64000 || quality <= 35) // Adjusted condition to prevent too low quality
                {
                    shouldExit = true;
                }
                else
                {
                    // Choose whether to reduce size or quality based on a threshold
                    if (quality <= 40) // Adjusted threshold for size reduction
                    {
                        // Reduce size (width and height)
                        outputImage.Mutate(x => x.Resize(outputImage.Width * 2 / 3, outputImage.Height * 2 / 3));
                    }
                    else
                    {
                        // Adjust quality based on size reduction rate
                        quality -= (int)(quality * 0.1); // Reduce quality by 10% of current quality
                    }
                }

                if (shouldExit)
                {
                    // We're exiting the loop, so return the last version
                    memoryStream.Position = 0;
                    return memoryStream.ToArray();
                }
            }

            return null; // In case we somehow exit the while loop without returning
        }

    }
}
