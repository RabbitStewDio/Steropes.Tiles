namespace Steropes.Tiles.Demo.Core.GameData.Strategy
{
    public static class MapData
    {
        public static readonly string TerrainData = @"
      |::::::aaaaa::aaaaaa::::::::::::
      |::::::::::::::::aaaa::a:::a::::
      |::::::::::::::::::::::a::a:::::
      |::::::::::::::::::::::a::::::::
      |::::::::::::::::::::::a::::::::
      |:                            : 
      |  hhmmmmmhp   mm      p    ::  
      |ppphhmmmhhhs              ::   
      |pppphhmmhhpsss   gg       ::   
      |ffphhphhhppsss  gg    hp  ::   
      |fffgggggppggs         pg  ::   
      |pfgggffgggpp      g      :::   
      |  pppppfffpppp g   ppp  :::::  
      |    ppppfff       ppddp    ::: 
      |:             ggppddddd        
      |:     pppgg    ggppd ddd   :   
      |::                        ::   
      |:::::::::::::::::::::::::::::::
      |:::::::::::::::::::::::::::::::
".Strip();

        public static readonly string ResourceData = @"
      |      f                        
      |
      |                        f      
      |
      |
      |
      |
      |      c
      |                 p
      |p                      p
      |             
      |    
      |   bb
      |    b
      |                     o 
      |
      |
      |
      |
".Strip();

        public static readonly string RoadData = @"
      |
      |
      |
      |
      |
      |
      |
      |
      |    ++
      |  ++###+
      |  ++# ##+
      |   ++##+++
      |     +#++ +        + +
      |     ++#++          +
      |                 +++++
      |                  ++
      |
      |
      |
".Strip();

        public static readonly string ImprovementData = @"
      |
      |
      |
      |
      |
      |
      |
      |
      |     m m
      |  
      |  
      |         
      |    fff
      |    fff 
      |          
      |          
      |
      |
      |
".Strip();

        public static readonly string RiverData = @"
      |
      |
      |
      |
      |
      |
      |
      |         x
      |xxx    xxx
      |  xxx xx
      |       xxxxxx
      |
      |
      |
      |                   x
      |                   x
      |
      |
      |
      |
".Strip();
    }
}
