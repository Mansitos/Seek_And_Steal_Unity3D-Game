using UnityEditor;
using UnityEngine;

// THIS SCRIPT IS A CUSTOMIZED VERSION OF A PUBLIC GITHUB REPOSITORY FROM Comp3interactive
// https://github.com/Comp3interactive/FieldOfView

[CustomEditor(typeof(AIVisualDetection))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        AIVisualDetection fov = (AIVisualDetection)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        if (fov.isTargetDetected())
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.targetRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
