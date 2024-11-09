# Worldly Planes Update 2: Shooting Projectiles & Object Pooling

## Overview
Implemented a projectile shooting system that uses an object pool to reuse projectiles which is more performant than instantiating and destroying them. Created fireball projectiles that use a particle system for graphics and spherecast for hit detection instead of using colliders to be more performant. The attack animations were made in Blender and the fireball vfx was made using Gimp and Unity's particle system.

## Video Demo
[Demo Link](https://youtu.be/bFWlolixR78)

## Script Rundown

### ObjectPool
- Takes in a prefab of the GameObject that needs to be pooled
- When the Start function is called, a specified amount of objects are instantiated, deactivated, and then stored in a list
- The GetPooledObject function searches the list of objects to see if one is disabled, and if it is then returns that object, else it returns null

### PlayerShoot
- Within the Update function, every frame it checks if the player is holding down the attack button and changes the value of the isAttacking boolean to match
- If isAttacking is true, then currentChargeTime is increased until it equals chargeTime or if isAttacking turns false then currentChargeTime resets
- if currentChargeTime is equal or greater than chargeTime, then the Attack coroutine is started
- In the Attack coroutine a boolean called shotProjectile is turned true to indicate that a projectile is to be fired
- yield retrun null is then used to delay the rest of the execution by a frame so that other classes have a chance to react to the projectile being fired
- shotProjectile is then turned false and then the Shoot function in the ShootProjectile class is called to shoot a projectile out

### ShootProjectile
- Takes a reference to an instantiated ObjectPool object
- The Shoot function calls ObjectPool.GetPooledObject to get a reference to an object from the object pool
- If what was returned is not null (there is a projectile available to shoot), it sets its position to where the projectile should shoot from, rotates it, and activates it

### ProjectileMove
- When a projectile is activated, every frame the GameObject this class is attached to will move in a specified direction at a pace dictated by a specified projectile speed
- Every frame a spherecast is casted at the position of the moving GameObject to check for collisions
- If the spherecast detects a collider or the distance the projectile has traveled exceeds its max range, the projectile will stop
- If the projectile particle system and impact particle system are not null then the impact particle system plays and then the projectile is deactivated
- If the projectile particle system or impact particle are null then the projectile is just deactivated with no impact played

### HoldSpell
- Every frame, checks if the player is charging up an attack from isCharging in the PlayerShoot class
- if isCharging is true, it plays the charging particle system until the player stops charging or a projectile is shot

### AnimationController
- Added the Attack function which sets animator parameters that keep track of if the player is holding the attack button and if a projectile was shot 

## Pictures
![Transition Tree - Worldly Planes](https://github.com/user-attachments/assets/c7c94577-5d5d-4ca9-a379-7612bc099b73)
![FireBall](https://github.com/user-attachments/assets/41f45a69-58d0-47fd-8cad-20e702cf8b08)


