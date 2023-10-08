using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using Dapper;
using ESD.Extensions;
using System.Data;
using ESD.DbAccess;
using ESD.Models;
using Newtonsoft.Json;
using ESD.Helpers;

namespace ESD.Services.Standard.Information
{
    public interface ILineService
    {
        Task<ResponseModel<IEnumerable<LineDto>?>> Get(LineDto model);
        Task<ResponseModel<IEnumerable<LineDto>?>> GetActive(LineDto model);
        Task<ResponseModel<LineDto?>> GetById(long? lineId);
        Task<string> Create(LineDto model);
        Task<string> Modify(LineDto model);
        Task<string> DeleteReuse(LineDto model);
        Task<ResponseModel<LineDto?>> CreateByExcel(List<LineExcelDto> model, long userCreate);
        Task<ResponseModel<IEnumerable<LineDto>?>> GetForSelect();
        Task<ResponseModel<IEnumerable<LineDto>?>> GetListPrintQR(List<long>? listQR);
    }

    [ScopedRegistration]
    public class LineService : ILineService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public LineService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<string> Create(LineDto model)
        {
            string proc = "Usp_Line_Create";
            var param = new DynamicParameters();
            param.Add("@LineId", model.LineId);
            param.Add("@AreaCode", model.AreaCode);
            param.Add("@LineName", model.LineName);
            param.Add("@Description", model.Description);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            //throw new NotImplementedException();
        }

        public async Task<string> DeleteReuse(LineDto model)
        {
            string proc = "Usp_Line_DeleteReuse";
            var param = new DynamicParameters();
            param.Add("@LineId", model.LineId);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@isActived", model.isActived);
            param.Add("@row_version", model.row_version);

            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<ResponseModel<IEnumerable<LineDto>?>> Get(LineDto model)
        {
            var returnData = new ResponseModel<IEnumerable<LineDto>?>();
            var proc = $"Usp_Line_Get";
            var param = new DynamicParameters();
            param.Add("@page", model.page);
            param.Add("@pageSize", model.pageSize);
            param.Add("@LineName", model.LineName);
            param.Add("@AreaCode", model.Location);
            param.Add("@isActived", model.isActived);
            param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<LineDto>(proc, param);

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

        public async Task<ResponseModel<IEnumerable<LineDto>?>> GetActive(LineDto model)
        {
            var returnData = new ResponseModel<IEnumerable<LineDto>?>();
            var proc = $"Usp_Line_GetActive";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<LineDto>(proc);

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

        public async Task<ResponseModel<LineDto?>> GetById(long? lineId)
        {
            var returnData = new ResponseModel<LineDto?>();
            var proc = $"Usp_Line_GetById";
            var param = new DynamicParameters();
            param.Add("@LineId", lineId);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<LineDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }

        public async Task<string> Modify(LineDto model)
        {
            string proc = "Usp_Line_Modify";
            var param = new DynamicParameters();
            param.Add("@LineId", model.LineId);
            param.Add("@LineName", model.LineName);
            param.Add("@AreaCode", model.AreaCode);
            param.Add("@Description", model.Description);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<ResponseModel<LineDto?>> CreateByExcel(List<LineExcelDto> model, long userCreate)
        {
            var returnData = new ResponseModel<LineDto?>();

            var jsonLotList = JsonConvert.SerializeObject(model);

            string proc = "Usp_Line_CreateByExcel";
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

        public async Task<ResponseModel<IEnumerable<LineDto>?>> GetForSelect()
        {
            var returnData = new ResponseModel<IEnumerable<LineDto>?>();
            var proc = $"Usp_Line_GetForSelect";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<LineDto>(proc);

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

        public async Task<ResponseModel<IEnumerable<LineDto>?>> GetListPrintQR(List<long>? listQR)
        {
            var returnData = new ResponseModel<IEnumerable<LineDto>?>();
            var proc = $"Usp_Line_GetPrint";
            var param = new DynamicParameters();
            param.Add("@listQR", ParameterTvp.GetTableValuedParameter_BigInt(listQR));
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<LineDto>(proc, param);
            returnData.Data = data;
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }
    }
}
