using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Jason.Down.Blog.MutliImageAddinDemo.Collections;

namespace Jason.Down.Blog.MutliImageAddinDemo.Model
{
    public interface IImageRepository
    {
        /// <summary>
        /// Occurs when a request for item images is made.
        /// </summary>
        event EventHandler<ImageRequestEventArgs> RequestItemImages;

        /// <summary>
        /// Gets or sets the collection of item images.
        /// </summary>
        /// <value>
        /// The collection of item images.
        /// </value>
        SmartObservableCollection<Image> GetImages();

        /// <summary>
        /// Sets the collection of item images.
        /// </summary>
        /// <param name="images">The images.</param>
        void SetImages(IEnumerable<Image> images);
    }
}
