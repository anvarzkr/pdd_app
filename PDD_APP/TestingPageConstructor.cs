using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using PDD_APP.Models;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace PDD_APP
{
    class TestingPageConstructor
    {
        public static TabControl tabControl;
        private static Border currentTab = null;
        private TextBlock[][] textBlockAnses;
        private Border[][] borderBlockAnses;
        private Border[] confirmButtons;
        private bool[] isConfirmed;
        private int[] ansChecked;
        private TestingPage testingPage;
        public bool isAnswersImmediatelyShown = false;

        public TestingPageConstructor(TestingPage testingPage) 
        {
            confirmButtons = new Border[20];
            ansChecked = new int[20];
            isConfirmed = new bool[20];
            textBlockAnses = new TextBlock[20][];
            borderBlockAnses = new Border[20][];
            for (int i = 0; i < textBlockAnses.Length; i++)
                textBlockAnses[i] = new TextBlock[4];
            for (int i = 0; i < borderBlockAnses.Length; i++)
                borderBlockAnses[i] = new Border[3];
            this.testingPage = testingPage;
            tabControl = testingPage.tabControl;
        }

        public Grid getTaskContent(Task task, int index) 
        {
            Grid taskContentGrid = new Grid();
            //taskContentGrid.ShowGridLines = true;

            taskContentGrid.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Auto, 1));
            taskContentGrid.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Star, 1));

            taskContentGrid.Children.Add(getHeaderGrid(task, index));
            taskContentGrid.Children.Add(getAnswersGrid(task, index));

            return taskContentGrid;
        }

        private Grid getAnswersGrid(Task task, int index) 
        {
            Grid answersGrid = new Grid();
            Grid.SetRow(answersGrid, 1);
            //answersGrid.ShowGridLines = true;

            if (task.type == 1)
            {
                answersGrid.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Star, 1));
                answersGrid.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Star, 1));
                answersGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 1));
                answersGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 1));

                for (int i = 0; i < task.answers_count; i++)
                {
                    isConfirmed[i] = false;
                    Grid singleAnswerGrid = new Grid();

                    singleAnswerGrid.Tag = new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(i + 1, task.right_answer), index);
                    singleAnswerGrid.MouseUp += ansChooseBorder_MouseUp;

                    Grid.SetColumn(singleAnswerGrid, ((i % 2 == 0) ? 0 : 1));
                    Grid.SetRow(singleAnswerGrid, ((i < 2) ? 0 : 1));

                    singleAnswerGrid.Margin = new Thickness(5);

                    Image singleAnswerImage = new Image();
                    String pathToImage = "components/images/" + task.id + "/" + (i + 1) + "." + task.img_format[i];
                    singleAnswerImage.Source = Utils.getBitmapImage(pathToImage);
                    singleAnswerImage.Stretch = Stretch.Fill;

                    Border ansChooseBorder = new Border();
                    ansChooseBorder.BorderBrush = new SolidColorBrush(Colors.White);
                    ansChooseBorder.BorderThickness = new Thickness(1, 0, 0, 0);
                    ansChooseBorder.Background = new SolidColorBrush(Utils.backgroundColor);
                    ansChooseBorder.Width = 50;
                    ansChooseBorder.HorizontalAlignment = HorizontalAlignment.Left;
                    ansChooseBorder.CornerRadius = new CornerRadius(0, 500, 500, 0);
                    //ansChooseBorder.Tag = new Tuple<int, int>(i + 1, task.right_answer);

                    //ansChooseBorder.MouseUp += ansChooseBorder_MouseUp;

                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = (char)((int)'А' + i) + "";
                    textBlock.FontFamily = Utils.stdFontFamily;
                    //textBlock.FontSize = 30;
                    textBlock.FontSize = Utils.fontSize + 6;
                    textBlock.FontWeight = FontWeights.Bold;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.Foreground = new SolidColorBrush(Colors.White);
                    textBlockAnses[index][i] = textBlock;
                    ansChooseBorder.Child = textBlock;

                    singleAnswerGrid.Children.Add(singleAnswerImage);
                    singleAnswerGrid.Children.Add(ansChooseBorder);

                    answersGrid.Children.Add(singleAnswerGrid);
                }
            }
            else if (task.type == 2)
            {
                Grid singleAnswerGrid = new Grid();

                singleAnswerGrid.Margin = new Thickness(5);

                Image singleAnswerImage = new Image();
                String pathToImage = "components/images/" + task.id + "/1." + task.img_format[0];
                singleAnswerImage.Source = Utils.getBitmapImage(pathToImage);
                singleAnswerImage.Stretch = Stretch.Fill;
                singleAnswerGrid.Children.Add(singleAnswerImage);

                Grid gridButtons = new Grid();
                gridButtons.ClipToBounds = false;
                singleAnswerGrid.Children.Add(gridButtons);
                for (int i = 0; i < 100; i++)
                {
                    gridButtons.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Star, 1));
                    gridButtons.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 1));
                }

                for (int i = 0; i < 3; i++)
                {
                    Canvas canvas = new Canvas();
                    Border border3 = new Border();
                    border3.Padding = new Thickness(40, 10, 40, 10);
                    //border.Margin = new Thickness(350, 200, 0, 0);
                    border3.HorizontalAlignment = HorizontalAlignment.Left;
                    border3.VerticalAlignment = VerticalAlignment.Top;
                    border3.BorderThickness = new Thickness(3);
                    border3.BorderBrush = new SolidColorBrush(Colors.White);
                    border3.Background = new SolidColorBrush(Utils.backgroundColor);
                    border3.CornerRadius = new CornerRadius(10);
                    border3.Tag = new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(i + 1, task.right_answer), index);
                    borderBlockAnses[index][i] = border3;

                    border3.MouseEnter += border3_MouseEnter;
                    border3.MouseLeave += border3_MouseLeave;
                    border3.MouseUp += ansChooseBorder_MouseUp;

                    canvas.Children.Add(border3);
                    canvas.HorizontalAlignment = HorizontalAlignment.Left;
                    canvas.VerticalAlignment = VerticalAlignment.Top;

                    int row = 0, col = 0;
                    if (i == 0)
                    {
                        row = 28;
                        col = 29;
                    }
                    else if (i == 1)
                    {
                        row = 81;
                        col = 51;
                    }
                    else if (i == 2)
                    {
                        row = 37;
                        col = 68;
                    }
                    Grid.SetRow(canvas, row);
                    Grid.SetColumn(canvas, col);

                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = (char)((int)'А' + i) + "";
                    textBlock.FontFamily = Utils.stdFontFamily;
                    //textBlock.FontSize = 60;
                    textBlock.FontSize = Utils.fontSize + 36;
                    textBlock.FontWeight = FontWeights.Bold;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.Foreground = new SolidColorBrush(Colors.White);
                    border3.Child = textBlock;

                    gridButtons.Children.Add(canvas);
                }

                answersGrid.Children.Add(singleAnswerGrid);
            }
            else if (task.type == 3)
            {
                Grid answerGrid = new Grid();
                answersGrid.Children.Add(answerGrid);

                answerGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 75));
                answerGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 25));

                answerGrid.Margin = new Thickness(5, 15, 5, 10);

                Border imageBorder = new Border();
                imageBorder.CornerRadius = new CornerRadius(5);
                imageBorder.ClipToBounds = true;
                
                Image singleAnswerImage = new Image();
                String pathToImage = "components/images/" + task.id + "/1." + task.img_format[0];
                //String pathToImage = "components/images/in.png";
                singleAnswerImage.Source = Utils.getBitmapImage(pathToImage);
                singleAnswerImage.Stretch = Stretch.Fill;
                singleAnswerImage.OpacityMask = imageBorder.OpacityMask;

                imageBorder.Child = singleAnswerImage;

                answerGrid.Children.Add(imageBorder);

                Grid gridButtons = new Grid();

                gridButtons.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Star, 1));
                gridButtons.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Star, 1));
                gridButtons.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Star, 1));
                gridButtons.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Star, 1));

                Grid.SetColumn(gridButtons, 1);
                gridButtons.ClipToBounds = true;
                answerGrid.Children.Add(gridButtons);

                for (int i = 0; i < task.answers_count; i++)
                {
                    isConfirmed[i] = false;
                    Grid singleAnswerGrid = new Grid();

                    //singleAnswerGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Auto, 1));
                    //singleAnswerGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 1));

                    singleAnswerGrid.Tag = new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(i + 1, task.right_answer), index);
                    singleAnswerGrid.MouseUp += ansChooseBorder_MouseUp;
                    //MessageBox.Show("Clicked (i + 1):" + (i + 1) + " right_answer: " + task.right_answer + " index: " + index);

                    Grid.SetRow(singleAnswerGrid, i);

                    singleAnswerGrid.Margin = new Thickness(5, 0, 5, 0);

                    Border ansChooseBorderLetter = new Border();
                    ansChooseBorderLetter.BorderBrush = new SolidColorBrush(Utils.darkBlueColor1);
                    ansChooseBorderLetter.BorderThickness = new Thickness(1, 0, 0, 0);
                    ansChooseBorderLetter.Background = new SolidColorBrush(Colors.White);
                    ansChooseBorderLetter.Margin = new Thickness(10, ((i > 0 && i < 3) ? 10 : 0), 5, ((i > 1 && i < 3) ? 10 : 0));

                    //ansChooseBorder.Width = 50;
                    //ansChooseBorder.HorizontalAlignment = HorizontalAlignment.Left;
                    ansChooseBorderLetter.CornerRadius = new CornerRadius(10);
                    //ansChooseBorder.Tag = new Tuple<int, int>(i + 1, task.right_answer);

                    Grid doubleColumnGrid = new Grid();

                    doubleColumnGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 1));
                    doubleColumnGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 4));

                    //ansChooseBorder.MouseUp += ansChooseBorder_MouseUp;

                    TextBlock textBlockAnswerLetter = new TextBlock();
                    textBlockAnswerLetter.Text = (char)((int)'А' + i) + "";
                    textBlockAnswerLetter.FontFamily = Utils.stdFontFamily;
                    //textBlock.FontSize = 30;
                    textBlockAnswerLetter.FontSize = Utils.fontSize + 24;
                    textBlockAnswerLetter.FontWeight = FontWeights.Bold;
                    textBlockAnswerLetter.VerticalAlignment = VerticalAlignment.Center;
                    textBlockAnswerLetter.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlockAnswerLetter.Foreground = new SolidColorBrush(Utils.darkBlueColor2);
                    textBlockAnswerLetter.Uid = "blue";
                    textBlockAnses[index][i] = textBlockAnswerLetter;
                    
                    //ansChooseBorderLetter.Child = textBlock;
                    Grid.SetColumn(textBlockAnswerLetter, 0);

                    TextBlock textBlockAnswerText = new TextBlock();
                    textBlockAnswerText.Text = task.ans_text[i];
                    textBlockAnswerText.FontFamily = Utils.stdFontFamily;
                    //textBlock.FontSize = 30;
                    textBlockAnswerText.Padding = new Thickness(10);
                    textBlockAnswerText.FontSize = Utils.fontSize - 8;
                    textBlockAnswerText.FontWeight = FontWeights.Normal;
                    textBlockAnswerText.VerticalAlignment = VerticalAlignment.Center;
                    textBlockAnswerText.HorizontalAlignment = HorizontalAlignment.Left;
                    textBlockAnswerText.Foreground = new SolidColorBrush(Colors.Black);
                    textBlockAnswerText.TextWrapping = TextWrapping.Wrap;
                    //textBlockAnses[index][i] = textBlockAnswerText;
                    //ansChooseBorderLetter.Child = textBlock;
                    Grid.SetColumn(textBlockAnswerText, 1);

                    //doubleColumnGrid.ShowGridLines = true;

                    doubleColumnGrid.Children.Add(textBlockAnswerLetter);
                    doubleColumnGrid.Children.Add(textBlockAnswerText);

                    ansChooseBorderLetter.Child = doubleColumnGrid;

                    singleAnswerGrid.Children.Add(ansChooseBorderLetter);

                    gridButtons.Children.Add(singleAnswerGrid);
                }
            }

            return answersGrid;
        }

        void border3_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border ansChooseBorder = (Border)sender;
            Tuple<Tuple<int, int>, int> tuple = (Tuple<Tuple<int, int>, int>)ansChooseBorder.Tag;

            Tuple<int, int> tuple2 = tuple.Item1;

            if (testingPage.test.answered[tuple.Item2] != Test.ANSWER.NOT_ANSWERED
                || ansChecked[tuple.Item2] == tuple2.Item1)
                return;

            ((Border)sender).Background = new SolidColorBrush(Utils.backgroundColor);
        }

        void border3_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border ansChooseBorder = (Border)sender;
            Tuple<Tuple<int, int>, int> tuple = (Tuple<Tuple<int, int>, int>)ansChooseBorder.Tag;

            if (testingPage.test.answered[tuple.Item2] != Test.ANSWER.NOT_ANSWERED)
                return;

            ((Border)ansChooseBorder).Background = new SolidColorBrush(Utils.lightBlueColor1);
        }

        void ansChooseBorder_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Tuple<Tuple<int, int>, int> tuple;

            try
            {
                Grid ansChooseBorder = (Grid)sender;
                tuple = (Tuple<Tuple<int, int>, int>)ansChooseBorder.Tag;
                if (testingPage.test.answered[tuple.Item2] != Test.ANSWER.NOT_ANSWERED)
                    return;
            }
            catch (Exception ex) {
                Border ansChooseBorder = (Border)sender;
                tuple = (Tuple<Tuple<int, int>, int>)ansChooseBorder.Tag;
                if (testingPage.test.answered[tuple.Item2] != Test.ANSWER.NOT_ANSWERED)
                    return;
                ansChooseBorder.Background = new SolidColorBrush(Utils.backgroundColor);
            }
            confirmButtons[tuple.Item2].Background = new SolidColorBrush(Utils.darkRedColor2);

            if (isConfirmed[tuple.Item2])
                return;

            if (testingPage.test.answered[tuple.Item2] != Test.ANSWER.NOT_ANSWERED)
                return;

            Tuple<int, int> ansTuple = tuple.Item1;

            if (textBlockAnses[tuple.Item2][0] != null)
            {
                for (int j = 0; j < textBlockAnses[tuple.Item2].Length; j++)
                {
                    if (textBlockAnses[tuple.Item2][j].Uid.Equals("blue"))
                    {
                        textBlockAnses[tuple.Item2][j].Foreground = new SolidColorBrush(Utils.darkBlueColor2);
                    }
                    else
                    {
                        textBlockAnses[tuple.Item2][j].Foreground = new SolidColorBrush(Colors.White);
                    }
                }
                textBlockAnses[tuple.Item2][ansTuple.Item1 - 1].Foreground = new SolidColorBrush(Colors.Red);
            }
            else 
            {
                for (int j = 0; j < borderBlockAnses[tuple.Item2].Length; j++)
                {
                    borderBlockAnses[tuple.Item2][j].Background = new SolidColorBrush(Utils.backgroundColor);
                }
                borderBlockAnses[tuple.Item2][ansTuple.Item1 - 1].Background = new SolidColorBrush(Utils.lightBlueColor1);
            }
            ansChecked[tuple.Item2] = ansTuple.Item1;
            
        }

        private Grid getHeaderGrid(Task task, int index) 
        {
            Grid headerGrid = new Grid();
            //headerGrid.ShowGridLines = true;

            headerGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 1));
            headerGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Auto, 1));
            headerGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Auto, 1));

            TextBlock[] textBlocks = new TextBlock[3];
            Border[] border = new Border[3];
            

            for (int i = 0; i < 3; i++){
                border[i] = new Border();
                border[i].CornerRadius = new CornerRadius(10);
                border[i].BorderThickness = new Thickness(1);
                border[i].BorderBrush = new SolidColorBrush(Colors.White);
                border[i].Background = new SolidColorBrush((i == 2) ? Utils.backgroundColor : (i == 0) ? Colors.White : Colors.Gray);
                border[i].Padding = new Thickness(5, 5, 5, 5);
                border[i].Margin = new Thickness(5, 0, 5, 0);
                textBlocks[i] = new TextBlock();
                textBlocks[i].Text = (i == 2) ? "20:00" : ((i == 0) ? task.task : "ПОДТВЕРДИТЬ");
                //textBlocks[i].FontSize = (i == 2) ? 30 : 20;
                textBlocks[i].FontSize = (i == 2) ? Utils.fontSize + 6 : Utils.fontSize - 4;
                textBlocks[i].FontFamily = Utils.stdFontFamily;
                textBlocks[i].Foreground = new SolidColorBrush((i == 0) ? Utils.backgroundColor : Colors.White);
                textBlocks[i].HorizontalAlignment = HorizontalAlignment.Center;
                textBlocks[i].VerticalAlignment = VerticalAlignment.Center;
                textBlocks[i].TextWrapping = TextWrapping.Wrap;

                border[i].Child = textBlocks[i];
                Grid.SetColumn(border[i], i);

                headerGrid.Children.Add(border[i]);
            }
            border[1].Tag = new Tuple<int, int>(index, task.right_answer);
            border[1].MouseUp += ConfirmButton_MouseUp;
            border[1].MouseEnter += ConfirmButton_MouseEnter;
            border[1].MouseLeave += ConfirmButton_MouseLeave;
            confirmButtons[index] = border[1];
            testingPage.timeCounter[index] = border[2];

            return headerGrid;
        }

        void ConfirmButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = (Border)sender;
            Tuple<int, int> tuple = (Tuple<int, int>)((Border)sender).Tag;

            if (testingPage.test.answered[tuple.Item1] != Test.ANSWER.NOT_ANSWERED)
                return;

            if (ansChecked[tuple.Item1] != 0)
                border.Background = new SolidColorBrush(Utils.darkRedColor1);
        }

        void ConfirmButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = (Border)sender;
            Tuple<int, int> tuple = (Tuple<int, int>)((Border)sender).Tag;

            if (testingPage.test.answered[tuple.Item1] != Test.ANSWER.NOT_ANSWERED)
                return;

            if (ansChecked[tuple.Item1] != 0)
                border.Background = new SolidColorBrush(Utils.darkRedColor2);
        }

        void ConfirmButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Tuple<int, int> tuple = (Tuple<int, int>)((Border)sender).Tag;

            if (isConfirmed[tuple.Item1] || ansChecked[tuple.Item1] == 0)
                return;

            isConfirmed[tuple.Item1] = true;

            if (ansChecked[tuple.Item1] == tuple.Item2)
            {
                testingPage.test.answered[tuple.Item1] = Test.ANSWER.RIGHT;
                testingPage.test.right_answered++;
                //MessageBox.Show("Right answer!");
            }
            else
            {
                testingPage.test.answered[tuple.Item1] = Test.ANSWER.WRONG;
                //MessageBox.Show("Wrong answer!");
            }
            testingPage.test.studentAnswer[tuple.Item1] = ansChecked[tuple.Item1];
            confirmButtons[tuple.Item1].Background = new SolidColorBrush(Colors.Gray);
            changeTab(tuple.Item1);

            testingPage.test.answeredCount++;
            if (testingPage.test.answeredCount == testingPage.test.answered.Length)
            {
                testingPage.test.timer.Stop();
                testingPage.test.timer.Dispose();
                //mistakesClarificationPage.testingPage = testingPage;
                Utils.goToPage("mistakesClarificationPage");
                return;
            }

            for (int i = tuple.Item1; i < testingPage.tasks.Length; i++) {
                if (testingPage.test.answered[i] == Test.ANSWER.NOT_ANSWERED)
                {
                    changeTab(i);
                    return;
                }
            }
            for (int i = 0; i <= testingPage.tasks.Length; i++)
            {
                if (testingPage.test.answered[i] == Test.ANSWER.NOT_ANSWERED)
                {
                    changeTab(i);
                    return;
                }
            }
        }

        public StackPanel getTabControls(int tabsCount, Boolean withText, Boolean isClickable) 
        {
            if (testingPage.test.answered == null)
                return null;
            StackPanel tabPanel = new StackPanel();
            tabPanel.Margin = new Thickness(0, 5, 0, 0);
            tabPanel.Orientation = Orientation.Vertical;
            Grid.SetColumn(tabPanel, 0);

            if (withText)
            {
                StackPanel studentIndex = new StackPanel();
                TextBlock textStudentPart = new TextBlock();
                textStudentPart.Text = "УЧАСТНИК";
                //textStudentPart.FontSize = 20;
                textStudentPart.FontSize = Utils.fontSize - 4;
                textStudentPart.FontFamily = Utils.stdFontFamily;
                textStudentPart.FontWeight = FontWeights.Bold;
                textStudentPart.Foreground = new SolidColorBrush(Colors.White);
                TextBlock textStudentIndex = new TextBlock();
                textStudentIndex.Text = "№ " + testingPage.currentStudent.id.ToString();
                //textStudentIndex.FontSize = 20;
                textStudentIndex.FontSize = Utils.fontSize - 4;
                textStudentIndex.FontFamily = Utils.stdFontFamily;
                textStudentIndex.FontWeight = FontWeights.Bold;
                textStudentIndex.Foreground = new SolidColorBrush(Colors.White);
                studentIndex.Children.Add(textStudentPart);
                studentIndex.Children.Add(textStudentIndex);

                tabPanel.Children.Add(studentIndex);
                studentIndex.Margin = new Thickness();
            }

            double k = Utils.windowWidth / Utils.fixedWindowWidth;

            for (int i = 0; i < tabsCount; i++) 
            {
                Border borderOut = new Border();
                borderOut.BorderThickness = new Thickness(2);
                borderOut.BorderBrush = new SolidColorBrush(Utils.ansNotCheckedColor);
                borderOut.CornerRadius = new CornerRadius(8);
                borderOut.Margin = new Thickness(10 * k, 0, 10 * k, 0);
                Border border = new Border();
                borderOut.Child = border;
                testingPage.borderTabs[i] = border;

                //border.BorderBrush = new SolidColorBrush(Utils.backgroundColor);
                border.BorderThickness = new Thickness(6);
                border.CornerRadius = new CornerRadius(4);

                

                border.Width = 50 * k;
                border.Height = 50 * k;
                //border.Padding = new Thickness(35);
                border.Tag = i;


                if (isClickable)
                {
                    border.MouseUp += border_MouseUp;
                    border.MouseEnter += border_MouseEnter;
                    border.MouseLeave += border_MouseLeave;
                }
                
                TextBlock textBlock = new TextBlock();
                textBlock.Text = (i + 1).ToString();
                //textBlock.FontSize = 30;
                textBlock.FontSize = Utils.fontSize + (6 * k);
                textBlock.FontFamily = Utils.stdFontFamily;
                //textBlock.FontWeight = FontWeights.Bold;
                textBlock.Foreground = new SolidColorBrush(Utils.darkBlueColor2);
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;

                border.Child = textBlock;

                tabPanel.Children.Add(borderOut);
                tabChangeBorderAndBackground(border, i);
            }

            return tabPanel;
        }

        void border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = (Border) sender;
            int id = (int)border.Tag;

            tabChangeBorderAndBackground(border, id);

            border.BorderThickness = new Thickness(6);
                
        }

        void border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (currentTab != ((Border)sender))
                ((Border)sender).BorderThickness = new Thickness(5);
        }

        void border_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            changeTab((int)((Border)sender).Tag);
        }

        public void changeTab(int tabIndex) 
        {
            if (tabIndex >= 4 && tabIndex <= 20 && !testingPage.isHelp2Used && !isAnswersImmediatelyShown)
            {
                testingPage.mainGrid.Visibility = Visibility.Collapsed;
                testingPage.info2.Visibility = Visibility.Visible;
                testingPage.isHelp2Used = true;
            }
            if (tabIndex >= testingPage.test.answered.Length)
                return;
            if (currentTab != null)
            {
                int id = (int)currentTab.Tag;
                tabChangeBorderAndBackground(currentTab, id);
                ((TextBlock)currentTab.Child).FontWeight = FontWeights.Normal;
            }
            currentTab = testingPage.borderTabs[tabIndex];
            ((TextBlock)currentTab.Child).FontWeight = FontWeights.Bold;
            tabControl.SelectedIndex = tabIndex;
        }

        private void tabChangeBorderAndBackground(Border border, int id) 
        {
            TextBlock textBlock = (TextBlock)border.Child;
            /*if (textBlock != null)
                textBlock.Foreground = new SolidColorBrush(Utils.darkBlueColor);*/
            border.Opacity = 1;
            if (testingPage.test.answered[id] == Test.ANSWER.NOT_ANSWERED)
            {
                border.Opacity = 0.5;
                border.Background = new SolidColorBrush(Utils.ansNotCheckedColor);
                border.BorderBrush = new SolidColorBrush(Utils.darkBlueColor1);
                /*if (textBlock != null)
                    textBlock.Foreground = new SolidColorBrush(Colors.White);*/
            }
            else if (testingPage.test.answered[id] == Test.ANSWER.RIGHT)
            {
                if (isAnswersImmediatelyShown)
                    textBlock.Foreground = new SolidColorBrush(Colors.White);
                border.Background = new SolidColorBrush(Utils.darkGreenColor1);
                border.BorderBrush = new SolidColorBrush(Utils.darkGreenColor2);
            }
            else
            {
                border.Background = new SolidColorBrush(Utils.darkRedColor1);
                border.BorderBrush = new SolidColorBrush(Utils.darkRedColor2);
            }
            if ( (testingPage.test.answered[id] == Test.ANSWER.RIGHT
                || testingPage.test.answered[id] == Test.ANSWER.WRONG) 
                && !isAnswersImmediatelyShown) {
                    border.Background = new SolidColorBrush(Utils.ansNotCheckedColor);
                    border.BorderBrush = new SolidColorBrush(Utils.darkBlueColor1);
            }
        }

        public static TabItem getInvisibleTabItem() 
        {
            TabItem tabItem = new TabItem();
            tabItem.Visibility = Visibility.Collapsed;

            return tabItem;
        }
    }
}
