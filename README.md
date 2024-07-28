# Left 4 Dead 2 Match Stats and Ranking Site

This ASP.NET MVC application (net8.0) serves as a ranking and statistics site for Left 4 Dead 2 matches.

## Features

- **Ranking System:** Utilizes data provided by the "Left 4 Dead 2 Match Data API" to create a comprehensive ranking system. [API Repository](https://github.com/altair-sossai/l4d2-playstats-api)
- **User-Friendly Interface:** Displays rankings, latest matches, player rankings, and player comparisons in an intuitive and accessible format.
- **Brazilian Community:** Currently available to the Brazilian community at [https://l4d2.com.br/](https://l4d2.com.br/).
- **Steam API Integration:** Requires a Steam developer key (SteamApiKey) to fetch player information. Instructions for creating a Steam API key are provided below.
- **Data Dependency:** Directly relies on the "Left 4 Dead 2 Match Data API" to retrieve and display player data.
- **In-Game Integration:** Can be opened directly within the game when used in conjunction with the [l4d2_playstats_sync](https://github.com/altair-sossai/l4d2-zone-server/blob/master/addons/sourcemod/plugins/optional/l4d2_playstats_sync.smx) plugin. Configuration details for the plugin are provided below.

## Steam API Key Setup

To use the Steam API, you will need to obtain a Steam API key. Follow these steps:

1. Visit the [Steam API key registration page](https://steamcommunity.com/dev/apikey).
2. Log in with your Steam account.
3. Register a new API key by providing a domain name.
4. Copy the generated API key and set it in your project's configuration as `SteamApiKey`.

## Plugin Configuration

To enable in-game integration with the l4d2_playstats_sync plugin, configure the following CVARs:

```ini
// [l4d2_playstats_sync.smx]
confogl_addcvar playstats_endpoint "https://l4d2-playstats-api.azurewebsites.net"
confogl_addcvar playstats_web_url "http://l4d2.com.br"
confogl_addcvar playstats_localhost_url "http://localhost:5000"
```

**Note:** Use HTTP for the `playstats_web_url` and `playstats_localhost_url` CVARs as the game does not support HTTPS. For external access, HTTPS is recommended.

## Example Screenshots

Here are some example screenshots from the site [https://l4d2.com.br/](https://l4d2.com.br/):

<p align="center">
Ranking Interface
</p>

![Ranking Interface](https://github.com/altair-sossai/l4d2-playstats-web/blob/main/screenshot/screenshot_01.jpg)

<p align="center">
Last matches
</p>

![Last matches](https://github.com/altair-sossai/l4d2-playstats-web/blob/main/screenshot/screenshot_02.jpg)

<p align="center">
Player Comparison
</p>

![Player Comparison](https://github.com/altair-sossai/l4d2-playstats-web/blob/main/screenshot/screenshot_03.jpg)
