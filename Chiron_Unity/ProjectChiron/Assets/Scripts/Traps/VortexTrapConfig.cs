using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "config/Traps/VortexTrapConfig")]
public class VortexTrapConfig : ScriptableObject
{

    public float duration;
    public float pullSpeed;
    public float spitSpeed;
    public float spitThreshold;
    public float directionThreshold;

}
