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

namespace camouflage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _MsgLen = 0;
        private bool _Res = false;
        string _Register = "01101000010";
        int _tapPos = 8;
        string _iImage = @"C:\E\projects\camouflage\diamond.png";
        string _oImage = @"C:\E\projects\camouflage\diamond_enc.png";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
             _iImage = @"C:\E\projects\camouflage\diamond.png";
            _oImage = @"C:\E\projects\camouflage\diamond_enc.png";

            if (string.IsNullOrEmpty(inputMessage.Text))
                return;
            _MsgLen = inputMessage.Text.Length;
            _Res = false;
            System.Drawing.Bitmap bmpOut = codec.HideMessage(inputMessage.Text, _iImage, _Register, _tapPos, _oImage,out _Res);
            if (_Res)
            {
                MessageBox.Show("Message hidden successfully");
            }
            else
            {
                MessageBox.Show("Failed to hide message.");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!_Res)
                return;

            string m = codec.RetrieveMessage(_MsgLen, _Register, _tapPos, _oImage);
            OutMessage.Text = m;
        }
    }
}
