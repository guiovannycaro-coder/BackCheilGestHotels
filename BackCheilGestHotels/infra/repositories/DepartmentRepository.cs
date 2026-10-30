using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;
using BackCheilGestHotels.infra.Persistence;
using Npgsql;

namespace BackCheilGestHotels.infra.repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DatabaseConnection _database;

        public DepartmentRepository(DatabaseConnection database)
        {
            _database = database;
        }

        // GET: Obtener todos los departamentos
        public List<DepartmentDto> GetDepartmentsAsync()
        {
            try
            {
                var departments = new List<DepartmentDto>();

                using var connection = _database.GetConnection();

                const string sql = @"
                    SELECT depid, depcontryid, name
                    FROM public.geshdepartament
                    ORDER BY name;";

                using var command = new NpgsqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    departments.Add(MapDepartment(reader));
                }

                return departments;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error retrieving departments.", ex);
            }
        }

        // GET: Obtener departamento por ID
        public DepartmentDto GetDepartmentByIdAsync(int depId)
        {
            try
            {
                using var connection = _database.GetConnection();

                const string sql = @"
                    SELECT depid, depcontryid, name
                    FROM public.geshdepartament
                    WHERE depid = @depId;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@depId", depId);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return MapDepartment(reader);
                }

                return null!;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error retrieving department by ID.", ex);
            }
        }

        // GET: Obtener departamentos por país
        public List<DepartmentDto> GetDepartmentsByCountryIdAsync(int countId)
        {
            try
            {
                var departments = new List<DepartmentDto>();

                using var connection = _database.GetConnection();

                const string sql = @"
                    SELECT depid, depcontryid, name
                    FROM public.geshdepartament
                    WHERE depcontryid = @countId
                    ORDER BY name;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@countId", countId);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    departments.Add(MapDepartment(reader));
                }

                return departments;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error retrieving departments by country.", ex);
            }
        }

        // POST: Crear departamento
        public string InsDepartmentAsync(DepartmentDto department)
        {
            try
            {
                using var connection = _database.GetConnection();

                const string sql = @"
                    INSERT INTO public.geshdepartament
                        (depcontryid, name)
                    VALUES
                        (@countId, @name)
                    RETURNING depid;";

                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue(
                    "@countId", department.DepContryId);

                command.Parameters.AddWithValue(
                    "@name", department.Name);

                int newDepartmentId = Convert.ToInt32(
                    command.ExecuteScalar());

                return $"Department created successfully. ID: {newDepartmentId}";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error creating department.", ex);
            }
        }

        // PUT: Actualizar departamento
        public string UpdDepartmentAsync(DepartmentDto department)
        {
            try
            {
                using var connection = _database.GetConnection();

                const string sql = @"
                    UPDATE public.geshdepartament
                    SET depcontryid = @countId,
                        name = @name
                    WHERE depid = @depId;";

                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue(
                    "@depId", department.DepId);

                command.Parameters.AddWithValue(
                    "@countId", department.DepContryId);

                command.Parameters.AddWithValue(
                    "@name", department.Name);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return "Department not found.";
                }

                return "Department updated successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error updating department.", ex);
            }
        }

        // DELETE: Eliminar departamento
        public string DropDepartmentByIdAsync(int depId)
        {
            try
            {
                using var connection = _database.GetConnection();

                const string sql = @"
                    DELETE FROM public.geshdepartament
                    WHERE depid = @depId;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@depId", depId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return "Department not found.";
                }

                return "Department deleted successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error deleting department.", ex);
            }
        }

        // Mapear registro de PostgreSQL a DepartmentDto
        private static DepartmentDto MapDepartment(NpgsqlDataReader reader)
        {
            return new DepartmentDto
            {
                DepId = reader.GetInt32(0),
                DepContryId = reader.GetInt32(1),
                Name = reader.GetString(2)
            };
        }
    }
}
