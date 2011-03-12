namespace URLExpander.ViewModels
{
    public interface IExpandedUrlViewModel
    {
        string NumberOfClicksText { get; }

        string Url { get; set; }
    }
}