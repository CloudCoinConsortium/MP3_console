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
        //Used for debugging.
        public static void ReadBytes(string Mp3Path, Encoding FileEncoding){
            Console.OutputEncoding = FileEncoding; //set the console output.
            byte[] MyMp3 = System.IO.File.ReadAllBytes(Mp3Path);
            string ByteFile = Hex.Dump(MyMp3); //Store Hex the Hex data from the Mp3 file.
            System.IO.File.WriteAllText("./Mp3HexPrintout.txt", ByteFile); //Create a document containing Mp3 ByteFile (debugging).
        }

        ///Method ensures the mp3 file is encapsulated with the appropriate data.
        ///Does not alter existing meta data. 
        ///Ensures the existence of an ApeTag item with the key: CloudCoinStack.
        ///Correct Apetag has a CloudCoinStack and StackName container.
        public static TagLib.Ape.Tag CheckApeTag(TagLib.File Mp3File){
            TagLib.Ape.Tag ApeTag;
            bool hasCCS = false; 
            bool hasStackName = false;

            // Pass a true parameter to the GetTag function in order to add one if the Mp3File doesn't already have a Mp3Tag.
            // By passing a the parameter 'TagTypes.Ape' we ensure the type is of Ape.

            try{ 
                ApeTag = (TagLib.Ape.Tag)Mp3File.GetTag(TagLib.TagTypes.Ape, true); 
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
            ApeTag.RemoveItem("StackName");
            Console.Out.WriteLine( " stacks deleted.");
        }


        //Collects the stacks saved in the mp3 file. saves them in the printouts folder.
        public static string ReturnCloudCoinStack(TagLib.File Mp3File){
            TagLib.Ape.Tag ApeTag = Methods.CheckApeTag(Mp3File);
            TagLib.Ape.Item CCS = ApeTag.GetItem("CloudCoinStack");
            TagLib.Ape.Item StackN = ApeTag.GetItem("StackName");

            if (CCS != null) {
                    string filename = StackN.ToString();
                    string message = "Press enter to extract the Cloudcoin stack from "+ filename + ".";

                    if(getEnter(message)){
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
                    return "null";
            }else{
                Console.Out.WriteLine("no stack in file" + CCS);
                return "no .stack in file";
            }
        }
        


        //Get the filepaths to any mp3 files in the specified folder.
        //call consolePrintList(paths to files[], argument for printing with index, note to user) 
        //call getUserInput(int max, string note) 
        //return the users choice.
        public static string ReturnMp3FilePath(){
            string message = "Mp3 files found: ";
            string note = "Select the file you wish to use.";
            try 
            {
                string[] mp3FilePaths = Directory.GetFiles("./mp3", "*.mp3");
                consolePrintList(mp3FilePaths, true, message);
                string choice = mp3FilePaths[getUserInput(mp3FilePaths.Length, note)];
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
                string[] ccStackFilePaths = Directory.GetFiles("./Bank", "*.stack");
                string note = "Select the file you wish to use.";
                consolePrintList(ccStackFilePaths, true, message);
                myStack[0] = ccStackFilePaths[getUserInput(ccStackFilePaths.Length, note)]; //Choose the cloudcoin to be added to the mp3.
                myStack[1] = System.IO.File.ReadAllText(myStack[0]);//save the cloudcoin stack data.
                myStack[2] = System.IO.Path.GetFileName(myStack[0]);//save the stacks name.
                return myStack;
            } 
            catch (Exception e) 
            {
                Console.Out.WriteLine("The process failed: {0}", e.ToString());
                myStack[0] = e.ToString();
                return myStack;
            }
        }

        //Method to prompt a user for input. 
        public static int getUserInput(int maxNum, string message)
        {     
            Console.Out.WriteLine("");
            Console.Out.WriteLine(message);
            int choice = reader.readInt(1, maxNum);
            return choice - 1;
        }

        
          //Method to prompt a user for input. 
        public static bool getEnter(string message)
        {     
            Console.Out.WriteLine("");
            Console.Out.WriteLine(message);
            if(Console.ReadKey().Key == ConsoleKey.Enter){
                return true;
            }else{
                return false;
            }
        }      

        //Methods accepts an array of strings. 
        //If indexed? indecese will be numbered 1 through selection.Length. 
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

        ///
        ///Methods to help standardise the UI.
        ///
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
            return getUserInput(8,note);//7? Range of inputs.
        } // End print welcome.

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
            return getUserInput(8,note);//7? Range of inputs.
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
