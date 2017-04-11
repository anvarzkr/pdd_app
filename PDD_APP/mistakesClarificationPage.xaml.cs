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
    /// Логика взаимодействия для mistakesClarificationPage.xaml
    /// </summary>
    public partial class mistakesClarificationPage : Page
    {
        public static TestingPage testingPage;
        public mistakesClarificationPage()
        {
            InitializeComponent();

            resultText.FontSize = Utils.fontSize + 16;
            TestingPageConstructor tpc = new TestingPageConstructor(testingPage);
            TestingPageConstructor.tabControl = tabControl;
            tpc.isAnswersImmediatelyShown = true;
            StackPanel tabs = tpc.getTabControls(testingPage.tasks.Length, false, true);

            if (tabs != null)
            {
                tpc.changeTab(0);
                for (int i = 0; i < testingPage.tasks.Length; i++)
                    if (testingPage.test.answered[i] == Test.ANSWER.WRONG)
                    {
                        tpc.changeTab(i);
                        break;
                    }

                tabs.HorizontalAlignment = HorizontalAlignment.Center;
                tabs.Orientation = Orientation.Horizontal;
                Grid.SetRow(tabs, 1);
                mainGrid.Children.Insert(0, tabs);
                mainGrid.Background = new SolidColorBrush(Utils.backgroundColor);

                resultTextBorder.Background = new SolidColorBrush(Utils.darkBlueColor2);
                resultText.FontFamily = Utils.stdFontFamily;

                ResultPageConstructor rpc = new ResultPageConstructor(this);

                for (int i = 0; i < testingPage.test.tasks.Length; i++)
                {
                    TabItem tabItem = TestingPageConstructor.getInvisibleTabItem();
                    tabItem.Content = rpc.getResultTaskGrid(testingPage.test.tasks[i], i);
                    tabControl.Items.Add(tabItem);
                }
            }
        }
    }
}
