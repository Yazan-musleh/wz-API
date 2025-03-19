using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json;
using whatsapp.Domain.DTOs;
using whatsapp.Domain.Service;
using whatsapp.Domain.ServiceContract;

namespace whatsapp.Application.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(IConfiguration config) : base(config)
        {
        }

        public async Task<List<GroupInfoResponseDto>> GetGroups()
        {
            var options = new RestClientOptions("https://waapi.app/api/v1/instances/41849/client/action/get-chats");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization", "Bearer 5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574");
            var response = await client.PostAsync(request);

            var groups = new List<GroupInfoResponseDto>();

            try
            {
                using JsonDocument doc = JsonDocument.Parse(response.Content);
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("data", out JsonElement dataElement) &&
                    dataElement.TryGetProperty("data", out JsonElement dataArray))
                {
                    foreach (JsonElement item in dataArray.EnumerateArray())
                    {
                        // Check if this is a group
                        if (item.TryGetProperty("isGroup", out JsonElement isGroupElement) && isGroupElement.GetBoolean())
                        {
                            var group = new GroupInfoResponseDto
                            {
                                id = item.GetProperty("id").TryGetProperty("_serialized", out JsonElement serializedId)
                                ? serializedId.GetString()
                                : "Unknown",
                                groupName = item.TryGetProperty("name", out JsonElement nameElement)
                                ? nameElement.GetString()
                                : "Unnamed Group"
                            };

                            groups.Add(group);
                            
                            Console.WriteLine($"Group Name: {group.groupName}, Group ID: {group.id}");
                        }
                            
                    }
                }
                else
                {
                    Console.WriteLine("Error: 'data' key not found in JSON response.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return groups;
        }

        public async Task<MemoryStream> AddUsersToGroup(string groupId, IFormFile file)
        {

            await Task.Delay(100);

            List<string> responses = new List<string> ();
            List<ExportAddMembersResultToExcelDto> exportResponses = new List<ExportAddMembersResultToExcelDto>();


            var contacts = await ExtractPhoneNumbers(file);


            var options = new RestClientOptions("https://waapi.app/api/v1/instances/41849/client/action/add-group-participant");
            var client = new RestClient(options);

            int i = 0;

            var tasks = contacts.Select(async contact =>
            {
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", "Bearer 5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574");

                var requestBody = new
                {
                    participant = contact,
                    chatId = groupId
                };
                request.AddJsonBody(requestBody);
                var response = await client.PostAsync(request);
                responses.Add(response.Content);
                Console.WriteLine("{0}"+i, response.Content);
                i++;
            });

            await Task.WhenAll(tasks);


            foreach (var json in responses)
            {
                var response = JsonConvert.DeserializeObject<AddMembersJsonResponse>(json);

                foreach (var entry in response.Data.Data.Result)
                {
                    string phoneNumber = entry.Key.Replace("@c.us", "");
                    int code = entry.Value.Code;
                    string message = entry.Value.Message;

                    exportResponses.Add(new ExportAddMembersResultToExcelDto { PhoneNumber = phoneNumber, Message = message, Code = code });

                    Console.WriteLine($"Phone: {phoneNumber}, Code: {code}, Message: {message}");
                }
            }


            return await ExportExcelSheet(exportResponses);
        }


        public async Task<MemoryStream> GetGroupMembers(string groupId)
        {

            List<ExportGetMembersResultToExcelDto> responses = new List<ExportGetMembersResultToExcelDto>();

            var options = new RestClientOptions("https://waapi.app/api/v1/instances/41849/client/action/get-group-participants");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization", "Bearer 5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574");
            request.AddJsonBody("{\"chatId\":\""+groupId+"\"}", false);
            var response = await client.PostAsync(request);

            Console.WriteLine("{0}", response.Content);

            var jsonDoc = JsonDocument.Parse(response.Content);
            var participants = jsonDoc.RootElement
                .GetProperty("data")
                .GetProperty("data")
                .GetProperty("participants");

            foreach (var member in participants.EnumerateArray())
            {
                var phoneNumber = member.GetProperty("id").GetProperty("user").GetString();
                var isAdmin = member.GetProperty("isAdmin").GetBoolean();

                responses.Add(new ExportGetMembersResultToExcelDto { PhoneNumber = phoneNumber, IsAdmin = isAdmin });

                Console.WriteLine("phoneNumber: " + phoneNumber + "isAdmin: " + isAdmin);
            }

            return await ExportExcelSheet(responses);

        }

        public async Task<MemoryStream> CreateGroup(string groupName, IFormFile file)
        {

            List<string> responses = new List<string>();
            List<ExportAddMembersResultToExcelDto> exportResponses = new List<ExportAddMembersResultToExcelDto>();


            List<string> contacts = await ExtractPhoneNumbers(file);

            var options = new RestClientOptions("https://waapi.app/api/v1/instances/41849/client/action/create-group");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization", "Bearer 5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574");
            var requestBody = new
            {
                groupParticipants = contacts, // Using the list dynamically
                groupName = groupName
            };
            request.AddJsonBody(requestBody);
            var restResponse = await client.PostAsync(request);

            responses.Add(restResponse.Content);

            foreach (var json in responses)
            {
                var response = JsonConvert.DeserializeObject<AddMembersJsonResponse>(json);

                foreach (var entry in response.Data.Data.Participants)
                {
                    string phoneNumber = entry.Key.Replace("@c.us", "");
                    int code = entry.Value.Code;
                    string message = entry.Value.Message;

                    exportResponses.Add(new ExportAddMembersResultToExcelDto { PhoneNumber = phoneNumber, Message = message, Code = code });

                    Console.WriteLine($"Phone: {phoneNumber}, Code: {code}, Message: {message}");
                }
            }

            Console.WriteLine("{0}", restResponse.Content);
            return await ExportExcelSheet(exportResponses);
        }

        private async Task<MemoryStream> ExportExcelSheet(List<ExportAddMembersResultToExcelDto> dtos)
        {
            using var workbook = new XLWorkbook();
            var workSheet = workbook.Worksheets.Add("Members data");

            workSheet.Cell(1, 1).Value = "Phone number";
            workSheet.Cell(1, 2).Value = "Message";
            workSheet.Cell(1, 3).Value = "Code";

            for (int i = 0; i < dtos.Count(); i++)
            {
                workSheet.Cell(i+2, 1).Value = dtos[i].PhoneNumber;
                workSheet.Cell(i+2, 2).Value = dtos[i].Message;
                workSheet.Cell(i+2, 3).Value = dtos[i].Code;

                Console.WriteLine("phone number: ", i, dtos[i].PhoneNumber);
            }

            workSheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
        
        private async Task<MemoryStream> ExportExcelSheet(List<ExportGetMembersResultToExcelDto> dtos)
        {
            using var workbook = new XLWorkbook();
            var workSheet = workbook.Worksheets.Add("Members data");

            workSheet.Cell(1, 1).Value = "Phone number";
            workSheet.Cell(1, 2).Value = "isAdmin";

            for (int i = 0; i < dtos.Count(); i++)
            {
                workSheet.Cell(i+2, 1).Value = dtos[i].PhoneNumber;
                workSheet.Cell(i+2, 2).Value = dtos[i].IsAdmin;

                Console.WriteLine("phone number: ", i, " ", dtos[i].PhoneNumber);
            }

            workSheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }


        private async Task<List<string>> ExtractPhoneNumbers(IFormFile file)
        {
            var phoneNumbers = new List<string>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1); // Read first sheet
                    int columnNumber = 1; // Assuming phone numbers are in column A


                    foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
                    {
                        var cell = row.Cell(columnNumber);

                        if (!cell.IsEmpty())
                        {
                            string phoneNumber = cell.CachedValue.ToString().Trim() ?? "";

                            // Ensure the value is a valid phone number
                            if (long.TryParse(phoneNumber, out _)) // Check if it's numeric
                            {
                                phoneNumbers.Add(phoneNumber + "@c.us"); // Format for WhatsApp
                            }
                        }
                    }
                }
            }

            return phoneNumbers;
        }

    }
}