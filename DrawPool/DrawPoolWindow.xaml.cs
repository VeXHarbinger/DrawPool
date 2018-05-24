using DrawPool.Properties;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrawPool
{
    /// <summary>
    /// Interaction logic for DrawPoolWindow.xaml
    /// </summary>
    public partial class DrawPoolWindow : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawPoolWindow"/> class.
        /// </summary>
        public DrawPoolWindow()
        {
            InitializeComponent();
            Initialize();
            Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Gets or sets the win opacity and converts it to a double.
        /// </summary>
        /// <value>The win opacity.</value>
        public double WinOpacity
        {
            get { return Settings.Default.Opacity; }
            set
            {
                Settings.Default.Opacity = (value / 100);
                Opacity = Settings.Default.Opacity;
            }
        }

        /// <summary>
        /// The Window position/configuration mode handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void WinPositionMode(object sender, EventArgs e)
        {
            ToggleButton btn = ((ToggleButton)sender);
            IsWindowDraggable = btn.IsChecked.Value;
        }
    }
}