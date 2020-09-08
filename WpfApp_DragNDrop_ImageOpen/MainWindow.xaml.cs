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

namespace WpfApp_DragNDrop_ImageOpen
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
                
        int maxZindex = 10;

        private Image CreateImage(BitmapSource bSource)
        {
            Image image = new Image()
            {
                Source=bSource,
                Width = bSource.Width,
                Height = bSource.Height
            };
            image.MouseLeftButtonDown += StartDrag;

            image.PreviewMouseLeftButtonDown += Image_PreviewMouseLeftButtonDown;
            image.PreviewMouseLeftButtonUp += Image_PreviewMouseLeftButtonUp;
//            image.PreviewMouseMove += Image_PreviewMouseMove;

            image.PreviewMouseDown += Image_PreviewMouseDown;
            image.PreviewMouseRightButtonDown += Image_PreviewMouseRightButtonDown;

            // Добавление пункта меню в контекстное меню для кнопки
            MenuItem contextItem = new MenuItem();
            contextItem.Header = "Close";
            contextItem.Click += Close_Click;

            image.ContextMenu = new ContextMenu();
            image.ContextMenu.Items.Add(contextItem);

            contextItem = new MenuItem();
            contextItem.Header = "Rotate";
            contextItem.Click += Rotate_Click;
            image.ContextMenu.Items.Add(contextItem);

            contextItem = new MenuItem();
            contextItem.Header = "Resize";
            contextItem.Click += Resize_Click;
            image.ContextMenu.Items.Add(contextItem);

            return image;
        }

        UIElement currentUIElement;
        Point currentPointElementClick;
        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement element)
            {
                // получить координаты мыши в listBox
                currentPointElementClick = e.GetPosition(element);

                currentUIElement = element;
            }
        }

        private void Image_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
//            if (sender is UIElement element)
//                currentUIElement = element;
        }

        bool flag_LeftButtonDown = false;

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                if (sender is UIElement element)
                    flag_LeftButtonDown = true;
        }
        
        private void Image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.LeftButton == MouseButtonState.Released && flag_LeftButtonDown)
                if (sender is UIElement element)
                {
                    int currentZindex = Canvas.GetZIndex(element);
                    currentZindex--;

                    Canvas.SetZIndex(element, currentZindex);
                }
        
            flag_LeftButtonDown = false;
        }

//        private Point mouseOffset;
//        private Point elementOffset;
//        private bool isMouseDown = false;
//
//        bool flag_LeftButtonDown = false;
//
//        private void Image_PreviewMouseMove(object sender, MouseEventArgs e)
//        {
//            if (sender is UIElement cur_element && !cur_element.Equals(currentUIElement) && isMouseDown)
//            {
//                if (cur_element == null)
//                    return;
//
//                int currentZindex = Canvas.GetZIndex(cur_element);
//                currentZindex--;
//                Canvas.SetZIndex(cur_element, currentZindex);
// 
//                return;
//            }
//            double xOffset = 0;
//            double yOffset = 0;
//
//            flag_LeftButtonDown = false;
//            if (e.LeftButton == MouseButtonState.Pressed && isMouseDown)
//                if (sender is UIElement element)
//                {
//                    // получить координаты мыши в listBox
//                    System.Windows.Point mousePos = e.GetPosition(element);
//
//                    mousePos.Offset(mouseOffset.X, mouseOffset.Y);
//
//                    Canvas.SetLeft(element, elementOffset.X + mousePos.X);
//                    Canvas.SetTop(element, elementOffset.Y + mousePos.Y);
//
//                    xOffset = Canvas.GetLeft(element);
//                    yOffset = Canvas.GetTop(element);
//
//                    elementOffset = new Point(xOffset, yOffset);
//
//                    CheckForOverlap(CanvasMain);
//                }
//        }
//
//        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
//        {
//            double xOffset = 0;
//            double yOffset = 0;
//
//            if (e.LeftButton == MouseButtonState.Pressed && !isMouseDown)
//                if (sender is UIElement element)
//                {
//                    // получить координаты мыши в listBox
//                    System.Windows.Point pt = e.GetPosition(element);
//
//                    xOffset = -pt.X;// - SystemInformation.FrameBorderSize.Width;
//                    yOffset = -pt.Y;// - SystemInformation.CaptionHeight - SystemInformation.FrameBorderSize.Height;
//                    mouseOffset = new Point(xOffset, yOffset);
//
//                    xOffset = Canvas.GetLeft(element);
//                    yOffset = Canvas.GetTop(element);
//
//                    //Canvas.SetZIndex(element, maxZindex);
//
//                    elementOffset = new Point(xOffset, yOffset);
//
//                    isMouseDown = true;
//                    flag_LeftButtonDown = true;
//                }
//        }
//
//        private void Image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
//        {
//            if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.LeftButton == MouseButtonState.Released && flag_LeftButtonDown)
//                if (sender is UIElement element)
//                {
//                    int currentZindex = Canvas.GetZIndex(element);
//                    //                    if (currentZindex > 0)
//                    currentZindex--;
//                    //                    else
//                    //                        currentZindex = maxZindex;
//                    Canvas.SetZIndex(element, currentZindex);
//                }
//
//            if (e.LeftButton == MouseButtonState.Released)
//                isMouseDown = false;
//
//            flag_LeftButtonDown = false;
//        }

        private bool CheckForOverlap(Panel panel)
        {
            int curZindex = Canvas.GetZIndex(currentUIElement);
            Rect thisRect = GetRect(currentUIElement as FrameworkElement);
            Rect otherRect;
            
            foreach (FrameworkElement child in panel.Children)
            {
                if (ReferenceEquals(child, currentUIElement))
                    continue;
                otherRect = GetRect(child);

                if (thisRect.IntersectsWith(otherRect))
                    Canvas.SetZIndex(child, curZindex - 1);
            }

            return false;
        }

        private void CheckIntersect(Panel panel, FrameworkElement element)
        {
            Rect rect = GetRect(element);
        }

        private Rect GetRect(FrameworkElement element)
        {
            return new Rect(new Point(Canvas.GetLeft(element), Canvas.GetTop(element)), new Size(element.ActualWidth, element.ActualHeight));
        }



        private void Rotate_Click(object sender, RoutedEventArgs e)
        {
            double x = 0;
            double y = 0;
            if (currentUIElement is Image image)
            {
//                x = Canvas.GetLeft(currentUIElement) + image.ActualWidth / 2;
//                y = Canvas.GetTop(currentUIElement) + image.ActualHeight / 2;
                x = image.ActualWidth / 2;
                y = image.ActualHeight / 2;
            }

            // Матричное преобразование
            Matrix m = currentUIElement.RenderTransform.Value;// new Matrix();
//            m.RotateAt(Rotate, currentPointElementClick.X, currentPointElementClick.Y);
            m.RotateAt(Rotate, x, y);
            MatrixTransform mt = new MatrixTransform(m);
            currentUIElement.RenderTransform = mt;
        }

        private void Resize_Click(object sender, RoutedEventArgs e)
        {
            // Матричное преобразование
            Matrix m = currentUIElement.RenderTransform.Value;// new Matrix();
            m.Scale(Scale, Scale);
            MatrixTransform mt = new MatrixTransform(m);
            
            currentUIElement.RenderTransform = mt;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if(currentUIElement != null)
                CanvasMain.Children.Remove(currentUIElement);
        }
             
        private void mainWindow_PreviewDragEnter(object sender, DragEventArgs e)
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

        private void mainWindow_PreviewDrop(object sender, DragEventArgs e)
        {
            // Если перетаскивается список файлов
            if (e.Data.GetDataPresent(DataFormats.FileDrop) &&
                (e.AllowedEffects & DragDropEffects.Copy) != 0 &&
                !e.Data.GetDataPresent("Myappformat"))
            {
                // Получить и напечатать список файлов
                string[] str = (string[])e.Data.GetData(DataFormats.FileDrop);

                // получить координаты мыши в listBox
                System.Windows.Point pt = e.GetPosition(this);

                //                FileInfo[] fileInfos = new FileInfo[str.Length];
                //                int i = 0;

                DirectoryInfo[] dirInfos = new DirectoryInfo[str.Length];
                int j = 0;

                FileInfo finfo;
                foreach (var item in str)
                {
                    finfo = new FileInfo(item);
                    if (finfo.Exists)
                        //                        fileInfos[i++] = finfo;
                        IsFile(finfo.FullName, pt);
                    else
                        if ((finfo.Attributes & FileAttributes.Directory) != 0)
                        dirInfos[j++] = new DirectoryInfo(finfo.FullName);
                }

                //                CheckFile(fileInfos, pt);
                if (j != 0)
                    CheckDirectory(dirInfos, pt);
            }

//            DisableEditing(CanvasMain);
//            EnableEditing(CanvasMain);
        }
        private void CheckFile(FileInfo[] fileInfos,Point pt)
        {
            foreach (var finfo in fileInfos)
            {
                IsFile(finfo.FullName, pt);
            }
        }
        
        private void CheckDirectory(DirectoryInfo[] dirInfos, Point pt)
        {
            foreach (var dinfo in dirInfos)
            {
                if (dinfo != null)
                {
                    CheckFile(dinfo.GetFiles(), pt);
                    CheckDirectory(dinfo.GetDirectories(), pt);
                }
            }
        }

        private void IsFile(string item,Point pt)
        {
            // Если выделено имя файла картинки - положить картинку в контейнер
            string ext = System.IO.Path.GetExtension(item);
            if (ext == ".bmp" || ext == ".jpg" || ext == ".gif" || ext == ".png")
            {
                BitmapSource bSource = new BitmapImage(new Uri(item));
                Image new_image = CreateImage(bSource);
                new_image.ToolTip = new ToolTip()
                {
                    Content = item
                };

                CanvasMain.Children.Add(new_image);
                Canvas.SetLeft(new_image, pt.X);
                Canvas.SetTop(new_image, pt.Y);
                Canvas.SetZIndex(new_image, maxZindex);
            }

        }

//        private void DisableEditing(Grid theGrid)
//        {
//            // Remove all Adorners of all Controls
//            foreach (FrameworkElement child in theGrid.Children)
//            {
//                var layer = AdornerLayer.GetAdornerLayer(child);
//                var adorners = layer.GetAdorners(child);
//                if (adorners == null)
//                    continue;
//                foreach (var adorner in adorners)
//                    layer.Remove(adorner);
//            }
//        }
//
//        private void EnableEditing(Grid theGrid)
//        {
//            foreach (FrameworkElement child in theGrid.Children)
//            {
//                // Add a MoveAdorner for every single child
//                Adorner adorner = new MoveAdorner(child);
//
//                // Add the Adorner to the closest (hierarchically speaking) AdornerLayer
//                AdornerLayer.GetAdornerLayer(child).Add(adorner);
//            }
//        }

        private void DisableEditing(Panel thePanel)
        {
            // Remove all Adorners of all Controls
            foreach (FrameworkElement child in thePanel.Children)
            {
                var layer = AdornerLayer.GetAdornerLayer(child);
                var adorners = layer.GetAdorners(child);
                if (adorners == null)
                    continue;
                foreach (var adorner in adorners)
                    layer.Remove(adorner);
            }
        }

        private void EnableEditing(Panel thePanel)
        {
            foreach (FrameworkElement child in thePanel.Children)
            {
                // Add a MoveAdorner for every single child
                Adorner adorner = new MoveAdorner(child);

                // Add the Adorner to the closest (hierarchically speaking) AdornerLayer
                AdornerLayer.GetAdornerLayer(child).Add(adorner);
            }
        }

        double Rotate;
        double Scale;
        private void RotateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue;
            Rotate = e.NewValue;
            textBlockRotate.Text = "Rotate " + Rotate.ToString("0##.#");  
        }

        private void ScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue;
            Scale = e.NewValue;
            textBlockScale.Text = "Scale " + Scale.ToString("0##.#");
        }

        private void mainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CanvasMain.Width = borderCanvasMain.ActualWidth - borderCanvasMain.Margin.Left - borderCanvasMain.Margin.Right;
            CanvasMain.Height = borderCanvasMain.ActualHeight - borderCanvasMain.Margin.Top - borderCanvasMain.Margin.Bottom;
        }

      

        Vector relativeMousePos;
        FrameworkElement draggedObject;
        
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            FinishDrag(sender, e);
            Mouse.Capture(null);        
        }
        
        void StartDrag(object sender, MouseButtonEventArgs e)
        {
            draggedObject = (FrameworkElement)sender;
            relativeMousePos = e.GetPosition(draggedObject) - new Point();
            draggedObject.MouseMove += OnDragMove;
            draggedObject.LostMouseCapture += OnLostCapture;
            draggedObject.MouseUp += OnMouseUp;
            Mouse.Capture(draggedObject);
        }

        void OnDragMove(object sender, MouseEventArgs e)
        {
            UpdatePosition(e);
        }

        void UpdatePosition(MouseEventArgs e)
        {
            var point = e.GetPosition(CanvasMain);
            var newPos = point - relativeMousePos;
            Canvas.SetLeft(draggedObject, newPos.X);
            Canvas.SetTop(draggedObject, newPos.Y);

            if(flag_LeftButtonDown)
                CheckForOverlap(CanvasMain);
        }

        void OnLostCapture(object sender, MouseEventArgs e)
        {
            FinishDrag(sender, e);
        }

        void FinishDrag(object sender, MouseEventArgs e)
        {
            draggedObject.MouseMove -= OnDragMove;
            draggedObject.LostMouseCapture -= OnLostCapture;
            draggedObject.MouseUp -= OnMouseUp;
            UpdatePosition(e);
        }

    }
}
