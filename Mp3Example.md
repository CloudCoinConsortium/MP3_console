# MP3 sample file
```
Shows an example file with a CloudCoin added to the meta data.
```


#### Byte-file Readout
|Byte Numbers Starting with Zero  |  Bytes   | Name                                                            | File Bytes                     |  Mandatory Hex Value  |  Value Varies? |
| :------------------------------------  |:--------:|:---------------------                                         | :--------------------------|---------------------------| :----------------: |
|0 => 2                                            |     **3**      |  `ID3 Tag Identifier`                              | *Always the first byte.*  |  49 44 33                       |  **YES**               |
| 3 => 5A2                                       |    **~**       |   `ID3 Tag`                                                    | *Follows ID3 Tag ID.*     |  Too Large to show       | **YES**                |
| 5A3 => 2256                                 |   **~**        |  `MP3 Audio`                                                 | *First three bytes always FFF/FFE followed by audio data.*   |  FFF  | **NO**  |
|  2557 => 2572                              |    **8**      | `APE Tag Header`                                        | *Contains number, length and attributes of all tag items.*  |  41 50 45 54 41 47 45 58 (APETAGEX)  |  **NO**  |
| 2573 => 25A7                               |   **~**        |  `APE Tag Items`                                         | *Stores information in the file using 'Item Keys' and 'Item Values'.*  |  Too Large to show  |  **NO**   |
| 25A8 => 25B9                               |  **18**      | `CloudCoin APE Key`                                   | *An APE tag Item Key is a key for accessing values stored in the meta data.*  |  43 6C 6F 75 64 43 6F 69 6E 43 6F 6E 74 61 69 6E 65 72  (CloudCoinContainer)  |  **NO**   |
|  25D0 => 25D9                              |  **1**        |  `NN: Network Number`                                | *Network Numbers range: 1 Through 256*  |  22 6E 6E 22 3A 20 22 31 22 2C ("nn": "1",)  |  **YES**   |
| 25DD => 25ED                              |   **8**     |  `SN: Serial Number`                                  | *SN in Hex 6 bytes FF FFFF = 16,777,215*  |  22 73 6E 22 3A 20 22 31 36 37 37 37 32 31 35 22 2C  ("sn": "16777215",)  |  **YES**  |
| 25F1 => 29E8                                | **400**      | `AN: Authentication Number`                  | *Each AN is 16 bytes and 25 x 16 = 400 bytes*  | Too Large to show  |  **YES** |
| 29E9 => 29F7                                |  **15**      | `ED: Exp Date`                                             | *24 Months from zero month (September 2016).*  | 22 65 64 22 3A 20 22 39 2D 32 30 31 36 22 2C    ("ed": "9-2016",)   | **YES** |
|2A11 => 2A33                                 | **13**        | `POWN: Password Own`                                | * (unknown), 1 (pass),2 (no Response), E (error) or F (fail)*  | 22 70 6F 77 6E 22 3A 22 70 70 65 70 70 70 70 70 70 66 70 70 70 70 70 70 6E 70 70 70 75 70 70 70 70 22 2C ("pown":"ppeppppppfppppppnpppupppp",)  | **YES** |
| 2A37 => 2A41                                |  **~**          |  `AOID: Array of Idiosyncratic Data` | *Array of key value pairs created by the user for the user. Ok to leave blank but at least add [].  | Too Large to show  | **YES** |
|  2A46 => 2AE7                               |  **~**           | `APE Tag Items`                                           | *Stores information in the file using 'Item Keys' and 'Item Values'.*  | Too Large to show  | **NO** |

#### MP3 Standard
![Alt text](./Mp3Standard.png?raw=true "MP3 Standard")
