=== HexGrid by Daniel Carrier ===

Email me at daniel_l_c@yahoo.com if you find a bug or you're having trouble or you make something cool. Especially the last one. I want to see what people make with this.

Game.unity is designed to be playable (if not particularly fun) out of the box. It's possible to make something prettier without delving into the code, but I would not recommend it. If you're afraid of touching code and have an interesting idea, contact me and we could work on the game together.

=== How to play: ===

Each player has two units, a sphere and a cube. The sphere has long range, low speed, low offense, and high HP. The cube has short range, high speed, high offense, and low HP. There is also a grey blob in the middle of the field. This is an obstacle, and cannot be moved through, but has no other effect on the game.

If you're using OneComputer.unity, first, you select one player or two player. If you choose one player, a simple AI takes the place of your opponent. If you use Networked.unity, I'm not actually entirely clear on how that works. I've only tested it on one computer with localhost. I assume you use the IP address you want to connect to if you're the client. Set one computer to the server, and login on the other as the client.

The game starts on Player 1's turn (the red player). Each turn, you can make each of your units move and attack. They can't move after they can attack, but beyond that it's flexible. You can move on unit, then make another unit move and attack, and then make your first unit attack. You can end your turn early by clicking "End Turn", and it will end automatically if there's nothing left you can do. The game ends when one player runs out of units.

The controls would take a while to explain, but they're meant to be intuitive. For the most part, just click on units you want to move, where you want to move them, and who to attack.

=== Making your own game: ===

This is not designed to be something ready to go. If you want a polished game, you'll have to do most of it yourself. It does not include any models. Just basic scripts. You could make a fairly simple game using it with little modification. If you want to do that, you can start with Game.unity and work from there, or you can use the prefabs in the prefab folder (which are just taken from that) as a basis.

--- Changing the initial position of the units: ---

Just move them around on the game board. Their initial position isn't stored anywhere. They just move to the center of the nearest hex at the start of the game. To illustrate this, the cubes are actually not quite in the right place initially, but they're fixed when the game starts. The same applies to the grey blobs. Just be careful to move the parent empties, not the mesh.

--- Adding more units: ---

UnitCube, UnitSphere, UnitCube2, and UnitSphere2 can be copied and pasted and will continue to work appropriately. Just make sure that they stay children of Units. More obstacles can be added by making them children of the blob.

--- Adding more units in-game: ---

Units can be spawned in using Object.Instantiate or Resources.Load and they will automatically be set to full health and added to the board. If you don't set their Coordinates after spawining, it will immediately be set to the nearest hex and they will be moved there. If you do, the unit will be moved to that hex. They can also be killed with Unit.die().

--- Adding more unit types: ---

The easy way is to just copy and paste one of the existing units and altering it. Each unit is made up of a parent, which is an empty running Unit.cs, and a child, which is a model with a collider. The collider is what lets you click on the unit, so if you want to change the model, be sure to give it an appropriate collider. To alter the attributes of the unit, just change the variables in the Unit.cs script. Their use is as follows:

Player:		0 for player 1, and 1 for player 2.
MaxHP:		Starting number of hitpoints.
Strength:	Average damage while attacking.
Variation:	The standard deviation of damage. The smaller this is, the less the damage will very. The damage is actually calculated using a negative binomial distribution and then adding one, which means that the minimum damage is one and there's no upper limit regardless of what value you set for Variation.
Speed:		How many hexes the unit can move in one turn.
Range:		How far the unit can attack. One means that they can only hit adjacent hexes, two means they can hit hexes adjacent to those, etc.

=== Editing the code: ===

If you've toyed around enough and you actually want to modify the code, here's overviews of each class. For more details, there's comments in the code itself, and generated documentation at Docs/index.html

For one computer, HexGrid.cs is the main script to control the game. I have the playing field running it, but you could just as well use an empty. It should be modified to change the controls of the game. For networks, Player.cs takes care of the game on each individual computer, but ServerGameController.cs takes care of sending commands between computers.

Instances of HexPosition.cs are used for positions on the grid and have various methods to make dealing with these positions easier. While it does have an underlying two-dimensional coordinate system, using it directly is not recommended. There are also static methods and variables centered around the playing field. They let you do things like assign variables to positions. This class is the most complete, and is intended to be modified little if at all. It is also the most heavily documented. You may need to read through the comments in that class to see what the public methods are, but there is no need to read the code itself. The documentation is in the code, though it's written in a way that lets the IDE show it to you when you use the methods.

Before the game, you can set variables to positions using DictSetter objects. The example scene has an object that sets the Obstacle flag as an example. You could also make terrain types by having a terrain key where you set the value to whatever terrain you want. Or you could use an integer to say how much movement it takes to cross it.

To use those variables when the game starts, you can use squre brackets to set and read variables from a given position. For example, say you have a posiiton called pos.

pos["terrain"] = "mountainous";         //Sets the terrain at pos to "mountainous"
if(pos["terrain"] == "mountainous") {}  //Checks if the terrain at pos is "mountainous"

You can also use flags using Position.flag(), position.remove(), and Position.isFlagged(). Really it's just creating a variable and setting it to null. For example:

pos.flag("obstacle");               //Sets an obstacle flag. Same as pos["flag"] = null;
if(pos.isFlagged["obstacle"]) {}    //Checks if there's an obstacle at pos. Same as pos.containsKey["obstacle"];
pos.remove("obstacle");             //Clears the flag. There's nothing it's the same as, but it can also be used to clear out variables you're not using anymore.

If you want to make terrain that slows units down, you can do that by altering Unit.getMoveTime(). This also lets you do things like have some units be able to fly over obstacles, but not others.

The recommended way to refer to hex positions is by using HexPosition.N, HexPosition.NE, HexPosition.SE, HexPosition.S, HexPosition.SW, and HexPosition.NW, to go one cell in each of those directions, and HexPosition.goN() etc. to move a certain amount in that direction. For example, if you want to find the hex that's one north and then one north east from the origin, you can use "new HexPosition().N.NE". If you want to go six north and then one north east, you could use "new HexPosition().goN(6).NE"

Unit.cs is a script that must be attached to units. All of the GameObjects running this script must be parented to a GameObject held by HexGrid.cs. It should be modified if you want to change how units in general work. If you want particular units that work differently, then it is recommended to make a new class that inherits from Unit.cs.

AStar.cs is a simple implementation of A*, a path-finding algorithm. It should be modified if you want to change which cells can be moved through, make some cells take longer to cross, etc. However, it is not currently set up to easily be modified, so if you need to do that, sorry. Currently, units can pass through any cell except ones containing units or marked "Obstacle". If you want to be specific about your obstacles, you can assign a value to the variable Obstacle at that HexPosition and it will work just as well.

NegativeBinomialDistribution.cs is a random number generator that generates numbers in a negative binomial distribution. I made it because I don't see any reason to stick with uniform distributions for damage and such in games. Negative binomial distribution is a discrete, positive, infinitely divisible distribution and I think that makes it perfect for games with discrete HP. If you want to use floats instead of integers for HP, I suggest gamma distribution, which is in attached SimpleRNG.cs (written by John D. Cook http://www.johndcook.com). If you feel the need to use a different distribution, replacing this class entirely would probably be better than modifying it.

AI.cs controls the AI. It's currently built to start attacking with its strongest unit, then move to weaker units, and to chase and attack enemies in order of their strength-to-HP ratio.

BlurryHexShader.shader and CrispHexShader.shader add hex grids to whatever material you use them on. I added BlurryHex.mat and CrispHex.mat as materials that use them, and BlurryHex.mat is used on the playing area by default. You can add your own images as textures and it will stil show the grid on top of it. I also have PixelHex.mat from earlier versions before I knew how shaders worked that just repeats a picture of a hexagon.