namespace EazyHttp.Contracts.Tests;

[Collection("HttpQuery Tests")]
public class HttpQueryTests
{
    [Fact]
    public void Test()
    {
        var query = new HttpQuery();
        query.AddParam(new("field1", "Value1_1"));
        query.AddParam(new("field2", "Value2_1", "Value2_2"));
        query.AddParam(new("field1", "Escaped & <> value"));

        var qString = query
            .ToString();

        qString
            .Should()
            .Be($"?field1=" +
                $"{HttpUtility.UrlEncode("Value1_1")}&field2=" +
                $"{HttpUtility.UrlEncode("Value2_1")}&field2=" +
                $"{HttpUtility.UrlEncode("Value2_2")}&field1=" +
                HttpUtility.UrlEncode("Escaped & <> value"));
    }
}