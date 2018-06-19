# MP3 console app
Contains code and standards for the insertion of CloudCoins into layer 3 mpeg files.


## Goal

#### To insert, save and retreive data from the MP3 file format.

## Summary of implementation

1) selectMp3().
`Allows the user to target an Mp3 from the "mp3" folder. `
2) selectStack().
`Allows user to select the CloudCoin stack from a list of available stacks.`
3) stackToMp3().
`Inserts the CloudCoins as a value in an Ape tag item with a key of CloudCoinStack.`
4) stackFromMp3().
`Searches the file for the Ape's key item (cloudcoinstack), creates a new file with the stacks original name, then writes the data to the file.`
5) deleteFromMp3().
`Deletes any cloudcoinstacks associated with the mp3. The only function to save the files new state.`
6) saveMp3()
`Save changes made to the original mp3 file.`


### Overview of implementation.

    The current implementation is a console app built in VS-Code, to be upgraded to a multi-platform application.
    it serves as a proof of concept for storing CloudCoins in the meta tags of MP3 files.


#### selectMp3()

Retrieve file-path to .mp3, store it as a string.

Using the TagLib-sharp library, and FilePath to encapsulate the file with the correct information.
```
TagLib.File Mp3File = TagLib.File.Create(FilePath);
```

#### selectStack()

Retrieve file-path to CloudCoin, store file content as a string.
```
string MyCloudCoin = System.IO.File.ReadAllText(<CloudCoinStack-Path>, Encoding.ASCII);
```

#### stackToMp3()

Checks file for APE Tag, returns the tag if it exists, creates of if none exist.
```
ApeTag = Methods.CheckApeTag(Mp3File)
```
Inserts the CloudCoins as meta data in the Ape tag.

#### stackFromMp3()

Returns the CloudCoins from the mp3 file, saves the stack in "./Printouts" with its original name.
```
TagLib.Ape.Item CCS = ApeTag.GetItem("CloudCoinStack");
return CCS.ToString();
```


#### deleteFromMp3()

Removes all references to the cloudcoin stacks, from the mp3's meta data.
```
Methods.RemoveExistingStacks(ApeTag)
```

#### MP3 Standard
Standards can be found in the standards folder.
![MP3 Standard](./standards/Mp3_Footer.png)


### References

Id3v2 [Link](https://www.loc.gov/preservation/digital/formats/fdd/fdd000108.shtml)

    ID3 Metadata for MP3, Version 2
    Description: Structured data chunk containing descriptive metadata about the file to which it is generally pre-pended, virtually always an MP3_FF (MP3 sound file). Referred to by its creators as an "informal standard" for a "container format," ID3v2 permits the identification of title, artist, date, genre, and more. See link.
