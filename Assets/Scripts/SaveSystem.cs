using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSystem
{
    public float[] PlayerPosition;
    public float[] LastSpawnpoint;

    public SaveSystem(PlayerMovement Player, SpawnPoint Spawn) 
    {
        PlayerPosition = new float[2];
        PlayerPosition[0] = Player.transform.position.x;
        PlayerPosition[1] = Player.transform.position.y;

        LastSpawnpoint = new float[2];
        LastSpawnpoint[0] = Spawn.getCheckpoint().x;
        LastSpawnpoint[1] = Spawn.getCheckpoint().y;
    }
}
