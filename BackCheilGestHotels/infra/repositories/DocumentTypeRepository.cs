using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;
using BackCheilGestHotels.infra.Persistence;
using Npgsql;

namespace BackCheilGestHotels.infra.repositories
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly DatabaseConnection _database;

        public DocumentTypeRepository(DatabaseConnection database)
        {
            _database = database;
        }

        // GET: Consultar todos los tipos de documento
        public List<DocumentTypeDto> GetDocumentTypesAsync()
        {
            var documentTypes = new List<DocumentTypeDto>();

            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    SELECT tipId, tipDescription, tipIsActive
                    FROM public.documentType
                    ORDER BY tipId;";

                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    documentTypes.Add(MapDocumentType(reader));
                }

                return documentTypes;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving document types.",
                    ex);
            }
        }

        // GET: Consultar tipo de documento por ID
        public DocumentTypeDto GetDocumentTypeByIdAsync(int tipId)
        {
            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    SELECT tipId, tipDescription, tipIsActive
                    FROM public.documentType
                    WHERE tipId = @tipId;";

                using var command = new NpgsqlCommand(query, connection);

                command.Parameters.AddWithValue("@tipId", tipId);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return MapDocumentType(reader);
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
                    500,
                    "Error retrieving document type by ID.",
                    ex);
            }
        }

        // GET: Buscar tipos de documento por descripción
        public List<DocumentTypeDto> GetDocumentTypesByNameAsync(
            string description)
        {
            var documentTypes = new List<DocumentTypeDto>();

            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    SELECT tipId, tipDescription, tipIsActive
                    FROM public.documentType
                    WHERE tipDescription ILIKE @description
                    ORDER BY tipDescription;";

                using var command = new NpgsqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@description",
                    $"%{description}%");

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    documentTypes.Add(MapDocumentType(reader));
                }

                return documentTypes;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error searching document types by description.",
                    ex);
            }
        }

        // POST: Insertar tipo de documento
        public string InsDocumentTypeAsync(DocumentTypeDto documentType)
        {
            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    INSERT INTO public.documentType
                    (
                        tipDescription,
                        tipIsActive
                    )
                    VALUES
                    (
                        @description,
                        @isActive
                    )
                    RETURNING tipId;";

                using var command = new NpgsqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@description",
                    (object?)documentType.TipDescription ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "@isActive",
                    documentType.TipIsActive);

                int newId = Convert.ToInt32(command.ExecuteScalar());

                return $"Document type created successfully. ID: {newId}";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error creating document type.",
                    ex);
            }
        }

        // PUT: Actualizar tipo de documento
        public string UpdDocumentTypeAsync(
            DocumentTypeDto documentType)
        {
            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    UPDATE public.documentType
                    SET tipDescription = @description,
                        tipIsActive = @isActive
                    WHERE tipId = @tipId;";

                using var command = new NpgsqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@tipId",
                    documentType.TipId);

                command.Parameters.AddWithValue(
                    "@description",
                    (object?)documentType.TipDescription ?? DBNull.Value);

                command.Parameters.AddWithValue(
                    "@isActive",
                    documentType.TipIsActive);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return "Document type not found.";
                }

                return "Document type updated successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error updating document type.",
                    ex);
            }
        }

        // DELETE: Eliminar tipo de documento por ID
        public string DropDocumentTypeByIdAsync(int tipId)
        {
            try
            {
                using var connection = _database.GetConnection();

                string query = @"
                    DELETE FROM public.documentType
                    WHERE tipId = @tipId;";

                using var command = new NpgsqlCommand(query, connection);

                command.Parameters.AddWithValue("@tipId", tipId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return "Document type not found.";
                }

                return "Document type deleted successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error deleting document type.",
                    ex);
            }
        }

        // Mapear registro de PostgreSQL a DocumentTypeDto
        private static DocumentTypeDto MapDocumentType(
            NpgsqlDataReader reader)
        {
            return new DocumentTypeDto
            {
                TipId = reader.GetInt32(0),

                TipDescription = reader.IsDBNull(1)
                    ? null!
                    : reader.GetString(1),

                TipIsActive = reader.GetString(2)
            };
        }
    }
}
