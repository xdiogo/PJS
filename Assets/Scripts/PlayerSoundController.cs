using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public CharacterMovController movController;

    

    public AudioSource source;

    public Sound[] footStepsGround;
    public Sound[] footStepsGrass;

    public LayerMask stepCastMask = 1;


    void Start()
    {


    }

    [ContextMenu("Play Step")]
    public void PlayStep()
    {
        bool hasHit = Physics.Raycast(movController.transform.position, -Vector3.up, out RaycastHit hitInfo, 3f, stepCastMask, QueryTriggerInteraction.Collide);

        if (hasHit)
        {
            var groundName = hitInfo.collider.name;

            if (groundName == "Grass")
            {
                source.PlayRandomShot(footStepsGrass);
                return;
            }

        }

        source.PlayRandomShot(footStepsGround);
    }


    //public void PlayFootstepSound()
    //{
    //    //if (stepCooldown <= 0f)
    //    //{
    //    //    if (footstepSounds.Length > 0)
    //    //    {
    //    //        int randomIndex = UnityEngine.Random.Range(0, footstepSounds.Length);
    //    //        audioSource.PlayOneShot(footstepSounds[randomIndex]);
    //    //    }

    //    //    stepCooldown = stepInterval;
    //    //}
    //}

    private void Update()
    {
    }

}
