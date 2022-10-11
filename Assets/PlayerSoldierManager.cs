using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoldierManager : MonoBehaviour
{
    [NonReorderable]
    public List<SoldierTransform> soldierTransforms = new List<SoldierTransform>();


    private void Update()
    {
        Debug.Log(transform.eulerAngles.y);
    }

}

[System.Serializable]
public class SoldierTransform
{
    public Transform soldierTransform;
   
    public bool isFull;

}
