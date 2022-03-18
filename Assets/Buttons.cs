using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void ButtonClick1()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            AudioManager.instance.Play("fart1");
        }
        else
        {
            AudioManager.instance.Play("fart2");
        }
    }
}
