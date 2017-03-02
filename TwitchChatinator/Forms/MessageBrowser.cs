using System;
using System.Windows.Forms;
using TwitchChatinator.Libs;

namespace TwitchChatinator.Forms
{
    public partial class MessageBrowser : Form
    {
        public MessageBrowser()
        {
            InitializeComponent();

            StartTimeDatepicker.Format = DateTimePickerFormat.Custom;
            StartTimeDatepicker.CustomFormat = @"yyyy-MM-dd hh:mm tt";
            EndTimeDatepicker.Format = DateTimePickerFormat.Custom;
            EndTimeDatepicker.CustomFormat = @"yyyy-MM-dd hh:mm tt";

            GetData();
        }

        private void GetData()
        {
            var dss = new DataSetSelection();
            if (StartTimeDatepicker.Checked) dss.Start = StartTimeDatepicker.Value;
            if (EndTimeDatepicker.Checked) dss.End = EndTimeDatepicker.Value;
            var ds = DataStore.GetDataSet(dss);
            MessagesList.DataSource = ds.Tables[0].DefaultView;

            MessagesList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void GetDataButton_Click(object sender, EventArgs e)
        {
            GetData();
        }
    }
}