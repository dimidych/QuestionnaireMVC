using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuestionnaireMVC.Models;

namespace QuestionnaireMVC
{
    public class Program
    {
        /// <summary>
        /// Создаем БД, в соответствии с контекстом (берем из DI)
        /// </summary>
        public static void Main(string[] args)
        {
            var whostBuilder = CreateWebHostBuilder(args).Build();

            using (var scope = whostBuilder.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<IQuestionnaireContext>();
                    InitialData.Initialize(context as QuestionnaireContext);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Setting database error.");
                }
            }

            whostBuilder.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
