using System.Collections;
using BlockClasses;
using UnityEngine;
using VRTK;

public class VRMovement : MonoBehaviour
{
    [SerializeField] float _translateSpeed = 1.5f;
    [SerializeField] Transform _sdkSetups;

    [SerializeField] float _minY = 0.0f;
    [SerializeField] float _maxY = 10.0f;

    VRTKBlockInteraction _blockInteraction;
    VRTK_ControllerEvents _controllerEvents;

    [Range (-1, 1)]
    [SerializeField]
    [Tooltip ("1, is Up and -1 is Down.")]
    int _verticalMoveDir = 1;

    bool _gripIsPressed;

    private void Awake ()
    {
        _blockInteraction = GetComponent<VRTKBlockInteraction> ();
        _controllerEvents = GetComponent<VRTK_ControllerEvents> ();
        _controllerEvents.GripPressed += new ControllerInteractionEventHandler (GripPressed);
        _controllerEvents.GripReleased += new ControllerInteractionEventHandler (GripReleased);
    }

    IEnumerator VerticalMovement ()
    {
        while (_gripIsPressed)
        {
            Vector3 pos = Vector3.up * _verticalMoveDir * _translateSpeed;
            _sdkSetups.Translate (pos);

            pos = _sdkSetups.position;
            pos.y = Mathf.Clamp (_sdkSetups.position.y, _minY, _maxY);
            _sdkSetups.position = pos;
            yield return null;
        }
    }

    void GripPressed (object sender, ControllerInteractionEventArgs e)
    {
        if (_blockInteraction != null && _blockInteraction._IsHolding) return;
        _gripIsPressed = true;
        StartCoroutine (VerticalMovement ());
    }

    void GripReleased (object sender, ControllerInteractionEventArgs e)
    {
        _gripIsPressed = false;
    }

    void OnValidate ()
    {
        if (_verticalMoveDir == 0) _verticalMoveDir = 1;
    }

    private void OnDestroy ()
    {
        _controllerEvents.GripPressed -= new ControllerInteractionEventHandler (GripPressed);
        _controllerEvents.GripReleased -= new ControllerInteractionEventHandler (GripReleased);
    }
}