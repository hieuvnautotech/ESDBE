﻿using Dapper;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Data;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using Newtonsoft.Json;

namespace ESD.Services.Standard.Information
{
    public interface ISupplierService
    {
        Task<ResponseModel<IEnumerable<SupplierDto>?>> GetAll(SupplierDto model);
        Task<ResponseModel<IEnumerable<SupplierDto>?>> GetActive(SupplierDto model);
        Task<ResponseModel<IEnumerable<SupplierDto>?>> Get(SupplierDto model);
        Task<ResponseModel<SupplierDto?>> GetById(long supplierId);
        Task<string> Create(SupplierDto model);
        Task<string> Modify(SupplierDto model);
        Task<string> DeleteReuse(SupplierDto model);
        Task<ResponseModel<IEnumerable<SupplierDto>?>> GetForSelect();
        Task<ResponseModel<IEnumerable<SupplierDto>?>> GetByMaterial(long MaterialId);
        Task<ResponseModel<SupplierDto?>> CreateByExcel(List<SupplierExcelDto> model, long userCreate);

    }

    [ScopedRegistration]
    public class SupplierService : ISupplierService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public SupplierService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<string> Create(SupplierDto model)
        {
            string proc = "Usp_Supplier_Create";
            var param = new DynamicParameters();
            param.Add("@SupplierId", model.SupplierId);
            param.Add("@SupplierCode", model.SupplierCode);
            param.Add("@SupplierType", model.SupplierType);
            param.Add("@SupplierName", model.SupplierName);
            param.Add("@Description", model.Description);
            param.Add("@Website", model.Website);
            param.Add("@PhoneNumber", model.PhoneNumber);
            param.Add("@Email", model.Email);
            param.Add("@Fax", model.Fax);
            param.Add("@Tax", model.Tax);
            param.Add("@Address", model.Address);
            param.Add("@DateSignContract", model.DateSignContract);
            param.Add("@DebtDate", model.DebtDate);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            //throw new NotImplementedException();
        }

        public async Task<string> DeleteReuse(SupplierDto model)
        {
            string proc = "Usp_Supplier_DeleteReuse";
            var param = new DynamicParameters();
            param.Add("@SupplierId", model.SupplierId);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@isActived", model.isActived);
            param.Add("@row_version", model.row_version);

            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<ResponseModel<IEnumerable<SupplierDto>?>> Get(SupplierDto model)
        {
            var returnData = new ResponseModel<IEnumerable<SupplierDto>?>();
            var proc = $"Usp_Supplier_Get";
            var param = new DynamicParameters();
            param.Add("@page", model.page);
            param.Add("@pageSize", model.pageSize);
            param.Add("@SupplierCode", model.SupplierCode);
            param.Add("@SupplierName", model.SupplierName);
            param.Add("@isActived", model.isActived);
            param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SupplierDto>(proc, param);

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

        public async Task<ResponseModel<IEnumerable<SupplierDto>?>> GetAll(SupplierDto model)
        {
            var returnData = new ResponseModel<IEnumerable<SupplierDto>?>();
            var proc = $"Usp_Supplier_GetAll";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SupplierDto>(proc);

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

        public async Task<ResponseModel<SupplierDto?>> GetById(long supplierId)
        {
            var returnData = new ResponseModel<SupplierDto?>();
            var proc = $"Usp_Supplier_GetById";
            var param = new DynamicParameters();
            param.Add("@SupplierId", supplierId);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SupplierDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }

        public async Task<ResponseModel<IEnumerable<SupplierDto>?>> GetActive(SupplierDto model)
        {
            var returnData = new ResponseModel<IEnumerable<SupplierDto>?>();
            var proc = $"Usp_Supplier_GetActive";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SupplierDto>(proc);

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

        public async Task<string> Modify(SupplierDto model)
        {
            string proc = "Usp_Supplier_Modify";
            var param = new DynamicParameters();
            param.Add("@SupplierId", model.SupplierId);
            param.Add("@SupplierCode", model.SupplierCode);
            param.Add("@SupplierType", model.SupplierType);
            param.Add("@SupplierName", model.SupplierName);
            param.Add("@Description", model.Description);
            param.Add("@Website", model.Website);
            param.Add("@PhoneNumber", model.PhoneNumber);
            param.Add("@Email", model.Email);
            param.Add("@Fax", model.Fax);
            param.Add("@Tax", model.Tax);
            param.Add("@Address", model.Address);
            param.Add("@DateSignContract", model.DateSignContract);
            param.Add("@DebtDate", model.DebtDate);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<ResponseModel<IEnumerable<SupplierDto>?>> GetForSelect()
        {
            var returnData = new ResponseModel<IEnumerable<SupplierDto>?>();
            var proc = $"Usp_Supplier_GetForSelect";

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SupplierDto>(proc);
            returnData.Data = data;
            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }

            return returnData;
        }

        public async Task<ResponseModel<IEnumerable<SupplierDto>?>> GetByMaterial(long MaterialId)
        {
            var returnData = new ResponseModel<IEnumerable<SupplierDto>?>();
            var proc = $"Usp_Supplier_GetByMaterial";
            var param = new DynamicParameters();
            param.Add("@MaterialId", MaterialId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SupplierDto>(proc, param);
            returnData.Data = data;
            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }

            return returnData;
        }
        public async Task<ResponseModel<SupplierDto?>> CreateByExcel(List<SupplierExcelDto> model, long userCreate)
        {
            var returnData = new ResponseModel<SupplierDto?>();

            var jsonLotList = JsonConvert.SerializeObject(model);

            string proc = "Usp_Supplier_CreateByExcel";
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
