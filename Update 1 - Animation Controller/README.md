# Worldly Planes Update 1: Twin Stick Animation Controller & Movement

## Overview
Implemented an animation controller to handle playing and blending animations for "twin stick" style inputs (when the player's aim and movement are independant). The character model was made in MagicaVoxel and the animations were made in Blender.

## Video Demo
[Demo Link](https://youtu.be/bFWlolixR78)

## Script Rundown

### PlayerAim
- Gets the 2D vector that points from the screen center to the Player's mouse position on the screen
- Converts the 2D vector into a 3D vector to be used in World Space
- Sets the Player's RigidBody forward vector (transform.forward) to be the newley created vector so the Player aims in that direction

### PlayerMove
- Retrieves Player input from an Input Action and stores it in a 2D input vector
- If the vector is not a zero vector the Move() function is called to move the RigidBody
- Move() sets the velocity vector of the Player's RigidBody to the direction specified by the input vector scaled by a constant that represents movement speed

### AnimationController
- If the Player's RigidBody is in motion, Walk() is called to calculate the values to blend together the Player movement animations
- The dot product and cross product are both calculated between the players forward aiming direction (which was calculated in the PlayerAim script) and the Player's movement direction (which is retireved from the PlayerMove script)
- The 2D freeform directional blend tree for the movement animations has two parameters: ForwardWalk and SideWalk
- ForwardWalk represents the Y axis of the blend tree and SideWalk represents the X axis
- The ForwardWalk parameter for the blend tree is set to be the dot product calculated before
- If the dot product is positive, the player is moving forward relative to where they are aiming and if its negative they are moving backwards
- The SideWalk parameter value is set based on the sign of the y value of the cross product
- If the cross product y value between the aim vector and the movement vector is positive, then the player is looking left else they are looking right
- Both the Walk and SideWalk parameters values are changed over time depending on input so animations transition smootly between each other

## Pictures
![Blend Tree - Worldly Planes](https://github.com/user-attachments/assets/11706793-3303-440f-aadf-6a7b1f4ce7d6)
