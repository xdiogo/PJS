using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public int totalSockets = 3;
    private int repairedSockets = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SocketRepaired()
    {
        repairedSockets++;
        UIQuestTracker.instance.UpdateSocketProgress(repairedSockets, totalSockets);
    }
}
