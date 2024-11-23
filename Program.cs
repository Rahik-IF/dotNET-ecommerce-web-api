
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_api.controllers;
using web_api.data;
using web_api.Interfaces;
using web_api.Services;

var builder = WebApplication.CreateBuilder(args);
// add services to the controller
builder.Services.AddControllers();
// add automapper service
builder.Services.AddAutoMapper(typeof (Program));
// add category service
builder.Services.AddScoped<ICategoryService ,CategoryService>();
//connnect with database service
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
// add global annotations
builder.Services.Configure<ApiBehaviorOptions>(
 options =>
 {
     options.InvalidModelStateResponseFactory = context =>
       {      
                var errors = context.ModelState
               .Where(e => e.Value != null && e.Value.Errors.Count > 0)
               .SelectMany(
                   e => e.Value != null ? e.Value.Errors.Select(x=> x.ErrorMessage) : new string[0]
               ).ToList();

           return new BadRequestObjectResult(
             ApiResponse<object>.ErrorResponse(errors, 400, "Validation Failed")
           );
       };
 }
);
// add a service to use swagger tools
builder.Services.AddEndpointsApiExplorer();
// add a service for swagger generator
builder.Services.AddSwaggerGen();

var app = builder.Build();

// add swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Results => Ok(200), Created(201), Content(HTML response), NoContent(204), Json, File, Problem
// for launching
app.MapGet("/", () =>
{
    var response = new
    {
        message = "success"
    };
    return Results.Json(response);
});

app.MapControllers();
app.Run();








