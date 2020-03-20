using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public Player player;

    public Ataque a;
    void Start()
    {
        Modificador m = new Modificador();
        m.Ataque = a;// Ataque.DISTANTIA;

        player.InitPlayer(m);
    }
}
