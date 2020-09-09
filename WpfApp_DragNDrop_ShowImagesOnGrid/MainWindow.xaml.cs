using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp_DragNDrop_ShowImagesOnGrid
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ContextMenu MainMenu;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
"\n\t 2. Разработать приложение, которое позволяет пользователю выполнять следующие функции:" +
"\n - картинки отображаются в Grid, одна картинка в ячейке" +
"\n - размер сетки можно задавать" +
"\n - при помощи DragNDrop можно затаскивать в окно одну картинку и она будет размещена в той" +
"\n ячейке, куда её затащили" +
"\n - функция перемешивания картинок на сетке",
"WPF DragNDrop Show Image on Grid");

            MenuItem contextItem;
            // Добавление пункта меню в контекстное меню для кнопки
            contextItem = new MenuItem();
            contextItem.Header = "Set Size";
            contextItem.Click += SetSize;

            MainGrid.ContextMenu = new ContextMenu();
            MainGrid.ContextMenu.Items.Add(contextItem);
            
            contextItem = new MenuItem();
            contextItem.Header = "Shuffle";
            contextItem.Click += Shuffle;
            MainGrid.ContextMenu.Items.Add(contextItem);

            MainGrid.ContextMenu.Items.Add(new Separator());

            contextItem = new MenuItem();
            contextItem.Header = "Close";
            contextItem.Click += Windows_Close_Click;
            MainGrid.ContextMenu.Items.Add(contextItem);

            // Добавление пункта меню в контекстное меню для кнопки
            contextItem = new MenuItem();
            contextItem.Header = "Set Size";
            contextItem.Click += SetSize;

            MainMenu = new ContextMenu(); 
            MainMenu.Items.Add(contextItem);

            contextItem = new MenuItem();
            contextItem.Header = "Shuffle";
            contextItem.Click += Shuffle;
            MainMenu.Items.Add(contextItem);

            MainMenu.Items.Add(new Separator());
            
            contextItem = new MenuItem();
            contextItem.Header = "Close";
            contextItem.Click += Close_Click;
            MainMenu.Items.Add(contextItem);

            PrepareMainGrid();
        }

        int countRowGrid = 5;
        int countColumnGrid = 5;
        int size = 3;//количество клеток на нашу кастомизированную клетку
        int shift = 1;//перекрытие клеток
        bool flag_even = true;//флаг четности, какие кубики выдвигать вперед

        List<Border> Border_Even = new List<Border>();
        List<Border> Border_Odd = new List<Border>();
        List<Image> Images = new List<Image>();

        private void PrepareMainGrid()
        {
            int cntRow = 0;
            int cntColumn = 0;

            cntRow = countRowGrid * 2 + 1;
            cntColumn = countColumnGrid * 2 + 1;

            MainGrid.RowDefinitions.Clear();
            for (int i = 0; i < cntRow; i++)
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            MainGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < cntColumn; i++)
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });

            int gridRow;
            int gridColumn;


            MainGrid.Children.Clear();
            Border_Even.Clear();
            Border_Odd.Clear();

            for (int i = 0; i < countRowGrid; i += 2)
            {
                gridRow = i / 2 * (size + size - 2 * shift);
                if (i % 2 != 0)
                    gridRow += 2;

                for (int j = 0; j < countColumnGrid; j++)
                {
                    int current_gridRow = gridRow;
                    gridColumn = j / 2 * (size + size - 2 * shift);
                    if (j % 2 != 0)
                    {
                        gridColumn += 2;
                        current_gridRow += 2;
                    }

                    if (current_gridRow + size > MainGrid.RowDefinitions.Count)
                        continue;

                    Border border = CreateBorderImage(current_gridRow, gridColumn);
                    MainGrid.Children.Add(border);


                    if (j % 2 == 0)
                    {
                        Grid.SetZIndex(border, maxZindex);
                        border.Child = rectEven;
                        Border_Even.Add(border);
                    }
                    else
                    {
                        Grid.SetZIndex(border, maxZindex - 1);
                        border.Child = rectOdd;
                        Border_Odd.Add(border);
                    }


                }
            }

//            if (flag_even)
//                for (int i = 0; i < countRowGrid; i += 2)
//                {
//                    gridRow = i / 2 * (size + size - 2 * shift);
//                    if (i % 2 != 0)
//                        gridRow += 2;
//
//                    for (int j = 0; j < countColumnGrid; j++)
//                    {
//                        int current_gridRow = gridRow;
//                        gridColumn = j / 2 * (size + size - 2 * shift);
//                        if (j % 2 != 0)
//                            continue;
//
//                        if (current_gridRow + size > MainGrid.RowDefinitions.Count)
//                            continue;
//
//                        Border border = CreateBorderImage(gridRow, gridColumn);
//                        MainGrid.Children.Add(border);
//
//                        if (j % 2 == 0)
//                            border.Child = new Rectangle()
//                            {
//                                Fill = new SolidColorBrush(Colors.Red)
//                            };
//                    }
//                }
//
//            for (int i = 0; i < countRowGrid; i += 2)
//            {
//                gridRow = i / 2 * (size + size - 2 * shift);
//                if (i % 2 != 0)
//                    gridRow += 2;
//
//                for (int j = 0; j < countColumnGrid; j++)
//                {
//                    int current_gridRow = gridRow;
//                    gridColumn = j / 2 * (size + size - 2 * shift);
//                    if (j % 2 != 0)
//                    {
//                        gridColumn += 2;
//                        current_gridRow += 2;
//                    }
//                    else
//                        continue;
//
//                    if (current_gridRow + size > MainGrid.RowDefinitions.Count)
//                        continue;
//
//                    Border border = CreateBorderImage(current_gridRow, gridColumn);
//                    MainGrid.Children.Add(border);
//
//                    if (j % 2 != 0)
//                        border.Child = new Rectangle()
//                        {
//                            Fill = new SolidColorBrush(Colors.Green)
//                        };
//                }
//            }
//
//            if (!flag_even)
//                for (int i = 0; i < countRowGrid; i += 2)
//                {
//                    gridRow = i / 2 * (size + size - 2 * shift);
//                    if (i % 2 != 0)
//                        gridRow += 2;
//
//                    for (int j = 0; j < countColumnGrid; j++)
//                    {
//                        int current_gridRow = gridRow;
//                        gridColumn = j / 2 * (size + size - 2 * shift);
//                        if (j % 2 != 0)
//                            continue;
//
//                        if (current_gridRow + size > MainGrid.RowDefinitions.Count)
//                            continue;
//
//                        Border border = CreateBorderImage(gridRow, gridColumn);
//                        MainGrid.Children.Add(border);
//
//                        if (j % 2 == 0)
//                            border.Child = new Rectangle()
//                            {
//                                Fill = new SolidColorBrush(Colors.Red)
//                            };
//                    }
//                }
        }

        int maxZindex = 10;
        private Border CreateBorderImage(int gridRow,int gridColumn)
        {
            Border border = new Border()
            {
                BorderThickness = new Thickness(2),
                BorderBrush = new SolidColorBrush(Colors.Coral),
                Name = "border_" + gridRow.ToString("0#") + "_" + gridColumn.ToString("0#")
            };
            border.PreviewMouseMove += border_PreviewMouseMove;

            border.PreviewDragEnter += border_PreviewDragEnter;
            border.PreviewDrop += border_PreviewDrop;

            border.PreviewMouseLeftButtonDown += border_PreviewMouseLeftButtonDown;

            Grid.SetRow(border, gridRow);
            Grid.SetColumn(border, gridColumn);
            Grid.SetRowSpan(border, size);
            Grid.SetColumnSpan(border, size);

            

            return border;
        }

        private Image CreateImage(BitmapSource bSource)
        {
            Image image = new Image()
            {
                Source = bSource,
                //Width = bSource.Width,
                //Height = bSource.Height,
                Stretch = Stretch.Fill
            };

            image.ContextMenu = MainMenu;

            return image;
        }

        private void SetSize(object sender, RoutedEventArgs e)
        {
            SetSizeWindows setSizeWindow = new SetSizeWindows();

            if (setSizeWindow.ShowDialog() == true)
            {
                countRowGrid = SetSizeWindows.rows;
                countColumnGrid = SetSizeWindows.colums;
            }
            else
                return;

            PrepareMainGrid();
            Shuffle();
        }

        private void Shuffle(object sender, RoutedEventArgs e)
        {
            Shuffle();
        }

        private void Shuffle()
        {
            foreach (var item in Border_Even)
                item.Child = rectEven;
            foreach (var item in Border_Odd)
                item.Child = rectOdd;

            int cnt_images = Images.Count;
            int cnt_size = Border_Even.Count + Border_Odd.Count;

            List<int> number_used = new List<int>();
            Random rand = new Random();
            int i = 0;
            int j = 0;
            while (true)
            {

                if (rand.Next(2) == 0)
                {
                    if (i < Border_Even.Count)
                        while (number_used.Count < cnt_images && number_used.Count < cnt_size)
                        {
                            int number = rand.Next(cnt_images);
                            if (number_used.Contains(number))
                                continue;

                            number_used.Add(number);
                            FreeChild(Images[number]);
                            Border_Even[i++].Child = Images[number];
                            break;
                        }
                }
                else
                {
                    if (j < Border_Odd.Count)
                        while (number_used.Count < cnt_images && number_used.Count < cnt_size)
                        {
                            int number = rand.Next(cnt_images);
                            if (number_used.Contains(number))
                                continue;

                            number_used.Add(number);
                            FreeChild(Images[number]);
                            Border_Odd[j++].Child = Images[number];
                            break;
                        }
                }

                if (((i < Border_Even.Count) || (j < Border_Odd.Count)) && number_used.Count < cnt_images)
                    continue;

                break;
            }
        }

        private void FreeChild(FrameworkElement element)
        {
            if (element.Parent != null)
            {
                if (element.Parent is Panel panel)
                    panel.Children.Remove(element);
                if (element.Parent is Decorator decorator)
                    decorator.Child=rectClear;
            }
        }

        Border currentBorder;
        Image currentImage;
        Rectangle rectOdd => new Rectangle()
        {
            Fill = new SolidColorBrush(Colors.Red)
        };
        Rectangle rectEven => new Rectangle()
        {
            Fill = new SolidColorBrush(Colors.Green)
        };
        Rectangle rectClear => new Rectangle()
        {
            Fill = new SolidColorBrush(Colors.White)
        };

        private void Windows_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (currentImage != null)
            {
                if (ImagesFiles.Contains((string)currentImage.Tag))
                    ImagesFiles.Remove((string)currentImage.Tag);
                if (Images.Contains(currentImage))
                    Images.Remove(currentImage);
                currentImage = null;
                currentBorder.Child = null;

                string[] str = currentBorder.Name.Split('_');
                int j = 0;
                foreach(var item in str)
                {
                    int.TryParse(item, out int tmp_int);
                    if (tmp_int != null)
                        j += tmp_int;
                }
                if (j % 2 == 0)
                {
                    currentBorder.Child = rectEven;
                }
                else
                {
                    currentBorder.Child = rectOdd;
                }
            }
        }

        private void border_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                if (border.Child is Image image)
                {
                    currentBorder = border;
                    currentImage = image;
                }

                int currentZindex;
                if (Border_Even.Contains(border) && !flag_even)
                {
                    currentZindex = Grid.GetZIndex(Border_Even[0]);
                    foreach (var item in Border_Even) 
                        Grid.SetZIndex(item,currentZindex+2);
                    flag_even = true;
                }
                else
                if (Border_Odd.Contains(border)&&flag_even)
                {
                    currentZindex = Grid.GetZIndex(Border_Odd[0]);
                    foreach (var item in Border_Odd)
                        Grid.SetZIndex(item, currentZindex + 2);
                    flag_even = false;
                }
            }
        }

        private void border_PreviewDragEnter(object sender, DragEventArgs e)
        {
            // Если пользователь копирует объект перетаскиванием и это список файлов и это не перетаскивание из listBox в него же
            if (e.Data.GetDataPresent(DataFormats.FileDrop) &&
                (e.AllowedEffects & DragDropEffects.Copy) != 0 &&
                !e.Data.GetDataPresent("Myappformat"))
            {
                // Разрешить копирование
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void border_PreviewDrop(object sender, DragEventArgs e)
        {
            // Если перетаскивается картинка
            if (e.Data.GetDataPresent("MyappformatImage"))
            {
                Image draggedImage = (Image)e.Data.GetData("MyappformatImage");
                if (sender is Border border)
                {
                    FreeChild(draggedImage);
                    border.Child = draggedImage;
          //          draggedImage.ContextMenu = MainMenu;
                }
            }

            // Если перетаскивается список файлов
            if (e.Data.GetDataPresent(DataFormats.FileDrop) &&
                (e.AllowedEffects & DragDropEffects.Copy) != 0 &&
                !e.Data.GetDataPresent("Myappformat"))
            {
                int cntImages = Images.Count;

                // Получить и напечатать список файлов
                string[] str = (string[])e.Data.GetData(DataFormats.FileDrop);
                           
                DirectoryInfo[] dirInfos = new DirectoryInfo[str.Length];
                int j = 0;

                FileInfo finfo;
                foreach (var item in str)
                {
                    finfo = new FileInfo(item);
                    if (finfo.Exists)
                        IsFile(finfo.FullName);
                    else
                        if ((finfo.Attributes & FileAttributes.Directory) != 0)
                        dirInfos[j++] = new DirectoryInfo(finfo.FullName);
                }

                 if (j != 0)
                    CheckDirectory(dirInfos);

                if (cntImages != Images.Count)
                {
                    if (sender is Border border)
                        border.Child = Images[cntImages];
                }
            }
        }

        private void border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // получить координаты мыши в listBox
            System.Windows.Point pt = e.GetPosition(this);

            // выяснить, над каким контролом находится курсор мыши
            HitTestResult res = System.Windows.Media.VisualTreeHelper.HitTest(this, pt);

            // если контрол не TextBlock, то ничего не делать
            if (!(res.VisualHit is Image))
                return;

            // получить TextBlock, соответствующий пункту, над которым находится курсор мыши
            Image draggedImage = (Image)res.VisualHit;

            // Создать контейнер для хранения данных
            DataObject draggedData = new DataObject();

            // Добавить признак пользовательского формата в контейнер
            draggedData.SetData("MyappformatImage", draggedImage);

            // НАЧАТЬ перетаскивание программно
            DragDropEffects dde = DragDrop.DoDragDrop(this, draggedData, DragDropEffects.Copy);
        }

        private void CheckFile(FileInfo[] fileInfos)
        {
            foreach (var finfo in fileInfos)
            {
                IsFile(finfo.FullName);
            }
        }

        private void CheckDirectory(DirectoryInfo[] dirInfos)
        {
            foreach (var dinfo in dirInfos)
            {
                if (dinfo != null)
                {
                    CheckFile(dinfo.GetFiles());
                    CheckDirectory(dinfo.GetDirectories());
                }
            }
        }

        List<string> ImagesFiles = new List<string>();
        private void IsFile(string item)
        {
            // Если выделено имя файла картинки - положить картинку в контейнер
            string ext = System.IO.Path.GetExtension(item);
            if (ext == ".bmp" || ext == ".jpg" || ext == ".gif" || ext == ".png")
            {
                if (!ImagesFiles.Contains(item))
                {
                    BitmapSource bSource = new BitmapImage(new Uri(item));
                    Image new_image = CreateImage(bSource);
                    new_image.ToolTip = new ToolTip()
                    {
                        Content = item
                    };
                    new_image.Tag = item;

                    Images.Add(new_image);

                    ImagesFiles.Add(item);
                }
            }
        }
    }
}
