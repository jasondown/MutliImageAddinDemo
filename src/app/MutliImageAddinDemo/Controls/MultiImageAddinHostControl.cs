using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using Jason.Down.Blog.MutliImageAddinDemo.Model;
using Jason.Down.Blog.MutliImageAddinDemo.View;
using Jason.Down.Blog.MutliImageAddinDemo.ViewModel;
using Microsoft.Dynamics.Framework.UI.Extensibility;
using Microsoft.Dynamics.Framework.UI.Extensibility.WinForms;

namespace Jason.Down.Blog.MutliImageAddinDemo.Controls
{
    /// <summary>
    /// This class wraps a custom multi-image WPF control so that it can be exposed to NAV.
    /// </summary>
    [ControlAddInExport("Jason.Down.Blog.MultiImageAddinDemo.Controls.MultiImageAddinHostControl")]
    public class MultiImageAddinHostControl : WinFormsControlAddInBase
    {
        private Panel _panel;
        private ElementHost _host;
        private MultiImageView _view;
        private PageableImageControlViewModel _vm;
        private IImageRepository _imageRepository;

        /// <summary>
        /// Creates the Windows Forms control for the control add-in.
        /// </summary>
        /// <returns>Returns the Windows Forms control.</returns>
        protected override Control CreateControl()
        {
            _panel = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle
            };

            _host = new ElementHost
            {
                Dock = DockStyle.Fill
            };

            _panel.Controls.Add(_host);

            _vm = new PageableImageControlViewModel
            {
                PageSize = 3
            };

            _view = new MultiImageView
            {
                DataContext = _vm
            };
            _view.InitializeComponent();

            _host.Child = _view;
            _host.Size = new Size((int) _view.MinWidth, (int) _view.MinHeight);
            _panel.Size = new Size((int) _view.MinWidth + 5, (int) _view.MinHeight + 5);

            return _panel;
        }

        /// <summary>
        /// Gets a value indicating whether to allow caption for the control.
        /// </summary>
        /// <value>
        ///   <c>true</c> if caption for control is allowed; otherwise, <c>false</c>.
        /// </value>
        public override bool AllowCaptionControl
        {
            get { return false; }
        }

        /// <summary>
        /// Sets the image repository.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <exception cref="System.ArgumentNullException">repository</exception>
        [ApplicationVisible]
        public void SetImageRepository(IImageRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            if (_imageRepository != null)
            {
                _imageRepository.RequestItemImages -= OnRequestItemImages;
            }

            _imageRepository = repository;
            _imageRepository.RequestItemImages += OnRequestItemImages;

            _vm.SetImageRepository(repository);
        }

        /// <summary>
        /// Occurs when a request item images is made.
        /// </summary>
        [ApplicationVisible]
        public event EventHandler<ImageRequestEventArgs> RequestImages = delegate { };  

        private void OnRequestItemImages(object sender, ImageRequestEventArgs e)
        {
            RequestImages(new object(), e);
        }

        /// <summary>
        /// Sets the image paths.
        /// </summary>
        /// <param name="imagePaths">The image paths.</param>
        [ApplicationVisible]
        public void SetImagePaths(IEnumerable<string> imagePaths)
        {
            var images = new List<System.Windows.Controls.Image>();
            var converter = new ImageSourceConverter();

            foreach (var path in imagePaths)
            {
                var img = new System.Windows.Controls.Image();
                img.SetValue(System.Windows.Controls.Image.SourceProperty, converter.ConvertFromString(path));
                images.Add(img);
            }
            _imageRepository.SetImages(images);
        }
    }
}