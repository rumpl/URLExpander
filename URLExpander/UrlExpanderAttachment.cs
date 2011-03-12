namespace URLExpander
{
    using System.ComponentModel.Composition;
    using System.Windows;

    using Seesmic.Sdp.Extensibility;
    using Seesmic.Sdp.Utils;

    using URLExpander.ViewModels;

    [Export(typeof(ITimelineItemAttachment))]
    public class UrlExpanderAttachment : ObservableObject, ITimelineItemAttachment
    {
        public UrlExpanderAttachment(IExpandedUrlViewModel viewModel)
        {
            Model = viewModel;
        }

        public IExpandedUrlViewModel Model { get; set; }

        #region ITimelineItemAttachment Members

        public DataTemplate SmallLogo
        {
            get { return UrlExpanderPlugin.SmallLogoTemplate; }
        }

        public DataTemplate LargeLogo
        {
            get { return null; }
        }

        public string CollapsedText
        {
            get { return "Original url..."; }
        }

        public DataTemplate ContentTemplate
        {
            get { return UrlExpanderPlugin.ContentTemplate; }
        }

        public bool IsInitiallyExpanded
        {
            get { return false; }
        }

        #endregion
    }
}