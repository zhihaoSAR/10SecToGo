using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Effect {  DISTANTIA, EXPLOTION,MAS_TIEMPO, MAS_TIEMPO_MATAR,EXPLOSIVO,MAS_DANYO,MAS_VIDA,ZOMBIFICAR }
public class Modificador 
{
    public static Effect RandomMod()
    {
        var rnd = new System.Random();
        return (Effect)rnd.Next(Enum.GetNames(typeof(Effect)).Length);
    }
}
