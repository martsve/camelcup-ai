# Camel Cup rules engine with bots

This repo contains a rules engine for playing Camel Cup, along with a few sample bots.

Feel free to fork the repo and create your own bots and modify CamelCup/Program.cs to fit your testing.

## Sample bots
Some sample players are included in the folder CamelCup/Bots/MartinBots
You can use them as examples for how to do basic stuff.
Feel free to utilize the static CamelHelper class

## Rules
- No multithreading
- No unsafe code
- Each bots name must be constant
- Each action the bot performs is limited to a benchmark time
- The total time each bot may use during a game is limited to a benchmark time

## Benchmark time
Run the program found in the TimeScaler-folder. It will first perform a warmup-step, then check how long it takes to run a benchmark test.
It will compare it to a benchmark, which is currently the time it takes to complete the task on my laptop in 1000ms. 
The program will give you a scaling factor which you can input in Delver.CamelCup.Program.timeScalingFactor to get your local max-compute-time.

## Tournament structure
- You can make as many bots as you like, but only one bot from each person can compete at a time.
-
