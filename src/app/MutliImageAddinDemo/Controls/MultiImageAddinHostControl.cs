using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
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
            _vm = new PageableImageControlViewModel()
            {
                PageSize = 3
            };

            _view = new MultiImageView()
            {
                DataContext = _vm
            };

            _host = new ElementHost()
            {
                Dock = DockStyle.Top,
                Child = _view,
                Size = new Size((int) _view.Width, (int) _view.Height)
            };

            return _host;
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
    }
}