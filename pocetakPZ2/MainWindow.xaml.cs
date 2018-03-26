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
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace pocetakPZ2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            List < double> fonts = new List<double>(new double[] { 1, 2, 3, 5, 10, 15, 20, 25, 50, 75, 100 });
            cmbSize.ItemsSource = fonts;
            cmbSize.Text = "10";
        }

        
        
        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            object tempI = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            object tempU = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            object tempS= rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);

            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));// oznaci ako je vec oznacen bold na slovu, chekirano
            btnItalic.IsChecked = (tempI != DependencyProperty.UnsetValue) && (tempI.Equals(FontStyles.Italic));
            btnUnderline.IsChecked = (tempU != DependencyProperty.UnsetValue) && (tempU.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp; // izbaci opcije datog teksta
            tempI = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = tempI;
            tempU = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = tempU;
            tempS = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbSize.Text = tempS.ToString();

            string whole_txt = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            var text = whole_txt.Trim();
            int cnt = 0, ind = 0;

            while (ind < text.Length)
            {
                while (ind < text.Length && !char.IsWhiteSpace(text[ind]))
                {
                    ind++;
                }

                cnt++;

                while (ind < text.Length && char.IsWhiteSpace(text[ind]) )
                {
                    ind++;
                }

            }
            lblBrReci.Content = cnt.ToString();
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             if (cmbFontFamily.SelectedItem != null)
             {
                 rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem); // menajs font, izabranom tekstu
                
             } 

        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove(); //mozes da pomeras prozor
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            rtbEditor.SelectAll();
            string pomText = rtbEditor.Selection.Text;

            if (!pomText.Equals(""))
            {
                if(pomText.Equals("\r\n"))
                {
                    this.Close();
                    return;
                }

                if (System.Windows.Forms.MessageBox.Show("Would u like to save?", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Filter = "Rich Text file (*.rtf)|*.rtf|All files (*.*)|*.*";
                    if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        FileStream fs = new FileStream(saveFile.FileName, FileMode.Create);
                        TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                        range.Save(fs, System.Windows.DataFormats.Rtf);

                    }

                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void formMouse(object sender,System.Windows.Forms.MouseEventArgs e)
        {
            int x = e.X;  // ovo za date time probaj
            int y = e.Y;
        }

        private void btnDateTime_Click(object sender, RoutedEventArgs e)
        {
            DateTime vreme =  DateTime.Now;
            rtbEditor.CaretPosition.InsertTextInRun(vreme.ToString("dd.MM.yyyy HH:mm:ss"));
        }


        
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Rich Text file (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileStream fs = new FileStream(saveFile.FileName,FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fs, System.Windows.DataFormats.Rtf);

            }
        }
 
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            rtbEditor.SelectAll();
            string pomText = rtbEditor.Selection.Text;

            if (!pomText.Equals(""))
            {
                if (pomText.Equals("\r\n"))
                {
                    Stream myStream;
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "Regular Text file (*.txt)|*.txt";

                    if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if ((myStream = openFile.OpenFile()) != null)
                        {
                            string open = openFile.FileName;
                            string fileText = File.ReadAllText(open);
                            rtbEditor.SelectAll();
                            rtbEditor.Selection.Text = fileText;
                        }
                    }
                }
                else if(System.Windows.Forms.MessageBox.Show("Would u like to save?", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Filter = "Rich Text file (*.rtf)|*.rtf|All files (*.*)|*.*";
                    if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        FileStream fs = new FileStream(saveFile.FileName, FileMode.Create);
                        TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                        range.Save(fs, System.Windows.DataFormats.Rtf);

                    }

                    Stream myStream;
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "Regular Text file (*.txt)|*.txt";

                    if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if ((myStream = openFile.OpenFile()) != null)
                        {
                            string open = openFile.FileName;
                            string fileText = File.ReadAllText(open);
                            rtbEditor.SelectAll();
                            rtbEditor.Selection.Text = fileText;
                        }
                    }
                }else
                {
                    Stream myStream;
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "Regular Text file (*.txt)|*.txt";

                    if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if ((myStream = openFile.OpenFile()) != null)
                        {
                            string open = openFile.FileName;
                            string fileText = File.ReadAllText(open);
                            rtbEditor.SelectAll();
                            rtbEditor.Selection.Text = fileText;
                        }
                    }
                }
                
            }
            else
            {
                Stream myStream;
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Regular Text file (*.txt)|*.txt";

                if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if ((myStream = openFile.OpenFile()) != null)
                    {
                        string open = openFile.FileName;
                        string fileText = File.ReadAllText(open);
                        rtbEditor.SelectAll();
                        rtbEditor.Selection.Text = fileText;
                    }
                }
            }

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            rtbEditor.SelectAll();
            string pomText = rtbEditor.Selection.Text;
            if (!pomText.Equals(""))
            {
                if (System.Windows.Forms.MessageBox.Show("Would u like to save?", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Filter = "Rich Text file (*.rtf)|*.rtf|All files (*.*)|*.*";
                    if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        FileStream fs = new FileStream(saveFile.FileName, FileMode.Create);
                        TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                        range.Save(fs, System.Windows.DataFormats.Rtf);

                    }
                }
                else
                {
                    rtbEditor.SelectAll();
                    rtbEditor.Selection.Text = "";
                }
            }

            rtbEditor.SelectAll();
            rtbEditor.Selection.Text = "";

        }

        
        private void cmbSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSize.SelectedItem != null)
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbSize.SelectedItem); 
            }

            
        }

        private void fontColor(System.Windows.Controls.RichTextBox rc)
        {
            var colorDialog = new System.Windows.Forms.ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var wpgColor = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                TextRange range = new TextRange(rc.Selection.Start, rc.Selection.End);
                range.ApplyPropertyValue(FlowDocument.ForegroundProperty, new SolidColorBrush(wpgColor));
            }
        }

        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            fontColor(rtbEditor);
        }

        private void rtbEditor_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F5)

            {
                DateTime vreme = DateTime.Now;
                rtbEditor.CaretPosition.InsertTextInRun(vreme.ToString("dd.MM.yyyy HH:mm:ss"));
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string find = textBoxSearch.Text;
            var range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            range.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);
            TextPointer current = range.Start.GetInsertionPosition(LogicalDirection.Forward);

            while (current != null)
            {
                string textInRun = current.GetTextInRun(LogicalDirection.Forward);
                if (!string.IsNullOrWhiteSpace(textInRun))
                {
                    int index = textInRun.IndexOf(find);
                    if (index != -1)
                    {
                        TextPointer selectionStart = current.GetPositionAtOffset(index, LogicalDirection.Forward);
                        TextPointer selectionEnd = selectionStart.GetPositionAtOffset(find.Length, LogicalDirection.Forward);
                        TextRange selection = new TextRange(selectionStart, selectionEnd);
                        //selection.Text = newString;
                        selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                        rtbEditor.Selection.Select(selection.Start, selection.End);
                        rtbEditor.Focus();
                    }
                }
                current = current.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        private void btnReaplce_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxSearch.Text.Trim().Equals(""))
            {
                System.Windows.MessageBox.Show("U didn't enter FIND space...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
                      

            string find = textBoxSearch.Text;
            var range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            //range.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);
            TextPointer current = range.Start.GetInsertionPosition(LogicalDirection.Forward);

            while (current != null)
            {
                string textInRun = current.GetTextInRun(LogicalDirection.Forward);
                if (!string.IsNullOrWhiteSpace(textInRun))
                {
                    int index = textInRun.IndexOf(find);
                    if (index != -1)
                    {
                        TextPointer selectionStart = current.GetPositionAtOffset(index, LogicalDirection.Forward);
                        TextPointer selectionEnd = selectionStart.GetPositionAtOffset(find.Length, LogicalDirection.Forward);
                        TextRange selection = new TextRange(selectionStart, selectionEnd);
                        selection.Text = textBoxReplace.Text;
                        //selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                        rtbEditor.Selection.Select(selection.Start, selection.End);
                        rtbEditor.Focus();
                    }
                }
                current = current.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
    }
}
