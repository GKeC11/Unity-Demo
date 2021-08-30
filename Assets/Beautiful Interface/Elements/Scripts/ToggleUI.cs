using System;
using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Elements.Scripts
{
    public class ToggleUI : Toggle
    {
        
        private const float Duration1 = 0.4f;
        private const float Duration2 = 0.15f;

        private bool isDragging;
        private float startingX;
        private RectTransform rect;

        [Tooltip("The background color when ON")]
        public Color onColor = Color.white;
        [Tooltip("The background color when OFF")]
        public Color offColor = Color.gray;

        [Space]

        public Image background;
        public Image outline;
        public Image highlighter;

        [Space]

        public Text onText;
        public Image onImage;
        
        [Space]

        public Text offText;
        public Image offImage;

        [Space] 
        
        [Tooltip("The highlighter on the left signifies ON")] 
        public bool leftIsOn;


#if UNITY_EDITOR
        
        protected override void OnValidate()
        {
            base.OnValidate();
            
            background.color = onColor;
            outline.color = onColor;
        }
        
#endif
        

        protected override void Start()
        {
            base.Start();

            if (Application.isPlaying)
            {
                rect = GetComponent<RectTransform>();
                startingX = highlighter.transform.localPosition.x;
                onValueChanged.AddListener(SetValue);
                Press();
            }
        }

        private void Update()
        {
            if (isDragging) Drag();
        }

        private void SetValue(bool on)
        {
            isOn = on;
            Press();
        }

        private void Drag()
        {
            var clamp = rect.rect.width / 4;

            var pos = highlighter.transform.localPosition;
            if (pos.x > Math.Abs(clamp))
                return;

            var dragPos = Input.mousePosition.x - transform.position.x;
            dragPos = Mathf.Clamp(dragPos, -clamp, clamp);
            highlighter.transform.localPosition = new Vector3(dragPos, pos.y, pos.z);

            if (leftIsOn)
            {
                isOn = dragPos < startingX;
            }
            else
            {
                isOn = dragPos > startingX;
            }
        }

        public void BeginDrag()
        {
            isDragging = true;
        }

        public void EndDrag()
        {
            isDragging = false;
            Press();
        }

        public void Press()
        {
            if (!interactable) return;

            if (isDragging) return;

            
            var width = rect.rect.width / 4;
            float to;
            
            if (isOn)
            {
                if (leftIsOn)
                {
                    to = startingX - width;
                }
                else
                {
                    to = startingX + width;
                }
            }
            else
            {
                if (leftIsOn)
                {
                    to = startingX + width;
                }
                else
                {
                    to = startingX - width;
                }
            }
            
            highlighter.TweenLocalPositionX(to, Duration1).SetEase(EaseType.ExpoInOut);

            if (isOn)
            {
                background.TweenGraphicColor(onColor, Duration2);
                onText.TweenGraphicAlpha(1, Duration2);
                onImage.TweenGraphicAlpha(1, Duration2);
                offText.TweenGraphicAlpha(0, Duration2);
                offImage.TweenGraphicAlpha(0, Duration2);
            }
            else
            {
                background.TweenGraphicColor(offColor, Duration2);
                onText.TweenGraphicAlpha(0, Duration2);
                onImage.TweenGraphicAlpha(0, Duration2);
                offText.TweenGraphicAlpha(1, Duration2);
                offImage.TweenGraphicAlpha(1, Duration2);
            }
        }
    }
}