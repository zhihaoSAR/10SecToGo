using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Effect effect;

    void Start()
    {
        //effect = Modificador.RandomMod();
    }
    public Effect GetEffect()
    {
        return effect;
    }

}
