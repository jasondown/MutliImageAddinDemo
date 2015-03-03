using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Jason.Down.Blog.MutliImageAddinDemo.Collections;
using Jason.Down.Blog.MutliImageAddinDemo.Command;
using Jason.Down.Blog.MutliImageAddinDemo.Model;

namespace Jason.Down.Blog.MutliImageAddinDemo.ViewModel
{
    public class PageableImageControlViewModel : ViewModelBase
    {
        #region Image Collection

        private SmartObservableCollection<ImageViewModel> _images;

        /// <summary>
        /// Gets or sets the image collection.
        /// </summary>
        /// <value>
        /// The image collection.
        /// </value>
        public SmartObservableCollection<ImageViewModel> Images
        {
            get
            {
                if (_images == null)
                {
                    _images = new SmartObservableCollection<ImageViewModel>();
                    _images.CollectionChanged += (s, e) => OnPropertyChanged();
                }
                return _images;
            }
            set
            {
                if (_images != value)
                {
                    _images = value;
                    OnPropertyChanged();
                    InitPageableImages();
                }
            }
        }

        private IImageRepository _repository;

        /// <summary>
        /// Sets the image repository.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public void SetImageRepository(IImageRepository repository)
        {
            _repository = repository;
            Images = new SmartObservableCollection<ImageViewModel>();

            var imageList = _repository.GetImages();

            if (imageList == null)
            {
                UseDefaultImage();
                return;
            }

            foreach (var image in imageList)
            {
                Images.Add(new ImageViewModel(image));
            }

            if (_images.Count > 0)
            {
                InitPageableImages();
                MainImageSource = new BitmapImage(new Uri(PageableImages[0].ItemImage.Source.ToString()));
            }
            else
            {
                UseDefaultImage();
            }
        }

        private IEnumerable<ImageViewModel> GetImages()
        {
            if (_images != null && _images.Count > 0)
            {
                return _images.Skip(CurrentPage * PageSize).Take(PageSize);
            }
            return Enumerable.Empty<ImageViewModel>();
        }

        #endregion

        #region Paging

        private SmartObservableCollection<ImageViewModel> _pageableImages;

        public SmartObservableCollection<ImageViewModel> PageableImages
        {
            get
            {
                if (_pageableImages == null)
                {
                    _pageableImages = new SmartObservableCollection<ImageViewModel>();
                    _images.CollectionChanged += (s, e) => OnPropertyChanged();
                }
                return _pageableImages;
            }
        }

        /// <summary>
        /// Initializes the pageable images and related properties.
        /// </summary>
        public void InitPageableImages()
        {
            CanPageNext = _images.Count > PageSize;
            CanPagePrevious = false;
            CurrentPage = 0;
            PageableImages.Reset(GetImages());
            OnPropertyChanged("PageableImages");
        }

        private int _pageSize;

        /// <summary>
        /// Gets or sets the number of images for a single page.
        /// </summary>
        /// <value>
        /// The number of images for a single page.
        /// </value>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _currentPage;

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged();
                }
            }
        }


        #endregion

        #region Paging - Previous

        private bool _canPagePrevious;

        /// <summary>
        /// Gets or sets a value indicating whether you can move to the previous page.
        /// </summary>
        /// <value>
        /// <c>true</c> if you can move to the previous page; otherwise, <c>false</c>.
        /// </value>
        public bool CanPagePrevious
        {
            get { return _canPagePrevious; }
            set
            {
                if (_canPagePrevious != value)
                {
                    _canPagePrevious = value;
                    OnPropertyChanged();
                }
            }
        }

        private ICommand _pagePreviousCommand;

        /// <summary>
        /// Gets the page previous command.
        /// </summary>
        /// <value>
        /// The page previous command.
        /// </value>
        public ICommand PagePreviousCommand
        {
            get
            {
                return _pagePreviousCommand ??
                       (_pagePreviousCommand = new RelayCommand(PagePrevious, () => CanPagePrevious));
            }
        }

        private void PagePrevious()
        {
            CurrentPage--;
            PageableImages.Reset(GetImages());
            CanPagePrevious = CurrentPage > 0;
            CanPageNext = true;
            OnPropertyChanged("PageableImages");
        }

        #endregion

        #region Paging - Next

        private bool _canPageNext;

        /// <summary>
        /// Gets or sets a value indicating whether move to the next page.
        /// </summary>
        /// <value>
        /// <c>true</c> if you can move to the next page; otherwise, <c>false</c>.
        /// </value>
        public bool CanPageNext
        {
            get { return _canPageNext; }
            set
            {
                if (_canPageNext != value)
                {
                    _canPageNext = value;
                    OnPropertyChanged();
                }
            }
        }

        private ICommand _pageNextCommand;

        /// <summary>
        /// Gets the page next command.
        /// </summary>
        /// <value>
        /// The page next command.
        /// </value>
        public ICommand PageNextCommand
        {
            get
            {
                return _pageNextCommand ??
                       (_pageNextCommand = new RelayCommand(PageNext, () => CanPageNext));
            }
        }

        private void PageNext()
        {
            CurrentPage++;
            PageableImages.Reset(GetImages());
            CanPageNext = _images.Count > (CurrentPage + 1)*PageSize;
            CanPagePrevious = true;
            OnPropertyChanged("PageableImages");
        }

        #endregion

        #region Image Display

        private ICommand _displayLargeImage;

        /// <summary>
        /// Gets the command to display the large (main) image.
        /// </summary>
        /// <value>
        /// The command to display the large (main) image.
        /// </value>
        public ICommand DisplayLargeImage
        {
            get
            {
                return _displayLargeImage ??
                       (_displayLargeImage = new RelayCommand(SwitchImage));
            }
        }

        private void SwitchImage(object o)
        {
            var img = o as Image;

            if (img != null)
            {
                MainImageSource = new BitmapImage(new Uri(img.Source.ToString()));
            }
        }

        private BitmapImage _mainImageSource;

        /// <summary>
        /// Gets or sets the main image source.
        /// </summary>
        /// <value>
        /// The main image source.
        /// </value>
        public BitmapImage MainImageSource
        {
            get { return _mainImageSource; }
            set
            {
                if (!Equals(_mainImageSource, value))
                {
                    _mainImageSource = value;
                    OnPropertyChanged();
                }
            }
        }

        private void UseDefaultImage()
        {
            // For now we will use a blank image.
            MainImageSource = new BitmapImage();
        }

        #endregion 
    }
}
