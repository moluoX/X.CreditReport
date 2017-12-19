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
                var m = CreditReportAnalyzer.Analyze(txtPath.Text);
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
    }
}
