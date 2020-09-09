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
using System.Windows.Shapes;

namespace WpfApp_DragNDrop_ShowImagesOnGrid
{
    /// <summary>
    /// Логика взаимодействия для SetSizeWindows.xaml
    /// </summary>
    public partial class SetSizeWindows : Window
    {
        public SetSizeWindows()
        {
            InitializeComponent();

            textBox_Colums.Text = colums.ToString();
            textBox_Rows.Text = rows.ToString();           
        }

        public static int rows { get; set; }
        public static int colums { get; set; }
    
        int min_cnt_rows = 2;
        int min_cnt_colums = 2;
        int max_cnt_rows = 20;
        int max_cnt_colums = 20;

        private void textBox_Colums_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(((TextBox)sender).Text, out int tmp_int);
            if (tmp_int != null && tmp_int > 0)
            {
                if (tmp_int >= max_cnt_colums)
                    colums = max_cnt_colums;
                else if (tmp_int <= min_cnt_colums)
                    colums = min_cnt_colums;
                else
                    colums = tmp_int;
                ((TextBox)sender).Text = colums.ToString();
            }
            else
                ((TextBox)sender).Text = min_cnt_colums.ToString();
        }

        private void textBox_Rows_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(((TextBox)sender).Text, out int tmp_int);
            if (tmp_int != null && tmp_int > 0)
            {
                if (tmp_int >= max_cnt_rows)
                    rows = max_cnt_rows;
                else if (tmp_int <= min_cnt_rows)
                    rows = min_cnt_rows;
                else
                    rows = tmp_int;
                ((TextBox)sender).Text = rows.ToString();
            }
            else
                ((TextBox)sender).Text = min_cnt_rows.ToString();
        }
               
        private void buttonSet_Click(object sender, EventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
