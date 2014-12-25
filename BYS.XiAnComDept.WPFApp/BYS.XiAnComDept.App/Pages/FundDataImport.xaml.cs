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
using System.Diagnostics;
using BYS.AAPT.Common;
using BYS.XiAnComDept.BussLogic;
using Microsoft.Win32;

namespace BYS.XiAnComDept.App.Pages
{
    /// <summary>
    /// Interaction logic for FundDataImport.xaml
    /// </summary>
    public partial class FundDataImport : System.Windows.Controls.UserControl
    {
        public FundDataImport()
        {
            InitializeComponent();
        }

        private void ButtonSelectExcelAndImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "All files (*.*)|*.xlsx";
            fileDialog.FilterIndex = 1;
            fileDialog.Title = "选择要导入的EXCEL文件";
            if (fileDialog.ShowDialog() == true)
            {
                FundBll fundBll = new FundBll();
                Result result = fundBll.ImportFunds(fileDialog.FileName);
                if (result.IsSuccess)
                {

                    MessageBox.Show(string.Format("数据导入成功！\n" + (result as Result<Dictionary<string, string>>).Tag.ToMsg()));
                }
                else
                {
                    MessageBox.Show(result.ErrorMsg);
                }
            }
        }

        private void ButtonViewDemoExcel_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo("FundDemo.xlsx");
            process.Start();
        }
    }
}
