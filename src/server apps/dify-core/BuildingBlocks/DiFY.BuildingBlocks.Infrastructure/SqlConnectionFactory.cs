using System;
using System.Data;
using System.Data.SqlClient;
using DiFY.BuildingBlocks.Application.Data;

namespace DiFY.BuildingBlocks.Infrastructure
{
    public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        private readonly string _connectionString;
        
        private IDbConnection _connection;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetOpenConnection()
        {
            if (_connection is { State: ConnectionState.Open }) return _connection;
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
            return _connection;
        }

        public IDbConnection CreateNewConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public string GetConnectionString() => _connectionString;

        public void Dispose()
        {
            if (_connection is { State: ConnectionState.Open })
            {
                _connection.Dispose();
            }
        }
    }
}