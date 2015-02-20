using System;

namespace Jason.Down.Blog.MutliImageAddinDemo.Model
{
    /// <summary>
    /// This class represents details for requested item images.
    /// </summary>
    public class ImageRequestEventArgs : EventArgs
    {
        private readonly string _itemNumber;
        private readonly int _maxNumberOfImages;

        /// <summary>
        /// No Maximum is 0
        /// </summary>
        public const int NoMaximum = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageRequestEventArgs"/> class.
        /// </summary>
        /// <param name="itemNumber">The item number.</param>
        /// <param name="maxNumberOfImages">The maximum number of images. Default is <see cref="NoMaximum"/></param>
        public ImageRequestEventArgs(string itemNumber, int maxNumberOfImages = NoMaximum)
        {
            _itemNumber = itemNumber;
            _maxNumberOfImages = maxNumberOfImages;
        }

        /// <summary>
        /// Gets the item number.
        /// </summary>
        /// <value>
        /// The item number.
        /// </value>
        public string ItemNumber
        {
            get { return _itemNumber; }
        }

        /// <summary>
        /// Gets the maximum number of images to retrieve from the image source.
        /// </summary>
        /// <value>
        /// The maximum number of images to retrieve from the image source.
        /// </value>
        public int MaxNumberOfImages
        {
            get { return _maxNumberOfImages; }
        }
    }
}