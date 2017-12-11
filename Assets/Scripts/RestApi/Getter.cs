using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Getter<ResponseType> : CRUDop<ResponseType>
{
    public delegate void Callback(ResponseType responseType);

    private Callback callback;
    private XmlSerializer responseSerializer;
    
    public Getter(String nurl, Callback ncallback) : base(nurl)
    {
        responseSerializer = new XmlSerializer(typeof(ResponseType));
        callback = ncallback;
    }

    public async void Run()
    {
        ResponseType response = await Work();
        callback(response);
    }

    public async Task<ResponseType> Work()
    {
        HttpResponseMessage httpResponseMessage = await client.GetAsync(resourceUrl).ConfigureAwait(false);
        Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
        ResponseType response = (ResponseType) responseSerializer.Deserialize(stream);
        return response;
    }
}