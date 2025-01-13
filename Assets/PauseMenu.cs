using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button continueButton;
    public Button quitButton;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        continueButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);

            if (pauseMenuUI.activeSelf)
                MouseLock.current.UnlockCursor();
            else
                MouseLock.current.LockCursor();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();

            else
                Pause();
        }
    }

    public void Resume()
    {
        
        pauseMenuUI.SetActive(false);
        isPaused = false;
        
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        isPaused = true;
        MouseLock.current.UnlockCursor();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Garante que o jogo não fique pausado ao sair
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}