using HtmlAgilityPack;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;

namespace SignalRChatClient
{
  internal class Authenticator
  {
    private HttpClient httpClient;
    private CookieContainer cookieContainer;
    public String baseUrl { get; set; }
    const String requestTokenName = "__RequestVerificationToken";
    const String SetCookieName = "Set-Cookie";
    const string homeUrl = "/";
    const string loginUrl = "/Account/Login";
    const string logoutUrl = "/Account/LogOff";
    public Authenticator(String baseUrl)
    {
      HttpClientHandler handler = new HttpClientHandler();
      handler.CookieContainer = cookieContainer = new CookieContainer();
      this.baseUrl = baseUrl;
      httpClient = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
    }
    internal async Task<CookieContainer> Login(string userName, string password)
    {
      HttpResponseMessage response = await httpClient.GetAsync(loginUrl);
      String requestVerificationToken = await GetHiddenTokenValue(response, "//input[@type='hidden'][@name='__RequestVerificationToken']");
      HttpContent content = new FormUrlEncodedContent(
        new Dictionary<string, string> { { "Email", userName }, { "Password", password }, { "RememberMe", "false" }, { requestTokenName, requestVerificationToken } }
        );
      response = await httpClient.PostAsync(loginUrl, content);
      return cookieContainer;
    }
    internal async Task Logoff()
    {
      HttpResponseMessage response = await httpClient.GetAsync(homeUrl);
      String requestVerificationToken = await GetHiddenTokenValue(response, "//form[@id='logoutForm']/input[@type='hidden'][@name='__RequestVerificationToken']");
      HttpContent content = new FormUrlEncodedContent(
        new Dictionary<string, string> { { requestTokenName, requestVerificationToken } }
        );
      response = await httpClient.PostAsync(logoutUrl , content);
      System.Diagnostics.Debug.WriteLine($"{response.StatusCode}");
    }

    private static async Task<string> GetHiddenTokenValue(HttpResponseMessage response, String xpathHiddenLocator)
    {
      HtmlDocument document = new HtmlDocument();
      document.Load(await response.Content.ReadAsStreamAsync(), Encoding.UTF8);
      string hiddenTokenValue = document.DocumentNode
        .SelectNodes(xpathHiddenLocator)
        .FirstOrDefault()
        .GetAttributeValue("value", null);
      return hiddenTokenValue;
    }
  }
}