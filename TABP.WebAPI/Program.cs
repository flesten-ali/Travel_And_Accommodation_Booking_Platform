using TABP.Application;
using TABP.Infrastructure;
using TABP.Presentation;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

///////////////////////////////////////////////////////////////////

builder.Services.AddApplication()
                 .AddInfrastructure(builder.Configuration)
                 .AddPresentation();

///////////////////////////////////////////////////////////////////


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
