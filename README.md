# ReFF1

**ReFF1** is a reimplemented Final Fantasy I engine.

## About
Reimplementation as is being able to run the original games by extracting original data and running on this engine (with possiblity of modding, or creating a whole new game!).

For now only NES versions are supported:
- Final Fantasy I (Japan)
- Final Fantasy I (USA)
- Final Fantasy I & II (Japan) (Of course the Final Fantasy II in this package is not supported yet.)

## Getting started (on GNU/Linux)

After compiling run the executable with `mono ReFF1.exe <argument> <parameter>`.

Valid command usage can be seen by running `mono ReFF1.exe cmdlist`.

## "Extracting game data"

The engine doesn't do anything more right now than getting info about the version of the game and displaying the introduction text in the terminal.

You can try that out by running: `mono ReFF1.exe add <path_to_rom>`, where `<path_to_rom>` is a path to the .nes ROM file.

## Playing the game

You can't right now. Sorry :(
