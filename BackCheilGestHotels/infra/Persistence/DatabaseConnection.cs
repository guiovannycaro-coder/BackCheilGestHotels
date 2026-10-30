using Npgsql;

namespace BackCheilGestHotels.infra.Persistence
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;


        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public NpgsqlConnection GetConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
    }
