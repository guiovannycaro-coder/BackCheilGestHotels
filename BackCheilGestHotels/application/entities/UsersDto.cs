namespace BackCheilGestHotels.application.entities
{
    public class UsersDto
    {
        public int UsuId { get; set; }

        public string? UsuNames { get; set; } 

        public string? UsuAddress { get; set; }

        public string? UsuPhone { get; set; }

        public string? UsuEmail { get; set; }

        public string? UsuDocumentId { get; set; }

        public string? UsuPassword { get; set; }

        public int? UsuTdocId { get; set; }

        public int? usuRolId { get; set; }

        public bool UsuIsActive { get; set; } 

        
        public DocumentTypeDto? DocumentType { get; set; }

      


    }
}
