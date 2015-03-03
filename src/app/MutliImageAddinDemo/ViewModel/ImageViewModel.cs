using System;
using System.Windows.Controls;

namespace Jason.Down.Blog.MutliImageAddinDemo.ViewModel
{
    /// <summary>
    /// This class represents an item image.
    /// </summary>
    [Serializable]
    public class ImageViewModel : ViewModelBase
    {
        private Image _itemImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageViewModel"/> class.
        /// </summary>
        /// <param name="itemImage">The item image.</param>
        public ImageViewModel(Image itemImage)
        {
            ItemImage = itemImage;
        }

        /// <summary>
        /// Gets or sets the item image.
        /// </summary>
        /// <value>
        /// The item image.
        /// </value>
        public Image ItemImage
        {
            get { return _itemImage; }
            set
            {
                if (!Equals(_itemImage, value))
                {
                    _itemImage = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
