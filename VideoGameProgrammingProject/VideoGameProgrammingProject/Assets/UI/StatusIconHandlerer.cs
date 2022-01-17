using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusIconHandlerer : MonoBehaviour
{
    [SerializeField] Sprite spottedIcon;
    [SerializeField] Sprite soundIcon;
    [SerializeField] Sprite lostIcon;
    private bool isDisplaying = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField] float displayTime = 1.0f;
    [SerializeField] GameObject cameraRef;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        if(cameraRef == null)
        {
            cameraRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().getMainCameraRef();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisplaying)
        {
            gameObject.transform.LookAt(cameraRef.transform);
        }
        
    }

    public void displayStatus(string statusName)
    {
        if (isDisplaying == false)  
        {
            isDisplaying = true;
            spriteRenderer.enabled = true;
            if (statusName == "spotted")
            {
                spriteRenderer.sprite = spottedIcon;
            }
            else if (statusName == "lost")
            {
                spriteRenderer.sprite = lostIcon;
            }
            else if (statusName == "sound")
            {
                spriteRenderer.sprite = soundIcon;
            }
            Invoke("turnOff", displayTime);
        }

        // If new icon should be displayed while lost icon is being displayed: then update to new icon
        if (isDisplaying == true)
        {
            if(spriteRenderer.sprite == lostIcon)
            {
                if (statusName == "spotted")
                {
                    spriteRenderer.sprite = spottedIcon;
                }
                else if (statusName == "sound")
                {
                    spriteRenderer.sprite = soundIcon;
                }
                CancelInvoke("turnOff");
                Invoke("turnOff", displayTime);
            }
        }
    }

    private void turnOff()
    {
        isDisplaying = false;
        spriteRenderer.enabled = false;

    }
}
