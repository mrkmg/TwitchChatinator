using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace TwitchChatinator
{
    public interface DataStore : IDisposable
    {
        DataSet getDataSet(DataSetSelection selection);
        bool InsertMessage(string Channel, string User, string Message);

    }

    public class DataSetSelection
    {
        public DateTime Start;
        public DateTime End;
    }
}
