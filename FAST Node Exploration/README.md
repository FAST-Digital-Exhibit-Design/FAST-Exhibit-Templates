# FAST Node Exploration

A FAST digital exhibit experience to explore nodes in a network. A node 
is selected by turning a rotary dial. The content for the selected node 
is displayed in a panel and animated in a video. 

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
4. Open **FAST-Node-Exploration.exe** to run the application

### Key Mapping

| Key Name | Description | 
| :-- | :-- |
| **L** | Change the language (optional) |
| **Right Arrow** | Select the next node (debug) |
| **Left Arrow** | Select the previous node (debug) |

## Documentation

The sample experience included in a release package is preconfigured with 
the settings needed to run the interactive. The following subsections are 
for reference if you want to customize the settings, make a new skin, or 
modify the software.

### Changing Activity Settings

1. Navigate to the **Assets** folder
2. Open *SkinName*-**settings.xml**, such as **ClimateImpacts-settings.xml**
3. For information about each setting, see 
[FAST SDK Reference Documentation](https://FAST-Digital-Exhibit-Design.github.io/FAST-SDK-Documentation/class_f_a_s_t_1_1_base_settings.html)

### Changing Node Settings

1. Navigate to the **Assets** folder
2. Open *SkinName*-**Node-settings.xml**, such as **ClimateImpacts-Node-settings.xml**
3. The `<PixelNode>` settings define the nodes using `x`, `y` Cartesian 
coordinates. The `<GeoNode>` settings define the nodes using `latitude`, 
`longitude` geographic coordinates. You can mix and match the usage of 
these nodes, if you want. The top-down order of each `<PixelNode>` or 
`<GeoNode>` will determine the their selection order in the activity.
    - The `name` must match the same name used in asset files.

### Adding a New Skin

1. Navigate to the **Assets** folder
2. Copy the folder containing the assets for the new skin
3. Navigate to the **root** folder where the application is installed
4. Open **config.xml** and change `<skin>ClimateImpacts</skin>` to the 
name of your new skin

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

The following media were used to create the ClimateImpacts skin media assets 
included in a release package:

| Description | Media Type | Attribution/Credit | 
| --- | --- | --- |
| Fenway | Image | Stock media from iStock.com/steinphoto |
| Sermermiut | Image | Stock media from iStock.com/javi02 |
| Rio de Janeiro | Image | Stock media from iStock.com/zxvisual |
| San Felipe de Barajas | Image | Stock media from iStock.com/Dougall_Photography |
| Giza | Image | Stock media from iStock.com/WitR |
| Shibam | Image | Stock media from iStock.com/vanbeets |
| Great Barrier Reef | Image | Stock media from iStock.com/pniesen |
| Cliff Palace Mesa Verde | Image | Stock media from iStock.com/pniesen |
| Kennedy Space Center | Image | Stock media from iStock.com/1971yes |
| Odesa National Theater | Image | Stock media from iStock.com/vlad_karavaev |
| Forbidden City | Image | Stock media from iStock.com/zhaojiankang |
| Stonehenge | Image | Stock media from iStock.com/danaibe12 |
| Hoi An Ancient Town | Image | Stock media from iStock.com/efired |
| Waitangi New Zealand | Image | Stock media from iStock.com/vale_t |
| Kilwa Kisivani | Image | Stock media from iStock.com/Meinzahn |
| Wailing Wall | Image | Stock media from iStock.com/VanderWolf-Images |
| Makli Necroplis | Image | Stock media from iStock.com/Konstantin_Novakovic |
| Sankoré Madrasah | Image | Stock media from iStock.com/Iwanami_Photos |
| Buenos Aires | Image | Stock media from iStock.com/diegograndi |
| Machu Picchu | Image | Stock media from iStock.com/SL_Photography |
| Statue of Liberty | Image | Stock media from iStock.com/Tzido |
| Rice terraces | Image | Stock media from iStock.com/Aleksey Gavrikov |
| Kirstenbosch | Image | Stock media from iStock.com/Subodh Agnihotri |
| Medina of Marrakesh | Image | Stock media from iStock.com/soldat007 |
| Lake Louise Banff | Image | Stock media from iStock.com/Ron and Patty Thomas |
| Port Lockroy | Image | Stock media from iStock.com/Anton Rodionov |
| Venice | Image | Stock media from iStock.com/Andrew_Mayovskyy |
| Altai Petroglyphs | Image | Stock media from iStock.com/undefined undefined |
| Moai Rapa Nui | Image | Stock media from iStock.com/Mlenny |
| Zocalo (Mexico city) | Image | Stock media from iStock.com/Medvedkov |
| Mount Everest | Image | Stock media from iStock.com/DanielPrudek |
| Sri Siva Subramaniya temple | Image | Stock media from iStock.com/chameleonseye |
| Nikkō Tōshō-gū Shrine | Image | Media from commons.wikimedia.org/wiki/User:Fg2 |
| Uluru-Kata Tjuta National Park | Image | Media from commons.wikimedia.org/wiki/User:Leonard_G. |
| Winter Palace | Image | Media from commons.wikimedia.org/wiki/User:Yaropolk |
| World map | Image | Media from NOAA |
| Click sound, Interface Sounds | Audio | Media from Kenney |
| Climate Change data (temperature, precipitation, dryness) | Data | Media from NOAA |

## End User License Agreement

By downloading or installing a release package (compiled executable of 
this software with assets), you agree to abide by the usage requirements 
of this software and the media assets that are included.