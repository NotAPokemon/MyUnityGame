using UnityEngine;

public class Calculator
{
    public static bool ChanceOf(float chanceDenominator)
    {

        return new System.Random().NextDouble() <= (1.0 / chanceDenominator);
    }


    public static Vector3 getPosInChunk(float yValue, float spawnRange)
    {
        float playerx = Player.player.transform.position.x;
        float playerz = Player.player.transform.position.z;
        float y = yValue;
        float x = Random.Range(playerx - spawnRange / 2f, playerx + spawnRange / 2f);
        float z = Random.Range(playerz - spawnRange / 2f, playerz + spawnRange / 2f);
        return new Vector3(x, y, z);

    }

    public static Color cloneColor(Color color)
    {
        return new Color(color.r,color.g,color.b);
    }

    public static float Round(float x, int places)
    {
        return Mathf.Round(x * Mathf.Pow(10,places))/Mathf.Pow(10,places);
    }


    public static bool IsLayerAbove(GameObject entity)
    {
        Vector3 origin = entity.transform.position;
        Vector3 direction = Vector3.up;
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity, 3))
        {

            return true;
        }

        return false;
    }

    public static float GetDistanceToLayerBelow(GameObject entity)
    {
        Vector3 origin = entity.transform.position;
        Vector3 direction = Vector3.down;

        RaycastHit hit;
        Physics.Raycast(origin, direction, out hit, Mathf.Infinity);
        float distance = Vector3.Distance(origin, hit.point);
        return distance;
    }


    public static float calculateExperiance(int x)
    {
        if (x == 0)
        {
            return 5;
        }
        else if (x <= 4)
        {
            return 5 + Mathf.Pow(2, x) / (5 * x + 2) + 1.5f * Mathf.Pow(x, 2);
        }
        return 5 + Mathf.Exp(x) / x;
    }

    public static float calculateDamageReduction(float x)
    {
        return (1 - Mathf.Exp(-0.00025f * x));
    }

 
    public static float randomDiv(float min, float max)
    {
        return Mathf.Clamp(Random.Range(min, max) * Mathf.Pow(Random.value, 2), min, max);
    }

}
