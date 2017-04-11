using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ini;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Documents;
using PDD_APP.Models;

namespace PDD_APP
{
    class Utils
    {
        public static double windowHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
        public static double windowWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
        public static string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Station one\\";
        public static IniFile iniFile = getIniFile();
        //public static Color backgroundColor = Color.FromRgb(8, 111, 139);
        public static Color backgroundColor = Color.FromRgb(195, 226, 245);
        public static Color headerCellBackgroundColor = Color.FromRgb(195, 226, 245);
        public static Color activeBackgroundColor = Color.FromRgb(125, 113, 177);
        public static Color notActiveBackgroundColor = Color.FromRgb(120, 191, 235);
        public static Color activeAnswerBackgroundColor = Color.FromRgb(175, 203, 31);
        public static Color notActiveAnswerBackgroundColor = Color.FromRgb(125, 113, 177);
        public static Color darkBlueColor1 = Color.FromRgb(12, 127, 156);
        public static Color darkBlueColor2 =  Color.FromRgb(41, 100, 134);
        public static Color darkBlueColor3 =  Color.FromRgb(17, 91, 116);
        public static Color darkRedColor1 =  Color.FromRgb(239, 171, 170);
        public static Color darkRedColor2 = Color.FromRgb(201, 52, 51);
        public static Color darkGreenColor1 = Color.FromRgb(49, 118, 64);
        public static Color darkGreenColor2 = Color.FromRgb(26, 95, 38);
        public static Color lightBlueColor1 = Color.FromRgb(149, 197, 209);
        public static Color orangeColor = Color.FromRgb(229, 120, 29);
        public static Color greenColor = Color.FromRgb(49, 118, 64);
        public static Color ansNotCheckedColor = Color.FromRgb(230, 241, 249);
        public static Color medicineBgColor = Color.FromRgb(230, 241, 249);
        //public static Color ansNotCheckedColor = Color.FromRgb(156, 156, 156);
        public static FontFamily stdFontFamily = new FontFamily("Arial");
        public static int fontSize = 24;
        public static int fixedWindowHeight = 864;
        public static int fixedWindowWidth = 1536;


        public static void changeFontSize()
        {
            double d = Utils.fontSize * (windowHeight / fixedWindowHeight);
            Utils.fontSize = Convert.ToInt32(d);
            //d = MainWindow.menuButtonsWidth / (MainWindow.windSize / MainWindow.wind.Height); ;
            //MainWindow.menuButtonsWidth = Convert.ToInt32(d);
            //MessageBox.Show(MainWindow.fontSize.ToString());
        }

        private static IniFile getIniFile() {
            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);
            return new IniFile(appDataPath + "config.ini");
        }

        public static void goToPage(String page)
        {
            MainWindow.mainFrame.Dispatcher.BeginInvoke(new Action(delegate()
            {
                MainWindow.mainFrame.Navigate(new Uri(page + ".xaml", UriKind.RelativeOrAbsolute));
            }));
        }

        public static void goToPage(Page page)
        {
            MainWindow.mainFrame.Dispatcher.BeginInvoke(new Action(delegate()
            {
                MainWindow.mainFrame.Navigate(page);
            }));
        }

        public static BitmapImage getBitmapImage(String pathToImage) 
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(pathToImage, UriKind.RelativeOrAbsolute);
            image.EndInit();
            image.Freeze();
            return image;
        }

        public static String CalculateMD5Hash(String input)
        {
            // step 1, calculate MD5 hash from input
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static ColumnDefinition getColDef(GridUnitType type, int value)
        {
            ColumnDefinition colDef = new ColumnDefinition();
            colDef.Width = new GridLength(value, type);
            return colDef;
        }

        public static RowDefinition getRowDef(GridUnitType type, int value)
        {
            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = new GridLength(value, type);
            return rowDef;
        }

        public static String getTimeFromInt(int time) 
        {
            String stringTime = (time / 60) + ":";
            if ((time % 60) < 10)
                stringTime += "0" + (time % 60);
            else
                stringTime += (time % 60);

            return stringTime;
        }

        public static long getCurrentTimeInMilliseconds() {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static String getDate(long milliseconds) {
            DateTime date = new DateTime(milliseconds * TimeSpan.TicksPerMillisecond);
            //return date.ToString("yyyy-MM-ddThh:mm:ssZ");
            return date.ToString("yyyy-MM-dd HH:mm");
        }

        private static FlowDocument CreateFlowDocument(Session session)
        {
            // Create a FlowDocument
            FlowDocument doc = new FlowDocument();
            doc.ColumnWidth = 999999;

            // Create a Section
            Section sec = new Section();

            Paragraph pHeader = new Paragraph(new Bold(new Run("Безопасное колесо. Станция №1")));
            pHeader.FontSize = 25;
            pHeader.TextAlignment = TextAlignment.Center;
            pHeader.Margin = new Thickness(0, 30, 0, 0);

            sec.Blocks.Add(pHeader);

            Paragraph pHeader2 = new Paragraph(new Bold(new Run("Статистика тестирования №" + session.id)));
            pHeader2.FontSize = 30;
            pHeader2.TextAlignment = TextAlignment.Center;
            pHeader2.Margin = new Thickness(0, 0, 0, 30);

            sec.Blocks.Add(pHeader2);

            List<TestDB> testsList = TestDB.getAll(session.id);
            foreach (TestDB test in testsList) 
            {
                Student student = Student.get(test.studentId);
                // Create first Paragraph
                Paragraph p = new Paragraph();
                p.Margin = new Thickness(0, 10, 0, 0);
                // Create and add a new Bold, Italic and Underline
                /*Bold bld = new Bold();
                bld.Inlines.Add(new Run("First Paragraph"));
                Italic italicBld = new Italic();
                italicBld.Inlines.Add(bld);
                Underline underlineItalicBld = new Underline();
                underlineItalicBld.Inlines.Add(italicBld);
                // Add Bold, Italic, Underline to Paragraph
                p.Inlines.Add(underlineItalicBld);*/
                Span span = new Span();
                span.Inlines.Add(student.id + " | " + student.fname + " " + student.lname + ": Правильных ответов " + test.rAns + " из " + test.ansCount + ", Время: " + Utils.getTimeFromInt(test.time));
                p.Inlines.Add(span);

                Paragraph pSignParticipant = new Paragraph();
                pSignParticipant.Margin = new Thickness(0, 5, 0, 10);
                Span spanSign = new Span();
                spanSign.Inlines.Add("Подпись участника: ");
                pSignParticipant.Inlines.Add(spanSign);
                // Add Paragraph to Section

                sec.Blocks.Add(p);
                sec.Blocks.Add(pSignParticipant);
            }

            Paragraph pSign = new Paragraph();
            pSign.FontSize = 25;
            pSign.TextAlignment = TextAlignment.Right;
            pSign.Margin = new Thickness(0, 10, 0, 0);

            Bold bldSign = new Bold();
            bldSign.Inlines.Add(new Run("Подпись судьи: _____"));

            pSign.Inlines.Add(bldSign);
            sec.Blocks.Add(pSign);

            Paragraph pDate = new Paragraph();
            pDate.FontSize = 20;
            pDate.TextAlignment = TextAlignment.Center;
            pDate.Margin = new Thickness(0, 20, 0, 0);

            Bold bldDate = new Bold();
            bldDate.Inlines.Add(new Run(Utils.getDate(getCurrentTimeInMilliseconds())));

            pDate.Inlines.Add(bldDate);
            sec.Blocks.Add(pDate);

            // Add Section to FlowDocument
            doc.Blocks.Add(sec);

            return doc;
        }

        public static void printStats(Session session) 
        {
            PrintDialog printDlg = new PrintDialog();

            printDlg.PageRangeSelection = PageRangeSelection.AllPages;
            printDlg.UserPageRangeEnabled = false;

            // Display the dialog. This returns true if the user presses the Print button.
            Nullable<Boolean> print = printDlg.ShowDialog();
            if (print == true)
            {
                //XpsDocument xpsDocument = new XpsDocument("C:\\FixedDocumentSequence.xps", FileAccess.ReadWrite);
                //FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();
                FlowDocument doc = CreateFlowDocument(session);
                doc.Name = "statistics";
                IDocumentPaginatorSource idpSource = doc;
                printDlg.PrintDocument(idpSource.DocumentPaginator, "Печать статистики тетирования.");
            }
        }
    }
}
