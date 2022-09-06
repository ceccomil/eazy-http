using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Doggo.Models;

public class DogImage
{
    [JsonPropertyName("message")]
    public string? ImgUrl { get; set; }

    public string Status { get; set; } = "Unknown";
}
