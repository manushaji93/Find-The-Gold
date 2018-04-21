using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementTile : MonoBehaviour {

    Renderer rend;
    //Use this for initialization

   void Start()
    {
       rend = GetComponent<MeshRenderer>();
       rend.material.color = Color.white;
    }

    void OnMouseOver()
    {
        rend.material.color = Color.green;
    }

    void OnMouseExit()
    {
        rend.material.color = Color.white;
    }
}
