using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(menuName = "config/AiConfig")]
public class AiConfig : ScriptableObject
{
    [Header("Difficulty")]
    //[Header("Number of how many level one run has")]
    //public int numberLevels = 4;
    [Header("How many difficulty token each level has")]
    public List<int> tokens;
    
    [Range(0f,1f)] [Header("more or better enemies?")]
    public float amountVsAbility = 0.5f;
    
    [Range(0f,1f)] [Header("easier or harder enemies?")]
    public float easyVsHardType = 0.5f;
    
    [Space(20)] [Header("Setup")] 
    [Header("Sort easier to harder")]
    public List<GameObject> enemyPrefabs;
    [Space(10)]
    public GameObject bossPrefab;
    public int amountArms = 4;
    public int amountMinionTokens = 60;
}
