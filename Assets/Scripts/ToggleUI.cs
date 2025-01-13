using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public GameObject page1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            page1.SetActive(!page1.activeSelf);

            if (page1.activeSelf)
                MouseLock.current.UnlockCursor();
            else
                MouseLock.current.LockCursor();
        }
    }
}