using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;           // using ��� ������������� ������� ������� � �����������

namespace EmployeeDataManager
{
    // ����� ������������ ��������� �������� ��������, �������������, ������ ���-�������
    public class Startup        
    {
        IWebHostEnvironment m_enviroment;       // ���������� ���������� ... ��� ��������� ������ � ����� ���������� ���������

        // ����������� ������
        public Startup(IWebHostEnvironment enviroment)
        {
            m_enviroment = enviroment;      // ��������� ������ �� ������ �������� � ���� ���������� � ����� ���������� ���������

            Console.WriteLine("DEBUG::STARTUP::CONSTRUCTOR::WORKED");
        }
        // ����� ��� ����������� ��������
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.Preload = true;                 // ��������� ������������ ������ ������� ��������� � ������� ���������
                options.IncludeSubDomains = true;       // ��������� HSTS ��� ���� ����������
                options.MaxAge = TimeSpan.FromDays(60); // ����� ����� ��������� HSTS
            });
            
            services.AddMvc();                          // ���������� ��������� �������� �������������� ������ ������������� ����������

            services.AddSingleton<DataBaseManager>();   // �������� ����������� ������ � �����������

            services.AddControllersWithViews();

            Console.WriteLine("DEBUG::STARTUP::CONFIGURE_SERVICES::WORKED");
        }
        // ����� ��� ���������� ���� ��� ����� �������������� ������
        // ���� ����� ����� ��� �� ��������� ����� ������ ������� ��������������� � ������ Configure Services
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)     
        {
            // ������ app ��������� ���������� ���������� ������� ����� ������������ ������
            // ������ env ��������� �������� ���������� � ����� ����������

            
            //  ------          ������� ��������� ��������          ------

            // ���� ����� ���������� ��������� ����������
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();    // ��������� ������ ���������� � ���� ��������� ��������
            }
            // ����� ���� ����� ���������� ��������� �� ����������
            else
            {
                app.UseHsts();                      // ��������� ������������� ������������ �� ���������� ����������� https
            }
            app.UseStaticFiles();       // ������������� ����������� ������
            app.UseRouting();           // ������������� ������� �������������
            app.UseHttpsRedirection();  // ��������� ���������� middle ware ��� 

            app.UseMiddleware<ErrorHandlingMiddleware>();       // ���������� ���������� ��������� ������ � �������� ��������� �������

            // ��������� �������� ����� ��� ������ �������� MVC
            app.UseEndpoints(endpoints =>
            {
                // ���������� ����������� � ���������
                endpoints.MapControllerRoute
                    (
                        name: "default",
                        pattern: "{controller=EmployeesManager}/{action=WelcomePage}/{id?}"           // ��������� ������������� ���������� ��� �������� MVC, ���������� = EmployeesManager, ����� = WelcomePage, id �� ������������ ��������
                    );
            });
        }

    }
}
