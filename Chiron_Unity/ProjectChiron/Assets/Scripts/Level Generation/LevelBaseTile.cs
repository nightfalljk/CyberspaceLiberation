using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level_Generation
{
    [CreateAssetMenu]
    public class LevelBaseTile : TileBase
    {
        [SerializeField] private Material outrun;
        [SerializeField] private Material vaporWave;
        [SerializeField] private GameObject gameObject;

        public override void GetTileData(Vector3Int cell, ITilemap tilemap, ref TileData tileData)
        {
            tileData.gameObject = gameObject;
            if (LevelGenerator.showVaporWave)
                ChangeMaterialInAllChildren(vaporWave, tileData.gameObject); 
            else if (!LevelGenerator.showVaporWave)
                ChangeMaterialInAllChildren(outrun, tileData.gameObject);
        }
        
        void ChangeMaterialInAllChildren(Material newMat, GameObject parentTile)
        {
            return;
            var children = parentTile.GetComponentsInChildren<Renderer>();
            foreach (var rend in children)
            {
                rend.material = newMat;
            }
        }
    }
}