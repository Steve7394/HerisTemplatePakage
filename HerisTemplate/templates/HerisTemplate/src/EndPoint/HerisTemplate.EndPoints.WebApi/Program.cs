using HerisTemplate.EndPoints.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();
await app.ConfigurePipeline();
app.Run();
