var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var tickets = new[] {
    new {Id = 1, Title = "Pierwsze zgłoszenie"},
    new {Id = 2, Title = "Drugie zgłoszenie!"},
};

app.MapGet("/tickets", () => tickets);
app.MapGet("/tickets/{id}", (int id) =>
{
    var ticket = tickets.FirstOrDefault(x => x.Id == id);
    return ticket is null ? Results.NotFound() : Results.Ok(ticket);
});

app.Run();
