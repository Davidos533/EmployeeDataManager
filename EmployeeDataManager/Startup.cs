using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;           // using для использования срдеств отладки и логирования

namespace EmployeeDataManager
{
    // класс реализаующий обработку входящих запросов, маршрутизацию, запуск веб-сервера
    public class Startup        
    {
        IWebHostEnvironment m_enviroment;       // переменная интерфейса ... для получения данных о среде выполнения программы

        // конструктор класса
        public Startup(IWebHostEnvironment enviroment)
        {
            m_enviroment = enviroment;      // получение ссылки на объект хранящий в себе информацию о среде выполнения программы

            Console.WriteLine("DEBUG::STARTUP::CONSTRUCTOR::WORKED");
        }
        // метод для подлкючения сервисов
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.Preload = true;                 // включение специального списка доменов обращение к которым безопасно
                options.IncludeSubDomains = true;       // включение HSTS для всех поддоменов
                options.MaxAge = TimeSpan.FromDays(60); // время жизни заголовка HSTS
            });
            
            services.AddMvc();                          // добавление поддрежки паттерна проектирования Модель Представление Контроллер

            services.AddSingleton<DataBaseManager>();   // создание зависимости модели и контроллера

            services.AddControllersWithViews();

            Console.WriteLine("DEBUG::STARTUP::CONFIGURE_SERVICES::WORKED");
        }
        // метод для определния того как будет обрабатываться запрос
        // этот метод может так же принимать любой сервис который зарегистрирован в методе Configure Services
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)     
        {
            // объект app позволяет установить компоненты которые будет обрабатывать запрос
            // объект env повзоляет получить информацию о среде выполнения

            
            //  ------          конвейр обработки запросов          ------

            // если среда выполнения программы разработка
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();    // включение вывода исключений в виде отдельной страницы
            }
            // иначе если среда выполнения программы не разработка
            else
            {
                app.UseHsts();                      // включение переадресации пользователя на безопасное подключение https
            }
            app.UseStaticFiles();       // использование статических файлов
            app.UseRouting();           // использование системы маршрутизации
            app.UseHttpsRedirection();  // включение компонента middle ware для 

            app.UseMiddleware<ErrorHandlingMiddleware>();       // добалвение компонента обработки оишбок в конвейер обработки запроса

            // настройка конечных точек для работы паттерна MVC
            app.UseEndpoints(endpoints =>
            {
                // связывание контроллера с маршрутом
                endpoints.MapControllerRoute
                    (
                        name: "default",
                        pattern: "{controller=EmployeesManager}/{action=WelcomePage}/{id?}"           // настройки маршрутизации приложения для паттерна MVC, контроллер = EmployeesManager, метод = WelcomePage, id не обязательный параметр
                    );
            });
        }

    }
}
