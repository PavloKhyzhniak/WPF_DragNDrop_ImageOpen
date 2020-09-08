using System;
using System.Collections.Generic;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
//            MessageBox.Show(
//"\n\t 2. Разработать приложение, которое позволяет пользователю выполнять следующие функции:" +
//"\n - картинки отображаются в Grid, одна картинка в ячейке" +
//"\n - размер сетки можно задавать" +
//"\n - при помощи DragNDrop можно затаскивать в окно одну картинку и она будет размещена в той" +
//"\n ячейке, куда её затащили" +
//"\n - функция перемешивания картинок на сетке",
//"WPF DragNDrop Show Image on Grid");

            PrepareMainGrid();
        }

        int countRowGrid = 5;
        int countColumnGrid = 5;
        int size = 3;//количество клеток на нашу кастомизированную клетку
        int shift = 1;//перекрытие клеток
        bool flag_even = true;//флаг четности, какие кубики выдвигать вперед
        private void PrepareMainGrid()
        {
            int cntRow = 0;
            int cntColumn = 0;

            cntRow = countRowGrid * 2 + 1;
            cntColumn = countColumnGrid * 2 + 1;

            for (int i = 0; i < cntRow; i++)
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            for (int i = 0; i < cntColumn; i++)
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });

            int gridRow;
            int gridColumn;
            //            for (int i = 0; i < countRowGrid; i+=2)
            //            {
            //                gridRow = i / 2 * (size + size - 2*shift);
            //                if (i % 2 != 0)
            //                    gridRow += 2;
            //
            //                for (int j = 0; j < countColumnGrid; j++)
            //                {
            //                    int current_gridRow = gridRow;
            //                    gridColumn = j / 2 * (size + size - 2*shift);
            //                    if (j % 2 != 0)
            //                    {
            //                        gridColumn += 2;
            //                        current_gridRow += 2;
            //                    }
            //
            //                    if (current_gridRow + size > MainGrid.RowDefinitions.Count)
            //                        continue;
            //
            //                    Border border = CreateBorderImage(current_gridRow, gridColumn);
            //                    MainGrid.Children.Add(border);
            //
            //
            //                    if (j % 2 == 0)
            //                        border.Child = new Rectangle()
            //                        {
            //                            Fill = new SolidColorBrush(Colors.Red)
            //                        };
            //                    else
            //                        border.Child = new Rectangle()
            //                        {
            //                            Fill = new SolidColorBrush(Colors.Green)
            //                        };
            //
            //
            //                }
            //            }

            if (flag_even)
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
                            continue;

                        if (current_gridRow + size > MainGrid.RowDefinitions.Count)
                            continue;

                        Border border = CreateBorderImage(gridRow, gridColumn);
                        MainGrid.Children.Add(border);

                        if (j % 2 == 0)
                            border.Child = new Rectangle()
                            {
                                Fill = new SolidColorBrush(Colors.Red)
                            };
                    }
                }

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
                    else
                        continue;

                    if (current_gridRow + size > MainGrid.RowDefinitions.Count)
                        continue;

                    Border border = CreateBorderImage(current_gridRow, gridColumn);
                    MainGrid.Children.Add(border);

                    if (j % 2 != 0)
                        border.Child = new Rectangle()
                        {
                            Fill = new SolidColorBrush(Colors.Green)
                        };
                }
            }

            if (!flag_even)
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
                            continue;

                        if (current_gridRow + size > MainGrid.RowDefinitions.Count)
                            continue;

                        Border border = CreateBorderImage(gridRow, gridColumn);
                        MainGrid.Children.Add(border);

                        if (j % 2 == 0)
                            border.Child = new Rectangle()
                            {
                                Fill = new SolidColorBrush(Colors.Red)
                            };
                    }
                }
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

            Grid.SetRow(border, gridRow);
            Grid.SetColumn(border, gridColumn);
            Grid.SetRowSpan(border, size);
            Grid.SetColumnSpan(border, size);

            if ((gridRow + gridColumn) % 2 == 0)
                Grid.SetZIndex(border, maxZindex);
            else
                Grid.SetZIndex(border, maxZindex-1);

            return border;
        }

        private Image CreateImage(BitmapSource bSource)
        {
            Image image = new Image()
            {
                Source = bSource,
                Width = bSource.Width,
                Height = bSource.Height
            };
//            image.MouseLeftButtonDown += StartDrag;

//            image.PreviewMouseLeftButtonDown += Image_PreviewMouseLeftButtonDown;
//            image.PreviewMouseLeftButtonUp += Image_PreviewMouseLeftButtonUp;
            //            image.PreviewMouseMove += Image_PreviewMouseMove;

//            image.PreviewMouseDown += Image_PreviewMouseDown;
//            image.PreviewMouseRightButtonDown += Image_PreviewMouseRightButtonDown;

            return image;
        }

    }
}
