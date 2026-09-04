using Microsoft.AspNetCore.Http.HttpResults;

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

var tickets = app.MapGroup("/tickets");

tickets.MapGet("/", (ITicketStore store) => store.GetAll());

tickets.MapGet("/{id:int}",
    Results<Ok<Ticket>, NotFound> (int id, ITicketStore store) =>
    {
        if (store.GetById(id) is { } ticket)
        {
            return TypedResults.Ok(ticket);
        }
        return TypedResults.NotFound();
    });

tickets.MapPost("/",
    (CreateTicketRequest request, ITicketStore store, TicketMetrics metrics, ICurrentUser user) =>
    {
        var ticket = store.Add(request.Title);
        metrics.RecordCreated(user);
        return TypedResults.Created($"/tickets/{ticket.Id}", ticket);
    });

tickets.MapPut("/{id:int}",
    Results<NoContent, NotFound> (int id, UpdateTicketRequest request, ITicketStore store) =>
    {
        if (store.Update(id, request.Title))
        {
            return TypedResults.NoContent();
        }
        return TypedResults.NotFound();
    });

tickets.MapDelete("/{id:int}",
    Results<NoContent, NotFound> (int id, ITicketStore store) =>
    {
        if (store.Delete(id))
        {
            return TypedResults.NoContent();
        }
        return TypedResults.NotFound();
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
