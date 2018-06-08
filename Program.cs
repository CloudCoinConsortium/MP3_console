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
            bool Working = true;
            //Path pointing to the MP3 & CloudCoin files.
            string Mp3Path = Methods.ReturnMp3FilePath();
            string CCPath = Methods.ReturnCloudCoinFilePath();

            //Define the encoding.
            Encoding FileEncoding = Encoding.ASCII;

            //Save CloudCoin as string
            string MyCloudCoin = System.IO.File.ReadAllText(CCPath);


            TagLib.File Mp3File = TagLib.File.Create(Mp3Path);
            TagLib.Ape.Tag ApeTag = Methods.CheckApeTag(Mp3File);

            while(Working){
                
            //Save CloudCoins to ApeTag
                Methods.SetApeTagValue(ApeTag, MyCloudCoin);
                Methods.Savefile(Mp3File); // Save changes.
                Methods.ReadBytes(Mp3Path, FileEncoding);
                Methods.ReturnCloudCoins(ApeTag);

                Console.WriteLine("Do you want to save the mp3 (y/n) ?");
                string save = Console.ReadLine();
                if(save == "y"){
                    Methods.Savefile(Mp3File);
                }

                Console.WriteLine("Do you want to view saved CloudCoins (y/n) ?");
                string CanSave = Console.ReadLine();
                if(CanSave == "y"){
                    string StoredCloudCoins = Methods.ReturnCloudCoins(ApeTag);
                    Console.WriteLine(StoredCloudCoins);
                }

                Console.WriteLine("Do you want to quit (y/n) ?");
                string CanQuit = Console.ReadLine();
                if(CanQuit == "y"){
                    Working = false;
                }
            }

        }
    }
}

//Removed code
            // TagLib.Id3v2.Tag Mp3Tag = (TagLib.Id3v2.Tag)Mp3File.GetTag(TagTypes.Id3v2);
            // Methods.CreateAnId3Frame(Mp3File, Mp3Tag, MyCloudCoin, FileEncoding); // Create private frame.
            // Methods.ReadAFrame(Mp3File, Mp3Tag, FileEncoding); // Read contents of private frame.
