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
            string CCPath = "./test.txt";
            Encoding FileEncoding = Encoding.UTF8;

            //Key passed into 
            string MyCloudCoin = System.IO.File.ReadAllText(CCPath, FileEncoding);
            string MyMp3 = System.IO.File.ReadAllText(Mp3Path, FileEncoding);
            int length = MyCloudCoin.Length;
            TagLib.File Mp3File = TagLib.File.Create(Mp3Path);

            // You can add a true parameter to the GetTag function if the Mp3File doesn't already have a Mp3Tag.
            // By passing a the parameter TagTypes.Id3V2 we ensure the type is of Id3v2.
            TagLib.Id3v2.Tag Mp3Tag = (TagLib.Id3v2.Tag)Mp3File.GetTag(TagTypes.Id3v2);
            
            Methods.CreateAFrame(Mp3File, Mp3Tag, MyCloudCoin, FileEncoding);
            Methods.ReadAFrame(Mp3File, Mp3Tag, FileEncoding);
            Mp3File.Save();
            Methods.ReadBytes(MyMp3, FileEncoding);
        }
    }
}

