# The Camel Cup AI Cup

To participate in the Camel Cup AI Cup you have to create your own AI to control a bot. A few simple bots are already provided for you as examples so there is not much to it!

## Sample bots
Some sample bots are included in the folder CamelCup.Player/Bots
You can use them as examples for how to do basic stuff.
Feel free to utilize the static CamelHelper class, which can help you brute force camel movement outcomes.

## Bot interface
The interface each bot must follow is included in the CamelCup.External folder, under ExternalModels/ICamelCupPlayer.cs.
Each bot must be assigned this interface. Each bot is required to have a parameterless constructor (not even default parameters)!
Give the bot a unique namespace and name.

To create the final bot, modify the CamelCup.Player/MyBot.cs file. After compiling the solution with your final bot, rename CamelCup.Player.dll to <YourName>.dll and provide it to the tournament organizer! Remember that the CamelCup.Player-project should only include 1 bot, so that the user of the final DLL-file know which one to use.

## Running a test tournament
Run server.bat and open http://localhost:5000. From there you can start a new game-runner, add the bots you want to compete with, and have them battle it out!

## Rules
- No multithreading
- No unsafe code / reflection / etc 
- Each bots name must be unique and constant. 
- Each action the bot performs is limited to a benchmark time
- The total time each bot may use during a game is limited to a benchmark time

## Benchmark time
Run the program found in the TimeScaler-folder. It will first perform a warmup-step, then check how long it takes to run a benchmark test.
It will compare it to a benchmark, which is currently the time it takes to complete the task on my laptop in 1000ms. 
The program will give you a scaling factor which you can input in Delver.CamelCup.Program.timeScalingFactor to get your local max-compute-time.

## Tournament structure/rules
- The only thing that matter each game is which player(s) has the most money. The amount doesn't matter; only who wins. Each win gets the bot 1 point.
- Each game is played with 4, 3 and 2 players
- Each round is any combinations of 4, 3 og 2 players playing ~1000 games (+-, limited by available time).
- There will first be preliminary rounds where all combinations of 4 players compete. The bottom half of the bots will be elliminated. Preliminary rounds continue until 4 bots remain.
- The last 4 bots will play 3 total rounds. The bot with the lowest score gets elliminated each round. 
