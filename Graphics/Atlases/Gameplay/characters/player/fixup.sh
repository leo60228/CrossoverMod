#!/bin/sh
cd "$(dirname "$0")"
parallel pngcrush -c 6 -force -ow -v -rem alla ::: ./*.png
