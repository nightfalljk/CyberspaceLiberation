using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "config/Abilities/TeleportConfig")]
public class TeleportConfig : ScriptableObject
{

    public float cooldown;
    public float moveSpeed;
    public float channelTime;
    [Range(0, 1)] public float timescaleFactor;

    public void Init(TeleportConfig teleportConfig)
    {
        this.cooldown = teleportConfig.cooldown;
        this.channelTime = teleportConfig.channelTime;
        this.timescaleFactor = teleportConfig.timescaleFactor;
    }
}
