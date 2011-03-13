namespace URLExpander.ViewModels
{
    using Seesmic.Sdp.Utils;

    public abstract class ExpandedUrlViewModelBase : ObservableObject, IExpandedUrlViewModel
    {
        public string NumberOfClicksText
        {
            get { return UserClicks.HasValue ? string.Format("{0} click{1} ({2} global)", UserClicks, UserClicks == 1 ? "" : "s", GlobalClicks) : null; }
        }

        public abstract int? UserClicks { get; set; }

        public abstract int? GlobalClicks { get; set; }

        public abstract string Url { get; set; }
    }
}