using Helpdesk.Common;
using Helpdesk.Features.Diagnostics;
using Helpdesk.Features.Tickets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTickets();
builder.Services.AddDiagnostics();
builder.Services.AddCurrentUser();

var app = builder.Build();

app.MapTickets();
app.MapDiagnostics();

app.Run();
