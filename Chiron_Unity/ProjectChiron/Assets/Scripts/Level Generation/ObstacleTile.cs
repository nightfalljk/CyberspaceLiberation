using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level_Generation
{
    public class ObstacleTile : MonoBehaviour
    {
        private Collider col; 
        private bool collisionWithWall = false;

        private void Awake()
        {
            col = GetComponent<Collider>(); 
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.gameObject.CompareTag("Wall")) return;
            collisionWithWall = true;
        }

        public bool CheckCollision()
        {
            if (collisionWithWall)
            {
                Debug.Log("Collision with Wall " + this.transform.position);
                return true;
            }
            else
            {
                Debug.Log("No collision with Wall " + this.transform.position);
                col.enabled = false;
                Destroy(GetComponent<Rigidbody>()); 
                return false; 
            }
        }
    }    
}
