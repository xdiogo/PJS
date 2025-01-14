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
        MouseLock.current.LockCursor();
    }

    void Update()
    {
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
        Time.timeScale = 1f;
        isPaused = false;
        MouseLock.current.LockCursor();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
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