using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SceneController : MonoBehaviour
{
    //---------------Posible van ser quitado---------------------
    public Ataque a;

    //---------------Variables no van quitar----------------------------
    public Player player;
    
    public Transform[] SpawnPoints;
    public LevelConfig config;
    int round;
    int enemiesSpawned;
    int enemiesDead;
    float[] probability;
    int enemyNum;
    bool roundFinish = false;

    float time;

    void Start()
    {
        Modificador m = new Modificador();
        m.Ataque = a;// Ataque.DISTANTIA;

        player.InitPlayer(m);

        probability = new float[config.Enemies.Length];
        for(int i = 0;i< config.Enemies.Length;i++)
        {
            probability[i] = config.InitProbability[i];
        }
        enemyNum = config.InitEnemyNum;
        round = 1;
        enemiesDead = 0;
        enemiesSpawned = 0;
        time = 10;
        StartCoroutine("Spawn");

    }

    void Update()
    {
        time -= Time.deltaTime;
        if(time <= 0)
        {
            //Debug.Log("Game Over");
        }
    }


    IEnumerator Spawn()
    {
        while(!roundFinish)
        {
            int spawnNum = UnityEngine.Random.Range(0, Math.Min((enemyNum - enemiesSpawned), config.MaxEnemieNumAtTime));
            for(int i =0;i<spawnNum;i++)
            {
                Vector3 spawnPos =SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length-1)].position;
                float p = UnityEngine.Random.Range(1, 100);
                int enemyInd;
                for(enemyInd = 0; enemyInd < config.Enemies.Length; enemyInd++)
                {
                    if (p <= probability[enemyInd])
                    {
                        break;
                    }
                    p -= probability[enemyInd];
                }
                Enemy e = Instantiate<Enemy>(config.Enemies[enemyInd]);
                e.transform.position = spawnPos;
                e.controller = this;
                enemiesSpawned++;
            }
            yield return new WaitForSeconds(config.SpawnTime);
        }
    }
}
