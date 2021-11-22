using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EmployeeDataManager
{
    // ������� ����� � ������� ���������� ����� ����� � ���������
    public class Program        
    {
        

        // ����� ����� � ���������
        public static void Main(string[] args)
        {
            IHostBuilder hostBuilder = CreateHostBuilder(args);     // �������� ������� ����������� �����
            IHost host= hostBuilder.Build();                        // �������� ������� �����
            host.Run();                                             // ����� �����

            // ���� �������, ��� �������� HTTP ������� ��������������
            Console.WriteLine("DEBUG::PROGRAM::MAIN::HOST::START");
        }

        // ����������� ����� ��� �������� � ��������� ������� IHsotBuilder
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>     // �������
                {
                    webBuilder.UseStartup<Startup>();       // ��������� ���������� ������ ���������� ��� Startup ��� ��������� �������� ��������
                    webBuilder.UseWebRoot("static");
                });
    }
}
