using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Services.Html;

namespace TABP.Infrastructure.Services.Html;

public class InvoiceHtmlGenerationService : IInvoiceHtmlGenerationService
{
    public string GenerateHtml(Booking booking)
    {
        var html =
       $@"
                    <!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Invoice</title>
                    <style>
                        body {{
                          font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                          margin: 0;
                          padding: 0;
                          background-color: #f4f7f6;
                          color: #333;
                        }}
    
                        .container {{
                          max-width: 800px;
                          margin: 40px auto;
                          background: #fff;
                          padding: 20px 40px;
                          box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
                        }}
    
                        .invoice-header {{
                          text-align: center;
                          border-bottom: 2px solid #ddd;
                          padding-bottom: 20px;
                          margin-bottom: 20px;
                        }}
    
                        .invoice-header h1 {{
                          margin: 0;
                          font-size: 36px;
                          color: #2c3e50;
                        }}
    
                        .invoice-info {{
                          line-height: 1.6;
                          margin-bottom: 20px;
                        }}
    
                        .invoice-info p {{
                          margin: 5px 0;
                          font-size: 16px;
                        }}
    
                        .invoice-info strong {{
                          color: #2c3e50;
                        }}
    
                        .invoice-table table {{
                          width: 100%;
                          border-collapse: collapse;
                          margin-bottom: 20px;
                        }}
    
                        .invoice-table th, 
                        .invoice-table td {{
                          padding: 12px;
                          border: 1px solid #ddd;
                          text-align: left;
                          font-size: 15px;
                        }}
    
                        .invoice-table th {{
                          background-color: #2c3e50;
                          color: #fff;
                          text-transform: uppercase;
                          letter-spacing: 0.03em;
                        }}
    
                        .invoice-table tr:nth-child(even) {{
                          background-color: #f9f9f9;
                        }}
    
                        .invoice-footer {{
                          text-align: right;
                          font-size: 18px;
                          padding-top: 10px;
                          border-top: 2px solid #ddd;
                          margin-top: 20px;
                          font-weight: bold;
                          color: #2c3e50;
                        }}
    
                        @media (max-width: 600px) {{
                          .container {{
                            padding: 20px;
                          }}
                          .invoice-header h1 {{
                            font-size: 28px;
                          }}
                          .invoice-table th, .invoice-table td {{
                            padding: 10px;
                          }}
                        }}
                      </style>
                    </head>
                    <body>

                        <div class=""invoice-header"">
                            <h1>Invoice</h1>
                        </div>

                        <div class=""invoice-info"">
                            <p><strong>Invoice ID:</strong> {booking.Invoice.Id}</p>
                            <p><strong>Issue Date:</strong> {booking.Invoice.IssueDate:yyyy-MM-dd}</p>
                            <p><strong>Check-in Date:</strong> {booking.CheckInDate:yyyy-MM-dd}</p>
                            <p><strong>Check-out Date:</strong> {booking.CheckOutDate:yyyy-MM-dd}</p>
                            <p><strong>Hotel Address:</strong> {booking.Rooms.FirstOrDefault()?.RoomClass.Hotel.City}</p>
                            <p><strong>Payment Status:</strong> {booking.Invoice.PaymentStatus}</p>
                        </div>

                        <div class=""invoice-table"">
                            <table>
                                <thead>
                                    <tr>
                                        <th>Room Number</th>
                                        <th>Floor</th>
                                    </tr>
                                </thead>
                                <tbody>";

        foreach (var room in booking.Rooms)
        {
            html += $@"
                                    <tr>
                                        <td>{room.RoomNumber}</td>
                                        <td>{room.Floor}</td>
                                    </tr>";
        }

        html += $@"
                                </tbody>
                            </table>
                        </div>

                        <div class=""invoice-footer"">
                            <p><strong>Total Price: </strong>${booking.Invoice.TotalPrice:F2}</p>
                        </div>

                    </body>
                    </html>";

        return html;
    }

}

