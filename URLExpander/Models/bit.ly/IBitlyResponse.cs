namespace URLExpander.Models
{
    public interface IBitlyResponse
    {
        int StatusCode { get; set; }

        string StatusText { get; set; }
    }
}