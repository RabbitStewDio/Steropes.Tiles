<p align="center">
    <a href="https://www.nuget.org/packages/Steropes.Tiles/" alt="NuGet Release">
        <img src="https://img.shields.io/nuget/vpre/Steropes.Tiles" />
    </a>
</p>

# Steropes Tiles

Steropes.Tiles is a fast tile rendering pipeline for grid and tile based 
games. 

This library helps you to keep your game's backend and simulation code neatly
separate from whatever frontend renderer you are using. It supports both flat 
tile sets as well as well as isometric tile sets out of the box. You can implement
your own TileRenderer class to select other tile representations, such as 
3D models or complex composite game objects. 

This library is agnostic to how your game data is organised, as long as the data 
can be queried in some sort of grid format (using X,Y coordinates).  

This library here provides a MonoGame renderer. An experimental Unity3D sprite 
renderer exists in the separate project [Steropes.Tiles.Unity2D](https://github.com/RabbitStewDio/Steropes.Tiles.Unity2D/). 

## Features

* support for various grid based rendering types
  * flat grid 
  * isometric diamond grid
  * isometric staggered grid
* rotated map rendering
* map navigation independent of underlying rendering type
* flexible rendering pipeline translates model data into rendering information
* non-opinionated and clean separation between your model and the graphical representation
* automatic generation of texture atlases
* highly optimized
* support for sprite layers and GUI integration

## Status

Steropes.Tiles is a functional library. However I am not quite happy with my 
original approach to read the map data, which feels complex and cumbersome.
Additionally, setting up the mapping between backend and rendering code is
currently overly complex too and probably should be more declarative in nature.

This should be addressed in the next major release along side making the 
Unity3D integration a bit more approachable.

## License

Steropes.Tiles is licensed under the MIT license. 

The Steropes.Tiles.Monogame.Demo module contains tilesets from the
FreeCiv project. Therefore the source code of this project is dual licensed
under the MIT license (that means you can use this code freely in
your own projects) and GPL 2.0 licensed (same license as FreeCiv).

All the graphics contained in that module are solely licensed under
the GPL 2.0 license to comply with the intent and spirit of the 
FreeCiv project.

## Rendering process

Steropes.Tiles assumes that maps are rendered in multiple layers. The 
rendering pipeline is divided into three parts:

1. The ***GridPlotter*** marks out the visible area based on the current 
   viewport settings. It then visits every visible tile position and hands 
   control to the PlotOperation.
   
   The GridPlotter ensures that only the absolute minimum of map coordinates
   gets visited regardless of the size of the underlying map.
   
2. The ***PlotOperation*** uses a preconfigured TileMatcher to query the 
   rendered tiles for the given map coordinate. A tile matcher is a function 
   in the general form of
    
       (x, y) => List of Tiles

   The library ships with a large number of standard matchers to lookup the 
   correct tile based on the map coordinate and its neighbours. Depending on 
   the matcher used this allows you to connect walls or roads or create 
   smooth shorelines from the raw map data given.
   
   This lookup is a cacheable operation that greatly increases performance 
   for maps that only change infrequently.
   
   The list of tiles returned by the matcher is handed over to a TileRenderer, 
   along with the computed viewport coordinates for the current tile.

3. The ***TileRenderer*** takes the tile, coordinates and possibly some 
   context information and displays the content on the screen. The renderer 
   translates the given tile into actual rendering operations of your 
   underlying graphics system or game engine. 
   
   The Steropes.Tiles.MonoGame library ships with a renderer for textured 
   tiles for XNA/MonoGame based games.

