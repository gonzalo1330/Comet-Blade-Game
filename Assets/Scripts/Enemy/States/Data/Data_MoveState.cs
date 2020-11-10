using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject is a data container that you can use to save large amouns of data 
// independent of object instances
// We will use this to store the movement data of our MoveState class
public class Data_MoveState : ScriptableObject
{
    public float movementSpeed = 5f;
}
