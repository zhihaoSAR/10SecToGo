using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "levelconfig", menuName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{

    public int MaxRound;
    public Enemy[] Enemies;
    public float[] InitProbability;
    public float MinProbability;
    public float[] DeltaProbability; // sólo [0] es cantidad y los demás son proporción
    public int MaxEnemieNumAtTime;
    public int InitEnemyNum;
    public int DeltaEnemies;
    public float SpawnTime;
}
