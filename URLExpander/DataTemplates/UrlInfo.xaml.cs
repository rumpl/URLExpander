namespace URLExpander.DataTemplates
{
    using System.Windows;

    public partial class UrlInfo
    {
        private static readonly DependencyProperty AttachmentProperty;

        static UrlInfo()
        {
            AttachmentProperty = DependencyProperty.Register("Attachment", typeof(UrlExpanderAttachment), typeof(UrlInfo), null);
        }

        public UrlInfo()
        {
            InitializeComponent();
        }

        public UrlExpanderAttachment Attachment
        {
            get { return (UrlExpanderAttachment)GetValue(AttachmentProperty); }
            set { SetValue(AttachmentProperty, value); }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Attachment;
        }
    }
}