using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level_Generation
{
    public class WallGenerator : MonoBehaviour
    {
        [SerializeField]private Tilemap tileMapWall;

        [SerializeField]private LevelBaseTile b; 
        [SerializeField]private LevelBaseTile t; 
        [SerializeField]private LevelBaseTile r; 
        [SerializeField]private LevelBaseTile l; 
    
        [SerializeField]private LevelBaseTile bl; 
        [SerializeField]private LevelBaseTile br;
        [SerializeField]private LevelBaseTile tl;
        [SerializeField]private LevelBaseTile tr;
        
        [SerializeField]private LevelBaseTile blc; 
        [SerializeField]private LevelBaseTile brc;
        [SerializeField]private LevelBaseTile tlc;
        [SerializeField]private LevelBaseTile trc;
        
        [SerializeField]private LevelBaseTile tlbrc;
        [SerializeField]private LevelBaseTile trblc;

        [SerializeField] private RuleTile decoTop;
        [SerializeField] private RuleTile decoTop2;
        [SerializeField] private RuleTile decoRight;
        [SerializeField] private RuleTile decoRight2;

        public void PlaceWall(int x, int y, Tilemap floorTile)
        {
            floorTile.SetTile(new Vector3Int(x,y,4), r);
        }

        public void PlaceWall(int[,] layout, int x, int y)
        {
  
            //Add Side Walls
            if (layout[x + 1, y] == 1 && layout[x - 1, y] == 0 && layout[x, y + 1] == 1 && layout[x, y - 1] == 1)
            {
                tileMapWall.SetTile(new Vector3Int(x,y,0), l);
            }
            else if (layout[x + 1, y] == 1 && layout[x - 1, y] == 1 && layout[x, y + 1] == 0 && layout[x, y - 1] == 1)
            {
                tileMapWall.SetTile(new Vector3Int(x,y,0), t);
                if (Random.Range(0, 10) < 2)
                {
                    if (Random.Range(0, 10) > 5)
                    {
                        tileMapWall.SetTile(new Vector3Int(x,y,1), decoTop);
                    }
                    else
                    {
                        tileMapWall.SetTile(new Vector3Int(x,y,1), decoTop2);
                    }
                }
            }
            else if (layout[x + 1, y] == 0 && layout[x - 1, y] == 1 && layout[x, y + 1] == 1 && layout[x, y - 1] == 1)
            {
                tileMapWall.SetTile(new Vector3Int(x,y,0), r);
                if (Random.Range(0, 10) < 2) 
                {
                    if (Random.Range(0, 10) > 5)
                    {
                        tileMapWall.SetTile(new Vector3Int(x,y,1), decoRight);
                    }
                    else
                    {
                        tileMapWall.SetTile(new Vector3Int(x,y,1), decoRight2);
                    }
                }
            }
            else if (layout[x + 1, y] == 1 && layout[x - 1, y] == 1 && layout[x, y + 1] == 1 && layout[x, y - 1] == 0)
            {
                tileMapWall.SetTile(new Vector3Int(x,y,0), b);
            }
            
            //AddCornerWalls
            else if (layout[x + 1, y] == 0 && layout[x, y + 1] == 0)
            {
                tileMapWall.SetTile(new Vector3Int(x,y,0), tr);
            }
            else if (layout[x - 1, y] == 0 && layout[x, y - 1] == 0)
            {
                tileMapWall.SetTile(new Vector3Int(x,y,0), bl);
            }
            else if (layout[x - 1, y] == 0 && layout[x, y + 1] == 0)
            {
                tileMapWall.SetTile(new Vector3Int(x,y,0), tl);
            }
            else if (layout[x + 1, y] == 0 && layout[x, y - 1] == 0)
            {
                tileMapWall.SetTile(new Vector3Int(x,y,0), br);
            }
            
            //Add Corers so that there are no dents
            if (layout[x - 1, y] == 1 && layout[x, y + 1] == 1 && layout[x - 1, y + 1] == 0)
            {
                tileMapWall.SetTile(new Vector3Int(x,y,1), tlc);
                if (layout[x + 1, y] == 1 && layout[x, y - 1] == 1 && layout[x + 1, y - 1] == 0)
                {
                    tileMapWall.SetTile(new Vector3Int(x,y,1), tlbrc);
                }
            }
            else if (layout[x + 1, y] == 1 && layout[x, y + 1] == 1 && layout[x + 1, y + 1] == 0)
            {
                tileMapWall.SetTile(new Vector3Int(x,y,1), trc);
                if (layout[x - 1, y] == 1 && layout[x, y - 1] == 1 && layout[x - 1, y - 1] == 0)
                {
                    tileMapWall.SetTile(new Vector3Int(x,y,1), trblc);
                }
            }
            else if (layout[x - 1, y] == 1 && layout[x, y - 1] == 1 && layout[x - 1, y - 1] == 0)
            {
                tileMapWall.SetTile(new Vector3Int(x,y,1), blc);
                if (layout[x + 1, y] == 1 && layout[x, y + 1] == 1 && layout[x + 1, y + 1] == 0)
                {
                    tileMapWall.SetTile(new Vector3Int(x, y, 1), trblc);
                }
            }
            else if (layout[x + 1, y] == 1 && layout[x, y - 1] == 1 && layout[x + 1, y - 1] == 0)
            {
                tileMapWall.SetTile(new Vector3Int(x,y,1), brc);
                if (layout[x - 1, y] == 1 && layout[x, y + 1] == 1 && layout[x - 1, y + 1] == 0)
                {
                    tileMapWall.SetTile(new Vector3Int(x, y, 1), tlbrc);
                }
            }
        }

        public void ResetMaterial()
        {
            
        }
    }
}
