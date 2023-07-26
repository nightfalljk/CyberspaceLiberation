using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Level_Generation
{
    public class LevelDoor : Object
    {
        private readonly GameObject _gameObjectDoor;

        public LevelDoor(Vector3Int pos, Tilemap tileMap, TileBase door)
        {
            var ruleTileDoor = door;
            tileMap.SetTile(new Vector3Int(pos.x, pos.y, 0), ruleTileDoor);
            _gameObjectDoor = tileMap.GetInstantiatedObject(new Vector3Int(pos.x, pos.y, 0));
        }

        public LevelDoor(GameObject levelDoorObject)
        {
            _gameObjectDoor = levelDoorObject; 
        }

        public void OpenDoor()
        {
            //Open the Door
            if (_gameObjectDoor && LevelGenerator.showVaporWave && SceneManager.GetActiveScene().name != "Tutorial")
            {
                GameObject doorpivot = GetChildGameObject(_gameObjectDoor, "DoorPivot");
                doorpivot.transform.eulerAngles = new Vector3(doorpivot.transform.eulerAngles.x, 120f, doorpivot.transform.eulerAngles.z);
            }
            else if (_gameObjectDoor && !LevelGenerator.showVaporWave && SceneManager.GetActiveScene().name != "Tutorial")
            {
                GameObject door1 = GetChildGameObject(_gameObjectDoor, "elevatordoor1");
                GameObject door2 = GetChildGameObject(_gameObjectDoor, "elevatordoor2");
                //door1.transform.localPosition = new Vector3(0,0, -0.35f); 
                //door2.transform.localPosition = new Vector3(0,0, 0.35f); 
                door1.SetActive(false);
                door2.SetActive(false);
            }
            else if (_gameObjectDoor && LevelGenerator.showVaporWave && SceneManager.GetActiveScene().name == "Tutorial")
            {
                GameObject doorpivot = GetChildGameObject(_gameObjectDoor, "DoorPivot");
                doorpivot.transform.eulerAngles = new Vector3(doorpivot.transform.eulerAngles.x, -300f, doorpivot.transform.eulerAngles.z);
            }
        }

        public void CloseDoor()
        {
            //Close the Door
            if (_gameObjectDoor && LevelGenerator.showVaporWave)
            {
                GameObject doorpivot = GetChildGameObject(_gameObjectDoor, "DoorPivot");
                doorpivot.transform.eulerAngles = new Vector3(doorpivot.transform.eulerAngles.x, 0, doorpivot.transform.eulerAngles.z);
            }
            else if (_gameObjectDoor && !LevelGenerator.showVaporWave)
            {
                GameObject door1 = GetChildGameObject(_gameObjectDoor, "elevatordoor1");
                GameObject door2 = GetChildGameObject(_gameObjectDoor, "elevatordoor2");
                //door1.transform.localPosition = new Vector3(0,0, 0); 
                //door2.transform.localPosition = new Vector3(0,0, 0); 
                door1.SetActive(true);
                door2.SetActive(true);
            }
        }
        private GameObject GetChildGameObject(GameObject fromGameObject, string withName)
        {
            Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
            return null;
        }
    }
}
