using Blazored.LocalStorage;
using Khandon.Shared.Dto.Base.User;
using Khandon.SharerdKernel.UI.Applications.IServices;
using Khandon.SharerdKernel.UI.Helper;
using Khandon.SharerdKernel.UI.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Khandon.SharerdKernel.UI.Applications.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient httpClient;

        private JsonSerializerOptions defaultJsonSerializerOptions =>
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };


        private readonly ILocalStorageService localStorageService;


        public HttpService(ILocalStorageService localStorageService)
        {
            httpClient = new HttpClient();
            this.localStorageService = localStorageService;


        }

        async Task<AuthenticationHeaderValue> getAuthenticationHeaderValue()
        {
            var result = await localStorageService.GetItemAsync<UserState>("_Khandon_auth_token");


            if (result != null && result.tokenDto != null)
            {
                return new AuthenticationHeaderValue("Bearer", result.tokenDto.Token);
            }
            return null;


        }
        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            httpClient.DefaultRequestHeaders.Authorization = await getAuthenticationHeaderValue();

            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache,no-store");
            HttpResponseMessage responseHTTP = await httpClient.GetAsync(url);
            Console.WriteLine(responseHTTP.Content.ReadAsStringAsync().Result.Length);
            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await Deserialize<T>(responseHTTP, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<T>(response, true, responseHTTP);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T data)
        {
            httpClient.DefaultRequestHeaders.Authorization = await getAuthenticationHeaderValue();

            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T data)
        {
            httpClient.DefaultRequestHeaders.Authorization = await getAuthenticationHeaderValue();

            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T data)
        {

            httpClient.DefaultRequestHeaders.Authorization = await getAuthenticationHeaderValue();

            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<TResponse>(response, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<TResponse>(responseDeserialized, true, response);
            }
            else
            {
                return new HttpResponseWrapper<TResponse>(default, false, response);
            }
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data)
        {

            httpClient.DefaultRequestHeaders.Authorization = await getAuthenticationHeaderValue();


            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<TResponse>(response, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<TResponse>(responseDeserialized, true, response);
            }
            else
            {
                return new HttpResponseWrapper<TResponse>(default, false, response);
            }
        }

        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            httpClient.DefaultRequestHeaders.Authorization = await getAuthenticationHeaderValue();


            var responseHTTP = await httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

        private async Task<T> Deserialize<T>(HttpResponseMessage httpResponse, JsonSerializerOptions options)
        {
            httpClient.DefaultRequestHeaders.Authorization = await getAuthenticationHeaderValue();


            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, options);
        }
    }
}
