using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public Text timerText;

    private void OnEnable()
    {
        StartCoroutine(CountdownStart());
    }

    private IEnumerator CountdownStart()
    {
        float countdownTime = 3;
        while (countdownTime > 0)
        {
            timerText.text = countdownTime.ToString("F1");
            yield return new WaitForSeconds(0.1f);
            countdownTime -= 0.1f;
        }
        GameManager.instance.ReactivatePlayer();
    }
}
