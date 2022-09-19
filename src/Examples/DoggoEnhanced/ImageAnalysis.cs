using ILocalEazyClients = DoggoEnhanced.EazyHttp.IEazyClients;

namespace DoggoEnhanced;

public interface IImageAnalysis
{
    ILocalEazyClients Clients { get; }

    Task<AnalysisResult> GetAnalysis(
        byte[] imageData,
        CancellationToken token = default);

    Task<byte[]> GetAiPicture(
        string text,
        CancellationToken cancellationToken = default);

    Task<(byte[], string)> GetAndSaveAiPicture(
        string text,
        CancellationToken token = default);
}

public class ImageAnalysis : IImageAnalysis
{
    public ILocalEazyClients Clients { get; }

    public ImageAnalysis(
        ILocalEazyClients clients)
    {
        Clients = clients;
    }

    public async Task<AnalysisResult> GetAnalysis(
        byte[] imageData,
        CancellationToken token = default)
    {
        var query = new HttpQuery();

        query
            .AddParam(new(
                "visualFeatures",
                "Description"));

        query
            .AddParam(new(
                "language",
                "en"));

        query
            .AddParam(new(
                "model-version",
                "latest"));

        var elements = new FormElement[]
        {
            new(
                "file",
                new ByteArrayContent(imageData))
        };

        var result = await Clients
            .ComputerVision
            .PostFormAsync<AnalysisResult>(
                $"analyze{query}",
                elements,
                cancellationToken: token);

        return result
            ?? throw new ApplicationException(
                "Resulting content is null!");
    }

    public async Task<byte[]> GetAiPicture(
        string text,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ApplicationException(
                "A valid description is required!");
        }

        var elements = new FormElement[]
        {
            new(
                "text",
                new StringContent(text))
        };

        var json = await Clients
            .DeepAi
            .PostFormAsync<AiImage>(
            "text2img",
            elements,
            cancellationToken: cancellationToken)
            ?? throw new ApplicationException(
                "Resulting content is null!");

        var result = await Clients
            .DeepAi
            .GetAsync<byte[]>(
                json.OutputUrl,
                cancellationToken: cancellationToken);

        return result
            ?? throw new ApplicationException(
                "Resulting content is null!");
    }

    public async Task<(byte[], string)> GetAndSaveAiPicture(
        string text,
        CancellationToken token = default)
    {
        var imageData = await GetAiPicture(
            text,
            token);

        var fileName = $"./{Guid.NewGuid()}.png";

        await File
            .WriteAllBytesAsync(
                fileName,
                imageData,
                token);

        return
            (imageData,
            fileName);
    }
}
