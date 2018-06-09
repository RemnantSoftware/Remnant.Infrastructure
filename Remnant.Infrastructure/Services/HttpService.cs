//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections.Specialized;
//using System.Net;
//using System.IO;
//using System.Web;
//using System.Text.RegularExpressions;

namespace Remnant.Core.Services
{
	//TODO: THIS MUST MOVE TO CORE WEB ASSEMBLY

	/// <summary>
	/// Http messenger that sends and receives http messages to a web server, making use of POST or GET.
	/// Note: Also contains two static methods to obtain ASP.NET view and event validation state from a web page.
	/// </summary>
	/// <remarks>
	/// Author: Neill Verreynne
	/// Date: Feb 2010
	/// </remarks>
	//public class HttpMessenger
	//{
	//  #region Enumerations

	//  /// <summary>
	//  /// Determines what type of method to perform for a message.
	//  /// </summary>
	//  public enum HttpMethodType
	//  {
	//    /// <summary>
	//    /// Does a get against the web server.
	//    /// </summary>
	//    Get,
	//    /// <summary>
	//    /// Does a post against the web server.
	//    /// </summary>
	//    Post
	//  }

	//  #region Constants

	//  private const string _contentType = "application/x-www-form-urlencoded";
	//  private const string _userAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; InfoPath.1; .NET CLR 1.1.4322; .NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618; FDM; .NET CLR 4.0.20506)";

	//  #endregion

	//  #endregion

	//  #region Fields

	//  private string _url = string.Empty;

	//  #endregion

	//  #region Constructors

	//  /// <summary>
	//  /// Constructor that accepts a url as a parameter
	//  /// </summary>
	//  /// <param name="url">The url where the message will be submitted to.</param>
	//  /// <param name="url">The method type to be used to send the message.</param>
	//  public HttpMessenger(string url, HttpMethodType methodType)
	//  {
	//    _url = url;
	//    MethodType = methodType;
	//    Data = new NameValueCollection();
	//  }

	//  #endregion

	//  #region Public Members

	//  /// <summary>
	//  /// Performs the send of the  data to a specified url using the specified method type.
	//  /// </summary>
	//  /// <returns>Returns the result of the send.</returns>
	//  public string Send()
	//  {
	//    HttpWebRequest request = null;

	//    // perform a get or post to web server
	//    switch (MethodType)
	//    {
	//      case HttpMethodType.Post: request = PerformPost(); break;
	//      case HttpMethodType.Get: request = PerformGet(); break;
	//      default: throw new Exception("Invalid Http method type specified.");
	//    }

	//    // receive the response from the web server
	//    return PerformResponse(request);
	//  }

	//  /// <summary>
	//  /// Sends the supplied data to specified url.
	//  /// </summary>
	//  /// <param name="values">The data  to send.</param>
	//  /// <returns>Returns a string containing the result of the send.</returns>
	//  public string Send(string url, NameValueCollection values)
	//  {
	//    _url = url;
	//    Data = values;
	//    return this.Send();
	//  }

	//  #endregion

	//  #region Public Properties

	//  /// <summary>
	//  /// Gets  the url to submit the message to.
	//  /// </summary>
	//  public string Url
	//  {
	//    get { return _url; }
	//  }

	//  /// <summary>
	//  /// Gets or sets the name value collection list of data to send with the message.
	//  /// </summary>
	//  public NameValueCollection Data { get; set; }

	//  /// <summary>
	//  /// Gets or sets the type of method  to perform against the url.
	//  /// </summary>
	//  public HttpMethodType MethodType { get; private set; }

	//  /// <summary>
	//  /// Gets or sets the  cookie container which to be used for the web server.
	//  /// </summary>
	//  public CookieContainer CookieContainer { get; set; }

	//  #endregion

	//  #region Private Members

	//  /// <summary>
	//  /// Creates a new Http web request with header information.
	//  /// </summary>
	//  /// <param name="url">The url for the web request.</param>
	//  /// <returns>Returns an instance of the Http web request.</returns>
	//  private HttpWebRequest CreateHttpRequest(string url)
	//  {
	//    System.Net.ServicePointManager.Expect100Continue = false;
	//    Uri uri = new Uri(url);
	//    HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
	//    //_httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
	//    httpWebRequest.UserAgent = _userAgent;
	//    httpWebRequest.KeepAlive = true;
	//    httpWebRequest.AllowAutoRedirect = true;
	//    httpWebRequest.ContentType = _contentType;
	//    httpWebRequest.CookieContainer = CookieContainer;
	//    return httpWebRequest;
	//  }

	//  /// <summary>
	//  /// Sends a web request with encoded data using the POST method type.
	//  /// </summary>
	//  /// <returns></returns>
	//  private HttpWebRequest PerformPost()
	//  {
	//    HttpWebRequest request = null;
	//    string result = string.Empty;
	//    string encodedData = UrlEncodeData();
	//    request = CreateHttpRequest(_url);
	//    request.Method = "POST";
	//    request.ContentLength = encodedData.Length;
	//    using (Stream writeStream = request.GetRequestStream())
	//    {
	//      UTF8Encoding encoding = new UTF8Encoding();
	//      byte[] bytes = encoding.GetBytes(encodedData);
	//      writeStream.Write(bytes, 0, bytes.Length);
	//    }
	//    return request;
	//  }

	//  /// <summary>
	//  /// Sends a web request with encoded data using the GET method type.
	//  /// </summary>
	//  /// <returns>Returns the web request that was used.</returns>
	//  private HttpWebRequest PerformGet()
	//  {
	//    HttpWebRequest request = null;
	//    string result = string.Empty;
	//    string encodedData = UrlEncodeData();
	//    request = CreateHttpRequest(_url + "?" + encodedData);
	//    request.Method = "GET";
	//    return request;
	//  }

	//  /// <summary>
	//  /// Retrieve the response of the web server.
	//  /// </summary>
	//  /// <param name="request">The http request that was made.</param>
	//  /// <returns>Returns the response or error if occurs.</returns>
	//  private string PerformResponse(HttpWebRequest request)
	//  {
	//    var result = string.Empty;
	//    try
	//    {
	//      var response = (HttpWebResponse)request.GetResponse();
	//      using (var responseStream = response.GetResponseStream())
	//      {
	//        using (var readStream = new StreamReader(responseStream, Encoding.UTF8))
	//        {
	//          result = readStream.ReadToEnd();
	//        }
	//      }
	//    }
	//    catch (Exception e)
	//    {
	//      result = "Error: " + e.Message;
	//    }
	//    return result;
	//  }

	//  /// <summary>
	//  /// Url encodes the data an item and ads it to the string.
	//  /// </summary>
	//  /// <returns>Returns the data Url encoded.</returns>
	//  private string UrlEncodeData()
	//  {
	//    var encodeData = new StringBuilder();
	//    for (int i = 0; i < Data.Count; i++)
	//    {
	//      if (i != 0)
	//        encodeData.Append("&");

	//      encodeData.Append(Data.GetKey(i));
	//      encodeData.Append("=");
	//      encodeData.Append(HttpUtility.UrlEncode(Data[i]));
	//    }
	//    return encodeData.ToString();
	//  }

	//  #endregion

	//  #region Public Static Members

	//  /// <summary>
	//  /// Gets the view state  hidden field's value (for a http web request or html document text).
	//  /// Note: Will first attempt to obtain the value from a http web request text, if not found then attempt from html document text.
	//  /// </summary>
	//  /// <param name="input">The hmtl page text.</param>
	//  /// <returns>Returns the view state  value.</returns>
	//  public static string GetViewState(string input)
	//  {
	//    string viewState = String.Empty;
	//    string pattern;

	//    // for http web request	
	//    pattern = "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(?<inputvalue>.*?)\" />";
	//    viewState = GeneralService.FindWithRegularExpression(pattern, "inputvalue", input);

	//    // try for html doc			
	//    if (viewState.Length == 0)
	//    {
	//      pattern = "<input id=__VIEWSTATE type=hidden value=(?<inputvalue>.*?) name=__VIEWSTATE>";
	//      viewState = GeneralService.FindWithRegularExpression(pattern, "inputvalue", input);
	//    }
	//    return viewState;
	//  }

	//  /// <summary>
	//  /// Gets the event validation hidden field's value (for a http web request or html document text).
	//  /// Note: Will first attempt to obtain the value from a http web request text, if not found then attempt from html document text.
	//  /// </summary>
	//  /// <param name="input">The hmtl page text.</param>
	//  /// <returns>Returns the event validation value.</returns>
	//  public static string GetEventValidation(string input)
	//  {
	//    string eventValidation = string.Empty;
	//    string pattern;

	//    // for a http web request	
	//    pattern = "<input type=\"hidden\" name=\"__EVENTVALIDATION\" id=\"__EVENTVALIDATION\" value=\"(?<inputvalue>.*?)\" />";
	//    eventValidation = GeneralService.FindWithRegularExpression(pattern, "inputvalue", input);

	//    // try for html doc
	//    if (eventValidation.Length == 0)
	//    {
	//      pattern = "<input id=__EVENTVALIDATION type=hidden value=(?<inputvalue>.*?) name=__EVENTVALIDATION>";
	//      eventValidation = GeneralService.FindWithRegularExpression(pattern, "inputvalue", input);
	//    }
	//    return eventValidation;
	//  }

	//  #endregion
	//}
}
