using System.Windows;

namespace URLExpander.DataTemplates
{
  public partial class UrlInfo
  {
    private static readonly DependencyProperty AttachmentProperty;

    static UrlInfo()
    {
      AttachmentProperty = DependencyProperty.Register("Attachment", typeof (URLExpanderAttachement), typeof (UrlInfo), null);
    }

    public UrlInfo()
    {
      InitializeComponent();
    }

    public URLExpanderAttachement Attachment
    {
      get { return (URLExpanderAttachement) GetValue(AttachmentProperty); }
      set { SetValue(AttachmentProperty, value); }
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      DataContext = Attachment;
    }
  }
}