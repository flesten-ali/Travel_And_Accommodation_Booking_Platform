using MediatR;

namespace TABP.Application.Bookings.Queries.PdfConfirmation;
public class InvoicePdfResponse 
{
    public byte[] PdfContent { get; set; }
}
