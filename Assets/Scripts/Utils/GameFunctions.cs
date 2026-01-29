using UnityEngine;
using UnityEngine.UIElements;

public class GameFunctions
{
    private static Collider[] locEnviroment = new Collider[2];
    public static bool GetPointOnWater(float sponeRadius, float spacing, Vector3 origin, LayerMask ScanLayerMask,out Vector3 result, Camera cameraToAvoidView = null)
    {
        int iters = 0;
        while (true)
        {
            if (iters++ > 1000)
                break;
            
            Vector2 randVal = Random.insideUnitCircle * sponeRadius;
            Vector3 pos = new Vector3(randVal.x, 0f, randVal.y) + origin;
            if (cameraToAvoidView != null)
            {
                var viewportPos = cameraToAvoidView.WorldToViewportPoint(pos);
                if (viewportPos.z > 0 && InRange(-.2f, 1.2f, viewportPos.x) && InRange(-.2f, 1.2f, viewportPos.x))
                    continue;
            }
            locEnviroment[0] = null;
            locEnviroment[1] = null;
            int count = Physics.OverlapSphereNonAlloc(pos, spacing, locEnviroment, ScanLayerMask);
            if (count == 0)
            {
                result = pos;
                return true;
            }
        }
        Debug.Log("Failed to find a clear spot on the water. Check if your map is overcrowded with colliders");
        result = Vector3.zero;
        return false;
    }


    public static bool IsGameMode(params GameMode[] gameModes)
    {
        foreach(var gm in gameModes)
        {
            if (gm == Game.Get.gameState) return true;
        }
        return false;
    }

    static bool InRange(float min, float max, float x) => x >= min && x <= max;

    /*public static void SpawnPrefabOnWater<T>(T prefab, float sponeRadius, float spacing, Vector3 origin, LayerMask ScanLayerMask, float maxDelaySeconds = 0) where T : Object
    {
        DelayedAction.Run(Random.Range(0, maxDelaySeconds), () =>
        {
            Vector3 pos = GetPointOnWater(sponeRadius, spacing, origin, ScanLayerMask);
            T clone = Object.Instantiate(prefab, pos, Quaternion.identity);
        });
    }*/
}
