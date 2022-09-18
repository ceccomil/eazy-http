using ILocalEazyClients = DoggoEnhanced.EazyHttp.IEazyClients;

namespace DoggoEnhanced;

public interface IImageAnalysis
{
    ILocalEazyClients Clients { get; }

    Task<AnalysisResult> GetAnalysis(
        byte[] imageData,
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
}
