using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using Ave.BLL.Interface;
using Ave.DAL.Context;
using ClosedXML.Excel;
using ClosedXML.Report;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ave.BLL.Services
{
    public class ExportDatabaseToExcel: IExportDatabaseToExcel
    {
        private readonly AppDbContext _dbContext;

        protected readonly string ExecutablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        protected string TemplatesPath => Path.Combine(ExecutablePath, @"Templates");

        protected string TemplateFilename { get; } = "Data.xlsx";

        protected Stream GetTemplate()
        {
            var templatePath = Path.Combine(TemplatesPath, TemplateFilename);
            return File.OpenRead(templatePath);
        }

        public ExportDatabaseToExcel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string ExportDbToExcel()
        {
            var entityTypes = _dbContext.Model.GetEntityTypes();

            byte[] result = null;

            using (var templateStream = GetTemplate())
            {
                var template = new XLTemplate(templateStream);

                foreach (var type in entityTypes)
                {
                    var tableNameAnnotation = type.GetAnnotation("Relational:TableName");
                    var tableName = tableNameAnnotation.Value.ToString();

                    if (tableName == "Medicaments")
                    {
                        var data = DataTable($"Select Id, Name, CategoryId, ManufacturerId, Instruction, Description, ContentType From {tableName}");
                        template.Workbook.Worksheets.Add(data, tableName);
                    }
                    else
                    {
                        var data = DataTable($"Select * From {tableName}");
                        template.Workbook.Worksheets.Add(data, tableName);
                    }


                }

                // template.Generate();

                var xlsxReportStream = SaveAsStream(template.Workbook);

                using (var tmpXlsStream = new MemoryStream())
                {
                    xlsxReportStream.CopyTo(tmpXlsStream);

                    tmpXlsStream.Position = 0;

                    result = tmpXlsStream.ToArray();
                }
            }

            return Convert.ToBase64String(result);
        }

        public DataTable DataTable(string sqlQuery)
        {
            DataTable dataTable = new DataTable();
            DbConnection connection = _dbContext.Database.GetDbConnection();
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;
                using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        public Stream SaveAsStream(IXLWorkbook workbook)
        {
            var stream = new MemoryStream();
            workbook.SaveAs(stream);

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }

}
