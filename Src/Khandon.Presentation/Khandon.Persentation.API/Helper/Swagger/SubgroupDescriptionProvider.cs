using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Zoon.Api.Helper.Swagger
{
    public class SubgroupDescriptionProvider : IApiDescriptionProvider
    {
        private readonly IOptions<ApiExplorerOptions> options;

        public SubgroupDescriptionProvider(IOptions<ApiExplorerOptions> options)
            => this.options = options;

        // Execute after DefaultApiVersionDescriptionProvider.OnProvidersExecuted
        public int Order => -1;

        public void OnProvidersExecuting(ApiDescriptionProviderContext context) { }

        public void OnProvidersExecuted(ApiDescriptionProviderContext context)
        {
            var format = options.Value.GroupNameFormat;
            var culture = CultureInfo.CurrentCulture;
            var results = context.Results;
            var newResults = new List<ApiDescription>(capacity: results.Count);

            for (var i = 0; i < results.Count; i++)
            {
                var result = results[i];
                var apiVersion = result.GetApiVersion();
                var versionGroupName = apiVersion.ToString(format, culture);

                // [ApiExplorerSettings(GroupName="...")] was NOT set so
                // nothing else to do
                if (result.GroupName == versionGroupName)
                {
                    continue;
                }

                // must be using [ApiExplorerSettings(GroupName="...")] so
                // concatenate it with the formatted API version
                result.GroupName += "-" + versionGroupName;

                // optional: add version grouping as well
                // note: this works because the api description will appear in
                // multiple, but different, documents
                //var newResult = result.Clone();

                //newResult.GroupName = versionGroupName;
                //newResults.Add(newResult);
            }

            newResults.ForEach(results.Add);
        }
    }
}
