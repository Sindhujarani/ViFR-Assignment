# ViFR-Assignment
ViFR Tech interview assignment
# ViFR Assignment
Unity 2022.3.10f1 LTS | Built-in Render Pipeline

## How to Play
- Move your mouse to look around the room
- Look at a cube to interact with it
- Find the correct cube!

## Controls
- Mouse - look around
- Escape - unlock cursor

---

## Part 1 - What I Built

A first person camera game where the player is inside a room
with three randomly coloured cubes. One cube is the correct
choice. The player interacts by looking at the cubes using
a raycast from the center of the camera - no clicking needed.

When the player looks at the correct cube:
- It turns white with a glow effect
- A success sound plays
- Correct appears on the screen
- The cube gets disabled so it cant be interacted with again

When the player looks at a wrong cube:
- It briefly flashes red
- Try Again appears on screen
- The cube stays active so the player can keep trying

---

## Part 2 - Architecture for 25 Interactable Objects

The problem with the original setup was that GazeController
was directly looking for CubeInteractable on whatever it hit.
This means if tomorrow we have a sphere or a coin it would
not work without changing GazeController.

So I created an IInteractable interface. Any object that
wants to work with the gaze system just needs to implement
this interface. This way:

- GazeController does not need to know what type of object
  it is looking at. It just looks for IInteractable
- If tomorrow we have a sphere or a coin or a door we just
  add IInteractable to it and it works straight away
- Zero changes needed in GazeController
- CubeManager now takes an array of prefabs instead of one
  single prefab. So 25 different objects can be added from
  the Inspector itself with no code changes needed

---

## Part 3 - CoinManager Issues

Here is the original code that was given:

public class CoinManager : MonoBehaviour
{
    public GameObject coinPrefab;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(coinPrefab);
        }
    }
}

I found these issues:

### Issue 1 - No spawn position
Instantiate(coinPrefab) does not have a position so every
coin spawns at 0,0,0 in the world. All coins pile up at
the same spot and you cannot even see them spawning.

Fix: give it a proper spawn position
Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity)

### Issue 2 - No null check
If someone forgets to drag the prefab into the Inspector
slot the game will crash the moment Space is pressed.
There is no check to handle this situation.

Fix: check if coinPrefab is assigned before using it

if (coinPrefab == null)
{
    Debug.LogError("coinPrefab is not assigned!");
    return;
}

### Issue 3 - No spawn limit
There is nothing stopping the player from holding Space
and spawning hundreds of coins. This will slow down the
game and eventually freeze it completely.

Fix: track how many coins are spawned and stop
after reaching a set limit

if (spawnedCount >= maxCoins) return;
spawnedCount++;

### Issue 4 - Public field
coinPrefab is public which means any other script in the
project can access and change it. It should be private
since only CoinManager needs to use it. This breaks
the idea of keeping things separate and controlled.

Fix: use SerializeField instead so it still shows in
Inspector but stays private to this script

[SerializeField] private GameObject coinPrefab;

### Issue 5 - Too much logic inside Update
Update its job is only to detect input not to handle
all the spawning logic as well. Putting everything
inside Update makes the code harder to read and harder
to fix or change later on.

Fix: move the spawning logic to its own method and
just call that method from Update

void Update()
{
    if(Input.GetKeyDown(KeyCode.Space))
        TrySpawnCoin();
}

private void TrySpawnCoin()
{
    // all spawning logic goes here
}

---

## Part 4 - My Decisions

### How I structured the project
I tried to give each script one job and one job only.

- GazeController - handles the raycast and detects
  what the player is looking at
- CubeInteractable - handles what happens to the cube
  when it is looked at
- CubeManager - handles spawning the objects
- UIManager - handles the text shown on screen
- AudioManager - handles the sound effects
- MouseLook - handles the camera mouse movement
- IInteractable - defines what any interactable object
  must be able to do

I kept them separate so if something breaks I know
exactly which script to look at. I also split folders
by type - Scripts, Prefabs, Audio, Materials, Scenes.
Nothing is dumped in the root Assets folder.

### If another developer joined tomorrow
Every script has a comment at the top explaining what
it does and why it exists. The method names are kept
simple so you can read the code like plain English.
For example OnGazed, PlaySuccess, ShowMessage - you
do not need extra explanation for these names.

The IInteractable interface also helps a lot here.
Any new developer can look at it and immediately
understand what an interactable object needs to do
to work with the gaze system.

### What I would improve with another day
- Add a GameManager to handle restarting the game
  without having to press Play again in the editor
- Add a countdown timer so the player has to find
  the correct cube before time runs out
- Add a particle effect when the correct cube is found
  to make it feel more satisfying
- Add a main menu and a game over screen
- Right now if you already found the correct cube
  and look away there is no clear visual to remind
  you which one it was. I would add a permanent
  glow or outline to make it obvious
- Add sound feedback for wrong guesses as well
  not just for the correct one
