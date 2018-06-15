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
            // You can add a true parameter to the GetTag function if the Mp3File doesn't already have a Mp3Tag.
            // By passing a the parameter 'TagTypes.Ape' we ensure the type is of Ape.
            try{ 
                ApeTag = (TagLib.Ape.Tag)Mp3File.GetTag(TagLib.TagTypes.Ape, false);
                hasCCS = ApeTag.HasItem("CloudCoinStack");
            }catch{
                ApeTag = (TagLib.Ape.Tag)Mp3File.GetTag(TagLib.TagTypes.Ape, true);
            }if(hasCCS){
                return ApeTag;
            }else{
                TagLib.Ape.Item item = new TagLib.Ape.Item("CloudCoinStack","");
                ApeTag.SetItem(item);
              return ApeTag;  
            }
        }
        
        ///Stores the CloudCoin.stack file as the value tied to the CloudCoinStack key.
        public static bool SetApeTagValue(TagLib.Ape.Tag ApeTag, string MyCloudCoin){
            // Get the APEv2 tag if it exists.
            try{
                TagLib.Ape.Item currentStacks = ApeTag.GetItem("CloudCoinStack");
                MyCloudCoin += currentStacks.ToString();
                ApeTag.SetValue("CloudCoinStack", MyCloudCoin);
                return true;
            }catch{
                ApeTag.SetValue("CloudCoinStack", MyCloudCoin);
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
            TagLib.Ape.Item item = ApeTag.GetItem("CloudCoinStack");
           
            if (item != null) {
                    string CloudCoinAreaValues = item.ToString();
                    string path = Mp3File.Name + ".stack";
                    System.IO.File.WriteAllText(path, CloudCoinAreaValues); //Create a document containing Mp3 ByteFile (debugging).
                    Console.Out.WriteLine("CCS: " + CloudCoinAreaValues);
                    return CloudCoinAreaValues;
            }else{
                Console.Out.WriteLine("no stack in file" + item);
                return "no .stack in file";
            }
        }
        public static string ReturnMp3FilePath(){
            string message = "Mp3 files found: ";
            try 
            {
                string[] dirs = Directory.GetFiles("./mp3", "*.mp3");
                string choice = getUserInput(dirs, message);
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
            string[] myStack = new String[2];
            try 
            {
                string[] dirs = Directory.GetFiles("./Bank_Backup", "*.stack");
                myStack[0] = getUserInput(dirs, message);
                myStack[1] = System.IO.File.ReadAllText(myStack[0]);
                return myStack;
            } 
            catch (Exception e) 
            {
                Console.Out.WriteLine("The process failed: {0}", e.ToString());
                myStack[0] = e.ToString();
                return myStack;
            }
        }

        ///
        ///Methods for console messages
        ///

        public static string getUserInput(string[] selection, string message)
        {     
            int index = 0;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("");
            foreach (string file in selection) 
            {   
                 String.Format("[{0, 150}]", file);
                 Console.Out.WriteLine("        " + (index + 1) +": " + file);
                 index++;
            }
            Console.Out.WriteLine(message + " {0}.", selection.Length);
            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Out.WriteLine("");
            Console.Out.WriteLine("Enter your selection: ");
            int choice = reader.readInt(1, index);


            return selection[choice-1];
        }

        public static void printWelcome()
        {
            Console.Out.WriteLine("         ");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("                                                                  ");
            Console.Out.WriteLine("                   CloudCoin Founders Edition                     ");
            Console.Out.WriteLine("                      Version: June.14.2018                       ");
            Console.Out.WriteLine("          Used to store CloudCoin stacks in mp3 files.            ");
            Console.Out.WriteLine("      This Software is provided as is with all faults, defects    ");
            Console.Out.WriteLine("          and errors, and without warranty of any kind.           ");
            Console.Out.WriteLine("                Free from the CloudCoin Consortium.               ");
            Console.Out.WriteLine("                                                                  ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Out.WriteLine("");
        } // End print options

        public static int printOptions()
        {
            Console.Out.WriteLine("             ");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("     Choose from the selection below                                  ");
            Console.Out.WriteLine("                                                                      ");
            Console.Out.WriteLine("     1: Select .mp3 file.                                             ");
            Console.Out.WriteLine("     2: Select .stack file from Bank folder                           ");
            Console.Out.WriteLine("     3: Insert the .stack file into the .mp3 file                     ");
            Console.Out.WriteLine("     4: Return .stack from .mp3                                       ");
            Console.Out.WriteLine("     5: Delete .stack from .mp3                                       ");        
            Console.Out.WriteLine("     6: Save .mp3's current state                                     ");                              
            Console.Out.WriteLine("     7: Quit (remember to save!)                                      ");                              
            Console.Out.WriteLine("     8: Show discriptions                                             ");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("Enter your selection: ");
            int choice = reader.readInt(1, 8);
            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Out.WriteLine("");
            return choice;
        } // End print welcome
         public static int printHelp()
        {
            Console.Out.WriteLine("             ");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("                                                                             ");
            Console.Out.WriteLine("                                                                             ");
            Console.Out.WriteLine(" 1: Select an .mp3 file from a list of files in the 'mp3' folder.            ");
            Console.Out.WriteLine(" 2: Choose a  .stack file from tge 'Bank' folder for storage in an mp3.      ");
            Console.Out.WriteLine(" 3: Insert the .stack file into the .mp3 file                                ");
            Console.Out.WriteLine(" 4: Search the SAVED mp3's data for cloudcoins, then write them to a file.   ");
            Console.Out.WriteLine(" 5: DELETE YOUR CLOUDCOINS (from the .mp3)                                   ");        
            Console.Out.WriteLine(" 6: Changes made to the .mp3 file will be saved                              ");                              
            Console.Out.WriteLine(" 7: End this session, this option does not save changes to the mp3 file.     ");                              
            Console.Out.WriteLine(" 8: Standard menu                                                            ");
            Console.Out.WriteLine("Enter your selection: ");
            int choice = reader.readInt(1, 8);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Out.WriteLine("");
            return choice;
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
