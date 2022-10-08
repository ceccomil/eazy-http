namespace EazyHttp.Contracts;

/// <summary>
/// Multipart data content element
/// </summary>
public class FormElement
{
    /// <summary>
    /// The name of the field.
    /// </summary>
    public string QueryParam { get; }

    /// <summary>
    /// The name of the file.
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// The content of the element.
    /// </summary>
    public HttpContent HttpElementContent { get; }

    /// <summary>
    /// Instructs to send the content as a string or bytes.
    /// </summary>
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
