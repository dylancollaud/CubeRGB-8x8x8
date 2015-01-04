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

namespace InterfaceCube
{
    /// <summary>
    /// Logique d'interaction pour Configuration.xaml
    /// </summary>
    public partial class Configuration : UserControl
    {
        private Cube _cube3D;
        public Configuration(Cube cube3D)
        {
            _cube3D = cube3D;
            InitializeComponent();
        }
    }
}
