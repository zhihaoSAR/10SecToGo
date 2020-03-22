using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public SceneController controller;
    void death()
    {
        Destroy(gameObject);
    }
}
