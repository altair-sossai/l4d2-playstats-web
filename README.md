# Left 4 Dead 2 Match Stats and Ranking Site

This ASP.NET MVC application (net8.0) serves as a ranking and statistics site for Left 4 Dead 2 matches.

## Features

- **Ranking System:** Utilizes data provided by the "Left 4 Dead 2 Match Data API" to create a comprehensive ranking system. [API Repository](https://github.com/altair-sossai/l4d2-playstats-api)
  - The ranking system is reset every two months by default, though this can be configured as needed.
  - Previous rankings can be consulted.
- **User-Friendly Interface:** Displays rankings, latest matches, player rankings, and player comparisons in an intuitive and accessible format.
- **Brazilian Community:** Currently available to the Brazilian community at [https://l4d2.com.br/](https://l4d2.com.br/).
- **Steam API Integration:** Requires a Steam developer key (SteamApiKey) to fetch player information. Instructions for creating a Steam API key are provided below.
- **Data Dependency:** Directly relies on the "Left 4 Dead 2 Match Data API" to retrieve and display player data.
- **In-Game Integration:** Can be opened directly within the game when used in conjunction with the [l4d2_playstats_sync](https://github.com/altair-sossai/l4d2-zone-server/blob/master/addons/sourcemod/plugins/optional/l4d2_playstats_sync.smx) plugin. Configuration details for the plugin are provided below.
- **Connected Players Indicator:** A flag in the site header shows how many players are currently connected to the server.


## Requirements

### 1. Left 4 Dead 2 Match Data API
This API is responsible for receiving data generated by the `l4d2_playstats` plugin ([GitHub Link](https://github.com/SirPlease/L4D2-Competitive-Rework/blob/master/addons/sourcemod/plugins/optional/l4d2_playstats.smx)), storing it in an Azure Storage Account table, and exposing the data through a REST API. You can find detailed setup instructions for this API at the following link: [L4D2 PlayStats API Setup](https://github.com/altair-sossai/l4d2-playstats-api).

### 2. Valid Steam API Key
At times, the application needs to access player data such as profile pictures, names, profile URLs, and server details. A Steam API Key is required for integration with Steam. You can create your key through the following link: [Steam API Key](https://steamcommunity.com/dev/apikey).

1. Visit the [Steam API key registration page](https://steamcommunity.com/dev/apikey).
2. Log in with your Steam account.
3. Register a new API key by providing a domain name.
4. Copy the generated API key and set it in your project's configuration as `SteamApiKey`.

### 3. Hosting Environment
This application needs to be hosted on a Linux or Windows machine, or in a cloud environment capable of hosting .NET Core 8 applications. The publishing process depends on the chosen hosting environment, but generally, you will need to generate the binary for the project:

```bash
dotnet publish L4D2PlayStats.Web/L4D2PlayStats.Web/L4D2PlayStats.Web.csproj -c Release -o /path/to/output
```

### 4. Configuration
The application requires a configuration file named `appsettings.json` with the following structure:

```json
{
  "ServerId": "",
  "PlayStatsApiUrl": "",
  "SteamApiKey": "",
  "ServerIPs": ""
}
```

- **ServerId**: The server ID generated by the "Left 4 Dead 2 Match Data API."
- **PlayStatsApiUrl**: The endpoint of the "Left 4 Dead 2 Match Data API."
- **SteamApiKey**: Your Steam API Key.
- **ServerIPs**: IP:Port of your servers. It's recommended to use only one server for simplicity.

## Required Plugins

To ensure the ranking system works properly, the following plugins must be installed on your server:

### 1. l4d2_playstats_sync.smx
This plugin sends data generated by `l4d2_playstats` to the "Left 4 Dead 2 Match Data API." You need to configure the following CVARs:

```bash
confogl_addcvar sm_stats_writestats 1
confogl_addcvar playstats_url "URL_MATCH_DATA_API"
confogl_addcvar playstats_access_token "SERVER_ID:SECRET"
```

- **Binary**: [Download Link](https://github.com/altair-sossai/l4d2-zone-server/blob/master/addons/sourcemod/plugins/optional/l4d2_playstats_sync.smx)
- **Source Code**: [GitHub Link](https://github.com/altair-sossai/l4d2-zone-server/blob/master/addons/sourcemod/scripting/l4d2_playstats_sync.sp)

### 2. l4d2_ranking.smx
This plugin displays the ranking to players when they type `!ranking` in the chat. It also shows the ranking at the start of a new game. Configure the following CVAR:

```bash
confogl_addcvar ranking_url "URL_SITE_RANKING"
```

- **Binary**: [Download Link](https://github.com/altair-sossai/l4d2-zone-server/blob/master/addons/sourcemod/plugins/optional/l4d2_ranking.smx)
- **Source Code**: [GitHub Link](https://github.com/altair-sossai/l4d2-zone-server/blob/master/addons/sourcemod/scripting/l4d2_ranking.sp)

### 3. l4d2_show_patent_icon.smx (Optional)
This plugin displays the player's rank above their head at the start of a new game. Configure the following CVARs:

```bash
sm_cvar sv_downloadurl ""
sm_cvar sv_allowdownload 1
confogl_addcvar patent_icon_api_url ""
confogl_addcvar patent_icon_version 2
confogl_addcvar patent_icon_max_level 15
```

The `sv_downloadurl` should point to a FastDL URL. FastDL is essentially a public location where the Left 4 Dead 2 client can download necessary files, such as the rank icons in this case. Currently, I use a public container within an Azure Storage Account for this purpose, but you can configure it however you prefer. The rank icon files can be found here: [Patents Assets](https://github.com/altair-sossai/l4d2-zone-server/tree/master/assets/sprites).

- **Binary**: [Download Link](https://github.com/altair-sossai/l4d2-zone-server/blob/master/addons/sourcemod/plugins/optional/l4d2_show_patent_icon.smx)
- **Source Code**: [GitHub Link](https://github.com/altair-sossai/l4d2-zone-server/blob/master/addons/sourcemod/scripting/l4d2_show_patent_icon.sp)

## Example Screenshots

Here are some example screenshots from the site [https://l4d2.com.br/](https://l4d2.com.br/):

<p align="center">
Ranking Interface (dark mode)
</p>

![Ranking Interface](https://github.com/altair-sossai/l4d2-playstats-web/blob/main/screenshot/screenshot_01.jpg)

<p align="center">
Ranking Interface (light mode)
</p>

![Ranking Interface](https://github.com/altair-sossai/l4d2-playstats-web/blob/main/screenshot/screenshot_02.jpg)

<p align="center">
Last matches
</p>

![Last matches](https://github.com/altair-sossai/l4d2-playstats-web/blob/main/screenshot/screenshot_03.jpg)

<p align="center">
Player Comparison
</p>

![Player Comparison](https://github.com/altair-sossai/l4d2-playstats-web/blob/main/screenshot/screenshot_04.jpg)

<p align="center">
Player rank displayed in-game
</p>

![Player Comparison](https://github.com/altair-sossai/l4d2-playstats-web/blob/main/screenshot/screenshot_05.jpg)
