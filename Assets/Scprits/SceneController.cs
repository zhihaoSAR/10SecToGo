using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public Player player;

    void Start()
    {
        Modificador m = new Modificador();
        m.Ataque = Ataque.DISTANTIA;

        player.InitPlayer(m);
    }
}
