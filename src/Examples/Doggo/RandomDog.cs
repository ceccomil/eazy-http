using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Doggo.Models;
using EazyHttp;

namespace Doggo;

public interface IRandomDog
{
    IEazyClients Clients { get; }

    Task<byte[]> GetPicture(
        CancellationToken token = default);

    Task<string> SavePicture(
        CancellationToken token = default);
}

public class RandomDog : IRandomDog
{
    private const string IMAGE_ROUTE = "https://dog.ceo/api/breeds/image/random";

    public IEazyClients Clients { get; }

    public RandomDog(
            IEazyClients clients)
    {
        Clients = clients;
    }

    public async Task<byte[]> GetPicture(
        CancellationToken token = default)
    {
        var dogDetail = await Clients
            .SharedHttpClient
            .GetAsync<DogImage>(
                IMAGE_ROUTE,
                cancellationToken: token);

        if (dogDetail?.Status != "success" ||
            dogDetail.ImgUrl is null)
        {
            throw new ApplicationException(
                $"[{dogDetail?.Status}] Dog picture not found!");
        }

        var imageData = await Clients
            .SharedHttpClient
            .GetAsync<byte[]>(
                dogDetail.ImgUrl,
                cancellationToken: token);

        if (imageData is null)
        {
            throw new ApplicationException(
                $"Dog picture data is null!");
        }

        return imageData;
    }

    public async Task<string> SavePicture(
        CancellationToken token = default)
    {
        var imageData = await GetPicture(token);

        var fileName = $"./{Guid.NewGuid()}.png";

        await File
            .WriteAllBytesAsync(
                fileName,
                imageData,
                token);

        return fileName;
    }
}
