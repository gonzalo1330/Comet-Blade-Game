using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Entity : ScriptableObject
{
    public float wallCheckDistance;
    public float ledgeCheckDistance;

    public LayerMask whatIsGround;
}
