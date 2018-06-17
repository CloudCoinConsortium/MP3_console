using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Runtime;
using TagLib;
using TagLib.Id3v2;


namespace AddToMp3
{
    public class Methods
    {
        public static KeyboardReader reader = new KeyboardReader();
        //Creates and saves a .txt ByteFile file, and outputs to the console.
        public static void ReadBytes(string Mp3Path, Encoding FileEncoding){
            Console.OutputEncoding = FileEncoding; //set the console output.
            byte[] MyMp3 = System.IO.File.ReadAllBytes(Mp3Path);
            string ByteFile = Hex.Dump(MyMp3); //Store Hex the Hex data from the Mp3 file.
            System.IO.File.WriteAllText("./Mp3HexPrintout.txt", ByteFile); //Create a document containing Mp3 ByteFile (debugging).
        }

        ///Method ensures the mp3 file is encapsulated with the appropriate data.
        ///Does not alter existing meta data. 
        ///Ensures the existence of an ApeTag item with the key: CloudCoinStack.
        public static TagLib.Ape.Tag CheckApeTag(TagLib.File Mp3File){
            TagLib.Ape.Tag ApeTag;
            bool hasCCS = false; 
            bool hasStackName = false;

            // You can add a true parameter to the GetTag function if the Mp3File doesn't already have a Mp3Tag.
            // By passing a the parameter 'TagTypes.Ape' we ensure the type is of Ape.

            try{ 
                ApeTag = (TagLib.Ape.Tag)Mp3File.GetTag(TagLib.TagTypes.Ape, true); //Adds one if none found.
                hasCCS = ApeTag.HasItem("CloudCoinStack");
                hasStackName = ApeTag.HasItem("StackName");
            }
            catch(Exception e)
            {
                Console.Out.WriteLine("The process failed: {0}", e.ToString());
                ApeTag = (TagLib.Ape.Tag)Mp3File.GetTag(TagLib.TagTypes.Ape, false);
            }
            if(!hasCCS){
                TagLib.Ape.Item item = new TagLib.Ape.Item("CloudCoinStack","");
                ApeTag.SetItem(item); 
            }
            if(!hasStackName){
                TagLib.Ape.Item itemName = new TagLib.Ape.Item("StackName","");
                ApeTag.SetItem(itemName);
            }
            return ApeTag;
        }
        
        ///Stores the CloudCoin.stack file as the value tied to the CloudCoinStack key.
        public static bool SetApeTagValue(TagLib.Ape.Tag ApeTag, string MyCloudCoin, string stackName){
            // Get the APEv2 tag if it exists.
            try{
                TagLib.Ape.Item currentStacks = ApeTag.GetItem("CloudCoinStack");
                ApeTag.SetValue("CloudCoinStack", MyCloudCoin);
                ApeTag.SetValue("StackName", stackName);
                return true;
            }
            catch(Exception e)
            {
                Console.Out.WriteLine("The process failed: {0}", e.ToString());
                return false;
            }
        }

        //Searches for and removes the specified Key and Value.
        public static void RemoveExistingStacks(TagLib.Ape.Tag ApeTag){
            ApeTag.RemoveItem("CloudCoinStack");
            Console.Out.WriteLine( " stacks deleted.");
        }

        public static string ReturnCloudCoinStack(TagLib.File Mp3File){
            TagLib.Ape.Tag ApeTag = Methods.CheckApeTag(Mp3File);
            TagLib.Ape.Item CCS = ApeTag.GetItem("CloudCoinStack");
            TagLib.Ape.Item StackN = ApeTag.GetItem("StackName");

            if (CCS != null) {
                    Console.Out.WriteLine("press enter to extract this stack.");
                    string filename = StackN.ToString();
                    if(Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        Console.Out.WriteLine("Stack: " + filename + " has been found");
                        string CloudCoinAreaValues = CCS.ToString();
                        string path ="./Printouts/"+ filename;
                        try
                        {
                            System.IO.File.WriteAllText(path, CloudCoinAreaValues); //Create a document containing Mp3 ByteFile (debugging).
                        }
                        catch(Exception e)
                        {
                            Console.Out.WriteLine("Failed to save CloudCoin data {0}", e);
                        }
                    
                        Console.Out.WriteLine("CCS: " + CloudCoinAreaValues);
                        return path;
                    }
                    return "ERROR";
            }else{
                Console.Out.WriteLine("no stack in file" + CCS);
                return "no .stack in file";
            }
        }
        public static string ReturnMp3FilePath(){
            string message = "Mp3 files found: ";
            try 
            {
                string[] dirs = Directory.GetFiles("./mp3", "*.mp3");
                string note = "Select the file you wish to use.";
                consolePrintList(dirs, true, message);
                string choice = dirs[getUserInput(dirs.Length, note , false)];
                return choice;
            } 
            catch (Exception e) 
            {
                Console.Out.WriteLine("The process failed: {0}", e.ToString());
                return e.ToString();
            }
        }

        ///Get stacks from the bank, print them to the console. 
        ///Allow user to choose the stack to insert.
        ///Returns the stack to the function call.
        public static string[] collectBankStacks()
        {
            string message = "Stack files found: ";
            string[] myStack = new String[3];
            try 
            {
                string[] dirs = Directory.GetFiles("./Bank", "*.stack");
                string note = "Select the file you wish to use.";
                consolePrintList(dirs, true, message);
                myStack[0] = dirs[getUserInput(dirs.Length, note , false)-1];
                myStack[1] = System.IO.File.ReadAllText(myStack[0]);
                myStack[2] = System.IO.Path.GetFileName(myStack[0]);
                return myStack;
            } 
            catch (Exception e) 
            {
                Console.Out.WriteLine("The process failed: {0}", e.ToString());
                myStack[0] = e.ToString();
                return myStack;
            }
        }

        public static int getUserInput(int range, string message, bool anyKey)
        {     
            Console.Out.WriteLine("");
            Console.Out.WriteLine(message);
            if(anyKey){
                Console.WriteLine("Press enter: ");
                Console.ReadKey();
            }
            int choice = reader.readInt(1, range);

            return choice;
        }

        public static void consolePrintList(string[] selection, bool indexed, string message){
            int index = 0;
            Console.Out.WriteLine("");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine(message);
            Console.Out.WriteLine("");
            foreach (string file in selection) 
            {   
                
                int fileLength = Console.WindowWidth - file.Length-1;

                if(indexed)
                {
                 string newFile = String.Format("{0, -25}", file);
                 string indexString = "     "+(index+1).ToString()+": ";
                 Console.Out.WriteLine("{0,-5} {1,3:N1}", indexString, newFile);
                 index++;
                }
                else
                {
                    Console.Out.WriteLine("{0, -4} {1,"+fileLength+":N1}", file, " ");
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public static void printWelcome()
        {
            string[] welcomeMsg = new string[6];
            welcomeMsg[0] = "                     CloudCoin Founders Edition                       ";
            welcomeMsg[1] = "                        Version: June.14.2018                         ";
            welcomeMsg[2] = "            Used to store CloudCoin stacks in mp3 files.              ";
            welcomeMsg[3] = "        This Software is provided as is with all faults, defects      ";
            welcomeMsg[4] = "            and errors, and without warranty of any kind.             ";
            welcomeMsg[5] = "                  Free from the CloudCoin Consortium.                 ";
            consolePrintList(welcomeMsg, false, ""); //false? message is not indexed.
            
        } // End print options

        public static int printOptions()
        {
             string note = "Enter your selection: ";
            string[] userChoices = new string[8];
            userChoices[0] = "Select .mp3 file.                                                     "; //Option 1
            userChoices[1] = "Select .stack file from Bank folder.                                  "; //Option 2
            userChoices[2] = "Insert the .stack file into the .mp3 file.                            "; //Option 3
            userChoices[3] = "Return .stack from .mp3                                               "; //Option 4
            userChoices[4] = "Delete .stack from .mp3                                               "; //Option 5
            userChoices[5] = "Save .mp3's current state                                             "; //Option 6
            userChoices[6] = "Quit (remember to save!)                                              "; //Option 7
            userChoices[7] = "Show discriptions                                                     "; //Option 8
            consolePrintList(userChoices, true, note); //true? message is indexed.
            return getUserInput(8,note,false);//7? Range of inputs.
        } // End print welcome
         public static int printHelp()
        {
            // string note = "message + " {0}.", selection.Length"
            string note = "Enter your selection: ";
            string[] userChoices = new string[8];
            userChoices[0] = "Select an .mp3 file from a list of files in the 'mp3' folder.         "; //Option 1
            userChoices[1] = "Choose a  .stack file from the 'Bank' folder for storage in an mp3.   "; //Option 2
            userChoices[2] = "Insert the .stack file into the .mp3 file.                            "; //Option 3
            userChoices[3] = "Search the SAVED mp3's data for cloudcoins, then write them to a file."; //Option 4
            userChoices[4] = "DELETE YOUR CLOUDCOINS (from the .mp3)                                "; //Option 5
            userChoices[5] = "Changes made to the .mp3 file will be saved                           "; //Option 6
            userChoices[6] = "End this session, this option does not save changes to the mp3 file.  "; //Option 7
            userChoices[7] = "Standard menu                                                         "; //Option 8
            consolePrintList(userChoices, true, note); //true? message is indexed.
            return getUserInput(8,note, false);//7? Range of inputs.
        } // End print welcome

        public static void printStates(string[] states)
        {
            int index = 0;
            foreach(string state in states){
                index++;
                if(state != null)
                { 
                    Console.WriteLine( index + ": " + state);
                }
                
            }
        }

    }
}
        //CloudCoinNaming
        // public static string stackNaming(string stack){
        //     string Denomination = (Regex.Match(stack, @"^\d{1,3}").ToString()); //Match up to 3 of the first didgits of the files name. 
        //     string cc = "cloudcoin"; //Always cloudcoin
        //     string networkNum = (Regex.Match(stack, @"^\d{1,3}\.").ToString());
        //     string serialNum = "";
        //     string mintTag = (Regex.Match(stack, @".*"));
        //     string fileExt = ".stack";


        //     return stack;
        // }

        ///
        ///Methods for console messages
        ///
