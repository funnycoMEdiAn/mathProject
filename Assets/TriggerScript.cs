using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{

    public float radius =1f;
    public GameObject target;


    public void OnDrawGizmos()
    {
        if(Vector3.Distance(this.gameObject.transform.position, target.gameObject.transform.position) >= radius)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1);
        }
        else if (Vector3.Distance(this.gameObject.transform.position, target.gameObject.transform.position) <= radius)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 1);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
