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
            string Mp3Path = Methods.ReturnMp3FilePath(); //Save file path.;
            TagLib.File Mp3File = TagLib.File.Create(Mp3Path); //Create TagLib file... ensures a Id3v2 header.;
            TagLib.Ape.Tag ApeTag = Methods.CheckApeTag(Mp3File); //return existing tag. create one if none.;
            bool makingChanges = true;
            string[] endState = new string[6];

            Methods.printWelcome();
            while (makingChanges){
                int userChoice = Methods.printOptions();
                string CloudCoinStack = "";
                switch(userChoice){
                    case 1: //Select .mp3 file.

                        Mp3Path = Methods.ReturnMp3FilePath(); //Save file path.
                        Mp3File = TagLib.File.Create(Mp3Path); //Create TagLib file... ensures a Id3v2 header. 
                        ApeTag = Methods.CheckApeTag(Mp3File); //return existing tag. create one if none.

                        string fileName = Mp3File.Name;
                        endState[0] = fileName;
                    break;
                    case 2://Select .stack file from Bank folder
                        try
                        {
                        CloudCoinStack = Methods.SaveBankStacks(ApeTag); //Select stacks to insert. 
                        endState[1] = ".stack copied from ./Bank";
                        }
                        catch
                        {
                        CloudCoinStack = Methods.SaveBankStacks(ApeTag); //Select stacks to insert. 
                        endState[1] = ".stack error ";
                        }
                    break;
                    case 3://Inset the .stack file into the .mp3 file 
                        if(CloudCoinStack != null)
                        {
                            Methods.RemoveExistingStacks(ApeTag);
                            Methods.SetApeTagValue(ApeTag, CloudCoinStack);
                            endState[2] = ".stack was successfully inserted in " + Mp3File.Name;
                        }
                        else{
                            Console.WriteLine("No saved cloud coin stack.");
                        }
                    break;
                    case 4://Return .stack from .mp3
                
                        Methods.ReturnCloudCoinStack(Mp3File);
                        Console.WriteLine("CloudCoinPrintout created at ./note1.stack");
                        endState[3] = "A file was created './note1.stack' with the CloudCoinStack found in " + Mp3File.Name;
                    break;
                    case 5://Delete .stack from .mp3 
                        Methods.RemoveExistingStacks(ApeTag);
                        endState[4] = "Any existing stacks in " + Mp3File.Name + " have been deleted.";
                    break;
                    case 6://Save .mp3's current state
                        Methods.Savefile(Mp3File); // Save changes.
                        endState[5] = Mp3File.Name + " has been saved with the changes made";
                    break;
                    case 7://Quit
                        makingChanges = false;
                    break;
                    default:
                        Console.WriteLine("No matches for input!");
                    break;
                }
            }
            

            
            Console.WriteLine("Do you want to save the mp3 (y/n) ?");
        }
    }
}

//Removed code
            // TagLib.Id3v2.Tag Mp3Tag = (TagLib.Id3v2.Tag)Mp3File.GetTag(TagTypes.Id3v2);
            // Methods.CreateAnId3Frame(Mp3File, Mp3Tag, MyCloudCoin, FileEncoding); // Create private frame.
            // Methods.ReadAFrame(Mp3File, Mp3Tag, FileEncoding); // Read contents of private frame.
