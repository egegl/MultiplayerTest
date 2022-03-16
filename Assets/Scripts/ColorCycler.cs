using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCycler : MonoBehaviour
{
    public Color[] colors;
    public float speed;
    private int _currIndex;
    private Camera _cam;
    
    void Start()
    {
        _cam = GetComponent<Camera>();
        _currIndex = 0;
        _cam.backgroundColor = colors[_currIndex];
    }

    void Update()
    {
        Color startColor = _cam.backgroundColor;

        Color endColor = colors[0];
        if (_currIndex + 1 < colors.Length)
        {
            endColor = colors[_currIndex + 1];
        }

        Color newColor = Color.Lerp(startColor, endColor, Time.deltaTime * speed);
        _cam.backgroundColor = newColor;

        if (newColor == endColor)
        {
            if (_currIndex + 1 < colors.Length)
            {
                _currIndex++;
            }
            else
            {
                _currIndex = 0;
            }
        }
    }
}
