# Splash Engine

Splash Engine is a game engine built using Unity3D. It contains simulations of fire, water and firefighters using water to put out fire.

## Building the project

The project can be opened as a regular project in Unity 4.3. Then the scenes are located in `Assets/Scenes` can be run.

## Structure

The project consists of several scenes, scripts and prefabs located in subfolders in the `Assets`-folder. In the `Assets/Scripts`-folder some extensions have subfolders.

`Assets/Scripts/Octree` and `Assets/Scripts/OrthantNeighbourhoodGraph` contains unused and unfinished code that has only been include for reference.

## Fluid simulation

The fluid simulation is currently displayed using Unity's gizmos. Therefore gizmos need to be enabled to see them. The scene `FluidTest` is a good scene to start experimenting with the fluids in. Water particles can be fired in a sphere by left clicking and by holding the right mouse button a stream can be fired. It may be desirable to set `RemoveExpiredParticles` to `false` in `FluidBehaviour` while testing the fluid simulation. 

The code for this extension is located in `Assets/Scripts/Fluid`. There is a prefab available in the `Assets/Prefabs`-folder.

## Fluid graphics

## Fire

The visual representation is in the FirePrefab_no_smoke
In the scene called Mathias, the splitting of meshes can be tested.
To split the mesh of the quad in the scene press 'space', this can be done several times
To remove triangles within the mesh press 'return', this can only be done once before you get an error.
The error is a consequence of lists not currently being saved correctly which results in outofbounds.

## Fire propagation

The scene FireSpread shows the fire propagation.
In the script FireCell, Gizmos can be enabled or disabled by changing the value of the variable drawGizmos. Enabling gizmos will prevent the fire prefab from being instantiated.

## Firefighter AI

The code for this extension is located in `Assets/Scripts/WaypointGeneration` and in `Assets/Scripts/ArtificialIntel`. To be able to see the waypoint graph, gizmos are required. The scene `IntegratedAIWaterFire` is the only scene in the project with AI. The standard view is the one from the first-person player, but if you want you can activate the top-down camera by checking the checkbox on the game object called `TopDownCamera` in the hierarchy.

The firefighters are quite fast in extinguish the fire at the moment so I would recommend starting with the top-down approach if you are interested in how the firefighters move and are controlled by the AI.
