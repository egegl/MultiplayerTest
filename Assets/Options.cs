using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    public Toggle fullscreen, vsync;
    public TextMeshProUGUI resText;
    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedRes;

    // Start is called before the first frame update
    void Start()
    {
        fullscreen.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsync.isOn = false;
        }
        else
        {
            vsync.isOn = true;
        }
        bool foundRes = false;
        for(int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width == resolutions[i].width && Screen.height == resolutions[i].height)
            {
                foundRes = true;
                selectedRes = i;
                UpdateResLabel();
            }
        }
        if(!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.width = Screen.width;
            newRes.height = Screen.height;
            resolutions.Add(newRes);
            selectedRes = resolutions.Count - 1;
            UpdateResLabel();
        }

    }

    public void ResLeft()
    {
        selectedRes--;
        if (selectedRes < 0)
        {
            selectedRes = 0;
        }
        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedRes++;
        if(selectedRes > resolutions.Count - 1)
        {
            selectedRes = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resText.text = resolutions[selectedRes].width.ToString() + "x" + resolutions[selectedRes].height.ToString();
    }

    public void ApplySettings()
    {
        // Screen.fullScreen = fullscreen.isOn;

        if (vsync.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedRes].width, resolutions[selectedRes].height, fullscreen.isOn);
    }

    [System.Serializable]
    public class ResItem
    {
        public int width, height;
    }
}
