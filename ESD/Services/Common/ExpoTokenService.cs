using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static ESD.Extensions.ServiceExtensions;
using static System.Net.Mime.MediaTypeNames;

namespace ESD.Services.Common
{
    public interface IExpoTokenService
    {
        Task<string> Create(ExpoTokenDto model);
        Task<ResponseModel<IEnumerable<ExpoTokenDto>>> GetActive();
        Task PushExpoNotification();
    }

    [SingletonRegistration]
    public class ExpoTokenService : IExpoTokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISqlDataAccess _sqlDataAccess;
        public ExpoTokenService(IHttpClientFactory httpClientFactory, ISqlDataAccess sqlDataAccess)
        {
            _httpClientFactory = httpClientFactory;
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<string> Create(ExpoTokenDto model)
        {
            string proc = "sysUsp_ExpoToken_Create";
            var param = new DynamicParameters();
            param.Add("@ExpoToken", model.ExpoToken);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<string>(proc, param);
        }

        public async Task<ResponseModel<IEnumerable<ExpoTokenDto>>> GetActive()
        {

            var returnData = new ResponseModel<IEnumerable<ExpoTokenDto>>();
            string proc = "sysUsp_ExpoToken_GetActive";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ExpoTokenDto>(proc);

            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            else
            {
                returnData.Data = data;
            }
            return returnData;
        }

        public async Task PushExpoNotification()
        {
            HttpClient _httpClient = _httpClientFactory.CreateClient("expoToken");

            var postData = new ExpoPushNotificationData();

            var expoTokenList = await GetActive();

            if (expoTokenList.Data != null)
            {
                foreach (var item in expoTokenList.Data)
                {
                    postData.registration_ids.Add(item.ExpoToken);
                }
            }

            //var httpResponseMessage = await client.PostAsync("/send", postData);

            var postDataJson = new StringContent(
                                    JsonSerializer.Serialize(postData),
                                    Encoding.UTF8,
                                    Application.Json); // using static System.Net.Mime.MediaTypeNames;

            postDataJson.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            using var httpResponseMessage = await _httpClient.PostAsync("/fcm/send", postDataJson);

            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            //var result = httpResponseMessage.Result;

            //httpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}
