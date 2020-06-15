using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Laba5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        ColorDialog StrokeColorDialog, BackgroundColorDialog;
        ObservableCollection<Star> starList = new ObservableCollection<Star>();
        OpenFileDialog OpenFileDialog;
        SaveFileDialog SaveFileDialog;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point pointClicked = e.GetPosition(canvas);

            Star currentStart = new Star(pointClicked.X, pointClicked.Y, BtnBackgroundColor.Background, BtnStrokeColor.Background, slWight.Value);
            Grid.Children.Add(currentStart.getPath());
            starList.Add(currentStart);

        }


        private void BtnBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            BackgroundColorDialog = new ColorDialog();
            BackgroundColorDialog.FullOpen = true;
            BackgroundColorDialog.ShowDialog();

            var Line = BackgroundColorDialog.Color;
            Color mediaColor = Color.FromRgb(Line.R, Line.G, Line.B);       
            BtnBackgroundColor.Background = new SolidColorBrush(mediaColor);
        }

        private void BtnStrokeColor_Click(object sender, RoutedEventArgs e)
        {
            StrokeColorDialog = new ColorDialog();
            StrokeColorDialog.FullOpen = true;
            StrokeColorDialog.ShowDialog();

            var Line = StrokeColorDialog.Color;
            Color mediaColor = Color.FromRgb(Line.R, Line.G, Line.B);
            BtnStrokeColor.Background = new SolidColorBrush(mediaColor);
        }

        private void canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point pointClicked = e.GetPosition(canvas);
            Status.Content = string.Format("Координаты X={0:f0} Y={1:f0}", pointClicked.X, pointClicked.Y);
        }

        private void CommandBinding_Executed_Open(object sender, ExecutedRoutedEventArgs e)
        {

            OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.InitialDirectory = "D:\\ПОИС\\СВПП";
            OpenFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            OpenFileDialog.CheckFileExists = true;
            OpenFileDialog.Multiselect = true;
            OpenFileDialog.ShowDialog();
            if (string.IsNullOrEmpty(OpenFileDialog.FileName) == false)
            {
                starList.Clear();
                var FileText = File.ReadAllLines(OpenFileDialog.FileName);
                statusBarFileName.Content = string.Format("Имя файла: {0}", OpenFileDialog.FileName);
                Grid.Children.Clear();
                Grid.Children.Add(canvas);           
                List<Star> newList = new List<Star>();
                XmlSerializer xs = new XmlSerializer(typeof(List<Star>));
                using (FileStream fs = new FileStream(OpenFileDialog.FileName, FileMode.Open))
                {
                    newList = (List<Star>)xs.Deserialize(fs);
                    foreach(Star _star in newList)
                    {
                        _star.PaintFigure();
                        Grid.Children.Add(_star.getPath());
                        starList.Add(_star);
                    }
                }
             
            }
        }

        private void CommandBinding_CanExecute_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = starList.Count > 0;
        }

        private void CommandBinding_Executed_Save(object sender, ExecutedRoutedEventArgs e)
        {      
            SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.InitialDirectory = "D:\\ПОИС\\СВПП";
            SaveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            SaveFileDialog.OverwritePrompt = false;
            SaveFileDialog.Title = "Сохранить это творение...";
            SaveFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(SaveFileDialog.FileName) == false)
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<Star>));
                using (FileStream fs = new FileStream(SaveFileDialog.FileName, FileMode.OpenOrCreate))
                {
                    xs.Serialize(fs, new List<Star>(starList));
                }               
                statusBarFileName.Content = string.Format("Имя файла: {0}", SaveFileDialog.FileName);
            }

        }

    }
}
