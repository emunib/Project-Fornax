using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Schemas;
using System.Xml.Serialization;
using UnityEngine;

public class Poster<ResponseType, RequestType> : CRUDop<ResponseType>
{
    public delegate void Callback(ResponseType responseType);

    private Callback callback;
    private XmlSerializer requestSerializer;
    private XmlSerializer responseSerializer;
    public Poster(String nurl, Callback ncallback) : base(nurl)
    {
        callback = ncallback;
        requestSerializer = new XmlSerializer(typeof(RequestType));
        responseSerializer = new XmlSerializer(typeof(ResponseType)); 
    }

    public async void Run(RequestType request)
    {
        ResponseType response = await Work(request);
        callback(response);
    }

    public async Task<ResponseType> Work(RequestType request)
    {
        MemoryStream memoryStream = new MemoryStream();
        requestSerializer.Serialize(memoryStream, request);
        memoryStream.Position = 0;
        StreamReader sr = new StreamReader(memoryStream);
        var stringContent = new StringContent(sr.ReadToEnd(), Encoding.UTF8, "application/xml");
        HttpResponseMessage httpResponseMessage = await client.PostAsync(
          resourceUrl, stringContent).ConfigureAwait(false);
        memoryStream.Dispose();
        Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
        ResponseType response = (ResponseType) responseSerializer.Deserialize(stream);
        return response;
    }
}