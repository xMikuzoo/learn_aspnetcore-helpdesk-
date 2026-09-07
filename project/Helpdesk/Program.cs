using Helpdesk.Common;
using Helpdesk.Common.Persistence;
using Helpdesk.Features.Diagnostics;
using Helpdesk.Features.Tickets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddCurrentUser();
builder.Services.AddDomainErrors();
builder.Services.AddDispatcher();
builder.Services.AddValidation();
builder.Services.AddTickets();
builder.Services.AddDiagnostics();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/", () => "Hello World!");

app.MapTickets();
app.MapDiagnostics();

app.Run();
