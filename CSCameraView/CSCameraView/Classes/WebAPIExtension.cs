using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace CSCameraView.Classes
{
    public class WebAPIExtension
    {
        #region Methods

        const string baseURL = "http://sesconnect.selectses.com/";

        const string cameraBaseURL = "http://sesconnect2.selectses.com/";

        public string GetBaseUrl()
        {
            string baseURL = string.Empty;
            switch (Settings.UrlSelectionMode)
            {
                case "Cancel":
                    break;
                case "Live":
                    baseURL = "http://sesconnect.selectses.com/";
                    break;
                case "Dev":
                    baseURL = "http://sesconnect.selectses.com/";
                    break;
                case "QA":
                    baseURL = "http://sesconnect.selectses.com/";
                    break;
                default:
                    baseURL = "http://sesconnect.selectses.com/";
                    break;
            }
            return baseURL;


        }

        protected Uri GetUri(string controller)
        {
            string url = string.Format("{0}/{1}", GetBaseUrl(), controller);
            return new Uri(url);
        }

        protected Uri GetCameraUri(string controller)
        {
            string url = string.Format("{0}/{1}", cameraBaseURL, controller);
            return new Uri(url);
        }

        protected Uri GetLogUri(string controller)
        {
            const string baseURL = "http://sesconnect.selectses.com/";
            string url = string.Format("{0}/{1}", baseURL, controller);
            return new Uri(url);
        }




        private HttpClient GetHttpClientwithORWithoutAuthorization(bool ISAuthorization)
        {
            var httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(30),

                // 100 MB buffer.
                MaxResponseContentBufferSize = 1024 * 1024 * 100
            };


            if (ISAuthorization)
            {
                if (Settings.AccessToken != null && Settings.refreshTokenExpiration > DateTime.UtcNow)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Settings.AccessToken);
                }
                else
                {
                    try
                    {

                        Helper.CheckTokenNeedToRefresh();

                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Settings.AccessToken);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return httpClient;
            }
            else
            {
                return httpClient;
            }

        }
        protected string GetAuthenticationToken()
        {
            var token = string.Format("Username:{0} Password:{1} DeviceId:{2}", "username", "password", "device");

            return token;
        }

        #endregion Methods

        #region WebApi Methods



        /// <summary>
        /// Get Async Generic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller">string</param>
        /// <returns>Task of T</returns>
        protected async Task<T> GetAsync<T>(string controller, bool IsCameraUrl = false) where T : new()
        {
            var client = GetHttpClientwithORWithoutAuthorization(false);

            HttpResponseMessage response;
            if (IsCameraUrl)
            {
                response = await client.GetAsync(GetCameraUri(controller));
            }
            else
            {
                response = await client.GetAsync(GetUri(controller));
            }

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                throw new Exception(responseContent);
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            T obj = default(T);
            var json = await response.Content.ReadAsStringAsync();
            obj = JsonConvert.DeserializeObject<T>(json, settings);
            return obj;
        }

        protected async Task<T> GetAsyncAuthorized<T>(string controller) where T : new()
        {
            var client = GetHttpClientwithORWithoutAuthorization(true);

            var response = await client.GetAsync(GetUri(controller));

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                throw new Exception(response.StatusCode.ToString());
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            T obj = default(T);
            var json = await response.Content.ReadAsStringAsync();
            obj = JsonConvert.DeserializeObject<T>(json, settings);
            return obj;
        }

        /// <summary>
        /// Put Async Generic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller">string</param>
        /// <param name="payload">T</param>
        /// <returns>Task</returns>
        /// <remarks>Update</remarks>
        protected async Task PutAsync<T>(string controller, T payload) where T : new()
        {
            var client = GetHttpClientwithORWithoutAuthorization(true);

            var data = JsonConvert.SerializeObject(payload);
            var content = new StringContent(data, UnicodeEncoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", this.GetAuthenticationToken());

            var response = await client.PutAsync(GetUri(controller), content);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new Exception(responseContent);
            }
        }

        /// <summary>
        /// Post Async Generic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller">string</param>
        /// <param name="payload">T</param>
        /// <returns>Task</returns>
        /// <remarks>Insert</remarks>
        protected async Task<TReturn> PostAsync<T, TReturn>(string controller, T payload)
            where T : new()
            where TReturn : new()
        {
            var client = GetHttpClientwithORWithoutAuthorization(true);

            var data = JsonConvert.SerializeObject(payload);
            var content = new StringContent(data, UnicodeEncoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));



            var response = await client.PostAsync(GetUri(controller), content);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new Exception(responseContent);
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TReturn>(json);
        }

        /// <summary>
        /// Post Async Generic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller">string</param>
        /// <param name="payload">T</param>
        /// <returns>Task</returns>
        /// <remarks>Insert</remarks>
        protected async Task<TReturn> PostAsyncV1<T, TReturn>(string controller, T payload)
            where T : new()
            where TReturn : new()
        {
            var client = GetHttpClientwithORWithoutAuthorization(false);

            var data = JsonConvert.SerializeObject(payload);
            var content = new StringContent(data, UnicodeEncoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));



            var response = await client.PostAsync(GetUri(controller), content);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new Exception(responseContent);
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TReturn>(json);
        }

        protected async Task<TReturn> PostAsyncV2<T, TReturn>(string controller, T payload)
        where T : new()
        where TReturn : new()
        {
            var client = GetHttpClientwithORWithoutAuthorization(false);

            var data = JsonConvert.SerializeObject(payload);
            var content = new StringContent(data, UnicodeEncoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));



            var response = await client.PostAsync(GetUri(controller), content);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TReturn>(json);
        }


        /// <summary>
        /// Post File to server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Url">string</param>
        /// <param name="payload">T</param>
        /// <returns>Task</returns>
        /// <remarks>Insert</remarks>
        protected async Task<bool> PostFile(string requestUri, HttpContent content)
        {
            try
            {
                var client = GetHttpClientwithORWithoutAuthorization(true);
                var response = await client.PostAsync(GetLogUri(requestUri), content).ConfigureAwait(true);
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseContent);
                }
            }
            catch (Exception ex)
            {


            }
            return true;
        }


        /// <summary>
        /// Post Async Generic
        /// </summary>
        /// <param name="controller">string</param>
        /// <param name="payload">Data to be sent.</param>
        /// <returns>Task</returns>
        /// <remarks>Insert</remarks>
        protected async Task<bool> PostAsync<T>(string controller, T payload)
            where T : new()
        {
            var client = GetHttpClientwithORWithoutAuthorization(true);

            var data = JsonConvert.SerializeObject(payload);
            var content = new StringContent(data, UnicodeEncoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsync(GetUri(controller), content);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                throw new Exception(responseContent);
            }
            return true;
        }

        #endregion WebApi Methods

        #region WebApi Custom Methods

        public async Task<bool> UrlExists(string url)
        {
            var client = GetHttpClientwithORWithoutAuthorization(true);
            var httpRequestMsg = new HttpRequestMessage(HttpMethod.Head, url);
            var response = await client.SendAsync(httpRequestMsg);
            return response.IsSuccessStatusCode;
        }

        #endregion WebApi Custom Methods

        protected async Task<string> GetAsyncstring(string controller)
        {
            var client = GetHttpClientwithORWithoutAuthorization(true);

            var response = await client.GetAsync(GetUri(controller));

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                throw new Exception(responseContent);
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(json, settings);
        }

    }
}
