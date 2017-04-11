using PDD_APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PDD_APP
{
    class ResultPageConstructor
    {
        private static TabControl tabControl;
        private mistakesClarificationPage mcp;

        public ResultPageConstructor(mistakesClarificationPage mcp) 
        {
            this.mcp = mcp;
            tabControl = mcp.tabControl;
        }

        public Grid getResultTaskGrid(Task task, int index) 
        {
            Grid ResultTaskGrid = new Grid();
            //ResultTaskGrid.ShowGridLines = true;

            ResultTaskGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 4));
            ResultTaskGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 1));

            ResultTaskGrid.Children.Add(getTaskContent(task, index));
            ResultTaskGrid.Children.Add(getClarificationColumn(task, index));

            return ResultTaskGrid;
        }

        public Grid getTaskContent(Task task, int index) 
        {
            Grid taskContentGrid = new Grid();
            //taskContentGrid.ShowGridLines = true;

            taskContentGrid.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Auto, 1));
            taskContentGrid.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Star, 1));

            taskContentGrid.Children.Add(getHeaderGrid(task));
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
                    Grid singleAnswerGrid = new Grid();

                    Grid.SetColumn(singleAnswerGrid, ((i % 2 == 0) ? 0 : 1));
                    Grid.SetRow(singleAnswerGrid, ((i < 2) ? 0 : 1));

                    singleAnswerGrid.Margin = new Thickness(5);

                    Image singleAnswerImage = new Image();
                    String pathToImage = "components/images/" + task.id + "/" + (i + 1) + "." + task.img_format[i];
                    singleAnswerImage.Source = Utils.getBitmapImage(pathToImage);
                    singleAnswerImage.Stretch = Stretch.Fill;

                    Border ansChooseBorder = new Border();
                    ansChooseBorder.Background = new SolidColorBrush(Utils.backgroundColor);
                    ansChooseBorder.BorderThickness = new Thickness(1, 0, 0, 0);
                    ansChooseBorder.BorderBrush = new SolidColorBrush(Colors.White);
                    ansChooseBorder.Width = 30;
                    ansChooseBorder.CornerRadius = new CornerRadius(0, 500, 500, 0);
                    ansChooseBorder.HorizontalAlignment = HorizontalAlignment.Left;

                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = (char)((int)'А' + i) + "";
                    textBlock.FontFamily = Utils.stdFontFamily;
                    //textBlock.FontSize = 30;
                    textBlock.FontSize = Utils.fontSize + 6;
                    textBlock.FontWeight = FontWeights.Bold;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.Foreground = new SolidColorBrush(Colors.White);
                    ansChooseBorder.Child = textBlock;

                    singleAnswerGrid.Children.Add(singleAnswerImage);
                    singleAnswerGrid.Children.Add(ansChooseBorder);
                    
                    answersGrid.Children.Add(singleAnswerGrid);
                }
            }
            else if (task.type == 2){ 
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
                for (int i = 0; i < 100; i++) {
                    gridButtons.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Star, 1));
                    gridButtons.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 1));
                }

                for (int i = 0; i < 3; i++)
                {
                    Canvas canvas = new Canvas();
                    Border border3 = new Border();
                    border3.Padding = new Thickness(40, 10, 40, 10);
                    border3.HorizontalAlignment = HorizontalAlignment.Left;
                    border3.VerticalAlignment = VerticalAlignment.Top;
                    border3.BorderThickness = new Thickness(3);
                    border3.BorderBrush = new SolidColorBrush(Colors.White);
                    border3.Background = new SolidColorBrush(Utils.backgroundColor);
                    border3.CornerRadius = new CornerRadius(10);

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
                    //textBlock.FontSize = 40;
                    textBlock.FontSize = Utils.fontSize + 16;
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
                    Grid singleAnswerGrid = new Grid();

                    Grid.SetRow(singleAnswerGrid, i);

                    singleAnswerGrid.Margin = new Thickness(5, 0, 5, 0);

                    Border ansChooseBorderLetter = new Border();
                    ansChooseBorderLetter.BorderBrush = new SolidColorBrush(Utils.darkBlueColor1);
                    ansChooseBorderLetter.BorderThickness = new Thickness(1, 0, 0, 0);
                    ansChooseBorderLetter.Background = new SolidColorBrush(Colors.White);
                    ansChooseBorderLetter.Margin = new Thickness(10, ((i > 0 && i < 3) ? 10 : 0), 0, ((i > 1 && i < 3) ? 10 : 0));
                    
                    ansChooseBorderLetter.CornerRadius = new CornerRadius(10);

                    Grid doubleColumnGrid = new Grid();

                    doubleColumnGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 1));
                    doubleColumnGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 4));
                    
                    TextBlock textBlockAnswerLetter = new TextBlock();
                    textBlockAnswerLetter.Text = (char)((int)'А' + i) + "";
                    textBlockAnswerLetter.FontFamily = Utils.stdFontFamily;
                    textBlockAnswerLetter.FontSize = Utils.fontSize + 18;
                    textBlockAnswerLetter.FontWeight = FontWeights.Bold;
                    textBlockAnswerLetter.VerticalAlignment = VerticalAlignment.Center;
                    textBlockAnswerLetter.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlockAnswerLetter.Foreground = new SolidColorBrush(Utils.darkBlueColor2);
                    
                    Grid.SetColumn(textBlockAnswerLetter, 0);

                    TextBlock textBlockAnswerText = new TextBlock();
                    textBlockAnswerText.Text = task.ans_text[i];
                    textBlockAnswerText.FontFamily = Utils.stdFontFamily;
                    textBlockAnswerText.Padding = new Thickness(10);
                    textBlockAnswerText.FontSize = Utils.fontSize - 12;
                    textBlockAnswerText.FontWeight = FontWeights.Normal;
                    textBlockAnswerText.VerticalAlignment = VerticalAlignment.Center;
                    textBlockAnswerText.HorizontalAlignment = HorizontalAlignment.Left;
                    textBlockAnswerText.Foreground = new SolidColorBrush(Colors.Black);
                    textBlockAnswerText.TextWrapping = TextWrapping.Wrap;
                    Grid.SetColumn(textBlockAnswerText, 1);

                    doubleColumnGrid.Children.Add(textBlockAnswerLetter);
                    doubleColumnGrid.Children.Add(textBlockAnswerText);

                    ansChooseBorderLetter.Child = doubleColumnGrid;

                    singleAnswerGrid.Children.Add(ansChooseBorderLetter);

                    gridButtons.Children.Add(singleAnswerGrid);
                }
            }

            return answersGrid;
        }

        private Grid getHeaderGrid(Task task)
        {
            Grid headerGrid = new Grid();
            //headerGrid.ShowGridLines = true;

            headerGrid.ColumnDefinitions.Add(Utils.getColDef(GridUnitType.Star, 1));

            Border border = new Border();
            border.BorderThickness = new Thickness(1);
            border.CornerRadius = new CornerRadius(10);
            border.BorderBrush = new SolidColorBrush(Colors.White);
            border.Background = new SolidColorBrush(Colors.White);
            border.Margin = new Thickness(5);
            TextBlock textBlocks = new TextBlock();
            textBlocks = new TextBlock();
            textBlocks.Text = task.task;
            textBlocks.Padding = new Thickness(15);
            //textBlocks.FontSize = 20;
            textBlocks.FontSize = Utils.fontSize - 4;
            textBlocks.FontFamily = Utils.stdFontFamily;
            textBlocks.Background = new SolidColorBrush(Colors.White);
            textBlocks.Foreground = new SolidColorBrush(Colors.DarkBlue);
            textBlocks.HorizontalAlignment = HorizontalAlignment.Center;
            textBlocks.VerticalAlignment = VerticalAlignment.Center;

            border.Child = textBlocks;
            Grid.SetColumn(border, 0);

            headerGrid.Children.Add(border);

            return headerGrid;
        }

        private Grid getClarificationColumn(Task task, int index)
        {
            Grid grid = new Grid();
            Grid.SetColumn(grid, 1);
            //grid.ShowGridLines = true;
            grid.Margin = new Thickness(10);

            grid.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Auto, 1));
            grid.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Auto, 1));
            grid.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Auto, 1));
            grid.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Auto, 1));
            grid.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Star, 1));
            grid.RowDefinitions.Add(Utils.getRowDef(GridUnitType.Auto, 1));

            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Далее";
            textBlock.Foreground = new SolidColorBrush(Colors.White);
            //textBlock.FontSize = 23;
            textBlock.FontSize = Utils.fontSize - 1;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;

            Border nextBorder = new Border();
            nextBorder.Background = new SolidColorBrush(Utils.darkGreenColor1);
            nextBorder.BorderBrush = new SolidColorBrush(Colors.White);
            nextBorder.BorderThickness = new Thickness(1);
            nextBorder.Margin = new Thickness(20);
            nextBorder.Padding = new Thickness(20);
            nextBorder.CornerRadius = new CornerRadius(4);
            nextBorder.VerticalAlignment = VerticalAlignment.Bottom;
            //nextBorder.HorizontalAlignment = HorizontalAlignment.Center;
            nextBorder.Child = textBlock;
            //Grid.SetColumn(nextBorder, 1);
            Grid.SetRow(nextBorder, 5);
            grid.Children.Add(nextBorder);

            nextBorder.MouseEnter += nextBorder_MouseEnter;
            nextBorder.MouseLeave += nextBorder_MouseLeave;
            nextBorder.MouseUp += nextBorder_MouseUp;

            //StackPanel clarificationStackPanel = new StackPanel();
            TextBlock clarificationIntroTitle = new TextBlock();
            clarificationIntroTitle.Text = "Пояснения";
            //clarificationIntroTitle.FontSize = 25;
            clarificationIntroTitle.FontSize = Utils.fontSize;
            clarificationIntroTitle.FontWeight = FontWeights.Bold;
            clarificationIntroTitle.Margin = new Thickness(0, 0, 0, 5);
            clarificationIntroTitle.Foreground = new SolidColorBrush(Colors.White);
            //clarificationStackPanel.Children.Add(clarificationIntroTitle);
            Grid.SetRow(clarificationIntroTitle, 0);
            grid.Children.Add(clarificationIntroTitle);

            TextBlock clarificationIntro = new TextBlock();
            clarificationIntro.Text = "Нажимай на номера с неправильными ответами и ты узнаешь, какой из вариантов правильный. Затем нажми кнопку \"Далее\".";
            clarificationIntro.TextWrapping = TextWrapping.Wrap;
            //clarificationIntro.FontSize = 20;
            clarificationIntro.FontSize = Utils.fontSize - 8;
            clarificationIntro.Margin = new Thickness(0, 0, 0, 5);
            clarificationIntro.Foreground = new SolidColorBrush(Colors.White);
            //clarificationStackPanel.Children.Add(clarificationIntro);
            Grid.SetRow(clarificationIntro, 1);
            grid.Children.Add(clarificationIntro);

            TextBlock clarificationRightNumber = new TextBlock();
            clarificationRightNumber.FontWeight = FontWeights.Bold;
            //clarificationRightNumber.FontSize = 25;
            clarificationRightNumber.FontSize = Utils.fontSize;
            clarificationRightNumber.Margin = new Thickness(0, 0, 0, 5);
            clarificationRightNumber.Text = "Верный ответ: " + (char)((int)'А' + (task.right_answer - 1)) + "";
            clarificationRightNumber.Foreground = new SolidColorBrush(Colors.White);
            //clarificationStackPanel.Children.Add(clarificationRightNumber);
            Grid.SetRow(clarificationRightNumber, 3);
            grid.Children.Add(clarificationRightNumber);

            TextBlock clarificationStudentNumber = new TextBlock();
            clarificationStudentNumber.FontWeight = FontWeights.Bold;
            //clarificationStudentNumber.FontSize = 25;
            clarificationStudentNumber.FontSize = Utils.fontSize + 1;
            clarificationStudentNumber.Margin = new Thickness(0, 0, 0, 5);
            if (mistakesClarificationPage.testingPage.test.studentAnswer[index] != 0)
                clarificationStudentNumber.Text = "Твой ответ: " + (char)((int)'А' + (mistakesClarificationPage.testingPage.test.studentAnswer[index] - 1)) + "";
            else
                clarificationStudentNumber.Text = "Твой ответ: -";
            clarificationStudentNumber.Foreground = new SolidColorBrush(Colors.White);
            //clarificationStackPanel.Children.Add(clarificationStudentNumber);
            Grid.SetRow(clarificationStudentNumber, 2);
            grid.Children.Add(clarificationStudentNumber);

            ScrollViewer scrollViewer = new ScrollViewer();

            TextBlock clarificationText = new TextBlock();
            clarificationText.Text = task.clarification;
            clarificationText.TextWrapping = TextWrapping.Wrap;
            //clarificationText.FontSize = 18;
            clarificationText.FontSize = Utils.fontSize - 6;
            clarificationText.Foreground = new SolidColorBrush(Colors.White);
            
            scrollViewer.Content = clarificationText;
            //clarificationStackPanel.Children.Add(scrollViewer);
            Grid.SetRow(scrollViewer, 4);
            grid.Children.Add(scrollViewer);

            //grid.Children.Add(clarificationStackPanel);

            return grid;
        }

        void nextBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Utils.goToPage("lastResultPage");
        }

        void nextBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkGreenColor2);
        }

        void nextBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkGreenColor1);
        }
    }
}
