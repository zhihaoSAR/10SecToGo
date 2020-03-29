using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ataque {DASH,DISTANTIA,EXPLOTION }
public enum Pasivo {MAS_TIEMPO_INI,MAS_TIEMPO_MATAR,EXPLOSIVO,MAS_DANYO,MAS_VIDA }
public class Modificador 
{
    public Ataque Ataque;

    public Pasivo[] pasivos = new Pasivo[2];


    
}
