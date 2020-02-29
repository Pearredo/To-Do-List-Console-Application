using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
/*Greetings my dear friend(s), my name is Pedro Arredondo and I have made
 *for you today is a todo list app the functions are simple where you can view,
 *add, check off, and remove from the app. what wou put in become objects with a string
 *that describes your plan and a boolean that describes whether you completed the
 *task or not.
 *
 *DISCLAIMER: this is my first c# project, criticism is much appreciated along with tips
 */
namespace To_Do_List
{
    class TodoList
    {
        //the main file that will hold the objects created by the user's submission
        private const string Path = "ToDoList.bin";

        //The following function checks whether the file that holds the list todo list items
        //exists within the folders, if not then it will create the file automatically
        private static void Filecheck()
        {
            //if the file does not exist then it will be created
            //along with an objective for you to begin filling
            if (!File.Exists(Path))
            {
                Stream stream = new FileStream(Path, FileMode.Create);
                stream.Close();
                Console.WriteLine("Your List Is Currently Empty\n");
                AddToList();
            }
        }
        /*This function's main purpose is to give the user a list of options on how to edit their
         *todo list which involves adding, removing, and checking off what goals they have listed
         */
        private static void Menu()
        {
            // n is used to print the number indicating which item you'll be selecting
            //when you check off or remove an item from the list
            int n=1;
            //prints out what you have on your list and whether they are completed or not
            Console.WriteLine("\n\nThe Following Items are on your List:\n");
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using Stream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
                while (stream.Position < stream.Length)
                {
                    Items obj = (Items)formatter.Deserialize(stream);
                    Console.WriteLine(n+")  "+obj.Goal);
                    if(obj.Finished == false)
                    {
                        Console.WriteLine("\nStatus:  Incomplete\n\n");
                    }
                    else
                    {
                        Console.WriteLine("\nStatus:  Complete\n\n");
                    }
                    n++;
                }
                stream.Close();
            }
            /*originally there was a bug/glitch where it could not read an empty file and crash
             * oddly enough I have not run into it after every test even when I go out of my
             * way to get it, the following catch statement will remain in case that the bug
             * may come back
             */
            catch
            {
                Console.WriteLine("Your Current List Is Empty");
                AddToList();
            }
            /*The following print statements and switch statements are used to
             * give the user a menu to work with so that they may easily access
             * adding, removing, and checking off their items in their list
             */
            Console.WriteLine("\nHow would you like to edit your todo list:");
            Console.WriteLine("\n1)  Add");
            Console.WriteLine("2)  Check Off");
            Console.WriteLine("3)  Remove\n");
            Console.WriteLine("4) Exit\n\n");
            var MenuChoice = Console.ReadLine();
            int Choice;
            try
            {
                Choice = int.Parse(MenuChoice);
            }
            catch
            {
                Choice = 5;
            }
            switch (Choice)
            {
                case 1:
                    AddToList();
                    Menu();
                    break;
                case 2:
                    Console.WriteLine("\nWhich from your todo list would you like to check off? (input the cooresponding number next to the task)\n");
                    CheckOffFromList();
                    Menu();
                    break;
                case 3:
                    Console.WriteLine("\nWhich would you like to remove from the list? (input the cooresponding number next to the task)\n");
                    DeleteFromList();
                    Menu();
                    break;
                case 4:
                    break;
                default:
                    Console.WriteLine("\nYou must input a number from 1-4\n");
                    Menu();
                    break;
            }
        }
        //the following function deletes items from your list
        private static void DeleteFromList()
        {
            //you choose which item to remove from the list
            //based on the number next to that item
            int Choice;
            var menuChoice = Console.ReadLine();
            try
            {
                Choice = int.Parse(menuChoice);
            }
            catch
            {
                //the user has to at least input a number
                Console.WriteLine("\nYou will be placed back to the orginal Menu, input a integer next time\n");
                return;
            }
            /*considering that Binary serialization does not directly allow to edit or remove serialized objects
             *I basically took the items that were not to be deleted and moved them to another file, then
             *replaced the original with the new file
             */
            int n = 1;
            IFormatter formatter = new BinaryFormatter();
            using Stream Originalstream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using Stream DeletionStream = new FileStream("Deletion.bin", FileMode.Create, FileAccess.Write, FileShare.Write);
            while(Originalstream.Position < Originalstream.Length)
            {
                Items obj = (Items)formatter.Deserialize(Originalstream);
                if (Choice != n)
                {
                    formatter.Serialize(DeletionStream, obj);
                }
                n++;
            }
            Originalstream.Close();
            DeletionStream.Close();
            File.Replace("Deletion.bin", Path, "ToDoListDelBac.bin");
            File.Delete("Deletion.bin");
        }
        /*this function allows you to check off items from your list, which
         *will declare whether you had finished the task without removing 
         *the object from your list*/
        private static void CheckOffFromList()
        {
            int Choice;
            var menuChoice = Console.ReadLine();
            try
            {
                Choice = int.Parse(menuChoice);
            }
            catch
            {
                Console.WriteLine("\nYou will be placed back to the orginal Menu, input a integer next time\n");
                return;
            }
            /*the object changes it's boolean value in the process of moving from one file to another
             *I basically create a second file to transfer all of the items from one file to another
             *and in the process change the selected item's boolean value inbetween transfer
             *and finishing off by replacing the original file with the new file
             */
            int n = 1;
            IFormatter formatter = new BinaryFormatter();
            using Stream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using Stream stream1 = new FileStream("Replacement.bin", FileMode.Create, FileAccess.Write, FileShare.Write);
            while (stream.Position < stream.Length)
            {
                Items obj = (Items)formatter.Deserialize(stream);
                if (Choice == n)
                {
                    if(obj.Finished == false)
                    {
                        obj.Finished = true;
                        formatter.Serialize(stream1, obj);
                    }
                    else
                    {
                        Console.WriteLine("That Objective was already completed");
                        return;
                    }
                }
                else
                {
                    formatter.Serialize(stream1, obj);
                }
                n++;
            }
            stream.Close();
            stream1.Close();
            File.Replace("Replacement.bin", Path, "ToDoListRepBac.bin");
            File.Delete("Replcement.bin");
        }
        /*Adding items to the list is a simple process
         *the user inputs a string that gets placed into
         *an object, along with the string what also
         *gets placed into the object is a boolean
         *value that is automatically false because
         * the user has not completed the objective yet
         */
        private static void AddToList()
        {
            Console.WriteLine("\nWhat would you like to add?\n");
            string s = Console.ReadLine();
            Items Objective = new Items(s, false);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(Path, FileMode.Append, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, Objective);
            stream.Close();
        }
        //The main function is used to go from the file check to the menu where
        //the user will spend their entire experience using
        static void Main(string[] args)
        {
            Filecheck();
            Menu();
        }
    }
}
