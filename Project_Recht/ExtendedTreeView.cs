using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Project_Recht
{
    //100% van internet gehaald. op deze manier kan ik een treeview maken in de xaml waar er een optie is om een item te selecteren
    //standaard lukte dit niet om deze te binden omdat er alleen maar via selectedvaluepath kon worden gewerkt
    //omdat selecteditem en selectedvalue alleen een getter hadden en geen setter dus de selectedvaluepath
    //was niet te gebruiken om een property van het type TreeViewItem op te vullen
    //aangezien valuepath gaat zoeken naar iets en zo de selectedvalue zal instellen.
    //deze klasse zorgt ervoor dat ik zelf een selecteditem kan binden naar mijn viewmodel
    public class ExtendedTreeView : TreeView
    {
        public ExtendedTreeView()
            : base()
        {
            this.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(ICH);
        }

        private void ICH(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedItem != null)
            {
                SetValue(SelectedItem_Property, SelectedItem);
            }
        }

        public object SelectedItem_
        {
            get { return (object)GetValue(SelectedItem_Property); }
            set { SetValue(SelectedItem_Property, value); }
        }
        public static readonly DependencyProperty SelectedItem_Property = DependencyProperty.Register("SelectedItem_", typeof(object), typeof(ExtendedTreeView), new UIPropertyMetadata(null));
    }
}
