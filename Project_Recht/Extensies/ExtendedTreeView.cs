using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Project_Recht
{
    // this way I can create a treeview in the xaml where there is an option to select an item
    //by default this could not be bound because it could only be done via selectedvaluepath
    //because selecteditem and selectedvalue only had a getter and no setter so the selectedvaluepath
    //could not be used to fill a property of type TreeViewItem
    //because valuepath is going to search for something and so will set the selectedvalue.
    //this class allows me to bind a selecteditem to my viewmodel and view myself

    public class ExtendedTreeView : TreeView
    {
        public ExtendedTreeView()
            : base()
        {
            this.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(SetProperty);
        }

        private void SetProperty(object sender, RoutedPropertyChangedEventArgs<object> e)
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
