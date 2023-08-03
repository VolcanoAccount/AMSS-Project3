using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AMSS
{
    /// <summary>
    /// 玩家手势监听
    /// </summary>
    public class PlayerGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
    {
        private static PlayerGestureListener instance = null;
        public static PlayerGestureListener Instance
        {
            get
            {
                return instance;
            }
        }

        [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
        public int playerIndex = 0;

        [Tooltip("UI-Text to display gesture-listener messages and gesture information.")]
        public Text gestureInfo;



        // 是否检测到所需的Gesture
        private bool swipeLeft = false;
        private bool swipeRight = false;
        public bool IsSwipeLeft()
        {
            if (swipeLeft)
            {
                swipeLeft = false;
                return true;
            }

            return false;
        }
        public bool IsSwipeRight()
        {
            if (swipeRight)
            {
                swipeRight = false;
                return true;
            }

            return false;
        }

        void Awake()
        {
            instance = this;
        }

        #region KinectGestures接口
        /// <summary>
        /// 检测到新用户时调用
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        public void UserDetected(long userId, int userIndex)
        {
            // 主要用户的手势允许
            KinectManager manager = KinectManager.Instance;
            if (!manager || (userIndex != playerIndex)) return;

            // 检测手势
            manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
            manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);

            if (gestureInfo != null)
            {
                gestureInfo.text = "请TPose开始游戏！";
            }
        }

        /// <summary>
        /// 当手势取消时调用
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        /// <param name="gesture">Gesture type</param>
        /// <param name="joint">Joint type</param>
        public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint)
        {
            if (userIndex != playerIndex) return false;
            return true;
        }

        /// <summary>
        /// 当用户离开时调用
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        public void UserLost(long userId, int userIndex)
        {
            // 主要用户的手势允许
            if (userIndex != playerIndex) return;

            if (gestureInfo != null)
            {
                gestureInfo.text = string.Empty;
            }
        }

        /// <summary>
        /// 当手势正在进行时调用
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        /// <param name="gesture">Gesture type</param>
        /// <param name="progress">Gesture progress [0..1]</param>
        /// <param name="joint">Joint type</param>
        /// <param name="screenPos">Normalized viewport position</param>
        public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectInterop.JointType joint, Vector3 screenPos) { }

        /// <summary>
        /// 当手势完成时调用
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        /// <param name="gesture">Gesture type</param>
        /// <param name="joint">Joint type</param>
        /// <param name="screenPos">Normalized viewport position</param>
        public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint, Vector3 screenPos)
        {
            if (userIndex != playerIndex) return false;

            if (gestureInfo != null)
            {
                string sGestureText = gesture + " detected";
                gestureInfo.text = sGestureText;
            }

            if (gesture == KinectGestures.Gestures.SwipeLeft) swipeLeft = true;
            else if (gesture == KinectGestures.Gestures.SwipeRight) swipeRight = true;
            return true;
        }
        #endregion
    }
}

