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

    Task GetPicture(
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

    public async Task GetPicture(
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
            Console.WriteLine("Dog picture not found!");
            return;
        }

        var imageData = await Clients
            .SharedHttpClient
            .GetAsync<byte[]>(
                dogDetail.ImgUrl);

        if (imageData is null)
        {
            Console.WriteLine("Dog picture not found!");
            return;
        }

        var fileName = $"./{Guid.NewGuid()}.png";
        await File
            .WriteAllBytesAsync(
                fileName,
                imageData,
                token);

        Console
            .WriteLine(
            $"Here is your picture: {fileName}");

        Process.Start($"powershell.exe", fileName);
    }
}
