using Helpdesk.Common;
using Helpdesk.Features.Diagnostics;
using Helpdesk.Features.Tickets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCurrentUser();
builder.Services.AddDispatcher();
builder.Services.AddTickets();
builder.Services.AddDiagnostics();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapTickets();
app.MapDiagnostics();

app.Run();
