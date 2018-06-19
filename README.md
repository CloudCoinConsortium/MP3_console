# MP3 console app
Contains code and standards for the insertion of CloudCoins into layer 3 mpeg files.


## Goal

#### To insert, save and retreive data from the MP3 file format.

## Summary of implementation

1) Get_MP3_file().
`Opens a browse window allows user to select the correct file.`
2) Get_CloudCoin().
`Opens a browse window allows user to select` **`multiple`** `files`
3) CheckApeTag().
`Checks file for APE Tag, returns the tag if it exists, creates of if none exist.`
4) SetApeTagValue().
`Inserts the CloudCoins as a value in a tag item named *CloudCoinContainer*.`
5) ReturnCloudCoins().
`Searches the files meta data for the key item *CloudCoinContainer* creates a file 'CloudCoinPrintout.json' and writes the data to the file.`
6) Save()
`Save changes made to the original mp3 file.`


### Overview of implementation.

    The current implementation is a console app built in VS-Code, to be upgraded to a multi-platform application.
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

#### CheckApeTag()

Checks file for APE Tag, returns the tag if it exists, creates of if none exist.
```
ApeTag = Methods.CheckApeTag(Mp3File)
```

#### SetApeTagValue()

Inserts the CloudCoins as a value in a tag item named *CloudCoinContainer*.
```
SetApeTagValue(ApeTag, MyCloudCoin);
```


#### ReturnCloudCoins()

Searches the files meta data for the key item *CloudCoinContainer* creates a file 'CloudCoinPrintout.json' and writes the data to the file.
```
ReturnCloudCoins(ApeTag)
```

#### MP3 Standard
Standards can be found in the standards folder.
![MP3 Standard](./standards/Mp3_Footer.png?raw=true "MP3 Standard")


### References

Id3v2 [Link](https://www.loc.gov/preservation/digital/formats/fdd/fdd000108.shtml)

    ID3 Metadata for MP3, Version 2
    Description: Structured data chunk containing descriptive metadata about the file to which it is generally pre-pended, virtually always an MP3_FF (MP3 sound file). Referred to by its creators as an "informal standard" for a "container format," ID3v2 permits the identification of title, artist, date, genre, and more. See link.
