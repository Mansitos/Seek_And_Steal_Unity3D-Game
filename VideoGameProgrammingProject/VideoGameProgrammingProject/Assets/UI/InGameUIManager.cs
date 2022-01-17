using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{

    // Andrea Mansi - Università Degli Studi di Udine

    [Header(" - - - InGame UI Manager - - - ")]
    [SerializeField] GameObject inGameMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject playerRef;

    PlayerMovementSystem player;
    private bool activeStatus = false;
    private bool isGameOverUIActive = false;
    private bool isWinUIActive = false;

    void Start()
    {
        player = playerRef.GetComponent<PlayerMovementSystem>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7)) && !isGameOverUIActive && !isWinUIActive)
        {
            inGameMenu.SetActive(!activeStatus);
            activeStatus = !activeStatus;
            player.setFreezeMotion(activeStatus);
            if (activeStatus)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void UnPauseGame()
    {
        activeStatus = false;
        Time.timeScale = 1;
        inGameMenu.SetActive(false);
        player.setFreezeMotion(false);
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        UnPauseGame();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UnPauseGame();
    }

    public void HandleGameOver()
    {
        isGameOverUIActive = true;
        Time.timeScale = 0;
        player.setFreezeMotion(true);

        inGameMenu.SetActive(false);
        winMenu.SetActive(false);

        gameOverMenu.SetActive(true); 
    }

    public void HandleWin()
    {
        isGameOverUIActive = true;
        Time.timeScale = 0;
        player.setFreezeMotion(true);

        inGameMenu.SetActive(false);
        gameOverMenu.SetActive(false);

        winMenu.SetActive(true);
    }
}
