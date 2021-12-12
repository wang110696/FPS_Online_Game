using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class test : MonoBehaviour
{
    void Start()
    {
        Debug.Log(this.gameObject.GetComponent<MeshRenderer>().bounds.center.ToString("f4"));
    }
}
