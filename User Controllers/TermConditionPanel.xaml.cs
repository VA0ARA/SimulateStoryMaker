using StoryMaker.Api;
using StoryMaker.Helpers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StoryMaker.User_Controllers
{
    /// <summary>
    /// Interaction logic for TermConditionPanel.xaml
    /// </summary>
    public partial class TermConditionPanel : UserControl
    {
        public TermConditionPanel()
        {
            InitializeComponent();
            ApiHelper.initClient();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private async Task LoadData()
        {
            var data = await RequestHandler.getTerms();
            terms_progressbar.Visibility = Visibility.Hidden;
            terms_desc.Text = Utils.HtmlToPlainText(data.data.Content);
            terms_content_panel.Visibility = Visibility.Visible;
        }


    }
}
