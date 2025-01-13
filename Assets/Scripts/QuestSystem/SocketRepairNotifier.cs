using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketRepairNotifier : MonoBehaviour
{
   public void NotifySocketRepaired()
    {
        QuestManager.instance.SocketRepaired();
    }
}
