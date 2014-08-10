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
        DataStoreSQLite DS;
        public MessageBrowser()
        {
            DS = new DataStoreSQLite();
            InitializeComponent();
            StartTimeDatepicker.Format = DateTimePickerFormat.Custom;
            StartTimeDatepicker.CustomFormat = "yyyy-MM-dd hh:mm tt";
            EndTimeDatepicker.Format = DateTimePickerFormat.Custom;
            EndTimeDatepicker.CustomFormat = "yyyy-MM-dd hh:mm tt";
            StartTimeDatepicker.Value = DateTime.Now.AddMinutes(-15);
            EndTimeDatepicker.Value = DateTime.Now;
        }

        private void GetDataButton_Click(object sender, EventArgs e)
        {
            DataSet ds = DS.getDataSet();
            MessagesList.DataSource = ds.Tables[0].DefaultView;
        }
    }
}
