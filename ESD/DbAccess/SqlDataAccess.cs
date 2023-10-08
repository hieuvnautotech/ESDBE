using Dapper;
using System.Data;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.DbAccess
{
    public interface ISqlDataAccess
    {

        #region Stored Procedure
        Task<IEnumerable<T>> LoadDataUsingStoredProcedure<T>(string storedProcedure, DynamicParameters? parameters = null);
        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> LoadMultiDataSetUsingStoredProcedure<T1, T2>(string storedProcedure, DynamicParameters? parameters = null);
        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> LoadMultiDataSetUsingStoredProcedure<T1, T2, T3>(string storedProcedure, DynamicParameters? parameters = null);
        Task<string> SaveDataUsingStoredProcedure<T>(string storedProcedure, DynamicParameters parameters);
        #endregion

        #region Raw Query
        Task<IEnumerable<T>> LoadDataUsingRawQuery<T>(string rawQuery, DynamicParameters? parameters = null);
        Task<T> LoadDataFirstOrDefaultAsync<T>(string rawQuery, DynamicParameters? parameters = null);
        Task<T> LoadDataExecuteScalarAsync<T>(string rawQuery, DynamicParameters? parameters = null);
        #endregion
        #region QueryEDI
        Task<IEnumerable<T>> LoadDataUsingStoredProcedureEDI<T>(string storedProcedure, DynamicParameters? parameters = null);
        Task<IEnumerable<T>> LoadDataUsingRawQueryEDI<T>(string rawQuery, DynamicParameters? parameters = null);
        #endregion
    }

    [SingletonRegistration]
    public class SqlDataAccess : ISqlDataAccess
    {
        //private readonly IConfiguration _configuration;
        private static readonly string connectionString = ConnectionString.CONNECTIONSTRING;
        private static readonly string connectionStringEDI = ConnectionString.CONNECTIONSTRINGEDI;

        //public SqlDataAccess(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        #region Stored Procedure
        //Used for getting gatas (select query) from database
        public async Task<IEnumerable<T>> LoadDataUsingStoredProcedure<T>(string storedProcedure, DynamicParameters? parameters = null)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);

            return await dbConnection.QueryAsync<T>(storedProcedure, parameters, transaction: null, commandTimeout: 20, commandType: CommandType.StoredProcedure);
        }

        //Used for saving datas (insert/update/delete) into database
        public async Task<string> SaveDataUsingStoredProcedure<T>(string storedProcedure, DynamicParameters parameters)
        {
            string result = string.Empty;
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using IDbTransaction tran = dbConnection.BeginTransaction();
                try
                {
                    await dbConnection.ExecuteAsync(storedProcedure, parameters, transaction: tran, commandTimeout: 20, commandType: CommandType.StoredProcedure);
                    tran.Commit();
                    result = parameters.Get<string?>("@output") ?? string.Empty;
                }
                catch (Exception e)
                {
                    tran.Rollback();
                }
            }

            return result;
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> LoadMultiDataSetUsingStoredProcedure<T1, T2>(string storedProcedure, DynamicParameters? parameters = null)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);

            using var multi = await dbConnection.QueryMultipleAsync(storedProcedure, parameters, transaction: null, commandTimeout: 20, commandType: CommandType.StoredProcedure);
            var result1 = await multi.ReadAsync<T1>();
            var result2 = await multi.ReadAsync<T2>();
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(result1, result2);
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> LoadMultiDataSetUsingStoredProcedure<T1, T2, T3>(string storedProcedure, DynamicParameters? parameters = null)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);

            using var multi = await dbConnection.QueryMultipleAsync(storedProcedure, parameters, transaction: null, commandTimeout: 20, commandType: CommandType.StoredProcedure);
            var result1 = await multi.ReadAsync<T1>();
            var result2 = await multi.ReadAsync<T2>();
            var result3 = await multi.ReadAsync<T3>();
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(result1, result2, result3);
        }

        #endregion

        #region Raw Query
        //Used for getting gatas (select query) from database
        public async Task<IEnumerable<T>> LoadDataUsingRawQuery<T>(string rawQuery, DynamicParameters? parameters = null)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);

            return await dbConnection.QueryAsync<T>(rawQuery, parameters, transaction: null, commandTimeout: 20, commandType: CommandType.Text);
        }

        public async Task<T> LoadDataFirstOrDefaultAsync<T>(string rawQuery, DynamicParameters? parameters = null)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);

            return await dbConnection.QueryFirstOrDefaultAsync<T>(rawQuery, parameters, transaction: null, commandTimeout: 20, commandType: CommandType.Text);
        }

        public async Task<T> LoadDataExecuteScalarAsync<T>(string rawQuery, DynamicParameters? parameters = null)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);

            return await dbConnection.ExecuteScalarAsync<T>(rawQuery, parameters, transaction: null, commandTimeout: 20, commandType: CommandType.Text);
        }
        #endregion
        #region QueryEDI
        public async Task<IEnumerable<T>> LoadDataUsingStoredProcedureEDI<T>(string storedProcedure, DynamicParameters? parameters = null)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionStringEDI);

            return await dbConnection.QueryAsync<T>(storedProcedure, parameters, transaction: null, commandTimeout: 20, commandType: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<T>> LoadDataUsingRawQueryEDI<T>(string rawQuery, DynamicParameters? parameters = null)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionStringEDI);

            return await dbConnection.QueryAsync<T>(rawQuery, parameters, transaction: null, commandTimeout: 20, commandType: CommandType.Text);
        }
        #endregion
    }
}
