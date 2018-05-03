// Frames Per Second Canvas|Prefabs|0010
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class VRMessage : MonoBehaviour
{
    public int _fontSize = 32;
    public Vector3 _textPos = Vector3.zero;
    public Color _textColor = Color.red;
    public Font _textFont;

    protected const float _updateInterval = 0.5f;
    protected Canvas _canvas;
    protected Text _text;
    protected VRTK_SDKManager _sdkManager;

    protected virtual void OnEnable ()
    {
        _sdkManager = VRTK_SDKManager.instance;
        if (_sdkManager != null) _sdkManager.LoadedSetupChanged += LoadedSetupChanged;
        InitCanvas ();
        // DisplayMessage ("Test Message");
    }

    protected virtual void OnDisable ()
    {
        if (_sdkManager != null && !gameObject.activeSelf) _sdkManager.LoadedSetupChanged -= LoadedSetupChanged;
    }

    // protected virtual void Update ()
    // {
    //     framesCount++;
    //     framesTime += Time.unscaledDeltaTime;

    //     if (framesTime > _updateInterval)
    //     {
    //         if (_text != null)
    //         {
    //             if (displayFPS)
    //             {
    //                 float fps = framesCount / framesTime;
    //                 _text._text = string.Format ("{0:F2} FPS", fps);
    //                 _text.color = (fps > (targetFPS - 5) ? goodColor :
    //                     (fps > (targetFPS - 30) ? warnColor :
    //                         badColor));
    //             }
    //             else
    //             {
    //                 _text._text = "";
    //             }
    //         }
    //         framesCount = 0;
    //         framesTime = 0;
    //     }
    // }

    protected virtual void LoadedSetupChanged (VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        SetCanvasCamera ();
    }

    protected virtual void InitCanvas ()
    {
        _canvas = transform.GetComponentInParent<Canvas> ();
        _text = GetComponent<Text> ();

        if (_canvas != null) _canvas.planeDistance = 0.5f;

        if (_text != null)
        {
            _text.fontSize = _fontSize;
            _text.transform.localPosition = _textPos;
            _text.color = _textColor;
            _text.font = _textFont;
        }

        SetCanvasCamera ();
    }

    protected virtual void SetCanvasCamera ()
    {
        Transform sdkCamera = VRTK_DeviceFinder.HeadsetCamera ();
        if (sdkCamera != null) _canvas.worldCamera = sdkCamera.GetComponent<Camera> ();
    }

    public void Log (string message_, bool log_ = false, float messageDuration_ = 5.0f)
    {
        StartCoroutine (DisplayMessageCour (message_, log_, messageDuration_));
    }

    IEnumerator DisplayMessageCour (string message_, bool log_ = false, float messageDuration_ = 5.0f)
    {
        _text.text = message_;
        if (log_) Debug.Log (message_);
        yield return new WaitForSeconds (messageDuration_);
        _text.text = "";
    }
}