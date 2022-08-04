using Blazored.LocalStorage;
using Khandon.Shared;
using Khandon.SharerdKernel.UI.Applications.IServices;
using Khandon.SharerdKernel.UI.Applications.Services;
using Khandon.SharerdKernel.UI.Helper;
using Khandon.SharerdKernel.UI.State;
using Khandon.SharerdKernel.UI.UtilitesService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Khandon.SharerdKernel.UI
{
    public static class Dependencies
    {
        public static IServiceCollection AddUI(this IServiceCollection services)
        {
            services.AddMudServices();

            services.AddScoped<NavigationService>();

            services.AddBlazoredLocalStorage(config =>
            {
                config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                config.JsonSerializerOptions.WriteIndented = false;
            });

            services.AddFluentValidationForShared();

            services.AddScoped<IBookHttpService, BookHttpService>();
            services.AddScoped<IChapterHttpService, ChapterHttpService>();
            services.AddScoped<IStudyHttpService, StudyHttpService>();
            services.AddScoped<IBookGroupHttpService, BookGroupHttpService>();

            //services.AddScoped<IApplicationConfigManger, ApplicationConfigManger>();

            services.AddScoped<BookStateService>();
            services.AddScoped<BookGroupStateService>();
            services.AddScoped<UserStateService>();


            //services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            //services.AddScoped<IUserState, CustomAuthenticationStateProvider>();


            //services.AddScoped<CustomAuthenticationStateProvider>();

            services.AddScoped<AuthenticationStateProvider, UserStateService>(
                provider => provider.GetRequiredService<UserStateService>()
                );

            //services.AddScoped<IUserState, UserStateService>(
            //    provider => provider.GetRequiredService<UserStateService>()
            //    );

            services.AddScoped<IHttpService, HttpService>();

            services.AddAuthorizationCore();
            return services;
        }
    }
}
