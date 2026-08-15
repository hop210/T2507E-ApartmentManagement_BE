using System.Net;
using System.Text.Json;

namespace ApartmentManagement.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Cho phép Request đi tiếp vào Controller
                await _next(context);
            }
            catch (Exception ex)
            {
                // Nếu có bất kỳ lỗi nào văng ra, tóm nó lại ở đây
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Báo cho Frontend biết đây là file JSON và là lỗi hệ thống (500)
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Đóng gói thông báo lỗi
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau!",
                Detailed = exception.Message // Chi tiết lỗi (có thể ẩn đi khi đẩy lên Production thật)
            };

            // Trả về cho Frontend
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}