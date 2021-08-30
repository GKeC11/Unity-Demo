using System;
using System.Collections;
using System.Collections.Generic;
using ElRaccoone.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Interface.Elements.Scripts
{
    public class Notification : MonoBehaviour
    {
        /// <summary>
        /// The previous mid - center notification
        /// </summary>
        private int prevCenterNotif;

        /// <summary>
        /// Background image is active only when mid - center notification is shown
        /// </summary>
        public Image background;

        [FormerlySerializedAs("notificationPrefab1")] 
        public GameObject notificationRectangle;
        [FormerlySerializedAs("notificationPrefab2")] 
        public GameObject notificationRounded;

        [Tooltip("The starting position for the notification corresponding to NotifPosition enum")]
        public Transform[] notifPositions = new Transform[9];

        public bool defaultIsRound;
        public float defaultDelay = 3;
        public Sprite defaultIcon;
        public NotifPosition defaultPosition = NotifPosition.BottomRight;
        public NotifStyle defaultStyle = NotifStyle.Rectangle;

        /// <summary>
        /// Static reference to the main handler
        /// </summary>
        private static Notification _i;


        #region Testing Methods

        public void Test(int position)
        {
            defaultPosition = (NotifPosition) position;
            var x = Show("Hello World", "Test description");
        }

        #endregion

        /// <summary>
        /// Show notification
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        /// <returns>The notification ID (used to destroy or hide the notification)</returns>
        public static int Show(string title, string description)
        {
            Debug.LogError("Lite version does not have Notification. Upgrade to FULL version for this feature");
            return 0;
        }

        /// <summary>
        /// Show notification
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        /// <param name="icon">Icon</param>
        /// <param name="delay">The time to show the notification</param>
        /// <param name="position">The position to show the notification</param>
        /// <param name="style">The notification style</param>
        /// <param name="isRoundIcon">The notification icon is round</param>
        /// <param name="outlineColor">The color of the icon outline</param>
        /// <param name="showButtons">Show extra buttons</param>
        /// <param name="positiveText">The text on the positive button</param>
        /// <param name="onPositive">The action to execute when the positive button is clicked</param>
        /// <param name="negativeText">The text on the negative button</param>
        /// <param name="onNegative">The action to execute when the negative button is clicked</param>
        /// <returns>The notification ID (used to destroy or hide the notification)</returns>
        public static int Show(string title, string description, Sprite icon, float delay = 3,
            NotifPosition position = NotifPosition.BottomRight, NotifStyle style = NotifStyle.Rectangle,
            bool isRoundIcon = false, Color outlineColor = default, bool showButtons = false,
            UnityAction onPositive = null, UnityAction onNegative = null, 
            string positiveText = "ACCEPT", string negativeText = "CANCEL")
        {
            Debug.LogWarning("Lite version does not have Notification. Upgrade to FULL version for this feature");
            return 0;
        }

        /// <summary>
        /// Hides a notification by referencing the ID
        /// </summary>
        public static void Hide(int id)
        {
            
        }

        /// <summary>
        /// Destroys a notification by referencing the ID
        /// </summary>
        public static void Destroy(int id)
        {
            
        }


        public static void BackgroundClicked()
        {
            _i.background.TweenGraphicAlpha(0, 0.1f);
            _i.background.raycastTarget = false;
            Hide(_i.prevCenterNotif);
        }
    }

    public enum NotifPosition
    {
        TopLeft = 0,
        TopCenter = 1,
        TopRight = 2,
        MidLeft = 3,
        MidCenter = 4,
        MidRight = 5,
        BottomLeft = 6,
        BottomCenter = 7,
        BottomRight = 8
    }

    public enum NotifStyle
    {
        Rectangle,
        Rounded
    }
}