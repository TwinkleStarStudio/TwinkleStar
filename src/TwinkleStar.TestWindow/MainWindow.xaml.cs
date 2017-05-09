using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwinkleStar.Common;
using TwinkleStar.Common.Logging;

namespace TwinkleStar.TestWindow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.btnClick.Click += BtnClick_Click;
        }

        private void BtnClick_Click(object sender, RoutedEventArgs e)
        {
            ShowVerifyCode();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Logger logger = Logger.GetLogger(this.GetType());
            logger.Info("Loaded");

            ShowVerifyCode();
        }

        private string ShowVerifyCode()
        {
            string code = "";
            BitmapImage bimg = new BitmapImage();
            byte[] temp = new VerifyCode().GetVerifyCode(out code);

            bimg.BeginInit();
            bimg.StreamSource = new MemoryStream(temp);
            bimg.EndInit();

            this.img.Source = bimg;

            return code;
        }
    }
}
