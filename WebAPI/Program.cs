using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new AutofacBusinessModule());
    });


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<IProductService, ProductManager>();
//builder.Services.AddSingleton<IProductDal, EfProductDal>();

//-- Autofac aktif olduktan sonra .NET Core'un servisleri devreye girebiliyor ANCAK di�er servisler mesela HttpContextAccessor, MemoryCacheManager, devreye giremiyor.
//ServiceTool di�er servisleri devreye koyabilmek i�in yaz�ld�.
//Bu �ekildede Busines katman�nda HttpContextAccessor'a eri�ebiliriz.
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//ServiceTool.Create(builder.Services);

//API'den eri�ime izin veriyoruz.
builder.Services.AddCors();

//Bu sistemde Authentication JWT kullan�lacak, haberin olsun dedi�imiz yer buras�.

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });

builder.Services.AddDependencyResolvers(new ICoreModule[]
{
    new CoreModule()
});








var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Apiden eri�ime izin veriyoruz. AllowAnyHeader:Apiden ne istek gelirse gelsin izin ver.
//E�er birden fazla yerden istek gelmesini istiyorsak domainleri virg�l koyup ekleriz.

app.ConfigureCustomExceptionMiddleware();//Backend exceptionlar�n� yakal�yoruz. Bu middleware � biz ekledik. Tum sistem try catch blogu arasina alindi.

app.UseCors(builder=>builder.WithOrigins("http://localhost:4200").AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();//Biz ekledik

app.UseAuthorization();

app.MapControllers();

app.Run();
