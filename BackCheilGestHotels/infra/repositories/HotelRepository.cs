using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;
using BackCheilGestHotels.infra.Persistence;
using Npgsql;

namespace BackCheilGestHotels.infra.repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly DatabaseConnection _database;

        public HotelRepository(DatabaseConnection database)
        {
            _database = database;
        }

        // GET: Consultar todos los hoteles
        public List<HotelDto> GetHotels()
        {
            var hotels = new List<HotelDto>();

            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    SELECT hotId, hotName, hotDescription, hotAddress,
                           hotPhone, hotEmail, hotCityId, hotRating,
                           hotRateSign, hotIsActive,hotImages
                    FROM public.geshHotels
                    ORDER BY hotId;";

                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    hotels.Add(MapHotel(reader));
                }

                return hotels;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error retrieving hotels.", ex);
            }
        }

        // GET: Consultar un hotel por ID
        public HotelDto GetHotelById(int hotId)
        {
            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    SELECT hotId, hotName, hotDescription, hotAddress,
                           hotPhone, hotEmail, hotCityId, hotRating,
                           hotRateSign, hotIsActive,hotImages
                    FROM public.geshHotels
                    WHERE hotId = @hotId;";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@hotId", hotId);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return MapHotel(reader);
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
                    500, "Error retrieving hotel by ID.", ex);
            }
        }

        // GET: Consultar hoteles por ciudad
        public List<HotelDto> GetHotelsByCityId(int cityId)
        {
            var hotels = new List<HotelDto>();

            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    SELECT hotId, hotName, hotDescription, hotAddress,
                           hotPhone, hotEmail, hotCityId, hotRating,
                           hotRateSign, hotIsActive,hotImages
                    FROM public.geshHotels
                    WHERE hotCityId = @cityId
                    ORDER BY hotName;";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@cityId", cityId);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    hotels.Add(MapHotel(reader));
                }

                return hotels;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error retrieving hotels by city.", ex);
            }
        }

        // GET: Buscar hoteles por nombre
        public List<HotelDto> GetHotelsByName(string name)
        {
            var hotels = new List<HotelDto>();

            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    SELECT hotId, hotName, hotDescription, hotAddress,
                           hotPhone, hotEmail, hotCityId, hotRating,
                           hotRateSign, hotIsActive,hotImages
                    FROM public.geshHotels
                    WHERE hotName ILIKE @name
                    ORDER BY hotName;";

                using var command = new NpgsqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@name", $"%{name}%");

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    hotels.Add(MapHotel(reader));
                }

                return hotels;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error searching hotels by name.", ex);
            }
        }

        // POST: Insertar hotel
        public string InsHotel(HotelDto hotel)
        {
            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    INSERT INTO public.geshHotels
                    (
                        hotName,
                        hotDescription,
                        hotAddress,
                        hotPhone,
                        hotEmail,
                        hotCityId,
                        hotRating,
                        hotRateSign,
                        hotImages
                    )
                    VALUES
                    (
                        @name,
                        @description,
                        @address,
                        @phone,
                        @email,
                        @cityId,
                        @rating,
                        @rateSign,
                        @hotImages
                    )
                    RETURNING hotId;";

                using var command = new NpgsqlCommand(query, connection);

                AddHotelParameters(command, hotel);

                int newHotelId = Convert.ToInt32(command.ExecuteScalar());

                return $"Hotel created successfully. ID: {newHotelId}";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error creating hotel.", ex);
            }
        }

        // PUT: Actualizar hotel
        public string UpdHotel(HotelDto hotel)
        {
            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    UPDATE public.geshHotels
                    SET hotName = @name,
                        hotDescription = @description,
                        hotAddress = @address,
                        hotPhone = @phone,
                        hotEmail = @email,
                        hotCityId = @cityId,
                        hotRating = @rating,
                        hotRateSign = @rateSign,
                        hotIsActive = @isActive,
                        hotImages = @hotImages
                    WHERE hotId = @hotId;";

                using var command = new NpgsqlCommand(query, connection);

                AddHotelParameters(command, hotel);

                command.Parameters.AddWithValue(
                    "@hotId", hotel.HotId);

                command.Parameters.AddWithValue(
                    "@isActive", hotel.HotIsActive);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return "Hotel not found.";
                }

                return "Hotel updated successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error updating hotel.", ex);
            }
        }

        // DELETE: Eliminar hotel por ID
        public string DropHotelById(int hotId)
        {
            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    DELETE FROM public.geshHotels
                    WHERE hotId = @hotId;";

                using var command = new NpgsqlCommand(query, connection);

                command.Parameters.AddWithValue("@hotId", hotId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return "Hotel not found.";
                }

                return "Hotel deleted successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, "Error deleting hotel.", ex);
            }
        }


        public List<HotelDto> searchHotels(string? Name, int? Rating)
        {
            try
            {
                using var connection = _database.GetConnection();

                string query = @"
            SELECT
                h.hotId, h.hotName, h.hotDescription, h.hotAddress,
                h.hotPhone, h.hotEmail, h.hotCityId, h.hotRating,
                h.hotRateSign, h.hotIsActive, h.hotImages
            FROM geshhotels h
            WHERE
                (NULLIF(@hotName, '') IS NULL
                 OR h.hotName ILIKE '%' || @hotName || '%')
                AND
                (@hotRating IS NULL
                 OR h.hotRating = @hotRating);
        ";

                using var command = new NpgsqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@hotName",
                    NpgsqlTypes.NpgsqlDbType.Text,
                    Name ?? string.Empty);

                var ratingParameter = command.Parameters.Add(
                    "@hotRating",
                    NpgsqlTypes.NpgsqlDbType.Integer);

                ratingParameter.Value = (object?)Rating ?? DBNull.Value;

                using var reader = command.ExecuteReader();

                List<HotelDto> hotels = new List<HotelDto>();

                while (reader.Read())
                {
                    hotels.Add(MapHotel(reader));
                }

                return hotels;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500, $"Error searching hotels: {ex.Message}", ex);
            }
        }
        // Mapear registro de PostgreSQL a HotelDto
        private static HotelDto MapHotel(NpgsqlDataReader reader)
        {
            return new HotelDto
            {
                HotId = reader.GetInt32(0),
                HotName = reader.GetString(1),
                HotDescription = reader.GetString(2),

                HotAddress = reader.IsDBNull(3)
                    ? string.Empty
                    : reader.GetString(3),

                HotPhone = reader.IsDBNull(4)
                    ? string.Empty
                    : reader.GetString(4),

                HotEmail = reader.IsDBNull(5)
                    ? string.Empty
                    : reader.GetString(5),

                HotCityId = reader.IsDBNull(6)
                    ? 0
                    : reader.GetInt32(6),

                HotRating = reader.IsDBNull(7)
                    ? 0
                    : reader.GetInt32(7),

                HotRateSign = reader.IsDBNull(8)
                    ? string.Empty
                    : reader.GetString(8),

                HotIsActive = reader.GetBoolean(9),

                hotImages = reader.IsDBNull(10)
                    ? string.Empty
                    : reader.GetString(10),

                City = null!
            };
        }

        // Agregar parámetros para INSERT y UPDATE
        private static void AddHotelParameters(
            NpgsqlCommand command,
            HotelDto hotel)
        {
            command.Parameters.AddWithValue(
                "@name", hotel.HotName);

            command.Parameters.AddWithValue(
                "@description", hotel.HotDescription);

            command.Parameters.AddWithValue(
                "@address",
                (object?)hotel.HotAddress ?? DBNull.Value);

            command.Parameters.AddWithValue(
                "@phone",
                (object?)hotel.HotPhone ?? DBNull.Value);

            command.Parameters.AddWithValue(
                "@email",
                (object?)hotel.HotEmail ?? DBNull.Value);

            command.Parameters.AddWithValue(
                "@cityId", hotel.HotCityId);

            command.Parameters.AddWithValue(
                "@rating", hotel.HotRating);
          
            command.Parameters.AddWithValue(
                "@hotImages", hotel.hotImages);

            command.Parameters.AddWithValue(
                "@rateSign",
                (object?)hotel.HotRateSign ?? DBNull.Value);
        }

     
    }
}
