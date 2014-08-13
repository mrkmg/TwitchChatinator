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
        DataSet GetDataSet(DataSetSelection selection);
        bool InsertMessage(string Channel, string User, string Message);
        int GetUniqueUsersCount(DataSetSelection selection);
        List<string> GetUniqueUsersString(DataSetSelection selection);

    }

    public class DataSetSelection
    {
        public DateTime Start;
        public DateTime End;
    }
}
