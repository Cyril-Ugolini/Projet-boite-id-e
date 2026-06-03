using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IdeaBox.Api.Data;
using IdeaBox.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services métier
builder.Services.AddScoped<IIdeeService, IdeeService>();
builder.Services.AddScoped<ICommentaireService, CommentaireService>();
builder.Services.AddScoped<IVoteService, VoteService>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFront", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Logs
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// ── Middleware ────────────────────────────────────────────

// app.UseExceptionHandler(err => err.Run(async ctx =>
// {
//     ctx.Response.StatusCode = 500;
//     await ctx.Response.WriteAsJsonAsync(new
//     {
//         error  = "Erreur interne du serveur",
//         status = 500
//     });
// }));

// Gestion globale des exceptions
app.UseExceptionHandler(err => err.Run(async ctx =>
{
    ctx.Response.StatusCode = 500;
    await ctx.Response.WriteAsJsonAsync(new
    {
        error  = "Erreur interne du serveur",
        status = 500
    });
}));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFront");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();