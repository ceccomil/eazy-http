using EazyHttp;
using EazyHttp.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Data;

internal class NoOptionsConfiguration
{
  public static void Main()
  {
    var services = new ServiceCollection()
      .ConfigureEazyHttpClients();
  }
}
