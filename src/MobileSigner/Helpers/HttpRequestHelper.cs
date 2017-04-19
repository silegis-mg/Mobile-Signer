using Almg.MobileSigner.Model;
using Almg.MobileSigner.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Helpers
{
	public class HttpRequest
	{
        private static ILog log = DependencyService.Get<ILog>(DependencyFetchTarget.GlobalInstance);
        private static TimeSpan timeout = TimeSpan.FromSeconds(120);

        public async static Task<HttpResponseMessage> GetHttpResponse(string url)
		{
            var token = GetToken();
            log.WriteLine("GET: " + url);

            HttpClientHandler handler = new HttpClientHandler();
			var httpClient = new HttpClient(handler);
            httpClient.Timeout = timeout;
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

			AddTokenHeader(request, token);

			HttpResponseMessage response = await httpClient.SendAsync(request);
			if (!response.IsSuccessStatusCode)
			{
				throw new HttpException(response.StatusCode);
			}
			return response;
		}

        public async static Task<HttpResponseMessage> Post(string url, HttpContent httpContent)
        {
            var token = GetToken();

            HttpClientHandler handler = new HttpClientHandler();
            var httpClient = new HttpClient(handler);
            httpClient.Timeout = timeout;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            if (httpContent != null)
            {
                request.Content = httpContent;
            }

            if(token!=null)
            {
                AddTokenHeader(request, token);
            }

            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpException(response.StatusCode);
            }
            return response;
        }

        public async static Task<HttpResponseMessage> Put(string url, string token = null)
        {
            if (token == null && Application.Current.Properties.ContainsKey(Const.CONFIG_API_KEY))
            {
                token = (string)Application.Current.Properties[Const.CONFIG_API_KEY];
            }

            HttpClientHandler handler = new HttpClientHandler();
            var httpClient = new HttpClient(handler);
            httpClient.Timeout = timeout;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);
            if (token != null)
            {
                AddTokenHeader(request, token);
            }

            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpException(response.StatusCode);
            }
            return response;
        }

        public async static Task<HttpResponseMessage> Delete(string url, string token = null)
        {
            if (token == null && Application.Current.Properties.ContainsKey(Const.CONFIG_API_KEY))
            {
                token = (string)Application.Current.Properties[Const.CONFIG_API_KEY];
            }

            HttpClientHandler handler = new HttpClientHandler();
            var httpClient = new HttpClient(handler);
            httpClient.Timeout = timeout;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
            if (token != null)
            {
                AddTokenHeader(request, token);
            }

            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpException(response.StatusCode);
            }
            return response;
        }

        public async static Task<HttpResponseMessage> Post(string url, Dictionary<string, string> postParams)
        {
            FormUrlEncodedContent formContent = null;
            if (postParams != null)
            {
                formContent = new FormUrlEncodedContent(postParams);
            }
            return await Post(url, formContent);
        }

        public async static Task<HttpResponseMessage> Post(string url, byte[] postBody)
        {
            ByteArrayContent content = null;
            if (postBody != null)
            {
                content = new ByteArrayContent(postBody);
            }
            return await Post(url, content);
        }

        public async static Task<HttpResponseMessage> Post(string url)
        {
            return await Post(url, (HttpContent)null);
        }

        public async static Task<HttpResponseMessage> Post(string url, string postBody, Encoding encoding, string mediatype)
        {
            StringContent content = null;
            if (postBody != null)
            {
                content = new StringContent(postBody, encoding, mediatype);
            }
            return await Post(url, content);
        }

        public static async Task<string> GetStr(string url)
		{
			HttpResponseMessage response = await GetHttpResponse(url);
			return await response.Content.ReadAsStringAsync();
		}

		public static async Task<T> GetJson<T>(string url)
		{
            try
            {
                HttpResponseMessage response = await GetHttpResponse(url);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpException(response.StatusCode);
                }
                String json = await response.Content.ReadAsStringAsync();
                T t = JsonHelper.FromJsonString<T>(json);
                return t;
            } catch(Exception e)
            {
                throw e;
            }
		}

		public static async Task<Stream> GetStream(string url)
		{
			HttpResponseMessage response = await GetHttpResponse(url);
			return await response.Content.ReadAsStreamAsync();
		}

        public static async Task<byte[]> GetByteArray(string url)
        {
            HttpResponseMessage response = await GetHttpResponse(url);
            return await response.Content.ReadAsByteArrayAsync();
        }

        public static async Task DownloadFileToStream(string url, Stream stream)
        {
            HttpResponseMessage response = await GetHttpResponse(url);
            if(response.IsSuccessStatusCode)
            {
                await response.Content.CopyToAsync(stream);
            } else
            {
                throw new HttpException(response.StatusCode);
            }
        }

        private static string GetToken()
        {
            string token = null;
            if (Application.Current.Properties.ContainsKey(Const.CONFIG_API_KEY))
            {
                token = (string)Application.Current.Properties[Const.CONFIG_API_KEY];
            }
            return token;
        }

        private static void AddTokenHeader(HttpRequestMessage request, string token)
		{
			if (!String.IsNullOrEmpty(token))
			{
				request.Headers.Add("Authorization", "Bearer " + token);
            }
		}

        public class HttpException: Exception
        {
           
			public HttpStatusCode HttpStatusCode { get; set; }

            public HttpException(string message): base(message)
            {

            }

            public HttpException(string message, Exception e): base(message, e)
            {

            }

			public HttpException(HttpStatusCode code): base()
			{
				HttpStatusCode = code;		
			}
        }
	}
}

