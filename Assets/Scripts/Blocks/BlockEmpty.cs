using UnityEngine;

public class BlockEmpty : Block
{
    void Start()
    {
        int x = Random.Range(0, 3);
        Vector3 vec = Vector3.forward;
        
        switch(x)
        {
            case 0:
            vec = Vector3.up;
            break;
            case 1: 
            vec = Vector3.left;
            break;
            case 2:
            vec = Vector3.forward;
            break;
        }
        
        transform.rotation = Quaternion.AngleAxis(x * 90, vec);
    }
}