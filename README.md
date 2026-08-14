# 🏢 Hệ thống Quản lý Căn hộ (Apartment Management - Backend)

Dự án Backend cung cấp RESTful API cho hệ thống quản lý chung cư, căn hộ. Dự án được xây dựng tuân thủ kiến trúc phân lớp N-Tier Architecture (Controller > Service > Repository) để đảm bảo tính mở rộng và dễ bảo trì.

## 🛠 Công nghệ sử dụng
* **Framework:** ASP.NET Core Web API
* **Database:** Microsoft SQL Server (triển khai qua Docker Container)
* **ORM:** Entity Framework Core (EF Core)
* **API Documentation:** Swagger / OpenAPI

---

## ⚙️ Yêu cầu hệ thống (Prerequisites)
Để chạy dự án, máy tính của bạn cần cài đặt sẵn:
* Visual Studio 2022 (khuyến nghị phiên bản mới nhất).
* .NET SDK tương thích.
* **Docker Desktop** (Bắt buộc phải bật và đang chạy ngầm).

---

## 🚀 Hướng dẫn cài đặt và chạy dự án (Dành cho Team)

**Bước 1: Tải source code về máy**
Mở thư mục muốn lưu dự án, chuột phải chọn `Open in Terminal` (hoặc Git Bash) và chạy lệnh:
`git clone https://github.com/hop210/T2507E-ApartmentManagement_BE.git`

**Bước 2: Dựng Database (SQL Server) bằng Docker**
Mở Developer PowerShell (hoặc Terminal thông thường) và chạy lệnh sau để tạo một container SQL Server hoàn toàn mới chạy trên cổng `1433`:
`docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Apartment@123" -p 1433:1433 --name sqlserver_db_ApartmentManagement -d mcr.microsoft.com/mssql/server:2022-latest`

> **Lưu ý:** Đảm bảo máy bạn chưa có phần mềm nào khác (như SQL Server cài trực tiếp) đang chiếm dụng cổng `1433`.

**Bước 3: Đồng bộ các bảng (Migrations) vào Database**
Mở file Solution (`.sln`) bằng Visual Studio.
Trên thanh Menu, chọn `Tools` > `NuGet Package Manager` > `Package Manager Console`.
Chạy lệnh sau để hệ thống tự động đọc file Migrations và tạo các bảng (`Buildings`, `Users`...) vào SQL Server vừa dựng:
`Update-Database`

**Bước 4: Chạy ứng dụng**
Nhìn lên thanh công cụ trên cùng của Visual Studio, bấm vào mũi tên xổ xuống cạnh nút Play.
Đảm bảo bạn **CHỌN cấu hình chạy là `http`** (không chọn Container hay Docker).
Nhấn **F5** để khởi chạy ứng dụng.

---

## 📖 Hướng dẫn Test API (Swagger)
Khi ứng dụng khởi chạy thành công, một cửa sổ Console đen sẽ bật lên và thông báo số cổng mạng đang lắng nghe (Ví dụ: `Now listening on: http://localhost:5131`).

Mở trình duyệt web và truy cập vào đường dẫn sau để vào giao diện test API:
`http://localhost:<số-cổng-của-bạn>/swagger`

Tại đây, bạn có thể gọi trực tiếp các lệnh GET, POST, PUT, DELETE để thao tác với cơ sở dữ liệu.
