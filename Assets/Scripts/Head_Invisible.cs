using UnityEngine;

public class HideHeadBone : MonoBehaviour
{
    public Transform headBone; // Drag the head bone here in the Inspector

    void Start()
    {
        // Hide the head by scaling the head bone to zero
        if (headBone != null)
            headBone.localScale = Vector3.zero;
    }
}