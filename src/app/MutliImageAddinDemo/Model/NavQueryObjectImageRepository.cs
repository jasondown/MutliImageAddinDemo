using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Jason.Down.Blog.MutliImageAddinDemo.Collections;

namespace Jason.Down.Blog.MutliImageAddinDemo.Model
{
    [Serializable]
    public class NavQueryObjectImageRepository : IImageRepository
    {
        private readonly string _itemNumber;

        [field: NonSerialized]
        private SmartObservableCollection<Image> _images;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavQueryObjectImageRepository"/> class.
        /// </summary>
        /// <param name="itemNumber">The item number.</param>
        public NavQueryObjectImageRepository(string itemNumber)
        {
            _itemNumber = itemNumber;
        }

        /// <summary>
        /// Occurs when a request for item images is made.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<ImageRequestEventArgs> RequestItemImages = delegate { };

        /// <summary>
        /// Gets or sets the collection of item images.
        /// </summary>
        /// <returns></returns>
        /// <value>
        /// The collection of item images.
        /// </value>
        public SmartObservableCollection<Image> GetImages()
        {
            RequestItemImages(this, new ImageRequestEventArgs(_itemNumber));

            return _images ?? (_images = new SmartObservableCollection<Image>());
        }

        /// <summary>
        /// Sets the collection of item images.
        /// </summary>
        /// <param name="images">The images.</param>
        public void SetImages(IEnumerable<Image> images)
        {
            _images.AddRange(images);
        }
    }
}