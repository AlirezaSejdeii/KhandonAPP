global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
using FluentValidation.AspNetCore;
using Khandon.Core;
using Khandon.Infrastructure.Book;
using Khandon.Infrastructure.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Zoon.Api.Helper.Swagger;
using Khandon.Persentation.API.Helper.SeedData;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddUsersServices(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(opt =>
{
    opt.RegisterValidatorsFromAssembly(
                    Assembly.GetAssembly(typeof(Khandon.Shared.ValidationInject)));
});
builder.Services.AddMvc().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApiDescriptionProvider, SubgroupDescriptionProvider>());
builder.Services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, SwaggerDocApiVersionConfiguration>();

builder.Services.AddVersionedApiExplorer(setupAction =>
{
    //setupAction.GroupNameFormat = "'v'VV";
    setupAction.GroupNameFormat = "'v'VVV";
    setupAction.SubstituteApiVersionInUrl = true;
});

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
    setupAction.ReportApiVersions = true;
});

var apiVersionDescriptionProvider =
    builder.Services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.OrderActionsBy(x => x.RelativePath);

    setupAction.OperationFilter<SwaggerVersionFilter>();

    //add xml document
    var xmlCommentFiles = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    setupAction.IncludeXmlComments(xmlCommentFiles);
});

//infrastructure

builder.Services.AddBookInfrastructure(builder.Configuration);

//build database and seed some data
builder.MigrationToDatabase();

builder.Services.AddCore();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
                        //builder.SetIsOriginAllowedToAllowWildcardSubdomains();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    
//}
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiDescriptionGroupCollectionProvider>();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    foreach (var item in provider.ApiDescriptionGroups.Items.OrderBy(x => x.Items[0].GetApiVersion().Status).ThenByDescending(x => x.Items[0].GetApiVersion()).ThenBy(x => x.GroupName))
    {
        if (item.Items[0].GetApiVersion().ToString() == "0")
        {
            c.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", $"API BETA {item.GroupName.ToUpperInvariant()}");
            continue;
        }
        c.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", $"API {item.GroupName.ToUpperInvariant()}");
    }

    c.ConfigObject.Filter = string.Empty;
    c.ConfigObject.DisplayRequestDuration = true;

    c.ConfigObject.DefaultModelRendering = Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model;
    c.RoutePrefix = string.Empty;

    c.EnableValidator();
    c.DisplayOperationId();
    c.EnableDeepLinking();
});
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
