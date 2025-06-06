using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using legal_document_analyzer.Infrastructure;
using legal_document_analyzer.Properties.Configuration;
using System.Text;
using Microsoft.AspNetCore.Rewrite;
using legal_document_analyzer.Domain.Repositories;
using legal_document_analyzer.Infrastructure.Persistence;
using legal_document_analyzer.Infrastructure.Services;
using legal_document_analyzer.Application.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Bind Swagger options from config
var swaggerOptions = new SwaggerOptions();
builder.Configuration.GetSection("Swagger").Bind(swaggerOptions);

// Add services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddScoped<ILegalDocumentRepository, LegalDocumentRepository>();
builder.Services.AddScoped<IClauseRepository, ClauseRepository>();
builder.Services.AddScoped<IDocumentSummaryRepository, DocumentSummaryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IAzureOpenAiService, AzureOpenAiService>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ILegalDocumentParser>(provider =>
    new LegalDocumentParser(
        //provider.GetRequiredService<IHttpClientFactory>().CreateClient(),
/*   builder.Configuration.GetValue<string>("DeepSeekR1Model:Endpoint"),
   builder.Configuration.GetValue<string>("DeepSeekR1Model:ApiKey"),
   builder.Configuration.GetValue<string>("DeepSeekR1Model:Model")*/

     builder.Configuration.GetValue<string>("OpenAI4oMiniModel:Enpoint1"),
     builder.Configuration.GetValue<string>("OpenAI4oMiniModel:ApiKey"),
     builder.Configuration.GetValue<string>("OpenAI4oMiniModel:Model"),
     builder.Configuration.GetValue<int>("OpenAI4oMiniModel:Max_Tokens")
));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
options.AddDefaultPolicy(builder =>
{
   builder.WithOrigins("http://localhost:4200", "https://localhost:4200", "https://frontend-blondie.azurewebsites.net")
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials();
});
});


builder.Services.AddSwaggerGen(options =>
{
options.SwaggerDoc(builder.Configuration["Swagger:Version"], new OpenApiInfo
{
   Title = builder.Configuration["Swagger:Title"],
   Version = builder.Configuration["Swagger:Version"],
   Description = builder.Configuration["Swagger:Description"]
});

options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
   In = ParameterLocation.Header,
   Description = "Enter JWT token",
   Name = "Authorization",
   Type = SecuritySchemeType.Http,
   Scheme = "bearer"
});

options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
       Array.Empty<string>()
   }
});
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
options.TokenValidationParameters = new TokenValidationParameters
{
   ValidateIssuer = true,
   ValidateAudience = true,
   ValidateLifetime = true,
   ValidateIssuerSigningKey = true,
   ValidIssuer = builder.Configuration["Jwt:Issuer"],
   ValidAudience = builder.Configuration["Jwt:Audience"],
   IssuerSigningKey = new SymmetricSecurityKey(
       Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
};
});


var app = builder.Build();

// Ensure database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
dbContext.Database.EnsureCreated();
}
app.UseHttpsRedirection();

// Use CORS before authentication
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
c.SwaggerEndpoint($"/swagger/{swaggerOptions.Version}/swagger.json", swaggerOptions.Title);
});

app.MapControllers();

app.Run();