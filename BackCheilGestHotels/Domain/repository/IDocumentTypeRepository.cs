using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.Domain.repository
{
    public interface IDocumentTypeRepository
    {
        List<DocumentTypeDto> GetDocumentTypesAsync();

        DocumentTypeDto GetDocumentTypeByIdAsync(int tipId);

       List<DocumentTypeDto> GetDocumentTypesByNameAsync(string description);

        String InsDocumentTypeAsync(DocumentTypeDto documentType);

        String UpdDocumentTypeAsync(DocumentTypeDto documentType);

        String DropDocumentTypeByIdAsync(int tipId);
    }
}
