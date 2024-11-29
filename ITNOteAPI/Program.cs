using System.Text;
using ITNOte.me.Model.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IStorage, Database>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "BaraGodLike", // Замените на ваш издатель
            ValidAudience = "ItnoteMeUser", // Замените на вашу аудиторию
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("PfxtvRhbxfnmRjulfYbrnjYtCksibnJXtvVsUjdjhbvVytRf;tnczXnjVsLfdyjYt:bdsPf;ukbcmBGj-nb[jymreLjujhbv")) // Секретный ключ
        };
    });

// Добавляем авторизацию
builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ITNote API v1");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Public API").AllowAnonymous(); 
app.MapGet("/protected", () => "Protected API").RequireAuthorization(); 

app.UseHttpsRedirection();

app.MapControllers();

app.Run();