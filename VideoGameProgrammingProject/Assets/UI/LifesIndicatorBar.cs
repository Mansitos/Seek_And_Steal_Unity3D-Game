using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifesIndicatorBar : MonoBehaviour
{
    // Andrea Mansi - Università Degli Studi di Udine

    [Header(" - - - Main Settings - - - ")]

    [Tooltip("x distance between each icon")]
    [SerializeField] float xIconOffset = 50.0f;

    [Tooltip("Max allowable icons to be displayed")]
    [SerializeField] int maxIconsToRender;

    [Tooltip("Icon prefab reference")]
    [SerializeField] Image icon;

    // The array containing the initialized icons. Some of them will be active, some not, it depends on the health status of player.
    private Image[] icons;

    // Health status of player <---> status of the health bar.
    private int statusValue = 0;

    void Awake()
    {
        InitializeIcons();
    }

    private void InitializeIcons()
    {
        icons = new Image[maxIconsToRender];
        for(int i = 0; i < maxIconsToRender; i++)
        {
            Image nextIcon = Instantiate(icon, new Vector3(i * xIconOffset, 0, 0), Quaternion.identity, this.gameObject.transform);
            nextIcon.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(i * xIconOffset, 0, 0);
            icons[i] = nextIcon;
        }
        updateGraphicalStatus();
    }

    public void setStatusValue(int value)
    {
        statusValue = value;
        updateGraphicalStatus();
    }

    private void updateGraphicalStatus()
    {
        for(int i=0; i < maxIconsToRender; i++)
        {
            if (i < statusValue)
            {
                icons[i].gameObject.SetActive(true);
            }
            else
            {
                icons[i].gameObject.SetActive(false);
            }
        }
    }
}

