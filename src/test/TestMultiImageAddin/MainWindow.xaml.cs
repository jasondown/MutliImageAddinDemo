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


            var image1 = new Image();
            image1.SetValue(Image.SourceProperty, converter.ConvertFromString(@"pack://application:,,,/TestImages/10100241.jpg"));
            images.Add(image1);

            var image2 = new Image();
            image2.SetValue(Image.SourceProperty, converter.ConvertFromString(@"pack://application:,,,/TestImages/10100242.jpg"));
            images.Add(image2);

            var image3 = new Image();
            image3.SetValue(Image.SourceProperty, converter.ConvertFromString(@"pack://application:,,,/TestImages/10100243.jpg"));
            images.Add(image3);

            var image4 = new Image();
            image4.SetValue(Image.SourceProperty, converter.ConvertFromString(@"pack://application:,,,/TestImages/10100244.jpg"));
            images.Add(image4);

            repo.SetImages(images);
            vm.SetImageRepository(repo);

            View.DataContext = vm;
        }
    }
}
