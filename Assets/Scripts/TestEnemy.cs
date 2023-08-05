using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour, IHitable
{
    public void Dead()
    {

    }

    public void Hit(float damage)
    {
        Debug.Log($"{damage}");
    }
}
