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
            string CloudCoinStack = "";

            Methods.printWelcome();
            while (makingChanges){
                int userChoice = Methods.printOptions();
                
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
                        CloudCoinStack = Methods.collectBankStacks(ApeTag); //Select stacks to insert. 
                        endState[1] = ".stack copied from ./Bank";
                        }
                        catch
                        {
                        CloudCoinStack = Methods.collectBankStacks(ApeTag); //Select stacks to insert. 
                        endState[1] = ".stack error ";
                        }
                        Console.WriteLine(endState[1]);
                    break;
                    case 3://Insert the .stack file into the .mp3 file 
                        if(CloudCoinStack != null)
                        {
                            Console.WriteLine("Checking Ape tag");
                            ApeTag = Methods.CheckApeTag(Mp3File);
                            Methods.SetApeTagValue(ApeTag, CloudCoinStack);
                            endState[2] = ".stack was successfully inserted in " + Mp3File.Name;
                        }
                        else{
                            Methods.SetApeTagValue(ApeTag, CloudCoinStack);
                            endState[2] = "No saved cloud coin stack.";
                        }
                         Console.WriteLine(endState[2]);
                    break;
                    case 4://Return .stack from .mp3
                
                        CloudCoinStack = Methods.ReturnCloudCoinStack(Mp3File);
                        endState[3] = "A file was created  with the CloudCoinStack found in " + Mp3File.Name + ".stack ";
                        Console.WriteLine(endState[3]);
                    break;
                    case 5://Delete .stack from .mp3 
                        Methods.RemoveExistingStacks(ApeTag);
                        endState[4] = "Any existing stacks in " + Mp3File.Name + " have been deleted.";
                        Console.WriteLine(endState[4]);
                    break;
                    case 6://Save .mp3's current state
                        Mp3File.Save(); // Save changes.
                        endState[5] = Mp3File.Name + " has been saved with the changes made";
                        Console.WriteLine(endState[5]);
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
