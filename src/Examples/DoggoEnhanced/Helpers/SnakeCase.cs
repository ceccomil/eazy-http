namespace DoggoEnhanced.Helpers;

public enum SnakeCase
{
    Default = 0,
    Begin,
    Lower,
    Upper
}

public class SnakeCasePolicy : JsonNamingPolicy
{
    public override string ConvertName(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return name;
        }

        var sb = new StringBuilder();
        var snakeCase = SnakeCase.Begin;

        for (var i = 0; i < name.Length; i++)
        {
            if (name[i] == '_')
            {
                sb.Append('_');
                snakeCase = SnakeCase.Begin;
                continue;
            }

            if (char.IsUpper(name[i]))
            {
                if (snakeCase is SnakeCase.Lower or
                    SnakeCase.Default)
                {
                    sb.Append('_');
                }

                if (snakeCase is SnakeCase.Upper &&
                    i + 1 < name.Length)
                {
                    var next = name[i + 1];

                    if (!char.IsUpper(next) &&
                        next != '_')
                    {
                        sb.Append('_');
                    }
                }

                sb.Append(
                    char.ToLowerInvariant(name[i]));

                snakeCase = SnakeCase.Upper;
                continue;
            }

            if (snakeCase is SnakeCase.Default)
            {
                sb.Append('_');
            }

            sb.Append(name[i]);
            snakeCase = SnakeCase.Lower;
        }

        return sb.ToString();
    }
}
