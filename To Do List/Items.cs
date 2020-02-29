using System;

namespace To_Do_List
{
    //the objects made from this class will be serialized and saved
    [Serializable]
    //this class of items are what are placed into your to do lists
    public class Items
    {
        //the string is what the user inputs into the list
        public string Goal;
        //and the boolean is what indicates whether it is complete or not
        public bool Finished;
        public Items(string aGoal, bool notFinished)
        {
            Goal = aGoal;
            Finished = notFinished;
        }
    }
}
