using LibHub.Web;
using LibHub.Web.Services;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSyncfusionBlazor();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7144/") });

builder.Services.AddScoped<IBookDescriptionInventoryService, BookDescriptionInventoryService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthorService, AuthorsService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IRenewalService, RenewalService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IEmailService, EmailService>();



await builder.Build().RunAsync();

