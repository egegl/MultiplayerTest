using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
{
    public int bulletSpeed;
    public int bulletDamage;
    public float fireRate;
    public bool fullAuto;
    public bool burst;
    public float width;
    public float height;

    // public Audio attackSound;
}
