using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using whatsapp.Domain.DTOs;

namespace whatsapp.Domain.ServiceContract
{
    public interface IGetPhoneNumService
    {
        /// <summary>
        /// Uploads phone numbers from excel sheet into database
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns>returns number of phone numbers added</returns>
        Task<int> UploadphoneNumsFromExcelSheet(IFormFile formFile);

        /// <summary>
        /// Get all phone nums from DB 
        /// </summary>
        /// <returns> returns list of getPhoneFromDBDto </returns>
        Task<MemoryStream> GetPhoneNums();
    }
}
