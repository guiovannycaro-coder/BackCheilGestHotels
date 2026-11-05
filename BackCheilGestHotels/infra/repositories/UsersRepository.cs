
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

        // ==========================================
        // GET ALL USERS
        // ==========================================

        public List<UsersDto> GetUsuarios()
        {
            try
            {
                const string sql = """
                    SELECT
                        u.usuid,
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
                        r.rolid,
                        r.rolname,
                        r.rolobservation,
                        r.rolisactive,
                        u.usuisactive

                    FROM public.geshusers u

                    LEFT JOIN public.documenttype d
                        ON u.usutdocid = d.tipid

                    LEFT JOIN public.geshroles r
                        ON u.usurolid = r.rolid

                    ORDER BY u.usuid;
                    """;

                var usuarios = new List<UsersDto>();

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    usuarios.Add(MapUsuario(reader));
                }

                return usuarios;
            }
            catch (NpgsqlException ex)
            {
                Trace.TraceError(
                    $"Database error while retrieving users: {ex}");

                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving users from database.",
                    ex);
            }
        }

        // ==========================================
        // GET USER BY ID
        // ==========================================

        public List<UsersDto> GetUsuariosById(int dato)
        {
            try
            {
                const string sql = """
                    SELECT
                        u.usuid,
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
                        r.rolid,
                        r.rolname,
                        r.rolobservation,
                        r.rolisactive,
                        u.usuisactive

                    FROM public.geshusers u

                    LEFT JOIN public.documenttype d
                        ON u.usutdocid = d.tipid

                    LEFT JOIN public.geshroles r
                        ON u.usurolid = r.rolid

                    WHERE u.usuid = @id
                    ORDER BY u.usuid;
                    """;

                var usuarios = new List<UsersDto>();

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("id", dato);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    usuarios.Add(MapUsuario(reader));
                }

                return usuarios;
            }
            catch (NpgsqlException ex)
            {
                Trace.TraceError(
                    $"Database error retrieving user by ID: {ex}");

                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving user by ID.",
                    ex);
            }
        }

        // ==========================================
        // GET USER BY NAME
        // ==========================================

        public List<UsersDto> GetUsuariosByName(string dato)
        {
            try
            {
                const string sql = """
                    SELECT
                        u.usuid,
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
                        r.rolid,
                        r.rolname,
                        r.rolobservation,
                        r.rolisactive,
                        u.usuisactive

                    FROM public.geshusers u

                    LEFT JOIN public.documenttype d
                        ON u.usutdocid = d.tipid

                    LEFT JOIN public.geshroles r
                        ON u.usurolid = r.rolid

                    WHERE u.usunames ILIKE @name
                    ORDER BY u.usuid;
                    """;

                var usuarios = new List<UsersDto>();

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("name", $"%{dato}%");

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    usuarios.Add(MapUsuario(reader));
                }

                return usuarios;
            }
            catch (NpgsqlException ex)
            {
                Trace.TraceError(
                    $"Database error retrieving user by name: {ex}");

                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving user by Name.",
                    ex);
            }
        }

        // ==========================================
        // INSERT USER
        // ==========================================

        public string InsUsuarios(UsersDto user)
        {
            try
            {
                const string sql = """
                    INSERT INTO public.geshusers
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

                command.Parameters.AddWithValue(
                    "names", (object?)user.UsuNames ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "address", (object?)user.UsuAddress ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "phone", (object?)user.UsuPhone ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "email", (object?)user.UsuEmail ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "documentId", (object?)user.UsuDocumentId ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "password", (object?)user.UsuPassword ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "documentTypeId", (object?)user.UsuTdocId ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "usuRolId", (object?)user.usuRolId ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "isActive", user.UsuIsActive);

                int id = Convert.ToInt32(command.ExecuteScalar());

                Trace.TraceInformation(
                    $"User created successfully. ID: {id}");

                return $"User successfully created. ID: {id}";
            }
            catch (NpgsqlException ex)
            {
                Trace.TraceError(
                    $"Database error inserting user: {ex}");

                throw new BackCheilGestHotelException(
                    500,
                    "Error inserting user.",
                    ex);
            }
        }

        // ==========================================
        // UPDATE USER
        // ==========================================

        public string UpdUsuarios(UsersDto user)
        {
            try
            {
                const string sql = """
                    UPDATE public.geshusers
                    SET
                        usunames = @names,
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

                command.Parameters.AddWithValue(
                    "names", (object?)user.UsuNames ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "address", (object?)user.UsuAddress ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "phone", (object?)user.UsuPhone ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "email", (object?)user.UsuEmail ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "documentId", (object?)user.UsuDocumentId ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "password", (object?)user.UsuPassword ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "documentTypeId", (object?)user.UsuTdocId ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "usuRolId", (object?)user.usuRolId ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "isActive", user.UsuIsActive);

                object? result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    return $"User with ID {user.UsuId} was not found.";
                }

                Trace.TraceInformation(
                    $"User updated successfully. ID: {user.UsuId}");

                return $"User with ID {user.UsuId} successfully updated.";
            }
            catch (NpgsqlException ex)
            {
                Trace.TraceError(
                    $"Database error updating user: {ex}");

                throw new BackCheilGestHotelException(
                    500,
                    "Error updating user.",
                    ex);
            }
        }

        // ==========================================
        // DELETE USER BY ID
        // ==========================================

        public string DropUserById(int dato)
        {
            try
            {
                const string sql = """
                    DELETE FROM public.geshusers
                    WHERE usuid = @id
                    RETURNING usuid;
                    """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("id", dato);

                object? result = command.ExecuteScalar();

                return result != null && result != DBNull.Value
                    ? $"User with ID {dato} successfully deleted."
                    : $"User with ID {dato} was not found.";
            }
            catch (NpgsqlException ex)
            {
                Trace.TraceError(
                    $"Database error deleting user: {ex}");

                throw new BackCheilGestHotelException(
                    500,
                    "Error deleting user.",
                    ex);
            }
        }

        // ==========================================
        // GET ROLE BY USER EMAIL
        // ==========================================

        public RolesDto? GetRolUser(string dato)
        {
            try
            {
                const string sql = """
                    SELECT
                        r.rolid,
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
                Trace.TraceError(
                    $"Database error retrieving user role: {ex}");

                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving the role assigned to the user.",
                    ex);
            }
        }

        // ==========================================
        // MAP USER DTO
        // ==========================================

        private static UsersDto MapUsuario(NpgsqlDataReader reader)
        {
            DocumentTypeDto? documentType = null;
            RolesDto? roles = null;

            // DOCUMENT TYPE: INDEX 7 - 9

            if (!reader.IsDBNull(7))
            {
                documentType = new DocumentTypeDto
                {
                    TipId = reader.GetInt32(7),

                    TipDescription = reader.IsDBNull(8)
                        ? null
                        : reader.GetString(8),

                    TipIsActive = reader.IsDBNull(9)
                        ? null
                        : reader.GetBoolean(9)
                };
            }

            // ROLE: INDEX 11 - 14

            if (!reader.IsDBNull(11))
            {
                roles = new RolesDto
                {
                    RolId = reader.GetInt32(11),

                    RolName = reader.IsDBNull(12)
                        ? null!
                        : reader.GetString(12),

                    RolObservation = reader.IsDBNull(13)
                        ? null!
                        : reader.GetString(13),

                    RolIsActive = reader.IsDBNull(14)
                        ? null
                        : reader.GetBoolean(14)
                };
            }

            // USER: INDEX 0 - 15

            return new UsersDto
            {
                UsuId = reader.GetInt32(0),

                UsuNames = reader.IsDBNull(1)
                    ? null
                    : reader.GetString(1),

                UsuAddress = reader.IsDBNull(2)
                    ? null
                    : reader.GetString(2),

                UsuPhone = reader.IsDBNull(3)
                    ? null
                    : reader.GetString(3),

                UsuEmail = reader.IsDBNull(4)
                    ? null
                    : reader.GetString(4),

                UsuDocumentId = reader.IsDBNull(5)
                    ? null
                    : reader.GetString(5),

                // Do not return the user's password.
                UsuPassword = null,

                UsuTdocId = reader.IsDBNull(6)
                    ? null
                    : reader.GetInt32(6),

                DocumentType = documentType,

                usuRolId = reader.IsDBNull(10)
                    ? null
                    : reader.GetInt32(10),

                Roles = roles,

                UsuIsActive = !reader.IsDBNull(15)
                    && reader.GetBoolean(15)
            };
        }
    }
}

