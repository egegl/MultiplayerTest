using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// THIS SCRIPT CAN BE ACCESSED BY ANY SCRIPT, JUST WRITE: "CoinCounter.instance.AddCoins(integer amount)"

public class CoinCounter : MonoBehaviour
{
    private Animator _anim;
    private TextMeshProUGUI _text;
    public static CoinCounter instance;
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _anim = GetComponent<Animator>();
    }
    public void AddCoins(int amount)
    {
        _anim.SetTrigger("Add");
        _text.text = (int.Parse(_text.text) + amount).ToString();
    }
}
