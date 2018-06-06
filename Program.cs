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
            //Path pointing to the MP3 & CloudCoin files.
            string Mp3Path = "./pew.mp3";
            string CCPath = "./CloudCoin.json";
            Encoding FileEncoding = Encoding.ASCII;

            //Key passed into 
            string MyCloudCoin = System.IO.File.ReadAllText(CCPath);

            TagLib.File Mp3File = TagLib.File.Create(Mp3Path);

            // You can add a true parameter to the GetTag function if the Mp3File doesn't already have a Mp3Tag.
            // By passing a the parameter TagTypes.Ape we ensure the type is of Ape.

            TagLib.Ape.Tag ApeTag = (TagLib.Ape.Tag)Mp3File.GetTag(TagLib.TagTypes.Ape, true);


            // Methods.CreateAnApeFrame(ApeTag, MyCloudCoin, FileEncoding); // Create an Ape frame
            Methods.CheckApeTag(ApeTag);
            Methods.SetApeTagValue(Mp3File, ApeTag, MyCloudCoin);
            Mp3File.Save(); // Save changes.

            byte[] MyMp3 = System.IO.File.ReadAllBytes(Mp3Path);
            Methods.ReadBytes(MyMp3, FileEncoding);
            Methods.ReturnCloudCoins(ApeTag);
            
        }
    }
}

//Removed code
            // TagLib.Id3v2.Tag Mp3Tag = (TagLib.Id3v2.Tag)Mp3File.GetTag(TagTypes.Id3v2);
            // Methods.CreateAnId3Frame(Mp3File, Mp3Tag, MyCloudCoin, FileEncoding); // Create private frame.
            // Methods.ReadAFrame(Mp3File, Mp3Tag, FileEncoding); // Read contents of private frame.

