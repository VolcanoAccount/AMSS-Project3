using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceTexGenerator : MonoBehaviour,KinectGestures.GestureListenerInterface
{
    public RawImage faceRawImage;
    public SetFaceTexture setFaceTexture;

    #region KinectGestures接口
    void KinectGestures.GestureListenerInterface.UserDetected(long userId, int userIndex)
    {
        faceRawImage.texture=setFaceTexture.GetFaceTex();
    }

    void KinectGestures.GestureListenerInterface.UserLost(long userId, int userIndex)
    {
        
    }

    void KinectGestures.GestureListenerInterface.GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
        
    }

    bool KinectGestures.GestureListenerInterface.GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint, Vector3 screenPos)
    {
        return false;
    }

    bool KinectGestures.GestureListenerInterface.GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint)
    {
        return false;
    }

    #endregion
}
