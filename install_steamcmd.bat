@echo off
mkdir SteamCmd
cd SteamCmd
..\cmdTools\curl\curl.exe "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip" -o steamcmd.zip
..\cmdTools\unzip.exe steamcmd.zip