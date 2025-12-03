#!/bin/bash

if screen -list | grep -q "l4d2-playstats-web"; then
  screen -S l4d2-playstats-web -X quit
fi

mkdir -p /home/devops
mkdir -p /home/l4d2-playstats-web/bash

tar -xzf /home/devops/bash.tar.gz -C /home/l4d2-playstats-web/bash
tar -xzf /home/devops/publish.tar.gz -C /home/l4d2-playstats-web

dos2unix /home/l4d2-playstats-web/bash/*.sh > /dev/null 2>&1

chmod +x /home/l4d2-playstats-web/bash/*.sh
chmod +x /home/l4d2-playstats-web/L4D2PlayStats.Web

screen -d -m -S "l4d2-playstats-web" sh -c 'cd /home/l4d2-playstats-web && ./L4D2PlayStats.Web'

rm /home/devops/publish.tar.gz
rm /home/devops/bash.tar.gz
