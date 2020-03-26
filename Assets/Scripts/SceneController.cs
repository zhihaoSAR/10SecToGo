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

    public float time;

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
            //Debug.Log(time);
            player.death();
        }
    }
    public void EnemyDead(float increase)
    {
        time += increase;
        if (++enemiesDead == enemyNum)
        {
            if(++round > config.MaxRound)
            {
                Debug.Log("Win");
            }
            else
            {
                Debug.Log("Ronda: " + round);
                NextRound();
            }
        }

    }
    void NextRound()
    {
        time = 10;
        enemiesDead = 0;
        enemiesSpawned = 0;
        time = 10;
        enemyNum += config.DeltaEnemies;
        float cantidad = probability[0] - config.DeltaProbability[0] < config.MinProbability ? probability[0] - config.MinProbability : config.DeltaProbability[0];
        probability[0] -= cantidad;
        for (int i = 1; i< probability.Length;i++)
        {
            probability[i] += cantidad * config.DeltaProbability[i];
        }
        StartCoroutine("Spawn");
        
    }

    IEnumerator Spawn()
    {
        while(enemiesSpawned < enemyNum)
        {
            int spawnNum = UnityEngine.Random.Range(0, Math.Min((enemyNum - enemiesSpawned), config.MaxEnemieNumAtTime - enemiesSpawned + enemiesDead)+1);
            for(int i =0;i<spawnNum;i++)
            {
                Vector3 spawnPos =SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)].position;
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
                e.playerDamage = player.damage;
                enemiesSpawned++;
            }
            yield return new WaitForSeconds(config.SpawnTime);
        }
    }
}
