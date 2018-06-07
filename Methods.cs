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
        //Creates and saves a .txt ByteFile file, and outputs to the console.
        public static void ReadBytes(string Mp3Path, Encoding FileEncoding){
            Console.OutputEncoding = FileEncoding; //set the console output.
            byte[] MyMp3 = System.IO.File.ReadAllBytes(Mp3Path);
            string ByteFile = Hex.Dump(MyMp3); //Store Hex the Hex data from the Mp3 file.
            System.IO.File.WriteAllText("./Mp3HexPrintout.txt", ByteFile); //Create a document containing Mp3 ByteFile (debugging).
            Console.WriteLine("ByteFile created at ./Mp3HexPrintout.txt"); //Log
        }
        public static TagLib.Ape.Tag CheckApeTag(TagLib.File Mp3File){
            // You can add a true parameter to the GetTag function if the Mp3File doesn't already have a Mp3Tag.
            // By passing a the parameter TagTypes.Ape we ensure the type is of Ape.
            TagLib.Ape.Tag ApeTag = (TagLib.Ape.Tag)Mp3File.GetTag(TagLib.TagTypes.Ape, false);

            //Check ApeTag for content
            ApeTag = (TagLib.Ape.Tag)Mp3File.GetTag(TagLib.TagTypes.Ape, true);
            if (ApeTag.GetItem("CloudCoinContainer") != null) { // Check for an ApeTag 

                Console.WriteLine("ApeTag exists: " ); //Log 
                return ApeTag;
            }else{
                ApeTag = (TagLib.Ape.Tag)Mp3File.GetTag(TagLib.TagTypes.Ape, true);
                return ApeTag;
            }
        }

        public static void SetApeTagValue(TagLib.File Mp3File, TagLib.Ape.Tag ApeTag, string MyCloudCoin){
            // Get the APEv2 tag if it exists.
            if(ApeTag != null) {
                ApeTag.SetValue("CloudCoinContainer", MyCloudCoin);
            }
        }
        public static void ReturnCloudCoins(TagLib.Ape.Tag ApeTag){
            TagLib.Ape.Item item = ApeTag.GetItem("CloudCoinContainer");
            if (item != null) {
                    string CloudCoinAreaValues = item.ToString();
                    System.IO.File.WriteAllText("./CloudCoinPrintout.json", CloudCoinAreaValues); //Create a document containing Mp3 ByteFile (debugging).
                    Console.WriteLine("CloudCoinPrintout created at ./CloudCoinPrintout.json");
            }else{
                Console.WriteLine("no CloudCoins");
            }
        }
        public static void Savefile(TagLib.File Mp3File){
            Mp3File.Save();
        }





// Removed Code


                    
                    
                    
                    // using (FileStream fs = System.IO.File.Create("./CloudCoinPrintout.json"))
                    // {
                    //     Byte[] info = new UTF8Encoding(true).GetBytes(CloudCoinAreaValues);
                    //     // Add some information to the file.
                    //     fs.Write(info, 0, info.Length);
                    // }
                    // 
        //Method for adding an Ape frame.
        // public static void CreateAnApeFrame(TagLib.Ape.Tag ApeTag, string MyCloudCoin, Encoding FileEncoding)
        // {
        //     if(ApeTag.IsEmpty){
        //         TagLib.Ape.Tag CloudCoinApeTag = ApeTag; //Create a new Apev2 frame. 
        //         CloudCoinApeTag.SetValue("CloudCoinContainer", MyCloudCoin); //Insert the CloudCoin into the new frame
        //         Console.WriteLine("ApeFrame created: " + CloudCoinApeTag); //Log 
        //         Console.WriteLine("ApeFrame data: " + CloudCoinApeTag.GetItem("CloudCoinContainer")); //Log
        //     }

        // }

        //     //CheckEncoding
        //     //Used for debugging.
        //     public static Encoding CheckEncoding(string filename){
        //     var bom = new byte[4];
        //     using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
        //     {
        //         file.Read(bom, 0, 4);
        //     }
        //     // Analyze the BOM
        //     if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
        //     if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
        //     if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
        //     if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
        //     if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
        //     return Encoding.ASCII;
        // }


        //Used with Id3 tags
        // public static void ReadAFrame(TagLib.File Mp3File, TagLib.Id3v2.Tag Mp3Tag, Encoding FileEncoding)
        // {
        //     // Retreive the ID3TAG.
        //     // Note that the third parameter is false while reading.
        //     PrivateFrame PrivFrame = PrivateFrame.Get(Mp3Tag, "CloudCoins", false); //Read a ID3 frame of type private.
        //     //Retrieve the CloudCoin from the PRIV frame.
        //     //Note without a PRIV frame will result in error. needs error check.
        //     string CloudCoin = FileEncoding.GetString(PrivFrame.PrivateData.Data); //Store the CloudCoins in CloudCoin variable. 
        //     System.IO.File.WriteAllText("./CloudCoin.stack.txt", CloudCoin); //Create and copy the CloudCoins to a new file.
        //     Console.WriteLine("Reading PrivFrame: " + CloudCoin); //Log

        // }


        //Used to create private frames for id3 tags
        //         public static void CreateAnId3Frame(TagLib.File Mp3File, TagLib.Id3v2.Tag Mp3Tag, string MyCloudCoin, Encoding FileEncoding)
        // {
        //      // Note that the third parameter is true while writing.
        //     PrivateFrame PrivFram = PrivateFrame.Get(Mp3Tag, "CloudCoins", true); //Create a new ID3 frame of type private. 
        //     PrivFram.PrivateData = MyCloudCoin; //Insert the CloudCoin into the new frame
        //     Console.WriteLine("PrivFrame created: " + PrivFram); //Log 
        //     Console.WriteLine("PrivFrame data: " + PrivFram.PrivateData); //Log
        // }
    }
}
