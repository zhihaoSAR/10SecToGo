using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Effect {  DISTANTIA=0, EXPLOTION=1,MAS_TIEMPO=2, MAS_TIEMPO_MATAR=3,EXPLOSIVO=4,MAS_DANYO=5,MAS_VIDA=6,ZOMBIFICAR=7,PARAR_TIEMPO=8}
public class Modificador 
{
    public static Effect RandomMod()
    {
        var rnd = new System.Random();
        return (Effect)rnd.Next(Enum.GetNames(typeof(Effect)).Length);
    }
}
