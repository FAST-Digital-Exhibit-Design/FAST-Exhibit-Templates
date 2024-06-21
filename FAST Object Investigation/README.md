# FAST Object Investigation

A FAST digital exhibit experience that allows for exploration of real
objects or physical models, identification or classification, and
learning about a variety of items.

## System Requirements

### Runtime Requirements

* Microsoft Windows 10 or 11
* [FAST Computer Vision](https://github.com/FAST-Digital-Exhibit-Design/FAST-Computer-Vision)

### Build Requirements

* Unity Editor 2022.3.0 or later
* [FAST SDK](https://github.com/FAST-Digital-Exhibit-Design/FAST-SDK)

## Installation

1. Download the most recent version of the application from 
[Releases](https://github.com/FAST-Digital-Exhibit-Design/FAST-Exhibit-Templates/releases)
2. Extract the ZIP file where you want the application installed
3. Navigate to the **Application** folder
4. Open **FAST-Object-Investigation.exe** to run the application

### Key Mapping

| Key Name | Description | 
| :-- | :-- |
| **L** | Change the language (required for selecting multiple languages) |
| **R** | Replay/reanimate the current screen (debug) |
| **N** or **Right Arrow** | Go to the next teaser (debug) |
| **Alpha 0** | No object in the scanner (debug) |
| **Alpha 1** | Object 1 in the scanner (debug) |
| **Alpha 2** | Object 2 in the scanner (debug) |
| **Alpha 3** | Object 3 in the scanner (debug) |
| **Alpha 4** | Object 4 in the scanner (debug) |
| **Alpha 5** | Object 5 in the scanner (debug) |
| **Alpha 6** | Object 6 in the scanner (debug) |
| **Alpha 7** | Object 7 in the scanner (debug) |
| **Alpha 8** | Object 8 in the scanner (debug) |
| **G** | Toggle guide lines for projector, camera, table, controls, and scanner areas (debug) |
| **M** | Toggle the marker drawing (debug) |
| **T** | Toggle the marker tool drawing for trackable objects (debug) |

## Documentation

The sample experience included in a release package is preconfigured with 
the settings needed to run the interactive. The following subsections are 
for reference if you want to customize the settings, make a new skin, or 
modify the software.

### Changing Activity Settings

1. Navigate to the **Assets** folder
2. Open *SkinName*-**settings.xml**, such as **Geology-settings.xml**
3. For information about each setting, see 
[FAST SDK Reference Documentation](https://FAST-Digital-Exhibit-Design.github.io/FAST-SDK-Documentation/class_f_a_s_t_1_1_base_settings.html)

### Changing Tangible Object Settings

1. Navigate to the **Assets** folder
2. Open *SkinName*-**Object-settings.xml**, such as **Geology-Object-settings.xml**
3. The `<Object>` settings define the Tangible Objects. The top-down 
order of each `<Object>` will determine the order they are used in the 
activity as a teaser.
    - The `name` must match the same name used in asset files.
4. The `<MaximumNumberOfGuesses>` setting defines the number of times a 
teaser can be guessed incorrectly before it is skipped and the activity 
moves on to the next teaser.

### Changing Marker Tracking Settings

1. Navigate to the **Assets** folder
2. Open *SkinName*-**MarkerTracking-settings.xml**, such as **Geology-MarkerTracking-settings.xml**
3. The `<UdpConnectionId>` setting defines the name of the UDP 
connection to use. Don't change this value unless you are editing the 
source code because the application is compiled with a UDP connection 
set to this name. The default value should be 
`<UdpConnectionId>Computer Vision</UdpConnectionId>`.
4. The `<MaximumNumberOfMarkers>` setting defines the maximum number of 
number of unique IDs in the marker set that can possibly be detected. 
This value should match the *Dictionary Size* parameter in the 
[FAST Computer Vision](https://github.com/FAST-Digital-Exhibit-Design/FAST-Computer-Vision/blob/main/fast-computer-vision/doc/UserManual.md) 
application.

### Adding a New Skin

1. Navigate to the **Assets** folder
2. Copy the folder containing the assets for the new skin
3. Navigate to the **root** folder where the application is installed
4. Open **config.xml** and change `<skin>Geology</skin>` to the name of 
your new skin

## Contributions

This repo is only maintained with bug fixes and Pull Requests are not accepted 
at this time. If you'd like to contribute, please post questions and 
comments about using FAST Exhibit Templates to 
[Discussions](https://github.com/FAST-Digital-Exhibit-Design/FAST-Exhibit-Templates/discussions) 
and report bugs using [Issues](https://github.com/FAST-Digital-Exhibit-Design/FAST-Exhibit-Templates/issues).

## Notices

Copyright (C) 2024 Museum of Science, Boston
<https://www.mos.org/>

This software and media assets were developed through a grant to the 
Museum of Science, Boston from the Institute of Museum and Library 
Services under Award #MG-249646-OMS-21. For more information about 
this grant, see <https://www.imls.gov/grants/awarded/mg-249646-oms-21>.

### Software
This software is open source: you can redistribute it and/or modify
it under the terms of the MIT License.

This software is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
MIT License for more details.

You should have received a copy of the MIT License along with this 
software. If not, see <https://opensource.org/license/MIT>.

`SPDX-License-Identifier: MIT`

### Media Assets

The media assets (images, videos, audio, etc.) included in a release 
package (compiled executable of this software with assets) are EXCLUDED 
from this software's MIT License and are only permitted for use with 
this software. The media assets cannot be modified, resold, repurposed, 
or separated from the compiled executable of this software in any way.

The following media were used to create the Geology skin media assets 
included in a release package:

| Description | Media Type | Attribution/Credit | 
| --- | --- | --- |
| Volcanic Eruption, Scoria | Video | Stock media from iStock.com/Olivier Vandeginste  |
| Marble Quarry, Marble | Video | Stock media from iStock.com/Panksvatouny |
| Gneiss Shore, Gneiss | Image | Stock media from iStock.com/davejpr |
| Glacier, Conglomerate | Video | Stock media from iStock.com/Yiming Li |
| Shark, Fossil | Video | Stock media from iStock.com/VIDEODIVE |
| Volcano, Scoria | Audio | Stock media from SoundIdeasCom/Pond5 |
| Rock Shift, Granite | Audio | Stock media from SoundMorph/Pond5 |
| Whoosh, Marble | Audio | Stock media from Elysium/Pond5 |
| Ground Movement, Gneiss | Audio | Stock media from SoundIdeasCom/Pond5 |
| Squeeze, Gneiss| Audio | Stock media from TS_Sound/Pond5 |
| Arctic Wind, Conglomerate | Audio | Stock media from Soundexperience/Pond5 |
| Ocean Water, Fossil | Audio | Stock media from SoundIdeasCom/Pond5 |
| Approve/Correct | Audio | Stock media from agcnf_media/Pond5 |
| Diabase dikes (animation), Granite | Video | Media from National Park Service |
| Metamorphic rock formation, Gneiss | Video | Media from SciShow Kids |
| Glacier animation, Conglomerate | Video | Media from SciShow Kids |

## End User License Agreement

By downloading or installing a release package (compiled executable of 
this software with assets), you agree to abide by the usage requirements 
of this software and the media assets that are included.