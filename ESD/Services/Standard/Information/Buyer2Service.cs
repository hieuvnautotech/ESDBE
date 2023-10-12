using Dapper;
using Newtonsoft.Json;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using System.Data;
using static ESD.Extensions.ServiceExtensions;


namespace ESD.Services.Standard.Information
{
    public interface IBuyer2Service
    {
        Task<ResponseModel<IEnumerable<BuyerDto>?>> Get(BuyerDto model);
        Task<ResponseModel<IEnumerable<BuyerDto>?>> GetAll(BuyerDto model);
        Task<string> Create(BuyerDto model);
        Task<ResponseModel<BuyerDto?>> GetById(long buyerid);
        Task<string> Modify(BuyerDto model);
        Task<string> Delete(BuyerDto model);
        Task<ResponseModel<IEnumerable<BuyerDto>?>> GetActive(BuyerDto model);
        Task<ResponseModel<BuyerDto?>> CreateByExcel(List<BuyerExcelDto> model, long userCreate);
    }

    [ScopedRegistration]
    public class Buyer2Service : IBuyerService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public Buyer2Service(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<BuyerDto>?>> Get(BuyerDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BuyerDto>?>();
                var proc = $"Usp_Buyer_Get";
                var param = new DynamicParameters();
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@BuyerCode", model.BuyerCode);
                param.Add("@BuyerName", model.BuyerName);
                param.Add("@isActived", model.isActived);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);
                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BuyerDto>(proc, param);

                if (!data.Any())
                {
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                    returnData.HttpResponseCode = 204;
                }
                else
                {
                    returnData.Data = data;
                    returnData.TotalRow = param.Get<int>("totalRow");
                }

                return returnData;
            }
            catch (Exception E)
            { 

                throw;
            }
            
        }
        public async Task<ResponseModel<IEnumerable<BuyerDto>?>> GetAll(BuyerDto model)
        {
            var returnData = new ResponseModel<IEnumerable<BuyerDto>?>();
            var proc = $"Usp_Buyer_GetAll";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BuyerDto>(proc);

            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }
            else
            {
                returnData.Data = data;
            }

            return returnData;
        }

        public async Task<string> Create(BuyerDto model)
        {
            string proc = "Usp_Buyer_Create";
            var param = new DynamicParameters();
            param.Add("@BuyerId", model.BuyerId);
            param.Add("@BuyerCode", model.BuyerCode);
            param.Add("@BuyerName", model.BuyerName);
            param.Add("@BrandName", model.BrandName);
            param.Add("@Description", model.Description);
            param.Add("@Website", model.Website);
            param.Add("@PhoneNumber", model.PhoneNumber);
            param.Add("@Email", model.Email);
            param.Add("@Fax", model.Fax);
            param.Add("@Tax", model.Tax);
            param.Add("@Address", model.Address);
            param.Add("@DateSignContract", model.DateSignContract);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            //throw new NotImplementedException();
        }

        public async Task<ResponseModel<BuyerDto?>> GetById(long buyerid)
        {
            var returnData = new ResponseModel<BuyerDto?>();
            var proc = $"Usp_Buyer_GetById";
            var param = new DynamicParameters();
            param.Add("@BuyerId", buyerid);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BuyerDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }

        public async Task<string> Modify(BuyerDto model)
        {
            string proc = "Usp_Buyer_Modify";
            var param = new DynamicParameters();
            param.Add("@BuyerId", model.BuyerId);
            param.Add("@BuyerCode", model.BuyerCode);
            param.Add("@BuyerName", model.BuyerName);
            param.Add("@BrandName", model.BrandName);
            param.Add("@Description", model.Description);
            param.Add("@Website", model.Website);
            param.Add("@PhoneNumber", model.PhoneNumber);
            param.Add("@Email", model.Email);
            param.Add("@Fax", model.Fax);
            param.Add("@Tax", model.Tax);
            param.Add("@Address", model.Address);
            param.Add("@DateSignContract", model.DateSignContract);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }
        public async Task<string> Delete(BuyerDto model)
        {
            string proc = "Usp_Buyer_Delete";
            var param = new DynamicParameters();
            param.Add("@BuyerId", model.BuyerId);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@isActived", model.isActived);
            param.Add("@row_version", model.row_version);

            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<ResponseModel<IEnumerable<BuyerDto>?>> GetActive(BuyerDto model)
        {
            var returnData = new ResponseModel<IEnumerable<BuyerDto>?>();
            var proc = $"Usp_Buyer_GetActive";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BuyerDto>(proc);

            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }
            else
            {
                returnData.Data = data;
            }

            return returnData;
        }
        public async Task<ResponseModel<BuyerDto?>> CreateByExcel(List<BuyerExcelDto> model, long userCreate)
        {
            var returnData = new ResponseModel<BuyerDto?>();

            var jsonLotList = JsonConvert.SerializeObject(model);

            string proc = "Usp_Buyer_CreateByExcel";
            var param = new DynamicParameters();
            param.Add("@Jsonlist", jsonLotList);
            param.Add("@createdBy", userCreate);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData.HttpResponseCode = 200;
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
    }
}
