using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;
using BackCheilGestHotels.infra.Persistence;
using Npgsql;

namespace BackCheilGestHotels.infra.repositories
{
    public class UsersSeguridadRepository : IUsersSeguridadRepository
    {

        private readonly DatabaseConnection _database;

        public UsersSeguridadRepository(DatabaseConnection database)
        {
            _database = database;
        }

        public bool authenticate(LoginDto datos)
        {
            try
            {
                const string sql = """
            SELECT COUNT(*)
            FROM geshusers
            WHERE usuemail = @email
              AND usupassword = @password
              AND usuisactive = TRUE;
            """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("email", datos.email);
                command.Parameters.AddWithValue("password", datos.password);

                int resultado = Convert.ToInt32(command.ExecuteScalar());

                return resultado > 0;
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error authenticating user.",
                    ex);
            }
        }

        public string getProfile(string email)
        {
            try
            {
                const string sql = """
            SELECT usuemail
            FROM public.geshusers
            WHERE usuemail = @email
              AND usuisactive = TRUE;
            """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("email", email);

                object? resultado = command.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    return "User not found or inactive.";
                }

                return resultado.ToString()!;
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving user profile.",
                    ex);
            }
        }

        public string rememberUser(LoginDto datos)
        {
            try
            {
                const string sql = """
            UPDATE geshusers
            SET usupassword = @password
            WHERE usuemail = @email
              AND usuisactive = TRUE
            RETURNING usuid;
            """;

                // Generar una nueva contraseña temporal
                string newPassword = Convert.ToBase64String(
                    System.Security.Cryptography.RandomNumberGenerator.GetBytes(12));

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue(
                    "email", datos.email ?? string.Empty);

                command.Parameters.AddWithValue(
                    "password", datos.password ?? string.Empty);

                object? result = command.ExecuteScalar();

                if (result == null)
                {
                    return "User not found or inactive.";
                }

                return $"Password updated successfully. New password: {newPassword}";
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error updating user password.",
                    ex);
            }
        }
    }
}
