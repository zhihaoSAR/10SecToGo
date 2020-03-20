using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ataque {DASH,DISTANTIA,EXPLOTION }
public enum Pasivo { }
public class Modificador 
{
    public Ataque Ataque;

    public Pasivo[] pasivos = new Pasivo[2];


    
}
