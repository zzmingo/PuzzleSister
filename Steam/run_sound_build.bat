@echo off
set "steamDir=%cd%"
..\SteamCmd\steamcmd +login aaronrzh +run_app_build_http %steamDir%\scripts\app_build_749790.vdf +quit