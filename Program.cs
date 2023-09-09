using Backend;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Timers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
});
builder.Services.AddDbContext<DB>(options => 
options.UseSqlite("Data Source='data2.db'")
);
builder.Services.AddAuthentication(configureOptions =>
{
    configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(configureOptions =>
{
    configureOptions.RequireHttpsMetadata = false;
    configureOptions.SaveToken = true;
    configureOptions.IncludeErrorDetails = true;
    configureOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //default : false
        ValidateAudience = true, //default : false
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RequireSignedTokens = true,
        RequireExpirationTime = true,
        ValidIssuer = "http://localhost:4200/",
        ValidAudience = "http://localhost:4200/",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("3333333333333333")),
        TokenDecryptionKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("44444")),
        ClockSkew = TimeSpan.Zero // default: 5 min
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

bool isRunning = false;
var client = new HttpClient();
app.MapControllers();
{
    var t = new System.Timers.Timer(30000);
    t.Enabled = true;
    t.Elapsed += T_Elapsed;
}
 
app.Run();

void T_Elapsed(object? sender, ElapsedEventArgs e)
{
    if (!isRunning)
    {
        isRunning = true;
        var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<DB>();
        var ddd = db.URLs.ToList();
        for (int i = 0; i < ddd.Count(); i++)
        {
            var req = new Request()
            {
                DateTime = DateTime.Now,
                URLId = ddd[i].Id,
                Id = 0
            };
            try
            {
                var res = client.GetAsync(ddd[i].Address).Result;
                req.Result =(int) res.StatusCode;
            }
            catch (Exception)
            {
                req.Result =StatusCodes.Status500InternalServerError;
            }
            db.Add(req);

        }
        db.SaveChanges();
        isRunning = false;
    }
}