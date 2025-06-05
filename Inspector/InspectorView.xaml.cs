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

namespace StoryMaker.Inspector
{
    /// <summary>
    /// Interaction logic for InspectorView.xaml
    /// </summary>
    public partial class InspectorView : UserControl
    {
        public InspectorView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PresentedObjectProperty = DependencyProperty.Register(
            nameof(PresentedObject),typeof(object),typeof(InspectorView),new PropertyMetadata(null,OnPresentedObjectChanged));

        private static void OnPresentedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = (InspectorView)d;
            me.VM.PresentedObject = e.NewValue;

            //me.itemsControl.ItemsSource=e.new
        }

        InspectorViewModel _vm;
        InspectorViewModel VM
        {
            get
            {
                if (_vm == null)
                    _vm = (InspectorViewModel)FindResource("VM");

                return _vm;
            }
        }

        public object PresentedObject
        {
            get
            {
                return (object)GetValue(PresentedObjectProperty);
            }

            set
            {
                SetValue(PresentedObjectProperty, value);
            }
        }

    }
}
