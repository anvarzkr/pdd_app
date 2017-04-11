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

namespace PDD_APP
{
    /// <summary>
    /// Логика взаимодействия для ResultPage.xaml
    /// </summary>
    public partial class ResultPage : Page
    {
        public ResultPage(TestingPage testingPage)
        {
            InitializeComponent();

            StackPanel tabs = new TestingPageConstructor(testingPage).getTabControls(2, false, false);
            if (tabs != null)
            {
                tabs.HorizontalAlignment = HorizontalAlignment.Center;
                tabs.Orientation = Orientation.Horizontal;
                mainStackPanel.Children.Insert(0, tabs);
            }

            timeBlock.Text = Utils.getTimeFromInt(testingPage.test.timeCounterPassed);
            mistakesClarification.MouseUp += mistakesClarification_MouseUp;
        }

        void mistakesClarification_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Utils.goToPage("mistakesClarificationPage");
        }

    }
}
