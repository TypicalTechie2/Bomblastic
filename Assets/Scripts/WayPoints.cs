using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Transform[] wayPoints; // Array of waypoints to be visualized

    // Method to draw gizmos in the editor to visualize waypoints
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
