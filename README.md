# MP3 console app
Contains code and standards for the insertion of CloudCoins into layer 3 mpeg files.


## Goal

#### To insert, save and retreive data from the MP3 file format.

## Summary of implementation

1) Get_MP3_file().
2) Get_CloudCoin().
3) Create(), a new MP3 frame of type ‘PRIV’ (private).
5) Read()  CloudCoin from MP3 header
6) Save()


### Overview of implementation.

    The current implementation is a console app built in VS-Code,
    it serves as a proof of concept for storing CloudCoins in the meta tags of MP3 files.
    
    
#### Get_MP3_file()

Retrieve file-path to .mp3, store it as a string.
```
string FilePath = "< path to .mp3 >";
```

Using the TagLib-sharp library, and FilePath to get the Id3 header.
```
TagLib.File Mp3File = TagLib.File.Create(FilePath);
```

#### Get_CloudCoin()

Retrieve file-path to CloudCoin, store file content as a string.
```
string MyCloudCoin = System.IO.File.ReadAllText("./text.txt", Encoding.ASCII);
```

Ensure file has the Id3v2 header.
Using the TagLib-sharp library and FilePath to get the Id3v2 tag type, Creates one if none are found.
```
TagLib.Id3v2.Tag Mp3Tag = (TagLib.Id3v2.Tag)Mp3File.GetTag(TagTypes.Id3v2);
```

#### Create()
Create a frame of type 'PRIV'(private), to encapsulate the data we are saving.

The method takes 3 arguments, 'MyCloudCoin' gets encapsulated by a newly created PRIV frame, then inserted into the ID3 tag and finnaly attached to the file.
```
CreateAFrame(Mp3File, Mp3Tag, MyCloudCoin);
```


#### Read()

The method takes 2 arguments, Mp3File and Mp3Tag, the method searches the file for the specified tag then prints the housed data to the console.
```
ReadAFrame(Mp3File, Mp3Tag)
```


![alt text](https://raw.githubusercontent.comCloudCoinConsortium/MP3_console/blob/master/Mp3Standard.png)



### References

Private frame [Link](http://id3.org/id3v2.4.0-frames)

    This frame is used to contain information from a software producer that its program uses and does not fit into the other frames. The frame consists of an 'Owner identifier' string and the binary data. The 'Owner identifier' is a null-terminated string with a URL [URL] containing an email address, or a link to a location where an email address can be found, that belongs to the organisation responsible for the frame. Questions regarding the frame should be sent to the indicated email address. The tag may contain more than one "PRIV" frame but only with different contents.

    <Header for 'Private frame', ID: "PRIV">
    Owner identifier      <text string> $00
    The private data      <binary data>

Id3v2 [Link](https://www.loc.gov/preservation/digital/formats/fdd/fdd000108.shtml)

    ID3 Metadata for MP3, Version 2
    Description: Structured data chunk containing descriptive metadata about the file to which it is generally pre-pended, virtually always an MP3_FF (MP3 sound file). Referred to by its creators as an "informal standard" for a "container format," ID3v2 permits the identification of title, artist, date, genre, and more. See link.







### Frame Header -32 bits
#### AAAAAAAA-AAABBCCD-EEEEFFGH-IIJJKLMM

Character | Name | Bits | Note
--- | --- | --- | ---
**A** | Frame Sync | 11 | All MP3 frames have a sync word marker containing all 1’s (FFF/FFE) at the beginning. May help find frame length by parsing Header Block after frame sync, seeking through the file by frame-length, if there is a sync word that means that the user is in-fact working with a MP3 file.
**B** | MPEG Audio Version ID | 2 | 2 bits following the sync word. Used to represent the MPEG version. MPEG v1 (11), MPEG v2 (10): Both 2 bit sets identify MP3.
**C** | Layer description | 2 | Describes one of four possible layers. L1 (11),  L2 (10), L3 (01), & Reserved(00). MP3 is a Layer 3 MPEG bits should be (01).
**D** | Protection bit | 1 |  1 designates the file is not protected. (with MP3 this is most often the value). 0 = Protected. Adds a 16 bit long checksum following the frame, to ensure data integrity, used in networks and storage devices to detect accidental changes to the data.
**E** |  Bit Rate Index | 1 | Four bytes used to describe the frames intended bit rate. Accepts values 0000 through 1110.
**F** | Sampling Rate Frequency | 2 | Samples of audio carried per second.
**G** | Padding bit | 1 | 0 = frame is not padded. 1 = frame is padded.
**H** | Private bit | 1 | Not being used.
**I** | Channel mode | 2 | To be updated. Will not utilize.
**J** | Mode extension | 2 | To be updated.
**K** | Copyright | 1 | Single bit designating the copyright status of the file.
**L** | Original | 1 | Marks the original copy of the file.
**M** | Emphasis | 2 | Rarely used.



