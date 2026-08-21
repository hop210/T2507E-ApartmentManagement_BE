# 🏢 Apartment Management BE

## 🚀 Công nghệ sử dụng

* **Framework:** .NET 10 (ASP.NET Core Web API)
* **Database:** SQL Server & Entity Framework Core (Code-First)
* **Architecture / Patterns:** Repository Pattern, Dependency Injection (DI), Data Transfer Object (DTO)
* **Authentication / Authorization:** JWT & Role-based Authorization
* **Password Security:** Isopoh.Cryptography.Argon2
* **File Management:** Multipart/form-data (`IFormFile`), lưu trữ và quản lý file tĩnh cục bộ (Hợp đồng PDF, Ảnh bảo trì), Tích hợp MinIO (Cloud Storage)
* **Documentation:** Swagger / OpenAPI

---

## 🛠 Hướng dẫn chạy dự án

### 1. Clone repository

```bash
git clone <https://github.com/hop210/T2507E-ApartmentManagement_BE.git>
cd ApartmentManagement
```

### 2. Cấu hình Database

Mở file `appsettings.json` và cập nhật chuỗi kết nối `DefaultConnection` phù hợp với cấu hình SQL Server của bạn.

### 3. Cập nhật Database

Chạy lệnh sau để khởi tạo cơ sở dữ liệu:

```bash
dotnet ef database update
```

### 4. Cấu hình MinIO (Lưu trữ file)

Hệ thống sử dụng MinIO chạy qua Docker. Mở Terminal và chạy lệnh sau để khởi động trạm lưu trữ:

```bash
docker run -p 9000:9000 -p 9001:9001 --name minio -e "MINIO_ROOT_USER=admin" -e "MINIO_ROOT_PASSWORD=admin123" quay.io/minio/minio server /data --console-address ":9001"
```

*(Lưu ý: API sẽ tự động tạo Bucket và cấp quyền Public khi upload file đầu tiên).*

### 5. Chạy Backend

```bash
dotnet run
```

Sau khi chạy thành công, console sẽ hiển thị địa chỉ endpoint, ví dụ: `https://localhost:<port>`

### 6. Truy cập Swagger UI

Mở trình duyệt và truy cập vào đường dẫn: `https://localhost:<port>/swagger` để xem tài liệu và test các API.

### 7. Đăng nhập và Xác thực

Sử dụng tài khoản quản trị mặc định sau để test:
* **Username:** `admin`
* **Password:** `admin123`

Sau khi gọi API đăng nhập, sao chép `JWT Token` nhận được. Trên Swagger, nhấn nút **Authorize** và điền theo định dạng:
```text
Bearer <JWT-Token>
```