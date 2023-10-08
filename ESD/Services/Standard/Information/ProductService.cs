using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services.Base;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.Common.Standard.Information
{
    public interface IProductService
    {
        Task<ResponseModel<IEnumerable<ProductDto>?>> GetAll(ProductDto pageInfo);
        Task<string> Create(ProductDto model);
        Task<ResponseModel<ProductDto?>> GetById(long id);
        Task<string> Modify(ProductDto model);
        Task<string> Delete(ProductDto model);
        Task<ResponseModel<IEnumerable<ProductDto>?>> GetForSelect();
    }
    [ScopedRegistration]
    public class ProductService : IProductService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public ProductService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<ProductDto>?>> GetAll(ProductDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<ProductDto>?>();
                string proc = "Usp_Product_GetAll"; var param = new DynamicParameters();
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@Description", model.Description);
                param.Add("@Model", model.ModelId);
                param.Add("@ProductType", model.ProductType);
                param.Add("@showDelete", model.showDelete);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ProductDto>(proc, param);
                returnData.Data = data;
                returnData.TotalRow = param.Get<int>("totalRow");
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<string> Create(ProductDto model)
        {
            try
            {
                string proc = "Usp_Product_Create";
                var param = new DynamicParameters();
                param.Add("@ProductId", model.ProductId);
                param.Add("@ProductCode", model.ProductCode?.Trim().ToUpper());
                param.Add("@ProductName", model.ProductName);
                param.Add("@ModelId", model.ModelId);
                param.Add("@ProjectName", model.ProjectName);
                param.Add("@SSVersion", model.SSVersion);
                param.Add("@ProductType", model.ProductType);
                param.Add("@Vendor", model.Vendor);
                param.Add("@PackingAmount", model.PackingAmount);
                param.Add("@ExpiryMonth", model.ExpiryMonth);
                param.Add("@Temperature", model.Temperature);
                param.Add("@Stamps", model.Stamps);
                param.Add("@RemarkBuyer", model.RemarkBuyer);
                param.Add("@Description", model.Description);
                param.Add("@createdBy", model.createdBy);
                param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

                return await _sqlDataAccess.SaveDataUsingStoredProcedure<string>(proc, param);
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public async Task<ResponseModel<ProductDto?>> GetById(long id)
        {
            try
            {
                var returnData = new ResponseModel<ProductDto?>();
                string proc = "Usp_Product_GetById";
                var param = new DynamicParameters();
                param.Add("@ProductId", id);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ProductDto?>(proc, param);
                returnData.Data = data.FirstOrDefault();
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public async Task<string> Modify(ProductDto model)
        {
            try
            {
                string proc = "Usp_Product_Modify";
                var param = new DynamicParameters();
                param.Add("@ProductId", model.ProductId);
                param.Add("@ProductCode", model.ProductCode?.Trim().ToUpper());
                param.Add("@ProductName", model.ProductName);
                param.Add("@ModelId", model.ModelId);
                param.Add("@ProjectName", model.ProjectName);
                param.Add("@SSVersion", model.SSVersion);
                param.Add("@ProductType", model.ProductType);
                param.Add("@Vendor", model.Vendor);
                param.Add("@PackingAmount", model.PackingAmount);
                param.Add("@ExpiryMonth", model.ExpiryMonth);
                param.Add("@Temperature", model.Temperature);
                param.Add("@Stamps", model.Stamps);
                param.Add("@RemarkBuyer", model.RemarkBuyer);
                param.Add("@Description", model.Description);
                param.Add("@modifiedBy", model.modifiedBy);
                param.Add("@row_version", model.row_version);
                param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

                return await _sqlDataAccess.SaveDataUsingStoredProcedure<string>(proc, param);
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public async Task<string> Delete(ProductDto model)
        {
            string proc = "Usp_Product_Delete";
            var param = new DynamicParameters();
            param.Add("@ProductId", model.ProductId);
            param.Add("@row_version", model.row_version);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<ResponseModel<IEnumerable<ProductDto>?>> GetForSelect()
        {
            var returnData = new ResponseModel<IEnumerable<ProductDto>?>();
            var proc = $"Usp_Product_GetForSelect";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ProductDto>(proc);

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
    }
}
