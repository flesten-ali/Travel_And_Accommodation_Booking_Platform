using MediatR;

namespace TABP.Application.Bookings.Queries.PdfConfirmation;
public class GetInvoicePdfResponse 
{
    public byte[] PdfContent { get; set; }
}
