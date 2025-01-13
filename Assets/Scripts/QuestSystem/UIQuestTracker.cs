using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestTracker : MonoBehaviour
{
    public static UIQuestTracker instance;
    public Text socketProgressText;
    public Color completedColor = Color.green;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateSocketProgress(int repaired, int total)
    {
        socketProgressText.text = $"Concertar Tomadas ( {repaired} / {total} )";
        if (repaired >= total)
            socketProgressText.color = completedColor;
    }
}
