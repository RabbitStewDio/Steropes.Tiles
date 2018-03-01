# Documentation

The Steropes.Tiles library helps you to separate your
game data from the game rendering. The library assumes
that your game data is organized in a 2D map where each
cell can be addressed with an X and Y coordinate pair.

The rendering system makes no further assumptions about
the implementation of the back-end data. You are therefore
free to generate data on the fly or implement your map
in any way you see fit.

## Grid Systems

* Plain Grid

The map is a regular grid, all tiles are rectangles.

      (0,0)  (1,0)  (2,0)
      (0,1)  (1,1)  (2,1)
      (0,2)  (1,2)  (2,2)


* Isometric Diamond

The map is a regular grid rotated by 45 degrees. 
Tiles are trapezoid shapes and rendered with half-tile 
offset on each line. 

                     (0,0)
                 (0,1)   (1,0)
             (0,2)   (1,1)   (2,0)
         (0,3)   (1,2)   (2,1)   (3,0)
             (1,3)   (2,2)   (3,1)
                 (2,3)   (3,2)   
                     (3,3)  

* Isometric Staggered

The map is a grid where every second column is offset
by half a tile in both x and y direction. This allows
the renderer to use isometric tiles while avoiding
the diamond shape of the underlying map. 

The navigation is irregular in this style so you have
to use the screen-space coordinate system to calculate
distances and to perform movements on the map.

      (0,0)     (1,0)     (2,0)     (3,0)
          (0,1)     (1,1)     (2,1)     (3,1)
      (0,2)     (1,2)     (2,2)     (3,2)
          (0,3)     (1,3)     (2,3)     (3,3)
      (0,4)     (1,4)     (2,4)     (3,4)
          (0,5)     (1,5)     (2,5)     (3,5)

Staggered maps can be used to avoid the typical diamond
look on isometric renderings. However, due to their
irregular nature, your back-end data structure will be 
tightly bound to your rendering choice as a naive 
manipulation of back-end data will result in invalid 
renderings.

## Further reading

* [Architecture](architecture.md)
* [Tile Matchers and Tile Registries](matchers.md)
* [Renderer Usage](usage.md)
