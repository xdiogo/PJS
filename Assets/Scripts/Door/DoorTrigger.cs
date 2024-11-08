using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private Door Door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterMovController>(out CharacterMovController controller))
        {
            if (!Door.IsOpen)
            {
                Door.Open(other.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterMovController>(out CharacterMovController controller))
        {
            if (Door.IsOpen)
            {
                Door.Close();
            }
        }
    }
}