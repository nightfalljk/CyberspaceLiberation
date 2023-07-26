using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UniRx.Triggers;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Level_Generation
{
    public class LevelGenerator : MonoBehaviour, IManager
    {
        [SerializeField] private GridLayout gridLayout;
        [SerializeField] private Tilemap tileMap;
        [SerializeField] private Tilemap wallTileMap;

        [SerializeField] private WallGenerator wallGenerator;
        [SerializeField] private LevelBaseTile levelFloorTile;

        [SerializeField] private TileBase ruleTileDoorVapor;
        [SerializeField] private TileBase ruleTileDoorOutrun;

        [SerializeField] private RuleTile ruleTilePoolVapor;
        [SerializeField] private RuleTile ruleTilePoolOutrun;

        [SerializeField] private List<GameObject> obstacleVW;
        [SerializeField] private List<GameObject> obstacleOR;

        [Header("Room Settings")] 
        [SerializeField] private int roomSizeX;
        [SerializeField] private int roomSizeY;
        [SerializeField] private bool cutOffSides;
        [SerializeField] private int numberOfEnemySpawnPoints = 10;
        [SerializeField] private int numberOfTrapSpawnPoints = 3;
        [SerializeField] private int numberOfObstacles;
        [SerializeField] private int numberOfRandomPoints = 20;
        
        [Header("Layout Creation Setting")] [SerializeField]
        private FillMode fillMode;

        [SerializeField] private int fillModeIterationNum;
        [Range(0.0f, 0.7f)] [SerializeField] private float fillModePercent;

        [Header("Rectangle Size Range")]
        [SerializeField] private int minRectangleSizeX;
        [SerializeField] private int maxRectangleSizeX;
        [SerializeField] private int minRectangleSizeY;
        [SerializeField] private int maxRectangleSizeY;
        
        [SerializeField] private bool doNotGenerate = true;

        private enum FillMode
        {
            Iterative,
            Percentage
        };

        //Array Index: 0 = Nothing, 1 = Floor, 2 = Wall, 3 = Player, 4 = Enemy, 5 = Door, Obstacles = 6, 7 = Pool Trap
        private int[,] _layout = new int[0, 0];
        private int _tileNumber;
        private Vector3 _playerSpawnPos = new Vector3();
        private List<Vector3> _enemySpawnPoints = new List<Vector3>();
        private List<Vector3> _trapSpawnPoints = new List<Vector3>();
        private List<GameObject> _instObstacles = new List<GameObject>();
        private List<Vector3> _randomPoints = new List<Vector3>();
        private Vector3 _bossSpawnPos = new Vector3(15,1,9);

        private int _seed = 0;
        private LevelDoor _levelDoor;
        private static Vector3 _topRightTile; 
        public static bool showVaporWave = true;
        

        // Start is called before the first frame Update
        private void Awake()
        {
            if (!tileMap) Debug.Log("No TileMap in LevelGenerator");
            if (!levelFloorTile) Debug.Log("No Tile in LevelGenerator");
            if (roomSizeX > maxRectangleSizeX && roomSizeY > maxRectangleSizeY) return;
            roomSizeX = maxRectangleSizeX + 1;
            roomSizeY = maxRectangleSizeY + 1;
            Debug.Log("RoomSize has been adjusted, due to invalid numbers: \n RoomSize X: " + roomSizeX + "\n RoomSize Y: " + roomSizeY);

            //_seed = Random.Range(0, 9999); 
            //Random.InitState(_seed);
            //GenerateLevel();
        }

        private void Update() 
        {
            /*if (((KeyControl) Keyboard.current["G"]).wasPressedThisFrame)
            {
                GenerateLevel();
            }
            if (((KeyControl) Keyboard.current["H"]).wasPressedThisFrame)
            {
                GenerateBossLevel();
            }*/
        }

        private void GenerateLevel()
        {
            _seed = Random.Range(0, 99999);
            Random.InitState(_seed);
            Debug.Log("Seed Number: " + _seed);
            
            showVaporWave = true;
            tileMap.RefreshAllTiles();
            wallTileMap.RefreshAllTiles();

            showVaporWave = (Random.value > 0.5f);

            //Do not touch the method order below! They are dependent on each other !
            GenerateLevelLayout();

            GenerateLevelWalls();

            GenerateLevelDoor();

            GeneratePlayerSpawnPoint();

            GenerateTrapSpawnPoint();

            DrawLevel();

            GenerateObstacles();

            GenerateEnemySpawnPoint();

            GenerateRandomPoints();

            TopRightTileImplementation(); 
        }

        private void GenerateLevel(int seed)
        {
            Random.InitState(_seed);
            showVaporWave = (Random.value > 0.5f);

            //Do not touch the method order below! They are dependent on each other !

            GenerateLevelLayout();

            GenerateLevelWalls();

            GenerateLevelDoor();

            GeneratePlayerSpawnPoint();

            GenerateTrapSpawnPoint();

            DrawLevel();

            GenerateObstacles();

            GenerateEnemySpawnPoint();

            GenerateRandomPoints();
            
            TopRightTileImplementation(); 
        }

        private void GenerateLevelLayout()
        {
            tileMap.ClearAllTiles();
            wallTileMap.ClearAllTiles();
            _tileNumber = 0;
            float y = roomSizeX * roomSizeY;
            _layout = new int[roomSizeX + 1, roomSizeY + 1];
            if (!cutOffSides)
            {
                y = (roomSizeX + maxRectangleSizeX) * (roomSizeY + maxRectangleSizeY);
                _layout = new int[roomSizeX + maxRectangleSizeX, roomSizeY + maxRectangleSizeY];
            }

            switch (fillMode)
            {
                case FillMode.Percentage:
                {
                    while (_tileNumber < (y * fillModePercent))
                    {
                        var xSize = Random.Range(minRectangleSizeX, maxRectangleSizeX);
                        var ySize = Random.Range(minRectangleSizeY, maxRectangleSizeY);
                        var recPos = RandomRectanglePlacement(xSize, ySize);

                        for (var i = 0; i < xSize * ySize; i++)
                        {
                            if (recPos[i].Equals(Vector3Int.zero) || (_layout[recPos[i].x, recPos[i].y] == 1)) continue;
                            _layout[recPos[i].x, recPos[i].y] = 1;
                            _tileNumber++;
                        }
                    }

                    break;
                }
                case FillMode.Iterative:
                {
                    for (var j = 0; j < fillModeIterationNum; j++)
                    {
                        var xSize = Random.Range(minRectangleSizeX, maxRectangleSizeX);
                        var ySize = Random.Range(minRectangleSizeY, maxRectangleSizeY);
                        var recPos = RandomRectanglePlacement(xSize, ySize);
                        for (var i = 0; i < xSize * ySize; i++)
                        {
                            if (recPos[i].Equals(Vector3Int.zero) || (_layout[recPos[i].x, recPos[i].y] == 1)) continue;
                            _layout[recPos[i].x, recPos[i].y] = 1;
                            _tileNumber++;

                        }
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!FloodFillTileAdjunction(_layout, _tileNumber)) GenerateLevelLayout();
        }

        private void DrawLevel()
        {
            for (var i = 0; i < _layout.GetLength(1); i++)
            {
                for (var j = 0; j < _layout.GetLength(0); j++)
                {
                    switch (_layout[j, i])
                    {
                        //Empty
                        case 0:
                            break;
                        //Floor
                        case 1:
                            tileMap.SetTile(new Vector3Int(j, i, 0), levelFloorTile);
                            break;
                        //Wall
                        case 2:
                            break;
                        //PlayerSpawnPos
                        case 3:
                            tileMap.SetTile(new Vector3Int(j, i, 0), levelFloorTile);
                            break;
                        //EnemySpawnPos
                        case 4:
                            tileMap.SetTile(new Vector3Int(j, i, 0), levelFloorTile);
                            break;
                        //Door
                        case 5:
                            tileMap.SetTile(new Vector3Int(j, i, 0), levelFloorTile);
                            wallTileMap.SetTile(new Vector3Int(j, i, 1), null);
                            _levelDoor = showVaporWave
                                ? new LevelDoor(new Vector3Int(j, i, 0), wallTileMap, ruleTileDoorVapor)
                                : new LevelDoor(new Vector3Int(j, i, 0), wallTileMap, ruleTileDoorOutrun);
                            //wallGenerator.PlaceWall(j,i, tileMap);
                            break;
                        //PoolTrap
                        case 7:
                            tileMap.SetTile(new Vector3Int(j, i, 0), showVaporWave ? ruleTilePoolVapor : ruleTilePoolOutrun);
                            break;
                    }
                }
            }
        }

        //Returns true if 2D Array input is adjunct, false if disjunctive
        //num is the number of total tiles in the array
        private bool FloodFillTileAdjunction(int[,] input, int num)
        {
            //Number of adjacent Tiles
            var x = 0;
            //Search for first Tile
            var firstTile = FirstTileInArray(input);
            //Copy of Input
            var inputCopy = new int[input.GetLength(0), input.GetLength(1)];

            System.Array.Copy(input, inputCopy, input.GetLength(0) * input.GetLength(1));
            //Assign x the number of adjacent tiles
            x = FloodFillTileAdjunctionImplementation(inputCopy, firstTile[0], firstTile[1]);
            return x == num;
        }

        //Returns number of adjacent tiles
        private int FloodFillTileAdjunctionImplementation(int[,] input, int x, int y)
        {
            var num = 0;
            if (cutOffSides)
            {
                if (x > roomSizeX || y > roomSizeY || y < 0 || x < 0)
                {
                    return 0;
                }
            }

            else if (!cutOffSides)
            {
                if (x > (roomSizeX + maxRectangleSizeX) || y > (roomSizeY + maxRectangleSizeY) || y < 0 || x < 0)
                {
                    return 0;
                }
            }

            switch (input[x, y])
            {
                case 0:
                    return 0;
                case 1:
                    input[x, y] = 0;
                    num += 1;
                    break;
            }

            num += FloodFillTileAdjunctionImplementation(input, x, y + 1);
            num += FloodFillTileAdjunctionImplementation(input, x, y - 1);
            num += FloodFillTileAdjunctionImplementation(input, x + 1, y);
            num += FloodFillTileAdjunctionImplementation(input, x - 1, y);
            return num;
        }

        //Places Walls into the the current Level Layout
        private void GenerateLevelWalls()
        {
            //CreateWall Array
            WallPlacementImplementation(_layout);
        }

        private void WallPlacementImplementation(int[,] input)
        {
            for (var i = 0; i < input.GetLength(1); i++)
            {
                for (var j = 0; j < input.GetLength(0); j++)
                {
                    switch (input[j, i])
                    {
                        case 0:
                            break;
                        case 1:
                            wallGenerator.PlaceWall(_layout, j, i);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //Generates Obstacles at every WallTile that has 3 adjacent floor tiles
        private void GenerateObstacles()
        {
            GenerateObstaclesImplementation(_layout);
        }

        private void GenerateObstaclesImplementation(int[,] input)
        {
            if (_instObstacles.Count > 0)
            {
                foreach (var ob in _instObstacles)
                {
                    Destroy(ob);
                }
            }

            List<GameObject> obstacles = new List<GameObject>();
            var z = 0;
            //Generate SpawnPoints
            while (z < numberOfObstacles)
            {
                var x = Random.Range(1, _layout.GetLength(0) - 1);
                var y = Random.Range(1, _layout.GetLength(1) - 1);
                var angle = Quaternion.Euler(0, Random.Range(0, 360), 0);

                var obstacle = showVaporWave ? obstacleVW[Random.Range(0, obstacleVW.Count - 1)] : obstacleOR[Random.Range(0, obstacleOR.Count - 1)];

                var pos = new Vector3Int(x, y, 1);
                var spawnPoint = gridLayout.CellToWorld(pos);

                if (!InstantiateGameObject(spawnPoint, obstacle, angle)) continue;
                foreach (var obs in obstacles)
                {
                    if (Vector3.Distance(obs.transform.position, spawnPoint) < 8f) goto Foo;
                }

                obstacles.Add(Instantiate(obstacle, spawnPoint, angle));
                obstacles[obstacles.Count - 1].GetComponent<Collider>().enabled = false;
                z++;

                Foo:
                continue;
            }

            _instObstacles = obstacles;

        }

        bool InstantiateGameObject(Vector3 pos, GameObject obj, Quaternion angle)
        {
            Physics.SyncTransforms();
            
            GameObject newOBJ = Instantiate(obj, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            var colliders = Physics.OverlapBox(pos, newOBJ.GetComponent<BoxCollider>().bounds.size / 2, angle);
            foreach (var col in colliders)
            {
                if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Obstacle"))
                {
                    Destroy(newOBJ);
                    return false;
                }
            }
            
            foreach (var col in colliders)
            {
                if (!col.gameObject.CompareTag("Floor")) continue;
                Destroy(newOBJ);
                return true;
            }
            
            Destroy(newOBJ);
            return false;
        }

        //Checks if the Tile at pos has 3 Adjacent FloorTiles
        private bool CheckIfObstacleTile(int[,] input, int[] pos)
        {
            var x = pos[0];
            var y = pos[1];

            return input[x + 1, y] == 1 && input[x - 1, y] == 1 && input[x, y + 1] == 1 && input[x, y - 1] == 1;
        }

        //Generates a Wall on the right middle part of the map, if not possible, a new position is being searched for in direction of -x until x/2 and then -y
        private void GenerateLevelDoor()
        {
            var doorPos = GenerateLevelDoorImplementation(_layout);
            _layout[doorPos[0], doorPos[1]] = 5;
        }

        private int[] GenerateLevelDoorImplementation(int[,] input)
        {
            for (var i = (input.GetLength(1) / 2); i > 0; i--)
            {
                for (var j = input.GetLength(0) - 1; j > 0; j--)
                {
                    if (input[j, i] != 1) continue;
                    if (input[j + 1, i] == 0 && input[j, i - 1] == 1 && input[j, i + 1] == 1)
                    {
                        return new int[] {j, i};
                    }
                }
            }

            return null;
        }

        //Takes a xSize and ySize for a Rectangle and returns a Vector3Int[] that contains the x/y/z coordinates that are needed to place all the rectangle tiles into the tilemap 
        //Position is in between X(0)/X(xSize) and Y(0)/Y(YSize) 
        private Vector3Int[] RandomRectanglePlacement(int xSize, int ySize)
        {
            var ret = new Vector3Int[xSize * ySize];

            var positionX = Random.Range(1, roomSizeX);
            var positionY = Random.Range(1, roomSizeY);
            var v = 0;

            for (var i = positionY; i < positionY + ySize; i++)
            {
                if (cutOffSides && (i > roomSizeY - 1)) break;
                if (i < 1) break;
                for (var j = positionX; j < positionX + xSize; j++)
                {
                    if (cutOffSides && (j > roomSizeX - 1)) break;
                    if (j < 1) break;
                    ret[v] = new Vector3Int(j, i, 0);
                    v++;
                }
            }

            return ret;
        }

        //Returns the first Cell with a Tile 
        //Return format: Array[xTile, yTile] 
        private int[] FirstTileInArray(int[,] input)
        {
            for (var i = input.GetLength(1) - 1; i > 0; i--)
            {
                for (var j = 0; j < input.GetLength(0); j++)
                {
                    if (input[j, i] != 1) continue;
                    return new int[2] {j, i};
                }
            }

            return null;
        }

        //Clears the level
        public void ResetLevel()
        {
            if (doNotGenerate) return;
            tileMap.ClearAllTiles();
            wallTileMap.ClearAllTiles();
            
            if (_instObstacles.Count > 0)
            {
                foreach (var ob in _instObstacles)
                {
                    Destroy(ob);
                }
            }
        }

        //Creates a new Level
        public void StartLevel()
        {
            GenerateLevel();
        }

        //Creates the level based on the seed
        public void StartLevel(int seed)
        {
            GenerateLevel(seed);
        }

        //Returns the enemy SpawnPoints
        public List<Vector3> GetEnemySpawnPoints()
        {
            return _enemySpawnPoints;
        }

        public List<Vector3> GetTrapSpawnPoints()
        {
            return _trapSpawnPoints;
        }

        //Generates the EnemySpawnPoints based on PoissonDiscSampling (Takes PlayerSpawnPosition and Other EnemySpawnPoints into consideration)
        private void GenerateEnemySpawnPoint()
        {
            var enemySpawns = new List<Vector3>();

            //Generate SpawnPoints
            //for(var i = 0; i < 50; i++)
            while (enemySpawns.Count() < numberOfEnemySpawnPoints)
            {
                var x = Random.Range(1, _layout.GetLength(0));
                var y = Random.Range(1, _layout.GetLength(1));
                var worldPos = gridLayout.CellToWorld(new Vector3Int(x, y, 1));
                worldPos = new Vector3(worldPos.x + 1f, worldPos.y, worldPos.z + 1f);

                //Check if in layout
                if (_layout[x, y] != 1) continue;
                //Check if far enough from player Spawn
                if (Vector3.Distance(worldPos, _playerSpawnPos) < 10f) continue;
                //Check if duplicate
                if (enemySpawns.Contains(worldPos)) continue;

                var colliders = Physics.OverlapBox(worldPos, new Vector3(2, 0, 2));
                foreach (var col in colliders)
                {
                    if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Obstacle"))
                    {
                        goto Foo;
                    }
                }

                foreach (var t in _trapSpawnPoints.Where(t => Vector3.Distance(t, worldPos) < 3f))
                {
                    goto Foo;
                }

                foreach (var t in enemySpawns.Where(t => Vector3.Distance(t, worldPos) < 3f))
                {
                    goto Foo;
                }

                //Add to List
                enemySpawns.Add(worldPos);
                //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cylinder), worldPos, new Quaternion(0,0,0,0));
                //Debug.Log("Positions X: " + x + " Y:" + y);

                Foo:
                continue;
            }

            _enemySpawnPoints = enemySpawns;
        }

        private void GenerateTrapSpawnPoint()
        {
            var trapSpawns = new List<Vector3>();
            var poolTraps = Random.Range(0, 2);

            //Generate SpawnPoints
            while (trapSpawns.Count() < numberOfTrapSpawnPoints)
            {
                var x = Random.Range(1, _layout.GetLength(0));
                var y = Random.Range(1, _layout.GetLength(1));
                var worldPos = gridLayout.CellToWorld(new Vector3Int(x, y, 1));
                worldPos = new Vector3(worldPos.x + 1f, worldPos.y, worldPos.z + 1f);
                //Check if in layout
                if (_layout[x, y] != 1) continue;
                //Check if far enough from player Spawn
                if (Vector3.Distance(worldPos, _playerSpawnPos) < 10f) continue;
                //Check if duplicate
                if (trapSpawns.Contains(worldPos)) continue;

                foreach (var t in trapSpawns.Where(t => Vector3.Distance(t, worldPos) < 3f))
                {
                    goto Foo;
                }

                if (poolTraps != 0)
                {
                    //Check if surrounding tiles are viable
                    if (_layout[x + 1, y] == 1 && _layout[x, y + 1] == 1 && _layout[x + 1, y + 1] == 1 && _layout[x + 1, y] == 1 && _layout[x + 2, y] == 1 &&
                        _layout[x + 2, y + 1] == 1 && _layout[x + 2, y + 2] == 1 && _layout[x + 1, y + 2] == 1 && _layout[x, y + 2] == 1 && _layout[x - 1, y] == 1 &&
                        _layout[x - 1, y + 1] == 1 && _layout[x - 1, y + 2] == 1 && _layout[x, y - 1] == 1 && _layout[x + 1, y - 1] == 1)
                    {
                        _layout[x, y] = 7;
                        _layout[x + 1, y] = 0;
                        _layout[x + 1, y + 1] = 0;
                        _layout[x, y + 1] = 0;
                        poolTraps--;
                        continue;
                    }

                    continue;
                }

                //Add to List
                trapSpawns.Add(worldPos);

                Foo:
                continue;
            }

            _trapSpawnPoints = trapSpawns;
        }

        //Returns PlayerSpawnPosition
        public Vector3 GetPlayerSpawnPoint()
        {
            return _playerSpawnPos;
        }

        //Generates PlayerSpawnPoint somewhere in the top right corner
        private void GeneratePlayerSpawnPoint()
        {
            var x = FirstTileInArray(_layout);
            var y = new Vector3Int(x[0] + 1, x[1] - 1, 1);
            _layout[x[0] + 1, x[1] - 1] = 3;
            _playerSpawnPos = gridLayout.CellToWorld(y);
            _playerSpawnPos = new Vector3(_playerSpawnPos.x + 1, _playerSpawnPos.y, _playerSpawnPos.z + 1);
            //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Capsule), _playerPos, new Quaternion(0, 0, 0, 0)); 
        }

        //Returns List of NavMeshSurfaces
        public List<NavMeshSurface> GetSurfaces()
        {
            NavMeshSurface[] nms = GetComponentsInParent<NavMeshSurface>();
            return new List<NavMeshSurface>(nms);
        }

        //Returns current Seed
        public int GetSeed()
        {
            return _seed;
        }

        public void OpenLevelDoor()
        {
            _levelDoor.OpenDoor();
        }

        public void CloseLevelDoor()
        {
            _levelDoor.CloseDoor();
        }

        public void OnApplicationQuit()
        {
            if (doNotGenerate) return; 
            showVaporWave = true;
            tileMap.RefreshAllTiles();
            wallTileMap.RefreshAllTiles();
        }

        public static Vector3 GetTopRightTilePosition()
        {
            return _topRightTile;
        }

        private void TopRightTileImplementation()
        {
            _topRightTile =  tileMap.CellToWorld(new Vector3Int(roomSizeX + maxRectangleSizeX, roomSizeY + maxRectangleSizeY, 0));
        }

        public List<Vector3> GetRandomPoints()
        {
            return _randomPoints;
        }

        private void GenerateRandomPoints()
        {
            var randomSpawns = new List<Vector3>();

            //Generate SpawnPoints
            while (randomSpawns.Count() < numberOfRandomPoints)
            {
                var x = Random.Range(1, _layout.GetLength(0));
                var y = Random.Range(1, _layout.GetLength(1));
                var worldPos = gridLayout.CellToWorld(new Vector3Int(x, y, 1));
                worldPos = new Vector3(worldPos.x + 1f, worldPos.y, worldPos.z + 1f);

                //Check if in layout
                if (_layout[x, y] != 1) continue;
                
                //Check if duplicate
                if (randomSpawns.Contains(worldPos)) continue;

                var colliders = Physics.OverlapBox(worldPos, new Vector3(1, 0, 1));
                foreach (var col in colliders)
                {
                    if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Obstacle"))
                    {
                        goto Foo;
                    }
                }

                foreach (var t in _trapSpawnPoints.Where(t => Vector3.Distance(t, worldPos) < 3f))
                {
                    goto Foo;
                }

                foreach (var t in randomSpawns.Where(t => Vector3.Distance(t, worldPos) < 1.5f))
                {
                    goto Foo;
                }

                //Add to List
                randomSpawns.Add(worldPos);
                //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cylinder), worldPos, new Quaternion(0,0,0,0));
                //Debug.Log("Positions X: " + x + " Y:" + y);

                Foo:
                continue;
            }

            _randomPoints = randomSpawns; 
        }

        public void GenerateBossLevel()
        {
            tileMap.ClearAllTiles();
            wallTileMap.ClearAllTiles();
            if (_instObstacles.Count > 0)
            {
                foreach (var ob in _instObstacles)
                {
                    Destroy(ob);
                }
            }
            _layout = new int[15,15];
            
            for (var i = 1; i < _layout.GetLength(1) -1; i++)
            {
                for (var j = 1; j < _layout.GetLength(0)-1; j++)
                {
                    _layout[j, i] = 1;
                    _tileNumber++; 
                }
            }

            GenerateLevelWalls();
            
            GenerateLevelDoor();
            
            GeneratePlayerSpawnPoint();

            DrawLevel();

            GenerateEnemySpawnPoint();

            GenerateRandomPoints();

            TopRightTileImplementation();
        }

        public Vector3 GetBossSpawn()
        {
            return _bossSpawnPos; 
        }
    }
}