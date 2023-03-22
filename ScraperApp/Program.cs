using HtmlAgilityPack;
using Newtonsoft.Json;
using ScraperApp;
using ScraperApp.Models;
using ScraperApp.Scrapers;
using System.Net;
using System.Text;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;

internal class Program
{
    private static void Main(string[] args)
    {
        Serializer serializer = new();

        serializer.SerializeDrivers();
    }
}