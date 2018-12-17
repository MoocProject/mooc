using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;


namespace Mooc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //if (AngularGenerator.ShouldRunMvc(args))
            //{
              
            //}
            //CreateWebHostBuilder(args).Build().Run();

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()//最小的输出单位是Debug级别的
                                       // .MinimumLevel.Override("Microsoft", LogEventLevel.Information)//将Microsoft前缀的日志的最小输出级别改成Information
            .Enrich.FromLogContext()
            .WriteTo.File(string.Format(@"D:\WebSite\WriteLog\Mooc\{0}.txt", DateTime.Now.ToString("yyyyMMdd")))//将日志输出到目标路径，文件的生成方式为每天生成一个文件
            .CreateLogger();

            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Error("Host terminated unexpectedly=" + ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()//添加这个
                .UseStartup<Startup>();
    }
}
