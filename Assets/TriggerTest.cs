using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    public float triggerRadius;
    private Color oldColor = Color.white;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.CompareTag("Player")) 
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.blue;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material.color = oldColor;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }

}
