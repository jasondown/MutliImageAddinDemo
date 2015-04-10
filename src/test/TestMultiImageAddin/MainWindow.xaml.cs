using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Jason.Down.Blog.MutliImageAddinDemo.Model;
using Jason.Down.Blog.MutliImageAddinDemo.ViewModel;
using Image = System.Windows.Controls.Image;

namespace TestMultiImageAddin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new PageableImageControlViewModel() {PageSize = 3};
            var converter = new ImageSourceConverter();

            var repo = new NavQueryObjectImageRepository("1010024");
            var images = new List<Image>();

            var paths = new List<string>
            {
                @"pack://application:,,,/TestImages/10100241.jpg",
                @"pack://application:,,,/TestImages/10100242.jpg",
                @"pack://application:,,,/TestImages/10100243.jpg",
                @"pack://application:,,,/TestImages/10100244.jpg"
            };

            foreach (var path in paths)
            {
                var image = new Image();
                image.SetValue(Image.SourceProperty, converter.ConvertFromString(path));
                images.Add(image);
            }

            repo.SetImages(images);
            vm.SetImageRepository(repo);

            View.DataContext = vm;
        }
    }
}
