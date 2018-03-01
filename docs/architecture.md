## Recommended Architecture

The sample games provided here demonstrate a recommended 
architecture on how to expose back-end data to the rendering
system. 

### Layered Map Data

Here all game logic is encoded as game rules where each 
rule element has an unique ID. For example, all terrain
types have an internal integer ID that uniquely identifies
the type of terrain (mountain, forest, grass) in the map. 

Terrain improvements, like roads and walls, are encoded as
separate map layer, again with each element having an unique
ID. 

Using separate map layers separates data that has different 
access characteristics. For instance in most games the 
background terrain is static, there is only one terrain type 
per cell, and the terrain cannot be modified by in game 
actions. Therefore such data can be held in an shared 
read-only array. On the other hand, item or characters 
placed on the map changes frequently - and there may be 
multiple items in the same spot - which may require 
alternative storage modes.

Steropes.Tiles uses grid matchers to find the most suitable
graphic for any given map data. In the most simple way, this
grid matcher is a simple lookup from the map data to a 
tile texture. For example, if the terrain data states that
the cell at (0,0) is grassland, the matcher can query the 
game-rules data for the graphics tag and use that to return
the correct texture. 

### Graphic Tagging

When developing games, you usually have a separation between
the programmer and the graphics artist. It is generally a
good idea to keep the graphics somewhat separate from your
game logic. This allows your graphic artist to work 
independently on the graphics and to map them to game objects
when new graphics become available. Although you could simply
tie all graphics to the game rules directly, it is better to
keep both slightly separate. To separate the graphic 
definitions from game rules add a 'GraphicsTag' element
to your game rules. This way, multiple game rule elements can 
use the same graphic if needed, and renaming or changing 
game rule elements can maintain the existing graphic references.

The map data itself holds a reference to the game rule
it represents. For terrain data, you could use an array 
of ``TerrainRule`` instances or use an array of the unique
ID of that terrain rule. 

In the samples, I choose to use a positive integer ID for
each rule element. This allows me to use an array as 
lookup-table to map rule data into textures in the fastest
possible way.

    byte[,] map;
    GameRules rules;

    // Map(x,y): int -> GameRule -> GraphicTag -> Texture

    var terrainId = map[x,y];
    var gameRule = rules.TerrainRules[terrainId];
    var tag = gameRule.GraphicTag;
    var texture = tileRegistry.Lookup(tag);

Given that the relationship between GameRule ID (the ``int``) and
the texture is not changing once the system has loaded all
game data, Steropes.Tiles can pre-compute most of the
resolver chain.

The lookup above can be wrapped into a function

    Texture LookupTextureForTerrain(byte terrainId) {
        var gameRule = rules.TerrainRules[terrainId];
        var tag = gameRule.GraphicTag;
        return tileRegistry.Lookup(tag);
    }

which depends only on the byte-parameter as input. Thus 
it can be trivially replaced by a precomputed or cached lookup.

    Texture[] cache = new Texture[255]; // or possible max of terrain id

    Texture LookupTextureForTerrain(byte terrainId) {
        var cached = cache[terrainId];
        if (cached == null) 
        {
          var gameRule = rules.TerrainRules[terrainId];
          var tag = gameRule.GraphicTag;
          cached = tileRegistry.Lookup(tag);
          cache[terrainId] = cached;
        }
        return cached;
    }

The supplied tile matchers allow you to perform similar 
operations with little effort.

