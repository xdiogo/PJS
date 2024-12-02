using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugShape : MonoBehaviour
{
    public float scale = 1f;

    [ColorUsage(true)]
    public Color color = Color.cyan;
    private void OnDrawGizmos()
    {
        if(!enabled) return;

        Gizmos.color = color;   
        //Gizmos.DrawWireCube(transform.
        Gizmos.DrawWireCube(transform.position,transform.localScale* scale);    
    }

}

