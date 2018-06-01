using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime;
using TagLib;
using TagLib.Id3v2;

namespace AddToMp3
{
    class Program
    {
        static void Main(string[] args)
        {
            //Key passed into 
            string MyCloudCoin = System.IO.File.ReadAllText("./text.txt", Encoding.ASCII);

            //Path pointing to the MP3 file.
            string FilePath = "./pewUni.mp3";
            TagLib.File Mp3File = TagLib.File.Create(FilePath);

            // You can add a true parameter to the GetTag function if the Mp3File doesn't already have a Mp3Tag.
            // By passing a the parameter TagTypes.Id3V2 
            TagLib.Id3v2.Tag Mp3Tag = (TagLib.Id3v2.Tag)Mp3File.GetTag(TagTypes.Id3v2);
            // CreateAFrame(Mp3File, Mp3Tag, MyCloudCoin);
            ReadAFrame(Mp3File, Mp3Tag, MyCloudCoin);
            Mp3File.Save();
        }

        static void CreateAFrame(TagLib.File Mp3File, TagLib.Id3v2.Tag Mp3Tag, string MyCloudCoin)
        {
             // Note that the third parameter is true while writing.
            PrivateFrame PrivFram = PrivateFrame.Get(Mp3Tag, "CloudCoins", true);

            //
            PrivFram.PrivateData = System.Text.Encoding.UTF8.GetBytes(MyCloudCoin);
        }


        static void ReadAFrame(TagLib.File Mp3File, TagLib.Id3v2.Tag Mp3Tag, string MyCloudCoin)
        {
            // Retreive the ID3TAG.
            // Note that the third parameter is false while reading.
            PrivateFrame PrivFrame = PrivateFrame.Get(Mp3Tag, "CloudCoins", false);

            //Retrieve the CloudCoin from the PRIV frame.
            //Note without a PRIV frame will result in error. needs error check.
            string CloudCoin = Encoding.UTF8.GetString(PrivFrame.PrivateData.Data);  

            Console.WriteLine("CloudCoin: " + CloudCoin); 

        }
    }
}

