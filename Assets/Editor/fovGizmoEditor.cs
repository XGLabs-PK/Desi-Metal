using UnityEditor;
using UnityEngine;
using XGStudios;

[CustomEditor(typeof(NavmeshAi))]
public class fovGizmoEditor : Editor
{
    void OnSceneGUI()
    {
        NavmeshAi enemy = (NavmeshAi)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(enemy.transform.position, Vector3.up, Vector3.forward, 360, enemy.detectionRadius);

        Vector3 viewAngle01 = directionOfAngle(enemy.transform.eulerAngles.y, -enemy.viewAngle / 2);
        Vector3 viewAngle02 = directionOfAngle(enemy.transform.eulerAngles.y, enemy.viewAngle / 2);
        Handles.color = Color.blue;
        Handles.DrawLine(enemy.transform.position, enemy.transform.position + viewAngle01 * enemy.detectionRadius);
        Handles.DrawLine(enemy.transform.position, enemy.transform.position + viewAngle02 * enemy.detectionRadius);

        if (enemy.canSeePlayers)
        {
            Handles.color = Color.green;
            Handles.DrawLine(enemy.transform.position, enemy._player.position);
        }
    }

    Vector3 directionOfAngle(float eulerY, float angleDegrees)
    {
        angleDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }
}
