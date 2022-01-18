using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject optionsMenuUI;
    [SerializeField] GameObject aboutMenuUI;
    [SerializeField] GameObject playMenuUI;
    [SerializeField] GameObject audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioSystem");
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void loadGameSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void backToMainMenuUI()
    {
        // Enable MAIN menu UI
        mainMenuUI.SetActive(true);
        // Disable all other menus
        aboutMenuUI.SetActive(false);
        playMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
    }

    public void loadAboutUI()
    {
        // Enable ABOUT menu UI
        aboutMenuUI.SetActive(true);
        // Disable all other menus
        mainMenuUI.SetActive(false);
        playMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
    }

    public void loadPlayUI()
    {
        // Enable PLAY LEVEL SELECTION menu UI
        playMenuUI.SetActive(true);
        // Disable all other menus
        mainMenuUI.SetActive(false);
        aboutMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
    }

    public void loadOptionsUI()
    {
        // Enable OPTIONS menu UI
        optionsMenuUI.SetActive(true);
        // Disable all other menus
        mainMenuUI.SetActive(false);
        aboutMenuUI.SetActive(false);
        playMenuUI.SetActive(false);

        audioManager.GetComponent<AudioSystemManager>().updateRefs();
    }

    public void quitApplication()
    {
        Debug.Log("Application is going to be quitted!");
        Application.Quit();
    }

}
