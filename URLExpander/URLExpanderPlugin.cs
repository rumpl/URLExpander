using System;
using System.ComponentModel.Composition;
using System.Windows;
using Seesmic.Sdp.Extensibility;

namespace URLExpander
{
  [Export(typeof (IPlugin))]
  public class URLExpanderPlugin : IPlugin
  {
    private static ResourceDictionary DataTemplates { get; set; }

    public static DataTemplate SmallLogoTemplate
    {
      get { return DataTemplates["SmallLogoTemplate"] as DataTemplate; }
    }

    public static DataTemplate ContentTemplate
    {
      get { return DataTemplates["ContentTemplate"] as DataTemplate; }
    }

    #region IPlugin Members

    public void Initialize()
    {
      DataTemplates = new ResourceDictionary
                        {
                          Source =
                            new Uri("/URLExpander;component/DataTemplates/ResourceDictionary.xaml", UriKind.Relative)
                        };
    }

    public void CommitSettings()
    {
    }

    public void RevertSettings()
    {
    }

    public Guid Id
    {
      get { return new Guid("AA1C2920-63B7-49A0-9BE4-011EDB81C2E8"); }
    }

    public DataTemplate SettingsTemplate
    {
      get { return null; }
    }

    #endregion
  }
}