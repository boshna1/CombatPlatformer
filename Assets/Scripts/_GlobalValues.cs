using UnityEngine;

public class _GlobalValues
{
    public static _CheckpointManager cm;
    public static GameObject player;


    public static void SetPlayer(GameObject value)
    {
        player = value;
    }

    public static void SetCM(_CheckpointManager value)
    {
        cm = value;
    }
}
