# 🏢 Apartment Management API

## 🚀 Công nghệ sử dụng

* **Framework:** .NET 10 (ASP.NET Core Web API)
* **Database:** SQL Server & Entity Framework Core
* **Authentication/Authorization:** JWT & Role-based Authorization
* **Documentation:** Swagger / OpenAPI
* **Password Security:** Isopoh.Cryptography.Argon2

## 🛠 Hướng dẫn chạy dự án

### 1. Clone repository

```bash
git clone <link-repo-cua-ban>
cd ApartmentManagement
```

### 2. Cấu hình Database

Mở file `appsettings.json` và cập nhật `DefaultConnection` phù hợp với SQL Server của bạn.

### 3. Cập nhật Database

Chạy lệnh:

```bash
dotnet ef database update
```

### 4. Chạy Backend

```bash
dotnet run
```

Sau khi chạy thành công, Backend sẽ hiển thị địa chỉ API trong terminal, ví dụ:

```text
https://localhost:<port>
```

### 5. Swagger

Truy cập:

```text
https://localhost:<port>/swagger
```

Swagger được sử dụng để xem và test các API.

### 6. Đăng nhập

Tài khoản Admin mặc định để test:

```text
Username: admin
Password: admin123
```

Sau khi đăng nhập, lấy `JWT Token` từ API Login.

Trong Swagger, nhấn **Authorize** và nhập:

```text
Bearer <JWT-Token>
```

Sau đó có thể test các API yêu cầu xác thực.
