# Renderer Usage and Setup

In Steropes.Tiles you normally do not need to deal with the 
internals of the rendering system beyond configuring the
system itself.

A configured renderer is encapsulated inside an instance of 
``IViewportControl``. The viewport controller allows you to
define the rendered area's size and position, control rotation
and map screen positions to map positions and vice versa.

## Set up phase

Steropes.Tiles assumes that your game logic is implemented in
an separate module that is independent of the rendering part
of the system. Ideally your game logic should not need to be 
aware of the actual rendering code. 

Your game will have an internal data model to represent the
game world (i.e. map and item data). The rendering subsystem
needs to query your game world to find out what to display on
the screen.

When you initialize your game rendering, we assume that the
game logic and its rules exist and can be queried. The rendering
system relies heavily on pre-computed and cached data to reduce
the amount of code that needs to run each frame. Therefore: If
you want to use caching rendering components, your game rules
(i.e. what elements can possibly exist in the game) should not 
change during the lifetime of the renderer.

To be clear: This does not mean your game world cannot change,
but it means that your game does not introduce new terrain or
item types during a game session. If your game is so dynamic
that it has to allow users to change the rules of the game, 
you will have to reinitialize the renderer after each change.

Steropes.Tiles assumes that the world is rendered in multiple
layers, where each layer adds additional information to the
screen. 

In a traditional 2D map based game (strategy game, RPGs etc.), 
you may have three or four basic layers:

 (1) Terrain - Grassland, Hills, Mountains, Water
 (2) Units - Soldiers, Tanks etc.
 (3) Unit state - Health bars, ownership icons, names
 (4) Fog of War - darken areas not visible from your own position.

Each layer in itself may in itself be rendered in multiple 
sub-layers. If you use traditional 2D rendering, like 
MonoGame's SpriteBatch or Unity's SpriteRenderer your layers need 
to be rendered in a particular order: to make the terrain appear
below the units, the terrain tile needs to be rendered first.

Within each layer, you also have to adhere to a specific rendering
order. Very often tile graphics are larger than the actual tile
base size (to emulate height or a pseudo-3D effect). Therefore
an tile that is placed towards the camera must be rendered after
any tile that is positioned more into the background.

Steropes.Tiles implements the painter algorithm and assumes
that tiles are rendered top to bottom and left to right. This
the correct strategy to make content on the top of the screen
appear behind content closer to the bottom of the screen.

A view-port in Steropes.Tiles contains the configured renderer
for each of the layers. The view-port is responsible for 
invoking all renderer in the correct order.

Each layer is represented by an ``IPlotOperation``. The plot
operation is called for each visible map coordinate and is
responsible for querying the game data to produce a series of 
tile that should be rendered for the given position.

These tiles are then handed over to a ``IRenderOperation``
instance that simply takes these tiles (and the pre-computed
screen coordinates) and displays them on the screen.

## Game Rendering Config

The first step in creating a new renderer is to set up
a ``GameRenderingConfig``. This class collects the common
initialization parameter and produces the necessary 
map navigators and plotters based on that data given.

The GameRenderingConfig only has one mandatory parameter,
the ``RenderType`` that will be used for rendering.
The render type specifies how to translate between map
and screen coordinate systems. 

* ``RenderType.Grid``  - a simple, 2D grid. This is usually
  used for top-down views or side-view games. Ultima 6 and 
  all RPG-Maker games use a grid mode, for instance.

* ``RenderType.IsoDiamon`` - a isometric display mode 
  where the view is rotated by (roughly) 45 degrees. 
  Games like Diablo (1 & 2) or Age of Empires used 
  an isometric view.

* ``RenderType.IsoStaggered`` - A isometric grid that 
  avoids the usual diamond shape of maps by offsetting
  every second column of map data by one. Civilisation 2
  used this mode. 

  Warning: With this mode the layout of your internal 
  map data will have to be aligned with this mode. 
  Using non-aligned map data in the background will 
  produce visible errors in your rendering.
  Choosing this map mode will affect how you have to 
  navigate on your map and this will affect path finding
  and object placement. _Only use this mode if you really
  really need it._

### Restricting maps: Wrapping and Limits

Most games use fixed size maps. Depending on your back end
implementation, your map implementation might require that
all access to the map is limited to a valid range of values.

To limit your map access, you can pass a separate Range 
structure for the X and Y axis as part of the constructor.
These limits will be applied to all map navigators so that
movement beyond the valid edges of the map is prevented.

Depending on your game you may want to simply block players
from moving over the edges of the available map space, or
you may want to create circular maps that loop back from
one edge to the opposite edge (to simulate cylindrical or
spherical terrain, like planets)

Use the ``wrapX`` and ``wrapY`` constructor parameter to 
define the wrap-around boundaries. If a player crosses 
over the lower or upper edge during map navigation, the
player will be teleported to the opposite edge instead.

You only need to specify either wrap or limit ranges.
If both are specified, wrap rules take precedence over 
limit rules.

## Game Rendering Factory

Once you have a ``GameRenderingConfig`` you can combine
this with the game rules, game graphics and game data
in a GameRenderingFactory to set up the GameRendering
instance.

``Game Data`` provides the current game state, including
your map data. ``Game Rules`` tell the system how to
interpret the data found in your Game Data. And last but
not least, the ``Game Graphics`` (usually in the form
of a tile set) define the textures that are used to render
each tile.

### Tile sets

A tile set is a named list of textures. The most primitive
tile set is simply a bunch of separate named texture files.
Textures for tiles are usually very small. (Common sizes 
are 128x128, 64,x64 or 32x32 pixels.)

In modern 3D systems, rendering separate textures has a 
large negative impact on the performance and frame rate of 
games. Therefore it is common to combine multiple smaller
textures into a texture atlas.

In a texture atlas, all textures are contained in a single
larger image and each texture contained in this atlas has
a defined region and name to tell the graphic system the
exact location of the tile in the larger file.

Steropes.Tiles comes with a minimalistic tile set format
that is documented in the [tiles.xsd Schema File](tiles.xsd).

For production use, you will probably prefer to use 
the TexturePacker or MonoGame.Extended texture atlas files 
directly. Steropes.UI can integrate with those and other
3rd party texture loading systems via a custom ``IContentLoader``
implementation. 

### Matcher

In Steropes.Tiles the system uses ``ITileMatcher`` 
implementations to define the various rules on how map data
is translated into one or more renderable tiles.

The [matcher documentation](matchers.md) contains a detailed
list of all available matchers. 

A matcher usually consists of 2 parts:

1. One or more Map Query functions provide data for a given
   map coordinate. Map Query functions are simple delegates
   in the form ``TResult Query<TResult>(int x, int y)``. 

   The most common query is a simple boolean query checking
   whether a given condition has been met (i.e. 
   ``bool IsRiver(x,y)`` to query whether a given map position
   contains a river).

2. Based on the map queries given, the matcher then computes
   a tile name. Matchers may combine results from multiple map
   queries into a tile. For instance the Cardinal matcher
   queries not only the rendered map coordinate but also queries
   information about the neighbouring tiles. This can be used
   to display different tile graphics based on the surroundings 
   of the current tile, which allows you to seamlessly connect 
   walls or rivers or to blend in tiles with their context.

   This computed tile context is then passed into a 
   ``ITileRegistry`` to lookup a matching tile by its name. 
   For a cardinal tile, the tile name might follow the pattern
   ``t.terrain.river_n0e1s0w1`` for a river that has connections
   to the east and west. 

Once one or more matching tiles have been found, the tile along
with any context information queried at that time will be passed
down the renderer.

### Renderer

A renderer is responsible for taking the selected tiles produced
by a tile selector, along with any (optional) context information,
to render the content using the underlying graphic system.
Steropes.Tiles ships with a MonoGame library that renders
Texture2D content via a SpriteBatch. 

Although passing textures is the most common use case, renderer 
are not restricted in the data they can receive. If you have
more complex models you need to render (including collections
of animations or 3D meshes), you can pass those around as well.
The tile selector code is agnostic to the content it produces.

Each renderer receives additional context information that can
be used to alter the rendering if needed. Context information
is produced when a tile is selected and is passed along with
the actual content itself.

#### Configuring Rendering Layers

Each rendering layer must be configured separately. A 
layer is represented via an PlotOperation instance. That
instance holds both the texture selector rules and the
renderer capable of displaying the textures that have 
been selected.

    PlotOperation := 
      TileSelector := 
         Map Query
         Tile Registry
      Renderer :=
         Device Information
         Viewport

When you have a working tile matcher that can produce
tiles for a map coordinate, you can create a plot
operation builder for your matcher via 



Steropes.Tiles provides two basic plot operation 
implementations for rendering.

* ``PlotOperation`` simply queries the tile selector for
  tiles to render and directly renders any received content.

       var plotBuilder = PlotOperations
                                .FromContext(gameRenderingConfig)
                                .Create(matcher);

* ``CachedPlotOperation`` wraps a normal plot operation
  into a cache so that repeated renderings of the same 
  content can avoid the cost of the tile selection.

  The cached plot operation contains a cache control
  API that can mark areas of map coordinates as dirty
  so that the rendered tiles are recomputed during the
  next draw operation.

  Use this type of operation if your underlying data does
  not change or does not change very frequently.

         var plotBuilder = PlotOperations
                                  .FromContext(gameRenderingConfig)
                                  .Create(matcher)
                                  .WithCache();

  Use ``IPlotOperation.Invalidate(MapCoordinate source, int radius)``
  or ``IPlotOperation.InvalidateAll()`` to force a re-computation
  of cached values.

Plot operations themselves are not aware of the viewport or 
other transient rendering information. The viewport contains
the offset mapping to make sure that a given focused tile is always
in view in your UI.

You control the viewport via an ``IViewportControl`` instance that
is the result of building all layers. All plot operations you create
must be connected to the viewport so that they render correctly.

    var viewportBuilder = plotBuilder.ForViewport();

Once you have added the viewport, you can build a usable plot operation

    var plotOperation = viewportBuilder.Build();

All that is left to do is to register the renderer with the newly
created plot operation. For MonoGame based games, Steropes.Tiles provides 
a renderer that is built around the SpriteBatch class.

    plotOperation.Renderer = new MonoGameRenderer(..);

All plot operations that have been created as part of this process
should be added into the ``GameRendering`` class. This class is a
MonoGame component that implements the ``IViewportControl`` interface.
It takes care of rendering the game content and allows you to control
position, size and rotation of the rendered game data.

#### Advanced Topic: Render Context Transformations

Sometimes it is necessary to extract higher level information from 
the game system to influence the rendering based on game state. 

As an example: 

In the demo RPG demo code, the moving characters are selected based
on the map coordinate they currently occupy. However to move smoothly
between positions, the renderer needs to query the actual fine-grained
position of the character that is rendered.

For this purpose, the tile matcher selects both the tile and as
context information returns the actual character object that represents
the non-player-character that is rendered. This allows the renderer
to compute a offset within the tile and to change the render position
for the underlying MonoGame renderer. 

You could use a similar technique to select different animations based
on internal game state so that game dependent state is kept separate
from the actual rendering code.