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
    class Program
    {
        static void Main(string[] args)
        {
            //Define the encoding.
            Encoding FileEncoding = Encoding.ASCII;
            string Mp3Path = null;// = Methods.ReturnMp3FilePath(); //Save file path.;
            TagLib.File Mp3File = null;// = TagLib.File.Create(Mp3Path); //Create TagLib file... ensures a Id3v2 header.;
            TagLib.Ape.Tag ApeTag = null;// = Methods.CheckApeTag(Mp3File); //return existing tag. create one if none.;


            bool makingChanges = true; //Keeps the session runnning.
            bool menuStyle = true;
            string[] endState = new string[6]; //Keeps the current state of each case.
            string[] collectCloudCoinStack = new string[2];
            string mp3CurrentCoinStack = "";
            Methods.printWelcome();
            int userChoice = Methods.printOptions();

            while (makingChanges){
                switch(userChoice){
                    case 1: //Select .mp3 file.
                        Mp3Path = Methods.ReturnMp3FilePath(); //Save file path.
                        Mp3File = TagLib.File.Create(Mp3Path); //Create TagLib file... ensures a Id3v2 header. 
                        ApeTag = Methods.CheckApeTag(Mp3File); //return existing tag. create one if none.
                        string fileName = Mp3File.Name;
                        endState[0] = "MP3 file: " + fileName + " has been selected. ";
                    break;
                    case 2://Select .stack file from Bank folder
                        try
                        {
                        collectCloudCoinStack = Methods.collectBankStacks(); //Select stacks to insert. 
                        endState[1] = "Stack: " + collectCloudCoinStack[0];
                        }
                        catch
                        {
                        collectCloudCoinStack = Methods.collectBankStacks(); //Select stacks to insert. 
                        endState[1] = ".stack error ";
                        }
                        Console.Out.WriteLine(endState[1]);
                    break;
                    case 3://Insert the .stack file into the .mp3 file 
                    string cloudCoinStack = collectCloudCoinStack[1];
                    Console.Out.WriteLine("Existing Stacks in the mp3 will be overwritten");
                    Console.Out.WriteLine("Enter/Return to continue, Any other key to go back.");
                    if(Console.ReadKey(true).Key == ConsoleKey.Enter)
                    {
                        if(cloudCoinStack != null && ApeTag != null)
                        {
                            Console.Out.WriteLine("Existing Stacks in the mp3 will be overwritten");
                            ApeTag = Methods.CheckApeTag(Mp3File);
                            Methods.SetApeTagValue(ApeTag, cloudCoinStack);
                            endState[2] = ".stack was successfully inserted in " + Mp3File.Name;
                            endState[4] = "Stacks in " + Mp3File.Name + " have been added.";
                        }
                        else{
                            Methods.SetApeTagValue(ApeTag, cloudCoinStack);
                            endState[2] = "No saved cloud coin stack.";
                        }
                         Console.Out.WriteLine(endState[2]);
                    }
                    break;
                    case 4://Return .stack from .mp3
                
                        mp3CurrentCoinStack = Methods.ReturnCloudCoinStack(Mp3File);
                        endState[3] = "A file was created  with the CloudCoinStack found in " + Mp3File.Name + ".stack ";
                        Console.Out.WriteLine(endState[3]);
                    break;
                    case 5://Delete .stack from .mp3 
                        Console.Out.WriteLine("WARNING: you are about to permenantley delete any stack files found in " + Mp3File.Name);
                        Console.Out.WriteLine("Enter/Return to continue, Any other key to go back.");
                        if(Console.ReadKey(true).Key == ConsoleKey.Enter){
                        Methods.RemoveExistingStacks(ApeTag);
                        endState[4] = "Any existing stacks in " + Mp3File.Name + " have been deleted.";
                        }else{
                        endState[4] = "Stacks in " + Mp3File.Name + " have NOT been deleted.";
                        }

                        Console.Out.WriteLine(endState[4]);
                    break;
                    case 6://Save .mp3's current state
                        Mp3File.Save(); // Save changes.
                        endState[5] = Mp3File.Name + " has been saved with the changes made";
                        Console.Out.WriteLine(endState[5]);
                    break;
                    case 7://Quit
                        makingChanges = false;
                    break;
                    case 8://Descriptions
                        menuStyle = !menuStyle;
                    break;
                    default:
                        Console.Out.WriteLine("No matches for input!");
                    break;
                }
                switch(menuStyle){
                    case true:
                        Console.Clear();
                        Methods.printStates(endState);
                        userChoice = Methods.printOptions();
                    break;
                    case false:
                        Console.Clear();
                        Methods.printStates(endState);
                        userChoice = Methods.printHelp();  
                    break;
                }
            }
            

            
            Console.Out.WriteLine("Do you want to save the mp3 (y/n) ?");
        }
    }
}

//Removed code
            // TagLib.Id3v2.Tag Mp3Tag = (TagLib.Id3v2.Tag)Mp3File.GetTag(TagTypes.Id3v2);
            // Methods.CreateAnId3Frame(Mp3File, Mp3Tag, MyCloudCoin, FileEncoding); // Create private frame.
            // Methods.ReadAFrame(Mp3File, Mp3Tag, FileEncoding); // Read contents of private frame.
