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
    /// Interaction logic for UserOptionsControl.xaml
    /// </summary>
    public partial class UserOptionsControl : UserControl
    {
        public UserOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [win position button click].
        /// </summary>
        public event EventHandler WinPositionButtonClick;

        /// <summary>
        /// Handles the Click event of the WinPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void WinPosition_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = ((ToggleButton)sender);
            btn.Content = btn.IsChecked.Value ? "Lock" : "Unlock";
            if (btn.IsChecked.Value)
                Settings.Default.Save();

            if (this.WinPositionButtonClick != null)
                this.WinPositionButtonClick(sender, e);
        }
    }
}