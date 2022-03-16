using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInventory : MonoBehaviourPunCallbacks
{
    private KeyCode[] _keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
     };
     private string[] _weapons = {
        "Pistol",
        "Rifle",
        "Uzi",
        "Burst",
        "Auto Burst",
    };
    private Shoot _shoot;
    private List<string> _inventory = new List<string>();
    private int _currIndex;
    private bool _isMine;

    void Awake()
    {
        if (photonView.IsMine)
        {
            _isMine = true;
        }
        else
        {
            _isMine = false;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _shoot = GameObject.Find("Ranged Weapon").GetComponent<Shoot>();
        foreach (Transform child in transform)
        {
            _inventory.Add(child.name);
        }
        _currIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMine)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                SwitchItem();
            }

            for(int i = 0 ; i < _keyCodes.Length; i ++ )
            {
                if(Input.GetKeyDown(_keyCodes[i]))
                {
                    if (_shoot.gameObject.activeSelf == false)
                    {
                        SwitchItem();
                    }
                    SwitchWeapon(i);
                }
            }
        }
    }

    private void SwitchItem()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        if (_currIndex + 1 < _inventory.Count)
        {
            _currIndex++;
        }
        else
        {
            _currIndex = 0;
        }
        transform.GetChild(_currIndex).gameObject.SetActive(true);
    }
   
    public void SwitchWeapon(int i)
    {
        _shoot.weapon = Resources.Load<Weapon>("Scriptable Objects/" + _weapons[i]);
        _shoot.Sync();
    }
}
