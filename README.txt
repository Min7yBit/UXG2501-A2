**Overview**
You woke up in a prison cell and the only way out is through the cell door. It is locked with a 3-combination lock, your objective is to find hints lying around the cell for the combination.

**Intended Camera View** @zhiming

**Controls** 
- Button "1": First-Person Camera (FPP)	
- Button "2": Third-Person Camera (TPP)
- Button "3": Fixed Camera
- Button "W": Walk Front
- Button "A": Walk Left
- Button "S": Walk Right
- Button "D": Walk Back
- Button "B": Exit Item Interaction Camera
- Button "Spacebar": Jump
- Button "Tab": Scroll through inventory
- Button "Esc": Pause Menu
- Left Mouse Button: Interact

**Game Instructions**
Explore the cell and try to find hints that will eventually help you unlock the cell door. 
Objects that you will need to interact with includes: Tray, Spoon, Potato, Poster, Bed, Screw, Bed Leg, Coin, Crack on wall, Mirror, Cell Door, Cell Lock.

**Game Features** (Each feature should include the list of scripts used to implement the feature and how to trigger the feature.) (feel free to remove or add anything involving ur work)

Player Task
- Player Movement (WASD, Jump) (✅ Zhi Ming) :  : PlayerMovement.cs, controls the players movement with given input using rigidbody system and direction based on camera direction if using first person or third person camera. CharacterAnimationController.cs, controls animations for the player.

- Collision (Walls, Obstacles, Floors, Ceilings) (✅ Zhi Ming) : Colliders on player gameobject to handle collision with environmental objects.

- 3D camera (FPP, TPP, Fixed Angle) (✅ Zhi Ming) : CameraControl.cs, switches between the camera modes (FPP, TPP, Fixed angles)

- Interaction With Objects (✅ Zhi Ming) : PlayerInteract.cs, makes use of collision volume to detect interactable objects (Classes that inherits IInteractable) in the scene and chooses the nearest interactable object to be interacted with when LMB is pressed e.g bed, tray. Raycast is also used when using cursor on screen to interact with objects e.g screw, removeable bed leg.


Level Task
- Modelling level (Art assets) (✅ Javier) :3D models were self made.

- Implementation of level (✅ Javier) Implemented 3D models into unity space

- Win Condition (✅ Koon Loong): WinCondition.cs, input the correct combination from rotating the combination wheel to see the win

Inventory System Task 

- UI display of entire inventory (✅ Kai Ming): Just unity grid layout group

- Auto push items forward when theres empty space (e.g. after combining 2 items, auto delete raw materials, push down all items then add new combined item at the back) (✅ Kai Ming): Inventory.cs, have at least 2 items in inventory, use the first item on the most left to solve a puzzle in order to remove it, the remaining items in the inventory will automatically flush towards the left 

- Able to register items into the inventory (✅ Kai Ming): Inventory.cs, click on items that can be collected, they will automatically get stored in player inventory if possible

- Change selected items using tab (✅ Kai Ming): Inventory.cs, just press Tab in game, UI will indicate which slot is selected

Audio Task

- Find suitable SFX,BGM & enviroment (✅ Koon Loong)

- add SFX into whatever’s necessary (✅ Koon Loong)

- add BGM & environment (✅ Koon Loong)

UI
- Start Menu (✅ Kai Ming): ChangeScene.cs, just press the start button in game

- Pause/Resume Menu using Esc Key and have at least back to Start Menu option (✅ Kai Ming): UIManager.cs, just press ESC in game

- HUD showing how many more hints needed (✅ Kai Ming): UIManager.cs, just find hints to progress

- After Win go back to main menu option (✅ Kai Ming): UIManager.cs, win the game to show popup

Puzzles

- Puzzle 1: Coin unscrew bed frame to get rod (✅ Zhi Ming): Screw.cs, BedLeg.cs select coin and click on the screw, to remove the bed then you can remove the bed leg.

- Puzzle 2: Use bedleg to break mirror and collect mirror shard (✅ Koon Loong): Mirror.cs, Item.cs, select bedleg and click the mirror, you will automatically get a mirror shard

- Puzzle 3: Use mirror shard to view door lock on the outside and unlock it (✅ Koon Loong) Door.cs, select mirrorshard and click the door, you will be able to see the lock with this

- Puzzle 4: Scrap the broken wall with spoon to find hint 2 (✅ Javier) CrackWall.cs, when holding a spoon, click the crack in the wall 3 times to reveal hint.

- Puzzle 5: Find hint 3 behind a poster (✅ Javier) RevealPoster.cs, on left click, flips open to reveal hint.

- Puzzle 6: Rotate the potato around to find the engraved hint 1 (✅ Kai Ming): Potato.cs, click the potato to rotate

- Puzzle 7: Rotate the combination wheel to unlock the door (✅ Kai Ming): LockCombination.cs, click on the lock combination to see it rotate

**Audio**

Basic Audio Implementation 

Non-diegetic audio
- Background ambience/BGM plays from the main menu and continues into gameplay.
Managed through Audio Mixer (BGM + Ambience groups).
BGM made from scratch with Musescore and Mixcraft

Player movement sounds
- Footsteps play only while walking AND grounded.
- Jump SFX triggers when jumping.
- Landing SFX triggers when hitting the ground.
- Camera switch sounds
- SFX when switching into First-Person View (FPV).
- SFX when switching back to Third-Person View (TPP).

Item interaction sounds
- SFX when picking up general items.
- SFX when picking up the bed leg.
- SFX when picking up the potato.
- Potato zoom-in and rotation SFX during the potato puzzle.
- SFX when clicking/interacting with the tray.
- SFX when zooming into the bed view.
- Puzzle-related sounds
- Unscrewing SFX when removing the bed screw.
- Scraping/digging SFX when using the spoon on the wall.
- Wall crumble SFX when the wall breaks open.
- Glass shatter SFX when breaking the mirror.
- Paper tear SFX when flipping the poster.
- Clicking SFX when rotating the combination lock.

Game progression sounds
- Metal gate opening SFX when the door unlocks.
- Final victory SFX when all lock combinations are correct.

**Win Instructions**
Step 1: Click on the tray, click the spoon to collect it
Step 2: On the same tray, click the potato to view it. After its zoomed in, click on it to rotate around to find Hint 1. Press "B" or click anywhere else on the screen to exit zoom.
Step 3: Click the poster on the wall that is near the tray area in FPP to reveal Hint 3.
Step 4: Go towards the cell door area, collect the coin on the floor in FPP.
Step 5: In the same corner area, find the crack on the wall. Select the spoon using tab and click on the wall in FPP 3 times to reveal Hint 2.
Step 6: Go towards the bed and click it, click the front left corner again, select the coin and remove the rusty screw. Once its removed, you can collect the bed leg.
Step 7: Go towards the mirror and use the bed leg to break it in FPP, you will get a piece of mirror shard.
Step 8: Go towards the cell door, select the mirror and click the door. You will see a reflection of the 3-combination lock on the mirror shard, click on the mirror shard to access the final puzzle.
Step 9: Using the 3 hints you will get a combination of 369, use this number to align each combination wheel on the lock. First wheel is 3, second wheel is 6, last wheel is 9.
Step 10: You win.