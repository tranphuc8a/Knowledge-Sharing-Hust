# Đồ án tốt nghiệp kỹ sư
- Đề tài: Mạng xã hội chia sẻ kiến thức cho sinh viên Bách Khoa - [Knowledge Sharing](https://knowledge-sharing-delta.vercel.app/)
- Sinh viên: **[Trần Văn Phúc](https://www.facebook.com/tranphuc8a) - 20194139**
- GVHD: [**ThS. Lê Đức Trung**](https://soict.hust.edu.vn/ths-le-duc-trung.html)
- **[SoICT](https://soict.hust.edu.vn/) - [HUST](https://hust.edu.vn/)**, kỳ 2023.2  
- *Hà Nội, tháng 06 năm 2024*

# Cài đặt và chạy ứng dụng

## Chuẩn bị môi trường
- Backend: [ASP.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Frontend: [Node.js](https://nodejs.org/en)
- Database:  [MySQL](https://www.mysql.com/)

## Khởi động frontend (Vue.js)  
- **Trỏ tới mã nguồn Frontend:** ` cd KnowledgeSharingVue/knowledge-sharing-vue `
- **Cài đặt dependencies và chạy frontend:**
```
npm install
npm run serve -- --port 8080
```  
Frontend sẽ chạy trên địa chỉ [localhost:8080](http://localhost:8080)


## Khởi động backend (ASP.NET Core API)
- **Trỏ tới mã nguồn Backend:** ` cd KnowledgeSharingApi `
- **Cài đặt dependencies và khởi động backend:**
```
dotnet build
dotnet run --project KnowledgeSharingApi/KnowledgeSharingApi.csproj --urls=http://localhost:5000
```
Backend sẽ chạy trên địa chỉ [localhost:5000](http://localhost:5000)

## Cấu hình cơ sở dữ liệu
- Khởi động hệ quản trị cơ sở dữ liệu MySQL
- Tạo cơ sở dữ liệu rồi chạy file script tạo cấu trúc bảng biểu `create-database.sql` trong thư mục gốc của project
- Cấu hình lại chuỗi kết nối cơ sở dữ liệu trong file `appsettings.json` của backend, biến `ConnectionStrings:MariaDb`

## Build và chạy toàn bộ ứng dụng

1. **Build Backend và Frontend:**
- Đối với frontend: ` npm run build `  
- Đối với backend: `dotnet build KnowledgeSharingApi.sln`


2. **Run Toàn bộ ứng dụng:**
- Khởi động Backend và Frontend theo hướng dẫn ở trên.
- Truy cập địa chỉ [localhost:8080](http://localhost:8080) trên trình duyệt để xem ứng dụng hoạt động.


# Triển khai

- Frontend  [knowledge-sharing-delta.vercel.app](https://knowledge-sharing-delta.vercel.app/)
- Backend [tranphuc8a.somee.com](https://tranphuc8a.somee.com/swagger/index.html)
- Database [db4free](https://db4free.net/)
