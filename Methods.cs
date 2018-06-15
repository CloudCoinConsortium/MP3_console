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
            Console.Out.WriteLine("ByteFile created at ./Mp3HexPrintout.txt"); //Log
        }
        public static TagLib.Ape.Tag CheckApeTag(TagLib.File Mp3File){
            TagLib.Ape.Tag ApeTag;
            bool hasCCS = false; 
            // You can add a true parameter to the GetTag function if the Mp3File doesn't already have a Mp3Tag.
            // By passing a the parameter 'TagTypes.Ape' we ensure the type is of Ape.
            try
            {
                ApeTag = (TagLib.Ape.Tag)Mp3File.GetTag(TagLib.TagTypes.Ape, false);
                hasCCS = ApeTag.HasItem("CloudCoinStack");
                Console.Out.WriteLine("check try"); //Log
            }
            catch
            {
                ApeTag = (TagLib.Ape.Tag)Mp3File.GetTag(TagLib.TagTypes.Ape, true);
                Console.Out.WriteLine("check catch"); //Log
            }



            if(hasCCS)
            {
                Console.Out.WriteLine("hasCCS"); //Log
                return ApeTag;
            }
            else 
            {
                Console.Out.WriteLine("no CCS"); //Log
                TagLib.Ape.Item item = new TagLib.Ape.Item("CloudCoinStack","");
                ApeTag.SetItem(item);
              return ApeTag;  
            }
        }

        public static bool SetApeTagValue(TagLib.Ape.Tag ApeTag, string MyCloudCoin){
            // Get the APEv2 tag if it exists.
            try{
                Console.Out.WriteLine("SetApeTagValue Try"); //Log
                TagLib.Ape.Item currentStacks = ApeTag.GetItem("CloudCoinStack");
                MyCloudCoin += currentStacks.ToString();
                ApeTag.SetValue("CloudCoinStack", MyCloudCoin);
                return true;
            }catch{
                Console.Out.WriteLine("SetApeTagValue Catch"); //Log
                ApeTag.SetValue("CloudCoinStack", MyCloudCoin);
                return false;
            }
        }
        public static void RemoveExistingStacks(TagLib.Ape.Tag ApeTag){
            ApeTag.RemoveItem("CloudCoinStack");
            ApeTag.RemoveItem("CloudCoinContainer");
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
                // string mp3 = System.IO.File.ReadAllText(choice);
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
        public static string collectBankStacks(TagLib.Ape.Tag ApeTag)
        {
            string message = "Stack files found: ";
            try 
            {
                string[] dirs = Directory.GetFiles("./Bank_Backup", "*.stack");
                string choice = getUserInput(dirs, message);
                string stackFile = System.IO.File.ReadAllText(choice);
                return stackFile;
            } 
            catch (Exception e) 
            {
                Console.Out.WriteLine("The process failed: {0}", e.ToString());
                return e.ToString();
            }
        }
        public static string getUserInput(string[] dirs, string message)
        {
            
            int index = 0;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine(message + " {0}.", dirs.Length);
            foreach (string dir in dirs) 
            {   
                 Console.Out.WriteLine("        " + (index + 1) +": " + dir + "                         ");
                 index++;
            }
            Console.Out.WriteLine("             Enter your selection:                                   ");
            int choice = reader.readInt(1, index);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            return dirs[choice-1];
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
            Console.Out.WriteLine("     Choose from the selection below                                 ");
            Console.Out.WriteLine("                                                                     ");
            Console.Out.WriteLine("     1: Select .mp3 file.                                            ");
            Console.Out.WriteLine("     2: Select .stack file from Bank folder                          ");
            Console.Out.WriteLine("     3: Insert the .stack file into the .mp3 file                     ");
            Console.Out.WriteLine("     4: Return .stack from .mp3                                      ");
            Console.Out.WriteLine("     5: Delete .stack from .mp3                                      ");        
            Console.Out.WriteLine("     6: Save .mp3's current state                                    ");                              
            Console.Out.WriteLine("     7: Quit                                                         ");                              
            Console.Out.WriteLine("         ");
            Console.Out.WriteLine("Enter your selection: ");
            int choice = reader.readInt(1, 7);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Out.WriteLine("");
            return choice;
        } // End print welcome

    }
}
