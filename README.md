# Issue

The CS scene is the only scene that is set up for "Leave Game" and "Switch Room" testing.

The problem lies in the player not being able to be instantiated when you leave the CS scene once and try to reenter it. Leaving the game is fine because the player leaves and reenters the Photon room. 

The logic behind switching rooms is that you stay in the same Photon room but try to swap to a different scene.
