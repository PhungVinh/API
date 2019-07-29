using AttributesManagement.Constant;
using AttributesManagement.Models;
using AttributesManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.DataAccess
{
    public class ImportExportDA : IExportAndImportRepository
    {
        private CRM_MPContext db = new CRM_MPContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CRM_MPContext>());
        private SP_Attributes sp;
        private IDistributedCache _distributedCache;
        public string strconnect { get; set; }
        public object Import(string fileName,string rootFolder)
        {
            //string fileName = @"ImportCustomers.xlsx";
            FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets["Customer"];
                int totalRows = workSheet.Dimension.Rows;

                List<TblCategory> customerList = new List<TblCategory>();

                for (int i = 2; i <= totalRows; i++)
                {
                    customerList.Add(new TblCategory
                    {
                        CategoryCode= workSheet.Cells[i, 1].Value.ToString(),
                        CategoryName= workSheet.Cells[i, 2].Value.ToString(),
                        CategoryTypeCode = workSheet.Cells[i, 3].Value.ToString(),
                        CategoryDescription= workSheet.Cells[i, 4].Value.ToString(),
                    });
                }
                db.TblCategory.AddRange(customerList);
                db.SaveChanges();

                return customerList;
            }
        }
        public string ExportCustomer(string rootFolder)
        {
            string fileName = @"ExportCustomers.xlsx";

            FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));

            using (ExcelPackage package = new ExcelPackage(file))
            {
                List<TblCategory> customerList = db.TblCategory.Where(c => c.IsDelete == false).ToList<TblCategory>();

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Customer");
                int totalRows = customerList.Count();

                worksheet.Cells[1, 1].Value = "Category Code";
                worksheet.Cells[1, 2].Value = "Category ParentCode";
                worksheet.Cells[1, 3].Value = "Category Name";
                worksheet.Cells[1, 4].Value = "Category Description";
                int i = 0;
                for (int row = 2; row <= totalRows + 1; row++)
                {
                    worksheet.Cells[row, 1].Value = customerList[i].CategoryCode;
                    worksheet.Cells[row, 2].Value = customerList[i].CategoryTypeCode;
                    worksheet.Cells[row, 3].Value = customerList[i].CategoryName;
                    worksheet.Cells[row, 4].Value = customerList[i].CategoryDescription;
                    i++;
                }

                package.Save();

            }

            return " Customer list has been exported successfully";
        }

        public void SetStringCache(string cacheKey, object obj)
        {
            throw new NotImplementedException();
        }

        public void SetContextFactory(ConnectionStrings connectionStrings)
        {
            throw new NotImplementedException();
        }

        public void LoadContext(string orgCode, IDistributedCache distributed)
        {
            if (db == null || strconnect != orgCode)
            {
                string conn = CategoryConstant.SQL_CONNECTION;
                var optionsBuilder = new DbContextOptionsBuilder<CRM_MPContext>();
                optionsBuilder.UseSqlServer(conn.Replace("MP", orgCode), providerOptions => providerOptions.CommandTimeout(300));
                db = new CRM_MPContext(optionsBuilder.Options);
            }
            sp = new SP_Attributes(db);
            _distributedCache = distributed;
        }
    }
}
