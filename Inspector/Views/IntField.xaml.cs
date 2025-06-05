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

namespace StoryMaker.Inspector.Views
{
    /// <summary>
    /// Interaction logic for IntField.xaml
    /// </summary>
    public partial class IntField : UserControl
    {
        public IntField()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FieldProperty = DependencyProperty.Register(
            nameof(Field),typeof(Models.IntProperty),typeof(IntField));

        public Models.IntProperty Field
        {
            get
            {
                return (Models.IntProperty)GetValue(FieldProperty);
            }

            set

            {
                SetValue(FieldProperty, value);
            }
        }
    }
}
