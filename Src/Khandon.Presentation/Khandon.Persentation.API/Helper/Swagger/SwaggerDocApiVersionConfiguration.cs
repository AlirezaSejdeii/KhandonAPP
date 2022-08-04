using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zoon.Api.Helper.Swagger
{
    public class SwaggerDocApiVersionConfiguration : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiDescriptionGroupCollectionProvider groupProvider;
        private readonly IApiVersionDescriptionProvider provider;

        public SwaggerDocApiVersionConfiguration(IApiDescriptionGroupCollectionProvider groupProvider,

            IApiVersionDescriptionProvider provider)
        {
            this.groupProvider = groupProvider;
            this.provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {

            var docs = options.SwaggerGeneratorOptions.SwaggerDocs;

            for (var i = 0; i < groupProvider.ApiDescriptionGroups.Items.Count; i++)
            {
                var group = groupProvider.ApiDescriptionGroups.Items[i];

                docs.Add(group.GroupName, CreateInfoForApiVersion(group));
            }
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiDescriptionGroup description)
        {
            var groupApiVersion = description.Items[0];

            var info = new OpenApiInfo
            {
                Title = description.GroupName,
                Version = groupApiVersion.GetApiVersion().ToString(),
            };

            if (groupApiVersion.IsDeprecated())
            {
                info.Description += $"{Environment.NewLine}This API version has been deprecated";
            }

            return info;
        }


    }
}
