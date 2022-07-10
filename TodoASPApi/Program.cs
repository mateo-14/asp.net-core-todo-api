using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoASPApi;
using TodoASPApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.EnableAnnotations();
    options.SwaggerDoc("v1",
        new OpenApiInfo {
            Version = "v1",
            Title = " ASP.NET Core TODO API",
            Description = "Made to learn Backend Development with ASP.NET Core",
            Contact = new OpenApiContact { Name = "My web", Url = new Uri("https://mateoledesma.vercel.app") },
        }
    );
});



builder.Services.AddDbContext<TodoAppContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING")));


builder.Services.AddScoped<ITodosService, TodosService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<TodoAppContext>();
    context.Database.Migrate();
}


app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
    options.RoutePrefix = "";
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
