using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchChatinator
{
    public partial class RunPoll : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        public bool _stopThread = false;
        Thread GetDataThread;
        public int ss = 1;

        private const int POLLBAR_PADDING = 10;

        public RunPoll()
        {
            InitializeComponent();
            CenterToScreen();
            SetStyle(ControlStyles.ResizeRedraw, true);
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Text = "Fun with graphics";
            this.Resize += RunPoll_Resize;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            BackColor = PollSetup.getColorFromString(Settings.Default.PollChromaKey);
            this.KeyUp += RunPoll_KeyUp;
            this.FormClosing += RunPoll_FormClosing;

            GetDataThread = new Thread(new ThreadStart(GetDataRunner));
            GetDataThread.Start();
        }

        void RunPoll_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopThread();
        }

        void DrawGraph(PollData data)
        {
            Invalidate();
            int windowWidth = Width;
            int windowHeight = Height;

            int countOptions = data.options.Length;

            int pollBarHeight = (windowHeight / countOptions) - ((countOptions + 1) * POLLBAR_PADDING);

            Graphics G = this.CreateGraphics();
            SolidBrush SBrush;
            Font F = new Font(FontFamily.GenericSansSerif, (float)16);

            for (int i = 0; i < countOptions; i++)
            {
                SBrush = new SolidBrush(Color.White);
 
            }


        }

        void GetDataRunner()
        {
            PollData data;

            int setItems = 0;

            if (Settings.Default.PollOption4 != String.Empty)
            {
                setItems = 4;
            }
            else if (Settings.Default.PollOption3 != String.Empty)
            {
                setItems = 3;
            }
            else if (Settings.Default.PollOption2 != String.Empty)
            {
                setItems = 2;
            }
            else if (Settings.Default.PollOption1 != String.Empty)
            {
                setItems = 1;
            }

            data = new PollData(setItems);
            for (int i = 0; i < setItems; i++)
            {
                switch (i)
                {
                    case 1:
                        data.options[i] = Settings.Default.PollOption1;
                        break;
                    case 2:
                        data.options[i] = Settings.Default.PollOption2;
                        break;
                    case 3:
                        data.options[i] = Settings.Default.PollOption3;
                        break;
                    case 4:
                        data.options[i] = Settings.Default.PollOption4;
                        break;
                }
            }

            int totalRows;
            int[] rowData;
            List<string> recordedUsers;

            while (!_stopThread)
            {
                if (setItems > 0)
                {
                    using (DataStore DS = Program.getSelectedDataStore())
                    {
                        DataSetSelection DSS = new DataSetSelection();
                        DataSet RawData = DS.getDataSet(DSS);

                        totalRows = 0;
                        rowData = new int[setItems];
                        recordedUsers = new List<string>();

                        foreach (DataRow row in RawData.Tables[0].Rows)
                        {
                            for (int i = 0; i < setItems; i++)
                            {
                                if (
                                    row.ItemArray[3].ToString().ToLower().Contains(Settings.Default.PollOption1.ToLower()) &&
                                    !recordedUsers.Contains(row.ItemArray[1].ToString())
                                   )
                                {
                                    recordedUsers.Add(row.ItemArray[1].ToString());
                                    rowData[i]++;
                                    totalRows++;
                                    break;
                                }
                            }
                        }

                        for (int i = 0; i < setItems; i++)
                        {
                            data.amounts[i] = rowData[i];
                        }
                        data.totalVotes = totalRows;
                    }
                }

                DrawGraph(data);
                Thread.Sleep(32);
            }
        }

        void RunPoll_Paint(object sender, PaintEventArgs e)
        {
        }

        void RunPoll_Resize(object sender, EventArgs e)
        {
            // Invalidate(); Will be invalidated soon
        }

        void RunPoll_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        //Dragging Forms.
        // http://stackoverflow.com/questions/4767831/drag-borderless-windows-form-by-mouse
        // Thank you http://stackoverflow.com/users/39106/filip-ekberg

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
                message.Result = (IntPtr)HTCAPTION;
        }

        private void StopThread()
        {
            _stopThread = true;
        }
    }

    class PollData
    {
        public string[] options;
        public int[] amounts;
        public int totalVotes = 0;

        public PollData(int count)
        {
            options = new string[count];
            amounts = new int[count];
        }
    }
}
