namespace Web.ApiGateWay.Extensions
{
    public static class HttpClientExtension
    {
        public async static Task<TResult> PostGetResponseAsync<TResult, TValue>(this HttpClient client, string url, TValue value)
        {
            var httpResponse = await client.PostAsJsonAsync(url, value);

            return httpResponse.IsSuccessStatusCode ? await httpResponse.Content.ReadFromJsonAsync<TResult>() : default;
        }

        public async static Task PostAsync<TValue>(this HttpClient client, string url, TValue value)
        {
            await client.PostAsJsonAsync(url, value);
        }

        public async static Task<TValue> GetResponseAsync<TValue>(this HttpClient client, string url)
        {
            return await client.GetFromJsonAsync<TValue>(url);
        }
    }
}
