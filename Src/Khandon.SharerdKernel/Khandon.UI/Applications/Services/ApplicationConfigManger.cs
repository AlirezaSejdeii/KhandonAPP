//using Blazored.LocalStorage;
//using Khandon.SharerdKernel.UI.Applications.IServices;
//using Khandon.SharerdKernel.UI.Helper;
//using MudBlazor;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Khandon.SharerdKernel.UI.Applications.Services
//{
//    public class ApplicationConfigManger : IApplicationConfigManger
//    {
//        private readonly ILocalStorageService localStorageService;
//        private readonly string key = "_asj_app_config";
//        private readonly ISnackbar snackbar;
//        private readonly ApplicationConfig applicationConfig;
//        public ApplicationConfigManger(ILocalStorageService localStorageService, ISnackbar snackbar, ApplicationConfig applicationConfig)
//        {
//            this.localStorageService = localStorageService;
//            this.snackbar = snackbar;
//            this.applicationConfig = applicationConfig;
//        }

//        public async Task Build()
//        {
//            if (await localStorageService.GetItemAsync<ApplicationConfigMangerDto>(key) is ApplicationConfigMangerDto config)
//            {
//                Console.WriteLine("true");
//                if (config.ExpireDate <= DateTime.Today)
//                {
//                    await GetData();
//                }
//            }
//            else
//            {
//                await GetData();
//            }

//            var myconfig = await localStorageService.GetItemAsync<ApplicationConfigMangerDto>(key);

//            Console.WriteLine(JsonConvert.SerializeObject(myconfig).ToString());
//            if (myconfig != null)
//            {
//                applicationConfig.SerErr = myconfig.ApplicationConfig.SerErr;
//                applicationConfig.ApiUrl = myconfig.ApplicationConfig.ApiUrl;
//            }
//        }

//        private async Task GetData()
//        {
//            try
//            {
//                using (HttpClient client = new HttpClient())
//                {
//                    var config_json = await client.GetAsync("https://localhost:7269/ClientAppSettings.json");
//                    if (config_json.IsSuccessStatusCode)
//                    {
//                        ApplicationConfig configDesrilized = JsonConvert.DeserializeObject<ApplicationConfig>(await config_json.Content.ReadAsStringAsync());
//                        ApplicationConfigMangerDto applicationConfigMangerDto = new ApplicationConfigMangerDto()
//                        {
//                            ApplicationConfig = configDesrilized,
//                            ExpireDate = DateTime.Today.AddDays(5)
//                        };

//                        await localStorageService.SetItemAsync(key, applicationConfigMangerDto);
//                    }
//                }
//            }
//            catch (Exception)
//            {
//                snackbar.Add("نرم افزار افلاین است", Severity.Warning);
//            }
//        }
//    }

//    public class ApplicationConfigMangerDto
//    {
//        public ApplicationConfig ApplicationConfig { get; set; }
//        public DateTime ExpireDate { get; set; }
//    }
//}
