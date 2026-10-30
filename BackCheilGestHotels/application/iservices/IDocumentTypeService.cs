using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.application.iservices
{
    public interface IDocumentTypeService
    {
        List<DocumentTypeDto> GetDocumentTypes();
        int InsDocumentType(DocumentTypeDto documentType);
        bool UpdDocumentType(DocumentTypeDto documentType);
        bool DropDocumentTypeById(int dato);
        List<DocumentTypeDto> GetDocumentTypeById(int dato);
        List<DocumentTypeDto> GetDocumentTypeByName(string dato);
    }
}
