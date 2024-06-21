# FAST Quiz Show

A FAST digital exhibit experience that allows for multiplayer 
participation, in-depth exploration of a topic, multiple-choice questions,
and true/false questions.

## System Requirements

### Runtime Requirements

* Microsoft Windows 10 or 11

### Build Requirements

* Unity Editor 2022.3.0 or later
* [FAST SDK](https://github.com/FAST-Digital-Exhibit-Design/FAST-SDK)

## Installation

1. Download the most recent version of the application from 
[Releases](https://github.com/FAST-Digital-Exhibit-Design/FAST-Exhibit-Templates/releases)
2. Extract the ZIP file where you want the application installed
3. Navigate to the **Application** folder
4. Open **FAST-Quiz-Show.exe** to run the application

### Key Mapping

| Key Name | Description | 
| :-- | :-- |
| **Alpha 1** | Player 1, select option 1 (required) |
| **Alpha 2** | Player 1, select option 2 (required) |
| **Alpha 3** | Player 1, select option 3 (required) |
| **Alpha 4** | Player 1, select option 4 (required) |
| **Alpha 5** | Player 2, select option 1 (required) |
| **Alpha 6** | Player 2, select option 2 (required) |
| **Alpha 7** | Player 2, select option 3 (required) |
| **Alpha 8** | Player 2, select option 4 (required) |
| **Keypad 1** | Player 3, select option 1 (required) |
| **Keypad 2** | Player 3, select option 2 (required) |
| **Keypad 3** | Player 3, select option 3 (required) |
| **Keypad 4** | Player 3, select option 4 (required) |
| **Keypad 5** | Player 4, select option 1 (required) |
| **Keypad 6** | Player 4, select option 2 (required) |
| **Keypad 7** | Player 4, select option 3 (required) |
| **Keypad 8** | Player 4, select option 4 (required) |
| **Right Arrow** | Go to the next screen state or question (debug, not robust) |

## Documentation

The sample experience included in a release package is preconfigured with 
the settings needed to run the interactive. The following subsections are 
for reference if you want to customize the settings, make a new skin, or 
modify the software.

### Changing Activity Settings

1. Navigate to the **Assets** folder
2. Open *SkinName*-**settings.xml**, such as **Mars-settings.xml**
3. For information about each setting, see 
[FAST SDK Reference Documentation](https://FAST-Digital-Exhibit-Design.github.io/FAST-SDK-Documentation/class_f_a_s_t_1_1_base_settings.html)

> [!WARNING]
> This activity doesn't have a language toggle and can only be configured 
using one (1) language.

### Changing Quiz Settings

1. Navigate to the **Assets** folder
2. Open *SkinName*-**Quiz-settings.xml**, such as **Mars-Quiz-settings.xml**
3. The `<Question>` settings define the quiz questions. The top-down 
order of each `<Question>` will determine the order they are used in the 
activity.
    - The `name` must match the same name used in asset files.
    - The `type` can be `text`, `image`, or `video`.
        - The `text` type will just show the question as text.
        - The `image` type will show an image with the question as text.
        - The `video` type will play the video, then show an image from 
        the video and the question as text.
3. The `<Option>` settings define the question answer options to choose 
from.
    - The `name` must match the same name used in asset files.
    - The `slot` determines which button will map to the option, where 
    the numbers can be [1, 4] corresponding to left-to-right position.
4. The `<Answer>` settings define which slots have the correct answer to 
the question. Use more than one `<Answer>` if there are multiple 
answers.
    - The `slot` specifies a slot with a correct answer.

### Adding a New Skin

1. Navigate to the **Assets** folder
2. Copy the folder containing the assets for the new skin
3. Navigate to the **root** folder where the application is installed
4. Open **config.xml** and change `<skin>Mars</skin>` to the name of 
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

The following media were used to create the Mars skin media assets 
included in a release package:

| Description | Media Type | Attribution/Credit | 
| --- | --- | --- |
| Mars Surface | Image | Stock media from iStock.com/24K-Production |
| Mars | Image | Stock media from iStock.com/Elen11 |
| Question 5 - Site A | Image | Media from NASA/JPL |
| Question 5 - Site B | Image | Media from NASA/JPL-Caltech/Univ. of Arizona |
| Question 5 - Site C | Image | Media from NASA/JPL-Caltech/Univ. of Arizona |
| Question 5 - Site D | Image | Media from NASA/JPL |
| Question 9 - Site A | Image | Media from NASA/JPL-Caltech |
| Question 9 - Site B | Image | Media from NASA/JPL-Caltech/MSSS |
| Question 9 - Site C | Image | Media from NASA/JPL |
| Question 9 - Site D | Image | Media from NASA/JPL-Caltech |

## End User License Agreement

By downloading or installing a release package (compiled executable of 
this software with assets), you agree to abide by the usage requirements 
of this software and the media assets that are included.