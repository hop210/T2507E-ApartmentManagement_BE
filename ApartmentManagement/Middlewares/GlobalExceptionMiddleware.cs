using ApartmentManagement.Exceptions;
using System.Net;

namespace ApartmentManagement.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        // Bơm ILogger vào để ghi log hệ thống
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                // Bắt các lỗi nghiệp vụ do chúng ta chủ động ném ra (VD: 409 Conflict, 400 Bad Request)
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex.StatusCode;

                await context.Response.WriteAsJsonAsync(new
                {
                    StatusCode = ex.StatusCode,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Ghi lại chi tiết lỗi vào log của Server để Dev debug
                _logger.LogError(ex, "Unhandled exception occurred.");

                // Che giấu chi tiết lỗi, chỉ trả về câu thông báo chung chung cho Frontend
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsJsonAsync(new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau!"
                });
            }
        }
    }
}