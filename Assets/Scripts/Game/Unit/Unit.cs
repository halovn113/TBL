using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitType type;
    UnitState state;

    public UnitState GetState()
    {
        return state;
    }

}
