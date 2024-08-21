using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.Endpoints;
using MinAPIMusicProject.Interfaces;
using MinAPIMusicProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MusicContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Local"))
        .UseLazyLoadingProxies());

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<IArtistService, ArtistService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddTrackEndpoints();
app.AddArtistEndpoints();
app.AddGenreEndpoints();

app.Run();