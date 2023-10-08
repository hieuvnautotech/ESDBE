using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Helpers;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.WMS.Material
{
    public interface IMaterialSplitMergeService
    {
        Task<ResponseModel<MaterialLabelDto?>> GetMaterial(string? MaterialLotCode);
        Task<ResponseModel<MaterialLabelDto?>> SplitLot(string? MaterialLotId, string? MaterialLotCode, string? MaterialLotCode2, int? Length, long NewMaterialLotId, long UserCreate);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetListPrintQR(List<long>? listQR);
        Task<ResponseModel<IEnumerable<MaterialReceivingDto>?>> GetAllSplit(MaterialReceivingDto model);
        Task<ResponseModel<IEnumerable<MaterialSplitDetailDto>?>> GetSplitDetail(MaterialSplitDetailDto model);
        Task<ResponseModel<MaterialSplitDetailDto?>> DeleteSplitDetail(MaterialSplitDetailDto model);
        Task<ResponseModel<MaterialLabelDto?>> MergeLot(string? MaterialLotId1, string? MaterialLotId2, long UserCreate);
    }
    [ScopedRegistration]
    public class MaterialSplitMergeService : IMaterialSplitMergeService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        public MaterialSplitMergeService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<MaterialLabelDto?>> GetMaterial(string? MaterialLotCode)
        {
            var returnData = new ResponseModel<MaterialLabelDto?>();
            if (MaterialLotCode == null || MaterialLotCode == null)
            {
                returnData.ResponseMessage = StaticReturnValue.FIELD_REQUIRED;
                returnData.HttpResponseCode = 400;
                return returnData;
            }

            var proc = $"Usp_MaterialSplit_GetLabel";
            var param = new DynamicParameters();
            param.Add("@MaterialLotCode", MaterialLotCode);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure
            param.Add("@status", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialLabelDto>(proc, param);
            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            returnData.ResponseMessage = result;

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData.Data = data.FirstOrDefault();
                    //returnData.ResponseMessage = StaticReturnValue.SUCCESS;
                    break;
                default:
                    returnData.ResponseMessage = result;
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return returnData;

        }
        public async Task<ResponseModel<MaterialLabelDto?>> GetMaterialById(long? MaterialLotId)
        {
            var returnData = new ResponseModel<MaterialLabelDto?>();

            if (MaterialLotId == null)
            {
                returnData.ResponseMessage = StaticReturnValue.FIELD_REQUIRED;
                returnData.HttpResponseCode = 400;
                return returnData;
            }

            var proc = $"Usp_MaterialSplit_GetLabelById";
            var param = new DynamicParameters();
            param.Add("@MaterialLotId", MaterialLotId);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialLabelDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            returnData.ResponseMessage = StaticReturnValue.SUCCESS;
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }

        public async Task<ResponseModel<MaterialLabelDto?>> SplitLot(string? MaterialLotId, string? MaterialLotCode, string? MaterialLotCode2, int? Length, long NewMaterialLotId, long UserCreate)
        {
            var returnData = new ResponseModel<MaterialLabelDto?>();

            if (MaterialLotId == null)
            {
                returnData.ResponseMessage = StaticReturnValue.FIELD_REQUIRED;
                returnData.HttpResponseCode = 400;
                return returnData;
            }

            long Id;
            if (!long.TryParse(MaterialLotId, out Id))
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 400;
                return returnData;
            }
            try
            {
                string proc = $"Usp_MaterialSplit_Create";
                var param = new DynamicParameters();
                param.Add("@MaterialLotId", MaterialLotId);
                param.Add("@MaterialLotCode", MaterialLotCode);
                param.Add("@NewMaterialLotId", NewMaterialLotId);
                param.Add("@Length", Length);
                param.Add("@createdBy", UserCreate);
                param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

                var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
                returnData.ResponseMessage = result;
                switch (result)
                {
                    case StaticReturnValue.SYSTEM_ERROR:
                        returnData.HttpResponseCode = 500;
                        break;
                    case StaticReturnValue.SUCCESS:
                        returnData = await GetMaterialById(NewMaterialLotId);
                        break;
                    default:
                        returnData.ResponseMessage = result;
                        returnData.HttpResponseCode = 400;
                        break;
                }

                return returnData;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetListPrintQR(List<long>? listQR)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_MaterialLot_Print";
            var param = new DynamicParameters();
            param.Add("@listQR", ParameterTvp.GetTableValuedParameter_BigInt(listQR));
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
            returnData.Data = data;
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }


        public async Task<ResponseModel<IEnumerable<MaterialReceivingDto>?>> GetAllSplit(MaterialReceivingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialReceivingDto>?>();
                string proc = model.Type == "S" ? "Usp_MaterialSplit_GetAll" : "Usp_MaterialMerge_GetAll"; 
                var param = new DynamicParameters();
                param.Add("@MaterialCode", model.MaterialCode);
                param.Add("@MaterialName", model.MaterialName);
                param.Add("@ReceivedDate", model.ReceivedDate);
                param.Add("@Type", model.Type);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialReceivingDto>(proc, param);
                returnData.Data = data;
                returnData.TotalRow = param.Get<int>("totalRow");
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<MaterialSplitDetailDto>?>> GetSplitDetail(MaterialSplitDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialSplitDetailDto>?>();
                string proc = model.Type=="S" ? "Usp_MaterialSplit_GetDetail": "Usp_MaterialMerge_GetDetail"; 
                var param = new DynamicParameters();
                param.Add("@MaterialLotId", model.MaterialLotId);
                param.Add("@Type", model.Type);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialSplitDetailDto>(proc, param);
                returnData.Data = data;
                returnData.TotalRow = param.Get<int>("totalRow");
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<MaterialSplitDetailDto?>> DeleteSplitDetail(MaterialSplitDetailDto model)
        {
            string proc = "Usp_MaterialSplit_DeleteDetail";
            var param = new DynamicParameters();
            param.Add("@MaterialLotChildId", model.MaterialLotChildId);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            var returnData = new ResponseModel<MaterialSplitDetailDto?>();
            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return returnData;
        }

        public async Task<ResponseModel<MaterialLabelDto?>> MergeLot(string? MaterialLotId1, string? MaterialLotId2, long UserCreate)
        {
            var returnData = new ResponseModel<MaterialLabelDto?>();

            if (MaterialLotId1 == null || MaterialLotId2 == null)
            {
                returnData.ResponseMessage = StaticReturnValue.FIELD_REQUIRED;
                returnData.HttpResponseCode = 400;
                return returnData;
            }

            long Id;
            if (!long.TryParse(MaterialLotId1, out Id) || !long.TryParse(MaterialLotId2, out Id))
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 400;
                return returnData;
            }

            string proc = "Usp_MaterialMerge_Create";
            var param = new DynamicParameters();
            param.Add("@MaterialLotId1", MaterialLotId1);
            param.Add("@MaterialLotId2", MaterialLotId2);
            param.Add("@createdBy", UserCreate);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure
            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await GetMaterialById(long.Parse(MaterialLotId1));
                    break;
                default:
                    returnData.ResponseMessage = result;
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return returnData;
        }
    }
}
