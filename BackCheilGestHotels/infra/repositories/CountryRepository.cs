using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;
using BackCheilGestHotels.infra.Persistence;
using Npgsql;

namespace BackCheilGestHotels.infra.repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DatabaseConnection _database;

        public CountryRepository(DatabaseConnection database)
        {
            _database = database;
        }

        // GET: Obtener todos los países
        public List<CountryDto> GetCountries()
        {
            try
            {
                var countries = new List<CountryDto>();

                using var connection = _database.GetConnection();

                const string sql = @"
                    SELECT countid, countname
                    FROM public.contry
                    ORDER BY countname;";

                using var command = new NpgsqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    countries.Add(MapCountry(reader));
                }

                return countries;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error retrieving countries.", ex);
            }
        }

        // GET: Obtener un país por ID
        public CountryDto GetCountryById(int countId)
        {
            try
            {
                using var connection = _database.GetConnection();

                const string sql = @"
                    SELECT countid, countname
                    FROM public.contry
                    WHERE countid = @countId;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@countId", countId);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return MapCountry(reader);
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
                    500, "Error retrieving country by ID.", ex);
            }
        }

        // GET: Buscar países por nombre
        public List<CountryDto> GetCountriesByName(string name)
        {
            try
            {
                var countries = new List<CountryDto>();

                using var connection = _database.GetConnection();

                const string sql = @"
                    SELECT countid, countname
                    FROM public.contry
                    WHERE countname ILIKE @name
                    ORDER BY countname;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@name", $"%{name}%");

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    countries.Add(MapCountry(reader));
                }

                return countries;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error searching countries by name.", ex);
            }
        }

        // POST: Crear un país
        public string InsCountry(CountryDto country)
        {
            try
            {
                using var connection = _database.GetConnection();

                const string sql = @"
                    INSERT INTO public.contry (countname)
                    VALUES (@name)
                    RETURNING countid;";

                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue(
                    "@name", country.CountName);

                int newCountryId = Convert.ToInt32(
                    command.ExecuteScalar());

                return $"Country created successfully. ID: {newCountryId}";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error creating country.", ex);
            }
        }

        // PUT: Actualizar un país
        public string UpdCountry(CountryDto country)
        {
            try
            {
                using var connection = _database.GetConnection();

                const string sql = @"
                    UPDATE public.contry
                    SET countname = @name
                    WHERE countid = @countId;";

                using var command = new NpgsqlCommand(sql, connection);

                command.Parameters.AddWithValue(
                    "@countId", country.CountId);

                command.Parameters.AddWithValue(
                    "@name", country.CountName);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return "Country not found.";
                }

                return "Country updated successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error updating country.", ex);
            }
        }

        // DELETE: Eliminar un país
        public string DropCountryById(int countId)
        {
            try
            {
                using var connection = _database.GetConnection();

                const string sql = @"
                    DELETE FROM public.contry
                    WHERE countid = @countId;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@countId", countId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return "Country not found.";
                }

                return "Country deleted successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error deleting country.", ex);
            }
        }

        // Mapear los datos de PostgreSQL al DTO
        private static CountryDto MapCountry(NpgsqlDataReader reader)
        {
            return new CountryDto
            {
                CountId = reader.GetInt32(0),
                CountName = reader.GetString(1)
            };
        }
    }
}