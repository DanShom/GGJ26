using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFollow : MonoBehaviour
{
    public List<Vector3> points;
    public int pointIndex = 0;
    public Vector3 destination;

    public int FindSetClosest(Vector3 curPosition, bool thoudest = false)
    {
        int index = -1;
        float dis = float.MaxValue;
        for(int i = 0; i < points.Count; i++)
        {
            float nDis = Vector3.Distance(curPosition, points[i]);

            if(thoudest ? nDis > dis : nDis < dis )
            {
                dis = nDis;
                index = i;
            }
        }
        destination = points[index];
        pointIndex = index;
        return index;
    }

    public Vector3 GetNextPos(int direction = 1)
    {
        pointIndex += direction < 0 ? -1 : 1;
        if(pointIndex < 0)
        {
            pointIndex = points.Count - 1;
        }
        if (pointIndex >= points.Count)
        {
            pointIndex = 0;
        }
        destination = points[pointIndex];
        return points[pointIndex];
    }


    /*private void Update()
    {
        if (pointIndex < points.Count) {
            Vector3 curDestination = points[pointIndex];

            destination = points[pointIndex];

            // IMPORTANT: This doesn't consider the y value of the gameobject (could cause bugs in the future but for now its enough)
            if (Mathf.Floor(transform.position.x) == Mathf.Floor(curDestination.x) && Mathf.Floor(transform.position.z) == Mathf.Floor(curDestination.z)) pointIndex += 1;
        }
        if (pointIndex >= points.Count && loopPath) {
            pointIndex = 0;
        }
    }*/
}