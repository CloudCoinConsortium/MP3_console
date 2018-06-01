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

Retrieve file-path to CloudCoin, store it as a string.
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
'PRIV': This frame is used to contain information from a software producer that a program may use, but does not fit into the other frames.

The method takes 3 arguments, 'MyCloudCoin' gets encapsulated by a newly created PRIV frame, then inserted into the ID3 tag and finnaly attached to the file.
```
CreateAFrame(Mp3File, Mp3Tag, MyCloudCoin);
```


#### Read()

The method takes 2 arguments, Mp3File and Mp3Tag, the method searches the file for the specified tag then prints the housed data to the console.
```
ReadAFrame(Mp3File, Mp3Tag)
```





