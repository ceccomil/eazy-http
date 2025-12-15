namespace EazyHttp.IntegrationTests;

[Collection("TestServer")]
public class SimpleClientTests
{
  [Fact]
  public async Task GetAsync_as_string()
  {
    // Arrange
    var query = new HttpQuery();
    query.AddParam(new HttpQueryParam("value", "123"));

    await TestApiHost.Send<ISimpleClient>(async client =>
    {
      // Act
      var result = await client.GetAsync<string>(
        route: "echo/query",
        query: query);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(
        """
        {
          "echoValue": "123"
        }
        """,
        result);
    });
  }

  [Fact]
  public async Task GetAsync_as_bytes()
  {
    // Arrange
    var query = new HttpQuery();
    query.AddParam(new HttpQueryParam("value", "123"));

    await TestApiHost.Send<ISimpleClient>(async client =>
    {
      // Act
      var result = await client.GetAsync<byte[]>(
        route: "echo/query",
        query: query);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(
        """
        {
          "echoValue": "123"
        }
        """,
        Encoding.UTF8.GetString(result));
    });
  }

  [Fact]
  public async Task GetAsync_as_stream()
  {
    // Arrange
    var query = new HttpQuery();
    query.AddParam(new HttpQueryParam("value", "123"));

    await TestApiHost.Send<ISimpleClient>(async client =>
    {
      // Act
      var result = await client.GetAsync<MemoryStream>(
        route: "echo/query",
        query: query);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(
        """
        {
          "echoValue": "123"
        }
        """,
        await new StreamReader(result).ReadToEndAsync());
    });
  }

  [Fact]
  public async Task GetAsync_as_json()
  {
    // Arrange
    var query = new HttpQuery();
    query.AddParam(new HttpQueryParam("value", "123"));

    await TestApiHost.Send<ISimpleClient>(async client =>
    {
      // Act
      var result = await client.GetAsync<EchoDto>(
        route: "echo/query",
        query: query);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(
        new("123"),
        result);
    });
  }

  [Fact]
  public async Task PostMultipartAsync_with_files_and_fields_returns_summary()
  {
    // Arrange: two fields + two files
    var file1Content = "Hello from file1";
    var file2Content = new byte[] { 1, 2, 3, 4 };

    List<FormElement> elements =
    [
      new("description", new StringContent("Test upload")),
      new("category", new StringContent("docs")),
      new("files", new ByteArrayContent(Encoding.UTF8.GetBytes(file1Content)))
      {
        FileName = "file1.txt"
      },
      new("files", new ByteArrayContent(file2Content))
      {
        FileName = "file2.bin"
      }
    ];

    await TestApiHost.Send<ISimpleClient>(async client =>
    {
      // Act
      var result = await client.PostFormAsync<MultipartEchoDto>(
        route: "echo/multipart",
        elements);

      // Assert
      Assert.NotNull(result);

      // Fields
      Assert.Equal("Test upload", result!.Fields["description"]);
      Assert.Equal("docs", result.Fields["category"]);

      // Files
      Assert.Equal(2, result.Files.Length);

      var file1 = result
        .Files
        .SingleOrDefault(x => 
          x.Name == "files" && 
          x.FileName == "file1.txt");

      Assert.NotNull(file1);


      var file2 = result
        .Files
        .SingleOrDefault(x =>
          x.Name == "files" &&
          x.FileName == "file2.bin");

      Assert.NotNull(file2);
    });
  }
}

