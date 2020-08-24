using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SpiderDatabase
{
    public class DataAccess : IDataAccess
    {

        private string _connectionString;
        public DataAccess(string connection)
        {
            _connectionString = connection;
        }
        
        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<T>(sql, parameters, null, null, CommandType.StoredProcedure);

                return rows.ToList();
            }
        }

        public Task SaveData<T>(string sql, T parameters)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                return connection.ExecuteAsync(sql, parameters, null, null, CommandType.StoredProcedure);
            }
        }
    }
}
