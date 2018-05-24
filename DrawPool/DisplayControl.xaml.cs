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

namespace DrawPool
{
    /// <summary>
    /// Interaction logic for DisplayControl.xaml
    /// </summary>
    public partial class DisplayControl : UserControl
    {
        public DisplayControl()
        {
            InitializeComponent();
            //System.Convert.ToDouble(0);
        }

        /// <summary>
        /// Resets this instance's <see cref="Card"/> lists.
        /// </summary>
        public void Reset()
        {
            CardList.Update(null, true);
            this.Visibility = Visibility.Collapsed;
        }
    }
}