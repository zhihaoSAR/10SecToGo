using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SceneController : MonoBehaviour
{

    //---------------Variables no van quitar----------------------------
    public Player player;
    public float damage=1;
    float time_mas_damage =0 ;
    public Transform[] SpawnPoints;
    public LevelConfig config;
    int round;
    int enemiesSpawned;
    int enemiesDead;
    float[] probability;
    int enemyNum;



    //--------------varibale de modificador---------------------------
    float extraTime;
    float time_mas_tiempo_matar = 0;
    [HideInInspector]
    public bool explosivo, zombificar;
    float time_explosivo = 0, time_zombificar = 0;


    public float time;
    //GUI
    public GameObject GUI,MenuEntreRondas,MenuMuerte;
    private GUImanager gui;
    public bool Paused = false;
    void Start()
    {

        probability = new float[config.Enemies.Length];
        for(int i = 0;i< config.Enemies.Length;i++)
        {
            probability[i] = config.InitProbability[i];
        }
        enemyNum = config.InitEnemyNum;
        round = 1;
        enemiesDead = 0;
        enemiesSpawned = 0;

        initSetting();
        player.InitPlayer(this);
        //GUI
        gui = GUI.GetComponent<GUImanager>();
        gui.UpdateRonda(round);
        StartCoroutine("Spawn");

    }

    void initSetting()
    {
        time = 10;
        time_mas_tiempo_matar = 0;
        extraTime = 0;
        time_explosivo = 0;
        explosivo = false;
        time_zombificar = 0;
        zombificar = false;
        time_mas_damage = 0;
        damage = 1;
        player.initSetting();
        
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
        time += increase + extraTime;
        if (++enemiesDead == enemyNum)
        {
            if(++round > config.MaxRound)
            {
                Debug.Log("Win");
            }
            else
            {
                Debug.Log("Ronda: " + round);
                ActivarMenuEntreRondas();
                NextRound();
            }
        }

    }
    void ActivarMenuEntreRondas()
    {
        player.pause();
        Paused = true;
        MenuEntreRondas.SetActive(true);
        Time.timeScale = 0f;
    }
    public void siguienteRonda()
    {
        Paused = false;
        MenuEntreRondas.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ReduceTime(float timeDamage)
    {
        if (!player.getImmune())
        {
            time -= timeDamage;
            Debug.Log("timedamage");
        }

    }

    public void NextRound()
    {
        time = 10f;
        enemiesDead = 0;
        enemiesSpawned = 0;
        player.initSetting();
        //GUI
        gui.UpdateRonda(round);

        enemyNum += config.DeltaEnemies;
        float cantidad = probability[0] - config.DeltaProbability[0] < config.MinProbability ? probability[0] - config.MinProbability : config.DeltaProbability[0];
        probability[0] -= cantidad;
        for (int i = 1; i< probability.Length;i++)
        {
            probability[i] += cantidad * config.DeltaProbability[i];
        }
        player.unPause();
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
                enemiesSpawned++;
            }
            yield return new WaitForSeconds(config.SpawnTime);
        }
    }

    public void masDanyo()
    {
        if (time_mas_damage <= 0)
        {
            damage = 2;
            time_mas_damage = 3;//duracion del modificador
            StartCoroutine("resetMasDanyo");
        }
        else
        {
            time_mas_damage = 3;
        }

    }
    IEnumerator resetMasDanyo()
    {
        while (time_mas_damage > 0)
        {
            yield return null;
            time_mas_damage -= Time.deltaTime;
        }
        damage = 1;
    }

    public void masTimepoMatar()
    {
        if (time_mas_tiempo_matar <= 0)
        {
            time_mas_tiempo_matar = 3;//duracion del modificador
            extraTime = 0.5f;
            StartCoroutine("resetMasTiempoMatar");
        }
        else
        {
            time_mas_tiempo_matar = 3;//duracion del modificador
        }

    }
    IEnumerator resetMasTiempoMatar()
    {

        while (time_mas_tiempo_matar > 0)
        {
            yield return null;
            time_mas_tiempo_matar -= Time.deltaTime;
        }

        extraTime = 0;
    }

    public void activarZombificar()
    {
        if (time_zombificar <= 0)
        {
            time_zombificar = 2;//duracion del modificador
            zombificar = true;
            StartCoroutine("resetZombificar");
        }
        else
        {
            time_zombificar = 2;//duracion del modificador
        }

    }
    IEnumerator resetZombificar()
    {

        while (time_zombificar > 0)
        {
            yield return null;
            time_zombificar -= Time.deltaTime;
        }

        zombificar = false; ;
    }

    public void activarExplosivo()
    {
        if(time_explosivo <= 0)
        {
            time_explosivo = 2;//duracion del modificador
            explosivo = true;
            StartCoroutine("resetExplosivo");
        }
        else
        {
            time_explosivo = 2;//duracion del modificador
        }
        
    }
    IEnumerator resetExplosivo()
    {

        while (time_explosivo > 0)
        {
            yield return null;
            time_explosivo -= Time.deltaTime;
        }

        explosivo = false; ;
    }


    public IEnumerator deadMenu()
    {
        Paused = true;
        yield return new WaitForSeconds(1);

        player.pause();
        Time.timeScale = 0f;
        MenuMuerte.SetActive(true);

    }
    public void pausePlayer()
    {
        player.pause();
    }
    public void unPausePlayer()
    {
        player.unPause();
    }
}
