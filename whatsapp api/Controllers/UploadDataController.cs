using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using whatsapp.Domain.ServiceContract;

namespace whatsapp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadDataController : ControllerBase
    {

        private readonly IGetPhoneNumService _service;

        public UploadDataController(IGetPhoneNumService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelSheet(IFormFile excelSheet)
        {
            if (excelSheet == null || excelSheet.Length == 0)
            {
                return BadRequest("Pls provide a file");
            }

            if (!Path.GetExtension(excelSheet.FileName)
                .Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Pls provide a valid file extension");
            }

            int phoneNumsUploaded = await _service.UploadphoneNumsFromExcelSheet(excelSheet);

            return Ok(phoneNumsUploaded);
        }

        [HttpGet]
        public async Task<IActionResult> GetPhoneNums()
        {
            MemoryStream memoryStream = await _service.GetPhoneNums();

            return File(memoryStream,
                "application/vnd.ms-excel",
                "phoneNums.xlsx");
        }
    }
}
