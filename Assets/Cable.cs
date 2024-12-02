using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    public string ID;

    public Type type = Type.IN;

    public bool isConnected;

    [Serializable]
    public enum Type { IN, OUT }


    public bool CanConnect(Cable originCable)
    {
        if (!originCable.ID.Equals(ID))
            return false;

        if (this.type == Type.IN)
            return false;

        if (isConnected)
            return false;


        return true;
    }

}
