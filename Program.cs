using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentCrudAppWithEFCoreCodeFirst.Data;
using StudentCrudAppWithEFCoreCodeFirst.Middleware;
using StudentCrudAppWithEFCoreCodeFirst.Models;
using StudentCrudAppWithEFCoreCodeFirst.Repository;
using StudentCrudAppWithEFCoreCodeFirst.Services;
using System.Text;
using Asp.Versioning.ApiExplorer;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddApiVersioning(options=> {
    options.DefaultApiVersion = new ApiVersion(1, 0); //default version
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    //use URL versioning:api/v1/students
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
}); 
var jwt = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer(options => {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context => 
            {
                context.Token = context.Request.Cookies["jwt"];
                return Task.CompletedTask;
            }
        };
    
    });
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("angular", policy =>
    {
        policy.WithOrigins("https://frontend-demo-studentapp.vercel.app")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title="Student API V1",
        Version="v1",
    });
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Student API V2",
        Version = "v2",
    });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Enter Jwt with Bearer Prefix",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
//builder.Services.AddRateLimiter(options => {
//    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context=>
//    RateLimitPartition.GetFixedWindowLimiter(
//        partitionKey:context.Connection.RemoteIpAddress?.ToString()??"global",
//        factory: _=>new FixedWindowRateLimiterOptions
//        {
//            PermitLimit=5, //max 5 requests
//            Window=TimeSpan.FromSeconds(60),//per 60 seconds
//            QueueProcessingOrder=QueueProcessingOrder.OldestFirst,
//            QueueLimit=0
//        }));
//    options.OnRejected = async (context, token) => {
//        context.HttpContext.Response.StatusCode = 429;
//        await context.HttpContext.Response.WriteAsync("Too many requests please try after some time");
//    };
//});

builder.Services.AddRateLimiter(options => 
{
    options.AddPolicy("fixed", context =>
    RateLimitPartition.GetFixedWindowLimiter(
        partitionKey:context.Connection.RemoteIpAddress?.ToString()??"fixedwindow",
        factory:_=>new FixedWindowRateLimiterOptions 
        {
            PermitLimit=2,
            Window=TimeSpan.FromSeconds(60)
        }));
    options.AddPolicy("sliding", context =>
    RateLimitPartition.GetSlidingWindowLimiter(
        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "slidingwindow",
        factory: _ => new SlidingWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromSeconds(60),
            SegmentsPerWindow=2
        }));
options.OnRejected = async (context, token) => {
    context.HttpContext.Response.StatusCode = 429;
    await context.HttpContext.Response.WriteAsync("Too many requests please try after some time");
};
});











builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStudentService,StudentService>();//scoped
builder.Services.AddTransient<IStudentGradeService,StudentGradeService>();//transient
builder.Services.AddSingleton<IAppLogger,AppLogger>();//singleton
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Student API V2");
    });
//}
app.UseMiddleware<ExceptionMiddleware>();//custom middleware

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseCors("angular");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
