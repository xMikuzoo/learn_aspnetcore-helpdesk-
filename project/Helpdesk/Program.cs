var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITicketStore, InMemoryTicketStore>();

builder.Services.AddTransient<ITransientStamp, Stamp>();
builder.Services.AddScoped<IScopedStamp, Stamp>();
builder.Services.AddSingleton<ISingletonStamp, Stamp>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, HeaderCurrentUser>();

builder.Services.AddSingleton<TicketMetrics>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/tickets", (ITicketStore ticketStore) => ticketStore.GetAll());
app.MapGet("/tickets/{id}", (int id, ITicketStore ticketStore) =>
{
    var ticket = ticketStore.GetById(id);
    return ticket is null ? Results.NotFound() : Results.Ok(ticket);
});
app.MapPost("/tickets", (string title, ITicketStore ticketStore, TicketMetrics metrics, ICurrentUser user) =>
{
    var ticket = ticketStore.Add(title);
    metrics.RecordCreated(user);
    return Results.Ok(ticket);
});

app.MapGet("/metrics", (TicketMetrics metrics) => new { created = metrics.Created });

app.MapGet("/lifetimes", (
    ITransientStamp t1, ITransientStamp t2,
    IScopedStamp s1, IScopedStamp s2,
    ISingletonStamp g1) => new
    {
        transient1 = t1.Id,
        transient2 = t2.Id,
        scoped1 = s1.Id,
        scoped2 = s2.Id,
        singleton = g1.Id
    });

app.Run();
