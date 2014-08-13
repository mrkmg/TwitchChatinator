using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchChatinator
{
    public partial class MessageBrowser : Form
    {
        
        public MessageBrowser()
        {
            InitializeComponent();

            StartTimeDatepicker.Format = DateTimePickerFormat.Custom;
            StartTimeDatepicker.CustomFormat = "yyyy-MM-dd hh:mm tt";
            EndTimeDatepicker.Format = DateTimePickerFormat.Custom;
            EndTimeDatepicker.CustomFormat = "yyyy-MM-dd hh:mm tt";

            GetData();
        }

        private void GetData()
        {
            using (DataStore DS = Program.getSelectedDataStore())
            {
                DataSetSelection dss = new DataSetSelection();
                if (StartTimeDatepicker.Checked) dss.Start = StartTimeDatepicker.Value;
                if (EndTimeDatepicker.Checked) dss.End = EndTimeDatepicker.Value;
                DataSet ds = DS.GetDataSet(dss);
                MessagesList.DataSource = ds.Tables[0].DefaultView;
            }

            MessagesList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void GetDataButton_Click(object sender, EventArgs e)
        {
            GetData();
        }
    }
}
