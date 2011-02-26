using System.ComponentModel.Composition;
using System.Windows;
using Seesmic.Sdp.Extensibility;
using Seesmic.Sdp.Utils;
using URLExpander.ViewModels;

namespace URLExpander
{
  [Export(typeof (ITimelineItemAttachment))]
  public class URLExpanderAttachement : ObservableObject, ITimelineItemAttachment
  {
    public URLExpanderAttachement(BitlyViewModel viewModel)
    {
      Model = viewModel;
    }

    public BitlyViewModel Model { get; set; }

    #region ITimelineItemAttachment Members

    public DataTemplate SmallLogo
    {
      get { return URLExpanderPlugin.SmallLogoTemplate; }
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
      get { return URLExpanderPlugin.ContentTemplate; }
    }

    public bool IsInitiallyExpanded
    {
      get { return false; }
    }

    #endregion
  }
}