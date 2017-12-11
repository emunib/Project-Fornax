

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Deleter : CRUDop<HttpResponseMessage>
{
    public delegate void Callback(HttpResponseMessage response);

    private Callback callback;
    public Deleter(String nurl, Callback ncallback) : base(nurl)
    {
        callback = ncallback;
    }

    public void SetHeader(String key, String value)
    {
        if (client.DefaultRequestHeaders.Contains(key))
        {
            client.DefaultRequestHeaders.Remove(key);
        }
        client.DefaultRequestHeaders.Add(key, value);
    }

    public async void Run()
    {
        HttpResponseMessage response = await Work();
        callback(response);
    }

    public async Task<HttpResponseMessage> Work()
    {
        return await client.DeleteAsync(
            resourceUrl).ConfigureAwait(false);
    }
}