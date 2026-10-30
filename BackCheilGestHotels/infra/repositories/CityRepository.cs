using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;
using BackCheilGestHotels.infra.Persistence;
using Npgsql;

namespace BackCheilGestHotels.infra.repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly DatabaseConnection _database;

        public CityRepository(DatabaseConnection database)
        {
            _database = database;
        }

        // GET: Obtener todas las ciudades
        public List<CityDto> GetCities()
        {
            try
            {
                var cities = new List<CityDto>();

                using var connection = _database.GetConnection();

                const string sql = @"
                    SELECT cityid, cityname, cityiddepartament
                    FROM public.geshCity
                    ORDER BY cityname;";

                using var command = new NpgsqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cities.Add(MapCity(reader));
                }

                return cities;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error retrieving cities.", ex);
            }
        }

        // GET: Obtener una ciudad por ID
        public CityDto GetCityById(int cityId)
        {
            try
            {
                using var connection = _database.GetConnection();

                const string sql = @"
                    SELECT cityid, cityname, cityiddepartament
                    FROM public.geshCity
                    WHERE cityid = @cityId;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@cityId", cityId);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return MapCity(reader);
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
                    500, "Error retrieving city by ID.", ex);
            }
        }

        // GET: Obtener ciudades por departamento
        public List<CityDto> GetCitiesByDepartmentId(int depId)
        {
            try
            {
                var cities = new List<CityDto>();

                using var connection = _database.GetConnection();

                const string sql = @"
                    SELECT cityid, cityname, cityiddepartament
                    FROM public.geshCity
                    WHERE cityiddepartament = @depId
                    ORDER BY cityname;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@depId", depId);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cities.Add(MapCity(reader));
                }

                return cities;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error retrieving cities by department.", ex);
            }
        }

        // GET: Buscar ciudades por nombre
        public List<CityDto> GetCitiesByName(string name)
        {
            try
            {
                var cities = new List<CityDto>();

                using var connection = _database.GetConnection();

                const string sql = @"
                    SELECT cityid, cityname, cityiddepartament
                    FROM public.geshCity
                    WHERE cityname ILIKE @name
                    ORDER BY cityname;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@name", $"%{name}%");

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cities.Add(MapCity(reader));
                }

                return cities;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error searching cities by name.", ex);
            }
        }

        // POST: Crear una ciudad
        public string InsCity(CityDto city)
        {
            try
            {
                using var connection = _database.GetConnection();

                const string sql = @"
                    INSERT INTO public.geshCity
                        (cityname, cityiddepartament)
                    VALUES
                        (@name, @depId)
                    RETURNING cityid;";

                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue(
                    "@name", city.CityName);

                command.Parameters.AddWithValue(
                    "@depId", city.CityidDepartament);

                int newCityId = Convert.ToInt32(
                    command.ExecuteScalar());

                return $"City created successfully. ID: {newCityId}";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error creating city.", ex);
            }
        }

        // PUT: Actualizar una ciudad
        public string UpdCity(CityDto city)
        {
            try
            {
                using var connection = _database.GetConnection();

                const string sql = @"
                    UPDATE public.geshCity
                    SET cityname = @name,
                        cityiddepartament = @depId
                    WHERE cityid = @cityId;";

                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue(
                    "@cityId", city.CityId);

                command.Parameters.AddWithValue(
                    "@name", city.CityName);

                command.Parameters.AddWithValue(
                    "@depId", city.CityidDepartament);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return "City not found.";
                }

                return "City updated successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error updating city.", ex);
            }
        }

        // DELETE: Eliminar una ciudad
        public string DropCityById(int cityId)
        {
            try
            {
                using var connection = _database.GetConnection();

                const string sql = @"
                    DELETE FROM public.geshCity
                    WHERE cityid = @cityId;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@cityId", cityId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return "City not found.";
                }

                return "City deleted successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error deleting city.", ex);
            }
        }

        // Mapear los datos de PostgreSQL al DTO
        private static CityDto MapCity(NpgsqlDataReader reader)
        {
            return new CityDto
            {
                CityId = reader.GetInt32(0),
                CityName = reader.GetString(1),
                CityidDepartament = reader.GetInt32(2)
            };
        }
    }
}



