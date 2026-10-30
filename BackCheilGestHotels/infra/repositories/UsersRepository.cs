using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;
using BackCheilGestHotels.infra.Persistence;
using Npgsql;
using System.Diagnostics;

namespace BackCheilGestHotels.infra.repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DatabaseConnection _database;

        public UsersRepository(DatabaseConnection database)
        {
            _database = database;
        }


        public List<UsersDto> GetUsuarios()
        {
            try
            {
                const string sql = """
            SELECT u.usuid,
                   u.usunames,
                   u.usuaddress,
                   u.usuphone,
                   u.usuemail,
                   u.usudocumentid,
                   u.usutdocid,
                   d.tipid,
                   d.tipdescription,
                   d.tipisactive,
                   u.usurolid,
                   u.usuisactive
            FROM public.geshusers u
            LEFT JOIN public.documenttype d
                ON u.usutdocid = d.tipid
            ORDER BY u.usuid;
            """;

                var usuarios = new List<UsersDto>();

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DocumentTypeDto? documentType = null;

                    if (!reader.IsDBNull(7))
                    {
                        documentType = new DocumentTypeDto
                        {
                            TipId = reader.GetInt32(7),

                            TipDescription = reader.IsDBNull(8)
                                ? null!
                                : reader.GetString(8),

                            TipIsActive = reader.IsDBNull(9)
                                ? null
                                : reader.GetString(9)
                        };
                    }

                    usuarios.Add(new UsersDto
                    {
                        UsuId = reader.GetInt32(0),

                        UsuNames = reader.IsDBNull(1)
                            ? null!
                            : reader.GetString(1),

                        UsuAddress = reader.IsDBNull(2)
                            ? null!
                            : reader.GetString(2),

                        UsuPhone = reader.IsDBNull(3)
                            ? null!
                            : reader.GetString(3),

                        UsuEmail = reader.IsDBNull(4)
                            ? null!
                            : reader.GetString(4),

                        UsuDocumentId = reader.IsDBNull(5)
                            ? null!
                            : reader.GetString(5),

                        UsuTdocId = reader.IsDBNull(6)
                            ? null
                            : reader.GetInt32(6),

                        DocumentType = documentType,

                        usuRolId = reader.IsDBNull(10)
                            ? null
                            : reader.GetInt32(10),

                        UsuIsActive = !reader.IsDBNull(11)
                            && reader.GetBoolean(11)
                    });
                }

                return usuarios;
            }
            catch (NpgsqlException ex)
            {
                System.Diagnostics.Trace.TraceError(
                    $"Database error while retrieving users: {ex}");

                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving users from database.",
                    ex);
            }
        }


        public List<UsersDto> GetUsuariosById(int dato)
        {
            try
            {
                const string sql = """
            SELECT u.usuid,
                   u.usunames,
                   u.usuaddress,
                   u.usuphone,
                   u.usuemail,
                   u.usudocumentid,
                   u.usupassword,
                   u.usutdocid,
                   u.usurolid,
                   d.tipid,
                   d.tipdescription,
                   d.tipisactive,
                   u.usuisactive
            FROM geshusers u
            LEFT JOIN documenttype d
                ON u.usutdocid = d.tipid
            WHERE u.usuid = @id
            ORDER BY u.usuid;
            """;

                var usuarios = new List<UsersDto>();

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("@id", dato);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    usuarios.Add(MapUsuario(reader));
                }

                return usuarios;
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving user by ID.",
                    ex);
            }
        }

        public List<UsersDto> GetUsuariosByName(string dato)
        {
            try
            {
                const string sql = """
            SELECT u.usuid,
                   u.usunames,
                   u.usuaddress,
                   u.usuphone,
                   u.usuemail,
                   u.usudocumentid,
                   u.usupassword,
                   u.usutdocid,
                   u.usurolid,
                   d.tipid,
                   d.tipdescription,
                   d.tipisactive,
                   u.usuisactive
            FROM geshusers u
            LEFT JOIN documenttype d
                ON u.usutdocid = d.tipid
            WHERE u.usunames ILIKE @name
            ORDER BY u.usuid;
            """;

                var usuarios = new List<UsersDto>();

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("@name", $"%{dato}%");

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    usuarios.Add(MapUsuario(reader));
                }

                return usuarios;
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving user by Name.",
                    ex);
            }
        }








        public string InsUsuarios(UsersDto user)
        {
            try
            {
                const string sql = """
            INSERT INTO geshusers
            (
                usunames,
                usuaddress,
                usuphone,
                usuemail,
                usudocumentid,
                usupassword,
                usutdocid,
                usurolid,
                usuisactive
            )
            VALUES
            (
                @names,
                @address,
                @phone,
                @email,
                @documentId,
                @password,
                @documentTypeId,
                @usuRolId,
                @isActive
            )
            RETURNING usuid;
            """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("names", (object?)user.UsuNames ?? DBNull.Value);
                command.Parameters.AddWithValue("address", (object?)user.UsuAddress ?? DBNull.Value);
                command.Parameters.AddWithValue("phone", (object?)user.UsuPhone ?? DBNull.Value);
                command.Parameters.AddWithValue("email", (object?)user.UsuEmail ?? DBNull.Value);
                command.Parameters.AddWithValue("documentId", (object?)user.UsuDocumentId ?? DBNull.Value);
                command.Parameters.AddWithValue("password", (object?)user.UsuPassword ?? DBNull.Value);
                command.Parameters.AddWithValue("documentTypeId", (object?)user.UsuTdocId ?? DBNull.Value);
                command.Parameters.AddWithValue("usuRolId", (object?)user.usuRolId ?? DBNull.Value);
                command.Parameters.AddWithValue("isActive", user.UsuIsActive);

                int id = Convert.ToInt32(command.ExecuteScalar());

                Console.WriteLine("========== USUARIO CREADO ==========");
                Console.WriteLine($"ID del usuario: {id}");
                Console.WriteLine($"ID del rol asignado: {user.usuRolId}");
                Console.WriteLine("=====================================");

                
                return $"User successfully created. ID: {id}";
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error inserting user.", ex);
            }
        }
        
        public string UpdUsuarios(UsersDto user)
        {
            try
            {
                const string sql = """
            UPDATE geshusers
            SET usunames = @names,
                usuaddress = @address,
                usuphone = @phone,
                usuemail = @email,
                usudocumentid = @documentId,
                usurolid = @usuRolId,
                usupassword = COALESCE(@password, usupassword),
                usutdocid = @documentTypeId,
                usuisactive = @isActive
            WHERE usuid = @id
            RETURNING usuid;
            """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("id", user.UsuId);
                command.Parameters.AddWithValue("names", (object?)user.UsuNames ?? DBNull.Value);
                command.Parameters.AddWithValue("address", (object?)user.UsuAddress ?? DBNull.Value);
                command.Parameters.AddWithValue("phone", (object?)user.UsuPhone ?? DBNull.Value);
                command.Parameters.AddWithValue("email", (object?)user.UsuEmail ?? DBNull.Value);
                command.Parameters.AddWithValue("documentId", (object?)user.UsuDocumentId ?? DBNull.Value);
                command.Parameters.AddWithValue("password", (object?)user.UsuPassword ?? DBNull.Value);
                command.Parameters.AddWithValue("documentTypeId", (object?)user.UsuTdocId ?? DBNull.Value);
                command.Parameters.AddWithValue("usuRolId", (object?)user.usuRolId ?? DBNull.Value);
                command.Parameters.AddWithValue("isActive", user.UsuIsActive);

                object? result = command.ExecuteScalar();

                Console.WriteLine("========== USUARIO CREADO ==========");
                Console.WriteLine($"ID del usuario: {user.UsuId}");
                Console.WriteLine($"ID del rol asignado: {user.usuRolId}");
                Console.WriteLine("=====================================");

                    return result != null
                    ? $"User with ID {user.UsuId} successfully updated."
                    : $"User with ID {user.UsuId} was not found.";
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error updating user.", ex);
            }
        }

        public string DropUserById(int dato)
        {
            try
            {
                const string sql = """
            DELETE FROM geshusers
            WHERE usuid = @id
            RETURNING usuid;
            """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("id", dato);

                object? result = command.ExecuteScalar();

                return result != null
                    ? $"User with ID {dato} successfully deleted."
                    : $"User with ID {dato} was not found.";
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error deleting user.", ex);
            }
        }

        public RolesDto? GetRolUser(string dato)
        {
            try
            {
                const string sql = """
            SELECT r.rolid,
                   r.rolname,
                   r.rolobservation,
                   r.rolisactive
            FROM public.geshusers u
            INNER JOIN public.geshroles r
                ON u.usurolid = r.rolid
            WHERE u.usuemail = @dato;
            """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("dato", dato);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new RolesDto
                    {
                        RolId = reader.GetInt32(0),
                        RolName = reader.IsDBNull(1)
                            ? null!
                            : reader.GetString(1),
                        RolObservation = reader.IsDBNull(2)
                            ? null!
                            : reader.GetString(2),
                        RolIsActive = reader.IsDBNull(3)
                            ? null
                            : reader.GetBoolean(3)
                    };
                }

                return null;
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving the role assigned to the user.",
                    ex
                );
            }
        }


        private static UsersDto MapUsuario(NpgsqlDataReader reader)
        {
            DocumentTypeDto? documentType = null;

            if (!reader.IsDBNull(8))
            {
                documentType = new DocumentTypeDto
                {
                    TipId = reader.GetInt32(8),
                    TipDescription = reader.IsDBNull(9)
                        ? null!
                        : reader.GetString(9),
                    TipIsActive = reader.IsDBNull(10)
                        ? null!
                        : reader.GetString(10)
                };
            }

            return new UsersDto
            {
                UsuId = reader.GetInt32(0),
                UsuNames = reader.IsDBNull(1) ? null : reader.GetString(1),
                UsuAddress = reader.IsDBNull(2) ? null : reader.GetString(2),
                UsuPhone = reader.IsDBNull(3) ? null : reader.GetString(3),
                UsuEmail = reader.IsDBNull(4) ? null : reader.GetString(4),
                UsuDocumentId = reader.IsDBNull(5) ? null : reader.GetString(5),

                // No devolver la contraseña en las consultas de lectura.
                UsuPassword = reader.IsDBNull(6)
        ? null : reader.GetString(6),

                UsuTdocId = reader.IsDBNull(7)
                    ? null
                    : reader.GetInt32(7),

                usuRolId = reader.IsDBNull(8)
                    ? null
                    : reader.GetInt32(8),

                DocumentType = documentType,
                UsuIsActive = reader.GetBoolean(11)
            };
        }
    } 
}
