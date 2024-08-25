using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.Endpoints;
using MinAPIMusicProject.Interfaces;
using MinAPIMusicProject.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MusicContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Local"))
        .UseLazyLoadingProxies());

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<IArtistService, ArtistService>();
builder.Services.AddTransient<IGenreService, GenreService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITrackService, TrackService>();
builder.Services.AddTransient<ILikeService, LikeService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("SessionTimeout"));
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("TokenKey")!)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession(); 

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.AddTrackEndpoints();
app.AddArtistEndpoints();
app.AddGenreEndpoints();
app.AddPlaylistEndpoints();
app.AddUserEndpoints();
app.AddAuthEndpoints();

app.Run();