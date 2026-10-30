using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;
using BackCheilGestHotels.infra.Persistence;
using Npgsql;

namespace BackCheilGestHotels.infra.repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly DatabaseConnection _database;

        public RolesRepository(DatabaseConnection database)
        {
            _database = database;
        }

        // Consultar todos los roles
        public List<RolesDto> GetRolesAsync()
        {
            try
            {
                const string sql = """
                    SELECT rolid, rolname, rolobservation, rolisactive
                    FROM geshroles
                    ORDER BY rolid;
                    """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                List<RolesDto> roles = new();

                while (reader.Read())
                {
                    roles.Add(new RolesDto
                    {
                        RolId = reader.GetInt32(0),
                        RolName = reader.IsDBNull(1) ? null : reader.GetString(1),
                        RolObservation = reader.IsDBNull(2) ? null : reader.GetString(2),
                        RolIsActive = reader.IsDBNull(3) ? null : reader.GetBoolean(3)
                    });
                }

                return roles;
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error retrieving roles.", ex);
            }
        }

        // Consultar un rol por ID
        public RolesDto GetRoleByIdAsync(int rolId)
        {
            try
            {
                const string sql = """
                    SELECT rolid, rolname, rolobservation, rolisactive
                    FROM geshroles
                    WHERE rolid = @roleId;
                    """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("roleId", rolId);

                using var reader = command.ExecuteReader();

                if (!reader.Read())
                {
                    return null!;
                }

                return new RolesDto
                {
                    RolId = reader.GetInt32(0),
                    RolName = reader.IsDBNull(1) ? null : reader.GetString(1),
                    RolObservation = reader.IsDBNull(2) ? null : reader.GetString(2),
                    RolIsActive = reader.IsDBNull(3) ? null : reader.GetBoolean(3)
                };
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error retrieving role by ID.", ex);
            }
        }

        // Consultar roles por nombre
        public List<RolesDto> GetRolesByNameAsync(string name)
        {
            try
            {
                const string sql = """
                    SELECT rolid, rolname, rolobservation, rolisactive
                    FROM geshroles
                    WHERE rolname ILIKE @name
                    ORDER BY rolname;
                    """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("name", $"%{name}%");

                using var reader = command.ExecuteReader();

                List<RolesDto> roles = new();

                while (reader.Read())
                {
                    roles.Add(new RolesDto
                    {
                        RolId = reader.GetInt32(0),
                        RolName = reader.IsDBNull(1) ? null : reader.GetString(1),
                        RolObservation = reader.IsDBNull(2) ? null : reader.GetString(2),
                        RolIsActive = reader.IsDBNull(3) ? null : reader.GetBoolean(3)
                    });
                }

                return roles;
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error searching roles by name.", ex);
            }
        }

        // Insertar un rol
        public int InsRolesAsync(RolesDto role)
        {
            try
            {
                const string sql = """
                    INSERT INTO geshroles
                        (rolname, rolobservation, rolisactive)
                    VALUES
                        (@name, @observation, @isActive)
                    RETURNING rolid;
                    """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue(
                    "name", (object?)role.RolName ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "observation", (object?)role.RolObservation ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "isActive", (object?)role.RolIsActive ?? DBNull.Value);

                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error inserting role.", ex);
            }
        }

        // Actualizar un rol
        public bool UpdRolesAsync(RolesDto role)
        {
            try
            {
                const string sql = """
                    UPDATE geshroles
                    SET rolname = @name,
                        rolobservation = @observation,
                        rolisactive = @isActive
                    WHERE rolid = @roleId;
                    """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("roleId", role.RolId);

                command.Parameters.AddWithValue(
                    "name", (object?)role.RolName ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "observation", (object?)role.RolObservation ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "isActive", (object?)role.RolIsActive ?? DBNull.Value);

                return command.ExecuteNonQuery() > 0;
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error updating role.", ex);
            }
        }

        // Eliminar un rol por ID
        public bool DropRoleByIdAsync(int rolId)
        {
            try
            {
                const string sql = """
                    DELETE FROM geshroles
                    WHERE rolid = @roleId;
                    """;

                using var connection = _database.GetConnection();
                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue("roleId", rolId);

                return command.ExecuteNonQuery() > 0;
            }
            catch (NpgsqlException ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error deleting role.", ex);
            }
        }
    }

}