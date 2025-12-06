using Doggo.Http.Generated.Clients;
using Doggo.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Doggo;

public interface IRandomDog
{
    public static string BaseUrl => "https://dog.ceo/api/";

    public static JsonSerializerOptions JsonOptions { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    IDoggoClient Http { get; }

    Task<byte[]> GetPicture(
        CancellationToken token = default);

    Task<(byte[], string)> GetAndSavePicture(
        CancellationToken token = default);
}

public class RandomDog(IDoggoClient http) : IRandomDog
{
    private const string IMAGE_ROUTE = "https://dog.ceo/api/breeds/image/random";

    public IDoggoClient Http { get; } = http;

    public async Task<byte[]> GetPicture(
        CancellationToken token = default)
    {
        var dogDetail = await Http
            .GetAsync<DogImage>(
                IMAGE_ROUTE,
                cancellationToken: token);

        if (dogDetail?.Status != "success" ||
            dogDetail.ImgUrl is null)
        {
            throw new ApplicationException(
                $"[{dogDetail?.Status}] Dog picture not found!");
        }

        var imageData = await Http
            .GetAsync<byte[]>(
                dogDetail.ImgUrl,
                cancellationToken: token);

        return imageData is null
            ? throw new ApplicationException(
                $"Dog picture data is null!")
            : imageData;
    }

    public async Task<(byte[], string)> GetAndSavePicture(
        CancellationToken token = default)
    {
        var imageData = await GetPicture(token);

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
