using AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using whatsapp.Domain.DTOs;
using whatsapp.Domain.Entities;
using whatsapp.Domain.RepoContract;
using whatsapp.Domain.ServiceContract;

namespace whatsapp.Application.Services
{
    public class GetPhoneNumService : IGetPhoneNumService
    {

        private readonly IPhoneNumRepo _repo;
        private readonly IMapper _mapper;

        public GetPhoneNumService(IPhoneNumRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<MemoryStream> GetPhoneNums()
        {
            MemoryStream memoryStream = new MemoryStream();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Use this for non-commercial usage

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {

                ExcelWorksheet worksheet =
                    excelPackage.Workbook.Worksheets.Add("phoneNumbers");

                worksheet.Cells["A1"].Value = "Name";
                worksheet.Cells["B1"].Value = "Phone numbers";
                worksheet.Cells["C1"].Value = "Status";


                var row = 2;

                IList<PhoneNumber> phoneNums = await _repo.GetPhoneNums();

                IList<GetPhoneFromDBDto> mappedPhonesNums =
                    _mapper.Map<IList<GetPhoneFromDBDto>>(phoneNums);

                foreach (var phoneNum in mappedPhonesNums)
                {
                    worksheet.Cells[row, 1].Value = phoneNum.Name;
                    worksheet.Cells[row, 2].Value = phoneNum.PhoneNumber;
                    worksheet.Cells[row, 3].Value = phoneNum.status;

                    row++;
                }

                worksheet.Cells[ $"A1:H{row}" ].AutoFitColumns();
                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;

            return memoryStream;
        }

        public async Task<int> UploadphoneNumsFromExcelSheet(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            IList<GetDataFromExcelDto> phoneNum = new List<GetDataFromExcelDto>();

            using (ExcelPackage excelPackage = new
                ExcelPackage(memoryStream))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Use this for non-commercial usage

                ExcelWorksheet excelWorksheet =
                    excelPackage.Workbook.Worksheets["phoneNumbers"];

                int rowCount = excelWorksheet.Dimension.Rows;

                for (int row = 2; row < rowCount; row++)
                {

                    string? cellValue = 
                        Convert.ToString(excelWorksheet.Cells[row, 1].Value);
                    string? cellValueNumbers =
                        Convert.ToString(excelWorksheet.Cells[row, 2].Value);


                    if (!string.IsNullOrEmpty(cellValue) 
                        && !string.IsNullOrEmpty(cellValueNumbers))
                    {
                        phoneNum.Add(
                            new GetDataFromExcelDto()
                            {name = cellValue, phoneNumber = cellValueNumbers});
                    }
                }

                IList<PhoneNumber> phoneNumbers = _mapper.Map<IList<PhoneNumber>>(phoneNum);

                await _repo.AddPhoneNums(phoneNumbers);
            }

            return phoneNum.Count();
        }
    }
}
