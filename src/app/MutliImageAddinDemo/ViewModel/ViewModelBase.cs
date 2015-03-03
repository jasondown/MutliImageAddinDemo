using System.ComponentModel;
using System.Runtime.CompilerServices;
using Jason.Down.Blog.MutliImageAddinDemo.Annotations;

namespace Jason.Down.Blog.MutliImageAddinDemo.ViewModel
{
    /// <summary>
    /// This class exposes the default behaviour for INotifyProperyChanged.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a property in the ViewModel has been changed and wishes to send out a notification.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
