using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Transform[] wayPoints;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < wayPoints.Length; i++)
        {
            Gizmos.DrawSphere(wayPoints[i].position, 0.5f);

            if (i + 1 < wayPoints.Length)
            {
                Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
            }
        }
    }
}
