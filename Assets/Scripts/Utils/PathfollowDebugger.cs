#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathFollow))]
public class PathFollowDebugger : Editor
{
    void OnSceneGUI()
    {
        PathFollow be = target as PathFollow;
        int numOfPoints = be.points.Count;
        if (be.points == null || numOfPoints == 0) return;
        // we store the start and end points of the line segments in this array
        Vector3[] linePoints = new Vector3[numOfPoints];

        // for each line segment we need two indices into the points array:
        // the index to the start and the end point
        int[] segmentIndices = new int[numOfPoints * 2];

        int prevIndex = numOfPoints - 1;
        int pointIndex = 0;
        int segmentIndex = 0;
        for (int currIndex = 0; currIndex < numOfPoints; currIndex++)
        {
            // find the position of our connected object and store it
            if (be.points[pointIndex] != null)
            {
                linePoints[pointIndex] = be.points[currIndex];
            }
            else
            {
                linePoints[pointIndex] = Vector3.zero;
            }

            be.points[pointIndex] = Handles.PositionHandle(linePoints[pointIndex], Quaternion.identity);
            Handles.Label(be.points[pointIndex], "Point " + pointIndex + "#");
            // the index to the start of the line segment
            segmentIndices[segmentIndex] = prevIndex;
            segmentIndex++;

            // the index to the end of the line segment
            segmentIndices[segmentIndex] = pointIndex;
            segmentIndex++;

            pointIndex++;
            prevIndex = currIndex;
        }
        Handles.color = Color.red;
        Handles.DrawLines(linePoints, segmentIndices);
        //be.startPoint = Handles.PositionHandle(be.startPoint, Quaternion.identity);
        //be.middlePoint = Handles.PositionHandle(be.middlePoint, Quaternion.identity);
        //be.endPoint = Handles.PositionHandle(be.endPoint, Quaternion.identity);


        //Handles.Label(be.startPoint, "startPoint: " + be.startPoint);
        //Handles.Label(be.middlePoint, "middlePoint : " + be.middlePoint);
        //Handles.Label(be.endPoint, "endPoint: " + be.endPoint);
        // Visualize the tangent lines
        //Handles.DrawDottedLine(be.startPoint, be.startTangent, 5);
        //Handles.DrawDottedLine(be.endPoint, be.endTangent, 5);

        // Handles.DrawLines(new[] { be.startPoint, be.middlePoint, be.endPoint });
    }
}
#endif