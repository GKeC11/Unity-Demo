using ElRaccoone.Tweens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Interface.Elements.Scripts
{
    public class InputUI : InputField
    {
        private bool animFocused;
        private Vector3 placeholderPos;
        private const float Duration1 = 0.15f;
        
        /// <summary>
        /// The height of this element (divided by 2)
        /// </summary>
        private float height;
        
        /// <summary>
        /// The original position of the Text (init. before animating)
        /// </summary>
        private Vector3 originalTextPos;

        /// <summary>
        /// The original color of secondaryColor (init. before highlighting)
        /// </summary>
        private Color originalSecondaryColor;
        

        [Tooltip("The color for images")]
        public Color primaryColor = Color.white;
        [Tooltip("The color for text fields")]
        public Color secondaryColor = Color.black;
        [Tooltip("The color of text field when finished editing")]
        public Color highlightTextColor = Color.black;

        [Space]
        
        [Tooltip("Different color onHover or onHighlight")]
        public bool differentTextColorOnHighlight;
        [Tooltip("Hide the placeholder text when selected. If false, the text will move up")]
        public bool hidePlaceholderOnSelect;
        
        [Space]
        
        [Tooltip("Background image of the input field")]
        public Image background;


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            var lightPrimary = primaryColor;
            lightPrimary.a = 0.7f;
            var lightSecondary = secondaryColor;
            lightSecondary.a = 0.7f;
            
            if (placeholder)
            {
                placeholder.color = lightSecondary;
            }

            if (textComponent)
            {
                textComponent.color = secondaryColor;
            }

            if (background)
            {
                background.color = lightPrimary;
            }
        }
#endif

        protected override void Start()
        {
            base.Start();

            height = GetComponent<RectTransform>().rect.height / 2f;
            originalSecondaryColor = secondaryColor;
            if (placeholder)
                placeholderPos = placeholder.transform.localPosition;
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                if (text.Length == 0) Focus(isFocused);
            }
        }

        private void Focus(bool focused)
        {
            if (animFocused == focused) return;
            animFocused = focused;

            if (!placeholder) return;

            if (focused)
            {
                if (hidePlaceholderOnSelect)
                {
                    placeholder.TweenGraphicAlpha(0f, 0.1f);
                }
                else
                {
                    placeholder.TweenLocalPositionY(placeholderPos.y + (height / 1.5f), 0.1f);
                    placeholder.TweenLocalScale(Vector3.one * 0.7f, 0.1f);
                    placeholder.TweenGraphicAlpha(0.5f, 0.1f);
                }
            }
            else
            {
                if (hidePlaceholderOnSelect)
                {
                    placeholder.TweenGraphicAlpha(1, 0.1f);
                }
                else
                {
                    placeholder.TweenLocalPositionY(placeholderPos.y, 0.1f);
                    placeholder.TweenLocalScale(Vector3.one, 0.1f);
                    placeholder.TweenGraphicAlpha(1, 0.1f);
                }
            }

        }

        /// <summary>
        /// Highlight when the input field is being edit
        /// </summary>
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            
            if (differentTextColorOnHighlight)
                secondaryColor = originalSecondaryColor;
            
            textComponent.TweenGraphicColor(secondaryColor, Duration1);
            background.TweenGraphicAlpha(0.6f, Duration1);
        }

        /// <summary>
        /// Highlight when the input field has text
        /// </summary>
        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            
            if (text.Length == 0) return;
            
            if (!interactable) return;
            
            if (differentTextColorOnHighlight) 
                secondaryColor = highlightTextColor;
            
            textComponent.TweenGraphicColor(secondaryColor, Duration1);
            background.TweenGraphicAlpha(0.9f, Duration1);
        }
    }
}