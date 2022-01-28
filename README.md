# Unity-Arcade-Collection
Collection of Arcade games made in Unity

Uses Actions for the [input manager](../../tree/main/Assets/Scripts/Input/InputBase.cs) to allow for easy expandability and allow for possible future touch-screen support. 

Once finished will have a arcade cabinet game select from an assortment of different games, from Pong, Space Invaders, to Tron!

Games currently in development are:

- [Space Invaders](../../tree/main/Assets/Games/SpaceInvaders)
- [Breakout](../../tree/main/Assets/Games/Breakout)

This project is aimed at strengthening my knowledge of Unity and to do so while implementing as many different design choices as possible.

In Space Invaders, I'm making use of pooling to keep the number of `Bullet` instances low, reusing them when they are freed up for use by the [SpaceInvadersBulletPool](../../tree/main/Assets/Games/SpaceInvaders/Scripts/SpaceInvadersBulletPool.cs). 

Breakout uses the in-built Unity 2D Physics to deal with collisions.
