using MediatR;

namespace TABP.Application.Bookings.Queries.PdfConfirmation;
public class GetPdfConfirmationResponse 
{
    public byte[] PdfContent { get; set; }
}
