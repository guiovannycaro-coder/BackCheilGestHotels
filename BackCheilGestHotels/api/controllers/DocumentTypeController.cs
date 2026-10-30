using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.Models;
using BackCheilGestHotels.infra.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BackCheilGestHotels.api.controllers
{
    [ApiController]
    [Route("api/gestHotel/DocumentType")]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IDocumentTypeService _documentTypeService;

        public DocumentTypeController(
            IDocumentTypeService documentTypeService)
        {
            _documentTypeService = documentTypeService;
        }

        // GET: api/gestHotel/DocumentType
        
        [HttpGet("ObtenerTodos")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<DocumentTypeDto>>> GetDocumentTypes()
        {
            try
            {
                List<DocumentTypeDto> documentTypes =
                    _documentTypeService.GetDocumentTypes();

                return Ok(new ApisResponse<List<DocumentTypeDto>>(
                    0,
                    "Document types retrieved successfully.",
                    documentTypes));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(
                        500,
                        "An unexpected error occurred.",
                        null));
            }
        }

        // GET: api/gestHotel/DocumentType/5
        
        [HttpGet("ObtenerPorId")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<DocumentTypeDto>>> GetDocumentTypeById(
           [FromQuery] int dato)
        {
            try
            {
                List<DocumentTypeDto> documentTypes =
                    _documentTypeService.GetDocumentTypeById(dato);

                if (documentTypes.Count == 0)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "Document type not found.",
                        null));
                }

                return Ok(new ApisResponse<List<DocumentTypeDto>>(
                    0,
                    "Document type retrieved successfully.",
                    documentTypes));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(
                        500,
                        "An unexpected error occurred.",
                        null));
            }
        }

        // GET: api/gestHotel/DocumentType/search/Passport
      
        [HttpGet("ObtenerPorName")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<DocumentTypeDto>>> GetDocumentTypeByName(
           [FromQuery] string dato)
        {
            try
            {
                List<DocumentTypeDto> documentTypes =
                    _documentTypeService.GetDocumentTypeByName(dato);

                return Ok(new ApisResponse<List<DocumentTypeDto>>(
                    0,
                    "Document types search completed successfully.",
                    documentTypes));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(
                        500,
                        "An unexpected error occurred.",
                        null));
            }
        }

        // POST: api/gestHotel/DocumentType
      
        [HttpPost("InsertarDocumentType")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<int>> CreateDocumentType(
            [FromBody] DocumentTypeDto documentType)
        {
            try
            {
                if (documentType == null)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "Document type data is required.",
                        null));
                }

                if (string.IsNullOrWhiteSpace(documentType.TipDescription))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "Document type description is required.",
                        null));
                }

                if (string.IsNullOrWhiteSpace(documentType.TipIsActive))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "Document type status is required.",
                        null));
                }

                int newId =
                    _documentTypeService.InsDocumentType(documentType);

                documentType.TipId = newId;

                return StatusCode(
                    201,
                    new ApisResponse<int>(
                        0,
                        "Document type created successfully.",
                        newId));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(
                  500,
                    new ApisResponse<string>(500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(
                        500,
                        "An unexpected error occurred.",
                        null));
            }
        }

        // PUT: api/gestHotel/DocumentType/5

        [HttpPut("ActualizarDocumentType")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<string>> UpdateDocumentType(
            int tipId,
            [FromBody] DocumentTypeDto documentType)
        {
            try
            {
                if (documentType == null)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "Document type data is required.",
                        null));
                }

                if (tipId != documentType.TipId)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The route ID does not match the document type ID.",
                        null));
                }

                if (string.IsNullOrWhiteSpace(documentType.TipDescription))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "Document type description is required.",
                        null));
                }

                if (string.IsNullOrWhiteSpace(documentType.TipIsActive))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "Document type status is required.",
                        null));
                }

                bool updated =
                    _documentTypeService.UpdDocumentType(documentType);

                if (!updated)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "Document type not found or not updated.",
                        null));
                }

                return Ok(new ApisResponse<string>(
                    0,
                    "Document type updated successfully.",
                    null));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(
                  500,
                    new ApisResponse<string>(500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(
                        500,
                        "An unexpected error occurred.",
                        null));
            }
        }

        // DELETE: api/gestHotel/DocumentType/5
        [HttpDelete("DeleteDocumentType")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<string>> DeleteDocumentType([FromQuery]int dato)
        {
            try
            {
                bool deleted =
                    _documentTypeService.DropDocumentTypeById(dato);

                if (!deleted)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "Document type not found.",
                        null));
                }

                return Ok(new ApisResponse<string>(
                    0,
                    "Document type deleted successfully.",
                    null));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(
                        500,
                        "An unexpected error occurred.",
                        null));
            }
        }
    }
}
