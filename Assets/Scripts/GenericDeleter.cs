

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Deleter
{
    public delegate void Callback(HttpResponseMessage response);

    private Callback callback;
    private String BaseUrl;
    private HttpClient client;
    public Deleter(String nurl, Callback ncallback)
    {
        BaseUrl = nurl;
        callback = ncallback;
        client = new HttpClient();
    }

    public void SetHeader(String key, String value)
    {
        client.DefaultRequestHeaders.Add(key, value);
    }

    public async void Run(String URL)
    {
        HttpResponseMessage response = await Work(URL);
        callback(response);
    }

    public async Task<HttpResponseMessage> Work(String URL)
    {
        return await client.DeleteAsync(
            BaseUrl + URL).ConfigureAwait(false);
    }
}