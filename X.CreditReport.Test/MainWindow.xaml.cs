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
using X.CreditReport.Analysis;
using System.IO;
using Newtonsoft.Json;
using Path = System.IO.Path;

namespace X.CreditReport.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Analyze()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPath.Text))
                {
                    MessageBox.Show("需先选择征信报告pdf文件");
                    return;
                }
                if (!File.Exists(txtPath.Text))
                {
                    MessageBox.Show("征信报告pdf文件不存在");
                    return;
                }
                txtContent.Text = "";
                var m = CreditReportSimpleAnalyzer.Analyze(txtPath.Text);
                txtContent.Text = SerializeToFormatJson(m);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private string SerializeToFormatJson(object m)
        {
            //格式化json字符串  
            JsonSerializer serializer = new JsonSerializer();
            StringWriter textWriter = new StringWriter();
            JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            serializer.Serialize(jsonWriter, m);
            return textWriter.ToString();
        }

        private void AnalyzeAll(string path)
        {
            try
            {
                var directoryTop = Path.GetDirectoryName(path);
                var pdfs = Directory.EnumerateFiles(directoryTop, "*.pdf");
                var count = pdfs.Count();
                ShowMessage(string.Format("共找到 {0} 个pdf", count));
                int index = 0;
                int succ = 0;
                foreach (var pdf in pdfs)
                {
                    index++;
                    ShowMessage(string.Format("\n{0} / {1} 正在解析 {2}", index, count, pdf));

                    try
                    {
                        var m = CreditReportAnalyzer.Analyze(pdf);
                        var json = SerializeToFormatJson(m);
                        var directory = Path.GetDirectoryName(pdf);
                        var fileName = Path.GetFileNameWithoutExtension(pdf);
                        var pathThis = Path.Combine(directory, fileName + ".json");
                        File.WriteAllText(pathThis, json, Encoding.UTF8);
                        succ++;
                        ShowMessage(string.Format("\n{0} / {1} 解析成功 {2}", index, count, pathThis));
                    }
                    catch (Exception e)
                    {
                        ShowMessage(string.Format("\n{0} / {1} 解析出错 {2}", index, count, e.Message));
                    }
                }
                ShowMessage(string.Format("\n解析完成，共 {0} 个，成功 {1} 个", count, succ));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ShowMessage(string message)
        {
            txtContent.Dispatcher.Invoke(() =>
            {
                txtContent.Text += message;
                txtContent.ScrollToEnd();
            });
        }

        private void btnAnalyze_Click(object sender, RoutedEventArgs e)
        {
            Analyze();
        }

        private void txtContent_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                txtPath.Text = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                Analyze();
            }
        }

        private void txtContent_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "PDF File (*.pdf)|*.pdf"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                this.txtPath.Text = openFileDialog.FileName;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtContent.Text))
                {
                    MessageBox.Show("需先解析征信报告");
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtPath.Text))
                {
                    MessageBox.Show("需先选择征信报告pdf文件");
                    return;
                }
                var directory = Path.GetDirectoryName(txtPath.Text);
                var fileName = Path.GetFileNameWithoutExtension(txtPath.Text);
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    MessageBox.Show("需先选择征信报告pdf文件");
                    return;
                }
                var path = Path.Combine(directory, fileName + ".json");
                File.WriteAllText(path, txtContent.Text, Encoding.UTF8);
                MessageBox.Show("成功：" + path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAnalyzeAll_Click(object sender, RoutedEventArgs e)
        {
            var path = txtPath.Text;
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("需先选择征信报告pdf文件");
                return;
            }
            if (!File.Exists(path))
            {
                MessageBox.Show("征信报告pdf文件不存在");
                return;
            }
            
            txtContent.Text = "";
            Task.Factory.StartNew(() => AnalyzeAll(path));
        }
    }
}
