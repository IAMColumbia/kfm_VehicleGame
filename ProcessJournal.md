## 03/28/2026
I landed on an idea pretty quickly this time. I knew I didn't want to do a racing game and ultimately decided to do something more exploratory. I liked the idea of using a bathysphere because it's far from the first thing that comes to mind when I think of a vehicle. (I also love the OG BioShock, so it's kind of an homage to that.) For now, I have an arena and a spherical game object for my vehicle. I added a script to the sphere to remove gravity and added drag so it feels like it's moving through water.

## 03/31/2026
I played around with the environmental rendering to create some fog & color the arena to make it look like deep ocean water. I also added a spotlight. (I'd love to eventually add functionality that allows the player to control the direction of the spotlight independently from the bathysphere's direction. Maybe by holding right mouse button?) I also found a couple of options for my bathysphere model.

During class, David A. had several suggestions that I'll be using:
- adding refraction effect/caustics to up the underwater feel
- using a simpler bathysphere model (to avoid animations)
- some way to show the player what's clickable

## 04/01/2026
After building my initial environment & adding creatures, I had my friends Aaron & Aiden playtest what I have so far. They both agreed that I should add a refraction effect to the underwater environment. Aiden says I should add text that tells the player to "click J to open your journal" on the scan pop-up.

Aaron suggested slowing down music (by around half) to avoid repetition & bring the pitch lower. They also said the "Unknown" creature should blink out once you click it. After some brainstorming together, I decided I want to have the Unknown creature follow behind the player & blink away whenever the player turns around & the spotlight hits it. (using raycast)

## 04/10/2026
I want to expand on the game eventually to make it a point-and-click adventure game, something like "you're on a mission to find out what happened to the last exploration crew".

## 04/16/2026
My friend Kate playtested remotely. They mentioned that the font was difficult to read. They also suggested adding book SFX to the journal and SFX like a quill scratching on parchment for the pop-up notification.

## 04/19/2026
My friend David M. playtested & suggested turning off camera movement while the journal's open. He also liked my idea for turning this into an investigation-focused adventure game. He recommended looking at [the demo for Locator](https://store.steampowered.com/app/2459030/Locator/) for inspiration.
