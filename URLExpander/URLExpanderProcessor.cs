using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using Seesmic.Sdp.Extensibility;

namespace URLExpander
{
  [Export(typeof(ITimelineItemProcessor))]
  public class URLExpanderProcessor : ITimelineItemProcessor
  {
    #region ITimelineItemProcessor Members

    public bool Filter(TimelineItemContainer timelineItemContainer)
    {
      return true;
    }

    public void Process(TimelineItemContainer timelineItemContainer)
    {
      var regex = new Regex("http://(bit.ly|4sq.com|tcrn.ch|nyti.ms|amzn.to|binged.it|huff.to|pep.si|n.pr|j.mp)/(?<Id>[0-9a-zA-Z]*)");
      if (!regex.IsMatch(timelineItemContainer.TimelineItem.Text)) return;
      var match = regex.Match(timelineItemContainer.TimelineItem.Text);
      var attachement = new URLExpanderAttachement(match.Value);
      timelineItemContainer.AddAttachment(attachement);
    }

    public void Deliver(TimelineItemContainer timelineItemContainer)
    {
    }

    public void Remove(TimelineItemContainer timelineItemContainer)
    {
    }

    #endregion
  }
}