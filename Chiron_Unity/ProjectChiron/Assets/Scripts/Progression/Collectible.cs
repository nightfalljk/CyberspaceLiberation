using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    [SerializeField] private int value;

    public void SetValue(int value)
    {
        this.value = value;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ProgressionSystem.currency += value;
            Destroy(gameObject);
        }
    }
}
