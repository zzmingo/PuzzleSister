#!/bin/bash
cd "$(dirname "$0")"
../SteamCmd/steamcmd +login zzm2q +run_app_build_http $(pwd)/scripts/app_build_710190.vdf +quit