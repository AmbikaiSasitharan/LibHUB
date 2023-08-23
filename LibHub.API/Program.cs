using LibHub.API.Data;
using LibHub.API.Repository;
using LibHub.API.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connecting The Database to the api project 
builder.Services.AddDbContext<LibHubDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibHubConnection"))
);

//Code to register the BookDescriptionRepository class for the Dependancy Injection System
builder.Services.AddScoped<IBookDescriptionRepository, BookDescriptionRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IRatingRespository, RatingRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IRenewalRepository, RenewalRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();


//Code to Handle Serialization error caused by related entities referencing one another
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
    policy.WithOrigins("http://localhost:7244", "https://localhost:7244")
    .AllowAnyMethod()
    .WithHeaders(HeaderNames.ContentType)
    );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
