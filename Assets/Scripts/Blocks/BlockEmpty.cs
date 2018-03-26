using UnityEngine;

public class BlockEmpty : Block
{
    void Start ()
    {
        RotateRandomly ();
    }

    void RotateRandomly ()
    {
        int x = Random.Range (0, 3);
        transform.rotation = Quaternion.AngleAxis (x * 90, Vector3.up);
    }
}