using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;

namespace BackCheilGestHotels.application.services
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;

        public DocumentTypeService(
            IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }

        public bool DropDocumentTypeById(int dato)
        {
            try
            {
                string result =
                    _documentTypeRepository.DropDocumentTypeByIdAsync(dato);

                return result == "Document type deleted successfully.";
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

        public List<DocumentTypeDto> GetDocumentTypeById(int dato)
        {
            try
            {
                DocumentTypeDto documentType =
                    _documentTypeRepository.GetDocumentTypeByIdAsync(dato);

                if (documentType == null)
                {
                    return new List<DocumentTypeDto>();
                }

                return new List<DocumentTypeDto> { documentType };
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

        public List<DocumentTypeDto> GetDocumentTypeByName(string dato)
        {
            try
            {
                return _documentTypeRepository
                    .GetDocumentTypesByNameAsync(dato);
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error searching document types by name.",
                    ex);
            }
        }

        public List<DocumentTypeDto> GetDocumentTypes()
        {
            try
            {
                return _documentTypeRepository.GetDocumentTypesAsync();
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

        public int InsDocumentType(DocumentTypeDto documentType)
        {
            try
            {
                string result =
                    _documentTypeRepository.InsDocumentTypeAsync(documentType);

                const string prefix = "Document type created successfully. ID: ";

                if (result.StartsWith(prefix))
                {
                    string id = result.Substring(prefix.Length);

                    if (int.TryParse(id, out int newId))
                    {
                        return newId;
                    }
                }

                return 0;
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

        public bool UpdDocumentType(DocumentTypeDto documentType)
        {
            try
            {
                string result =
                    _documentTypeRepository.UpdDocumentTypeAsync(documentType);

                return result == "Document type updated successfully.";
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
    }
}