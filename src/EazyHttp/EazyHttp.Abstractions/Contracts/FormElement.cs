namespace EazyHttp.Contracts;

/// <summary>
/// Represents a single element in a multipart form data request, including its field name, content, and optional file
/// name.
/// </summary>
/// <remarks>Use this class to encapsulate the details of a form field or file when constructing multipart HTTP
/// requests. The element can represent either a simple form field or a file upload, depending on the content and file
/// name provided. The SendAsString property indicates whether the content will be sent as a string or as raw bytes,
/// based on the type of HttpElementContent.</remarks>
public class FormElement
{
  /// <summary>
  /// Gets the query parameter value associated with the current request.
  /// </summary>
  public string QueryParam { get; }

  /// <summary>
  /// Gets or sets the name of the file associated with the current operation.
  /// </summary>
  public string? FileName { get; set; }

  /// <summary>
  /// Gets the HTTP content associated with the current element.
  /// </summary>
  public HttpContent HttpElementContent { get; }

  /// <summary>
  /// Gets a value indicating whether the message content is sent as a string instead of as binary data.
  /// </summary>
  /// <remarks>When <see langword="true"/>, the message will be transmitted as a UTF-8 encoded string. When <see
  /// langword="false"/>, the message will be sent as raw binary data. Use this property to control the format of
  /// outgoing messages based on the requirements of the receiving system.</remarks>
  public bool SendAsString { get; }

  /// <summary>
  /// Default constructor
  /// </summary>
  public FormElement(
    string queryParam,
    HttpContent elementContent)
  {
    QueryParam = queryParam;
    HttpElementContent = elementContent;

    if (HttpElementContent is StringContent)
    {
      SendAsString = true;
    }
  }
}
