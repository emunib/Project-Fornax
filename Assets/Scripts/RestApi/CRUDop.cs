﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

interface Lookup
{
    String Lookup(String value);
}

public abstract class CRUDop<ResponseType>: Lookup
{
    protected string resourceUrl;
    protected HttpClient client;
    protected Dictionary<String, String> dictionary;
    protected LinkedList<Url> urls;
    public CRUDop(String nurl)
    {
        urls = Parse(nurl, this);
        if (urls.Count == 1)
        {
            resourceUrl = urls.First.Value.ToString();
        }
        client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/xml"));
    }

    public void SetUrlVariables(Dictionary<String, String> newDictionary)
    {
        dictionary = newDictionary;
        resourceUrl= "";
        foreach (var url in urls)
        {
            resourceUrl += url.ToString();
        }
    }


    public String Lookup(String value)
    {
        return dictionary[value];
    }

    protected interface Url
    {
        String ToString();
    }

    class StaticUrl : Url
    {
        readonly String Value;
        public StaticUrl(String value)
        {
            Value = value;
        }

        public override String ToString()
        {
            return Value;
        }
    }

    class DynamicUrl : Url
    {
        
        readonly String Value;
        private readonly Lookup Resolver;
        public DynamicUrl(String value, Lookup resolver)
        {
            Value = value;
            Resolver = resolver;
        }

        public override String ToString()
        {
            return Resolver.Lookup(Value);
        }
    }

    LinkedList<Url> Parse(String str, Lookup lookup)
    {
        LinkedList<Url> result = new LinkedList<Url>();
        StringBuilder stringBuilder = new StringBuilder();
        int position = 0;
        while (true)
        {
            if (position >= str.Length) break;
            result.AddLast(parseStatic(ref position, str, stringBuilder));
            stringBuilder.Clear();
            if (position >= str.Length) break;
            result.AddLast(parseDynamic(ref position, str, stringBuilder, lookup));
            stringBuilder.Clear();
        }
        return result;
    }

    StaticUrl parseStatic(ref int position, string str, StringBuilder stringBuilder)
    {
        int i = position;
        for (; i < str.Length; i++)
        {
            if (str[i] == '{')
            {
                i++;
                break;
            }
            stringBuilder.Append(str[i]);
        }
        position = i;
        return new StaticUrl(stringBuilder.ToString());
    }

    DynamicUrl parseDynamic(ref int position, string str, StringBuilder stringBuilder, Lookup lookup)
    {
        int i = position;
        for (; i < str.Length; i++)
        {
            if (str[i] == '}')
            {
                i++;
                break;
            }
            stringBuilder.Append(str[i]);
        }
        position = i;
        return new DynamicUrl(stringBuilder.ToString(), lookup);
    }
}