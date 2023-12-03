using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavBaker : MonoBehaviour
{
    public NavMeshSurface surface;
    private Transform currentChild;
    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < this.gameObject.transform.childCount; i++)
        {
           currentChild = this.gameObject.transform.GetChild(i);
            currentChild.gameObject.isStatic = true;
            
        }
        surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
