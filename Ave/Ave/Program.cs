using System.Threading.Tasks;
using Ave.DAL.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ave.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                //var reportBuilder = services.GetRequiredService<IReportGenerator>();

                //var reportFormat = ReportSavingOption.DownloadExcel;

                //var reportCommandData = new CreateReportCommandData()
                //{
                //    ReportTypeId = 23,
                //    SavingOptions = reportFormat,
                //    Grouping = ReportGroupingOption.Package,
                //    CustomerId = 23,
                //    SupplierId = 1,
                //    ProductId = 7,
                //    DestinationCountryId = 8,
                //    Title = "Test report"
                //};

                //var report = await reportBuilder.CreateReport(reportCommandData);

                //var date = DateTime.Now.ToFilenameDateTimeString();
                //string filename = @$"C:\Users\Artem Balianytsia\Desktop\Reports\Report {date}.";

                //switch (reportFormat)
                //{
                //    case ReportSavingOption.DownloadPdf:
                //        filename += "pdf";
                //        await report.pdfReportStream.SaveAsFileAsync(filename);
                //        break;
                //    case ReportSavingOption.DownloadExcel:
                //        filename += "xlsx";
                //        await report.xlsxReportStream.SaveAsFileAsync(filename);
                //        break;
                //    default:
                //        throw new ArgumentOutOfRangeException();
                //}

                //System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filename)
                //{
                //    UseShellExecute = true
                //});

                //return;

                var appContext = services.GetRequiredService<AppDbContext>();
                await appContext.Database.MigrateAsync();
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
