using UnityEngine;
using VRTK;
using System.Collections;
using BlockClasses;

public class VRMovement : MonoBehaviour
{
    [SerializeField] float _translateSpeed = 1.5f;
    [SerializeField] Transform _sdkSetups;

    [Range(-1, 1)]
    [SerializeField]
    [Tooltip("1, is Up and -1 is Down.")]
    int _verticalMoveDir = 1;
	
    bool _gripIsPressed;

    private void Start()
    {
        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(GripPressed);
        GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(GripReleased);
    }

    IEnumerator VerticalMovement()
    {
        while (_gripIsPressed)
        {
            Vector3 pos = Vector3.up * _verticalMoveDir * _translateSpeed;
            _sdkSetups.Translate(pos);

            pos = _sdkSetups.position;
            pos.y = Mathf.Clamp(_sdkSetups.position.y, 0, 10);
            _sdkSetups.position = pos;
            yield return null;
        }
    }

    void GripPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (VRTKBlockInteraction.instance != null)
        {
            if (VRTKBlockInteraction.instance._IsHolding) return;
            _gripIsPressed = true;
            StartCoroutine(VerticalMovement());
        }
    }

    void GripReleased(object sender, ControllerInteractionEventArgs e)
    {
        _gripIsPressed = false;
    }

    void OnValidate()
    {
        if (_verticalMoveDir == 0) _verticalMoveDir = 1;
    }
}