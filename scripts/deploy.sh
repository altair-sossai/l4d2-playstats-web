#!/bin/bash

if screen -list | grep -q "l4d2-playstats-web"; then
  screen -S l4d2-playstats-web -X quit
fi

mkdir -p /home/l4d2-playstats-web
tar -xzf /home/devops/publish.tar.gz -C /home/l4d2-playstats-web
chmod +x /home/l4d2-playstats-web/L4D2PlayStats.Web
screen -d -m -S "l4d2-playstats-web" sh -c 'cd /home/l4d2-playstats-web && ./L4D2PlayStats.Web'
rm /home/devops/publish.tar.gz
