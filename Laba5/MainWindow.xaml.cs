using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

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
        Star _startInFile;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point pointClicked = e.GetPosition(canvas);

            Star currentStart = new Star(pointClicked.X, pointClicked.Y, BtnBackgroundColor.Background, BtnStrokeColor.Background, slWight.Value);
            Grid.Children.Add(currentStart.path);
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
            OpenFileDialog.Filter = "Текстовый файл (*.txt)|*.txt|Все файлы (*.*)|*.*";
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
                var s = 0;
                _startInFile = new Star();
                for (int i = 0; i < FileText.GetLength(0); i++)
                {
                    s++;

                    switch (s)
                    {
                        case 1: _startInFile.X = double.Parse(FileText[i]); break;
                        case 2: _startInFile.Y = double.Parse(FileText[i]); break;
                        case 3: _startInFile.BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(FileText[i])); break;
                        case 4: _startInFile.StrokeColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(FileText[i])); break;
                        case 5:
                            _startInFile.Thickness = double.Parse(FileText[i]);
                            _startInFile.PaintFigure();
                            Grid.Children.Add(_startInFile.path); //Выводим на Canvas
                            starList.Add(_startInFile);// Добавляем в список
                            s = 0;
                            _startInFile = new Star();
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Команда сохраняет данные о фигурах в файл указанный в Dialog окне
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_Executed_Save(object sender, ExecutedRoutedEventArgs e)
        {

            SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.InitialDirectory = "D:\\ПОИС\\СВПП";
            SaveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            SaveFileDialog.OverwritePrompt = false;
            SaveFileDialog.Title = "Сохранить это творение...";
            SaveFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(SaveFileDialog.FileName) == false)
            {
                File.WriteAllLines(SaveFileDialog.FileName, GetFileText());
                statusBarFileName.Content = string.Format("Имя файла: {0}", SaveFileDialog.FileName);
            }

        }


        /// <summary>
        /// Метод возвращает Список с исходными данными всех фигур построчно (X,Y,BuFon,BuLine,Thickness)
        /// </summary>
        /// <returns></returns>
        private List<string> GetFileText()
        {
            var FileText = new List<string>();
            foreach (Star a in starList)
            {
                FileText.Add(a.X.ToString());
                FileText.Add(a.Y.ToString());
                FileText.Add(a.BackgroundColor.ToString());
                FileText.Add(a.StrokeColor.ToString());
                FileText.Add(a.Thickness.ToString());
            }

            return FileText;
        }




    }
}
