using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDataManager
{
    // класс реализующий компонент конвейера обработки запроса для обработки кодов ошибок
    public class ErrorHandlingMiddleware
    {
        private RequestDelegate m_next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            m_next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.ContentType = "text/html;charset=utf-8";

            await m_next.Invoke(context);
            
            // проверка кодов ошибок после работы других компонентов конвейера обработк запроса
            
            // если был кстановлен код ошибки 403
            if (context.Response.StatusCode == 403)
            {
                //context.Response.Redirect("accessDenied.html");         // переадресация на статическую страницу с сообщением об отказе о досутпе
                await context.Response.WriteAsync("<div style='margin-left:40%; font-family:Consolas; font-size:1.5em; color:brown;'>Access Denied</div>");         // переадресация на статическую страницу с сообщением об отказе о досутпе
            }
            // иначе если был установлен код ошибки 404
            else if (context.Response.StatusCode == 404)
            {
                await context.Response.WriteAsync("<div style='margin-left:40%; font-family:Consolas; font-size:1.5em; color:cornflowerblue;'>Page Not Found</div>");         // переадресация на статическую страницу с сообщением о том что страница не найдена
            }
        }
    }
}
