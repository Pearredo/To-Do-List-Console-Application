using System;
using System.Collections.Generic;
using System.Text;

namespace To_Do_List
{
    [Serializable]
    public class Items
    {
        public string Goal;
        public bool Finished;
        public Items(string aGoal, bool notFinished)
        {
            Goal = aGoal;
            Finished = notFinished;
        }
    }
}
