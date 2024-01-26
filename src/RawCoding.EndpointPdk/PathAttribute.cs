namespace RawCoding.EndpointPdk;

public class PathAttribute : Attribute
{
    public string Method { get; }
    public string Path { get; }

    public PathAttribute(string method, string path)
    {
        Method = method;
        Path = path;
    }
}