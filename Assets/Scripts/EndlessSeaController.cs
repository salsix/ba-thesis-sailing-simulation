using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessSeaController : MonoBehaviour
{
    public GameObject player;
    public GameObject activeSquare;
    private GameObject tempSquare;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, activeSquare.transform.position) >= 10) {
            activeSquare.transform.position = player.transform.position;
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;

        // float width = activeSquare.GetComponent<Renderer>().bounds.size.x;
        // Gizmos.DrawSphere(transform.position + new Vector3(width,0,0), 1);
        // Gizmos.DrawSphere(transform.position + new Vector3(0,0,width), 1);
        // Gizmos.DrawSphere(transform.position + new Vector3(width,0,width), 1);
        // Gizmos.DrawSphere(transform.position + new Vector3(-width,0,0), 1);
        // Gizmos.DrawSphere(transform.position + new Vector3(0,0,-width), 1);
        // Gizmos.DrawSphere(transform.position + new Vector3(-width,0,-width), 1);
        // Gizmos.DrawSphere(transform.position + new Vector3(-width,0,width), 1);
        // Gizmos.DrawSphere(transform.position + new Vector3(width,0,-width), 1);

    }
}
