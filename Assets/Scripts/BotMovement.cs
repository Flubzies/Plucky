using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    [SerializeField] List<Transform> _colliders;
    [SerializeField] Transform _currentGround;
    [SerializeField] LayerMask _blocksLM;
    [SerializeField] float _moveTimer = 2.0f;
    bool _moving;

    private void Start()
    {
        StartCoroutine(MoveTo());
    }

    IEnumerator MoveTo()
    {
        while (!_moving)
        {
            if (CheckForBlockEffects()) yield return new WaitForSeconds(_moveTimer);

            CheckColliders(); yield return new WaitForSeconds(_moveTimer);
        }
    }

    private bool CheckForBlockEffects()
    {
        Collider[] cols = Physics.OverlapSphere(_currentGround.position, 0.2f, _blocksLM);
        if (cols.Length != 0)
        {
            if (cols[0].GetComponent<Block>().BlockEffect(transform)) return true;
            else return false;
        }
        return false;
    }

    private void CheckColliders()
    {
        _moving = true;
        int config = 0;

        for (int i = 0; i < _colliders.Count; i++)
        {
            Collider[] cols = Physics.OverlapSphere(_colliders[i].position, 0.2f, _blocksLM);
            if (cols.Length != 0) config += (int)Math.Pow(2, i);
        }

        ActOnColliders(config);
    }

    void ActOnColliders(int configuration_)
    {
        Debug.Log(configuration_);
        switch (configuration_)
        {
            case 1:
            case 2:
            case 3:
            case 17:
            case 18:
            case 19:
            case 33:
            case 34:
            case 50:
            case 51:
                Move();
                break;
            case 4:
            case 5:
            case 6:
            case 7:
                Climb();
                break;
            default:
                Turn();
                break;
        }
    }

    void Turn()
    {
        Quaternion newRot = transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f);
        transform.rotation = newRot;
        _moving = false;
    }

    void Climb()
    {
        transform.Translate(Vector3.forward + Vector3.up);
        _moving = false;
    }

    void Move()
    {
        transform.Translate(Vector3.forward);
        _moving = false;
    }

}