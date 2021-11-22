using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EmployeeDataManager
{
    // главный класс в котором содержится точка входа в программу
    public class Program        
    {
        

        // точка входа в программу
        public static void Main(string[] args)
        {
            IHostBuilder hostBuilder = CreateHostBuilder(args);     // созадние объекта построителя хоста
            IHost host= hostBuilder.Build();                        // создание объекта хоста
            host.Run();                                             // запус хоста

            // хост запущен, все входящие HTTP запросы прослушиваются
            Console.WriteLine("DEBUG::PROGRAM::MAIN::HOST::START");
        }

        // статический метод для создания и настройки объекта IHsotBuilder
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>     // делегат
                {
                    webBuilder.UseStartup<Startup>();       // установка стартового класса приложения как Startup для обработки входящих запросов
                    webBuilder.UseWebRoot("static");
                });
    }
}
