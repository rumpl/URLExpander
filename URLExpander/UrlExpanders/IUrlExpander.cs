namespace URLExpander
{
    using System;

    using URLExpander.ViewModels;

    public interface IUrlExpander
    {
        void IfCanExpand(Uri uri, Action<IExpandedUrlViewModel> callback);
    }
}