using UnityEngine;

public abstract class GlobalData
{
    private static string currentIngredient = "";

    public static string CurrentIngredient
    {
        get { return currentIngredient; }
        set { currentIngredient = value; }
    }

    private static float currentDistance = 0f;

    public static float CurrentDistance
    {
        get { return currentDistance; }
        set { currentDistance = value; }
    }

    private static float confidence = 0f;

    public static float Confidence
    {
        get { return confidence; }
        set { confidence = value; }
    }

    private static float angleOfScan = 0f;

    public static float AngleOfScan
    {
        get { return angleOfScan; }
        set { angleOfScan = value; }
    }

    private static string expectedIngredient = "";

    public static string ExpectedIngredient
    {
        get { return expectedIngredient; }
        set { expectedIngredient = value; }
    }
}
