using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject[] _enemies;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (collider.CompareTag("Player") && _enemies.Length == 0)
        {
            SceneLoader.instance.LoadNextScene();
        }
    }
}
