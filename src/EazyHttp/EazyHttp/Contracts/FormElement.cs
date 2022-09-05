namespace EazyHttp.Contracts;

/// <summary>
/// TODO docume
/// </summary>
public class FormElement
{
    /// <summary>
    /// TODO docume
    /// </summary>
    public string QueryParam { get; }

    /// <summary>
    /// TODO docume
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// TODO docume
    /// </summary>
    public HttpContent HttpElementContent { get; }

    /// <summary>
    /// TODO docume
    /// </summary>
    public bool SendAsString { get; }

    /// <summary>
    /// TODO docu
    /// </summary>
    /// <param name="queryParam"></param>
    /// <param name="elementContent"></param>
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
