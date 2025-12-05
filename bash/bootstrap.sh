#!/bin/bash

sudo timedatectl set-timezone America/Sao_Paulo

sudo chmod +x /home/l4d2-playstats-web/L4D2PlayStats.Web
sudo screen -d -m -S "l4d2-playstats-web" sh -c 'cd /home/l4d2-playstats-web && ./L4D2PlayStats.Web'