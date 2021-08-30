using System;
using System.Collections.Generic;
using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Interface.Elements.Scripts
{
    public class ButtonUI : Button
    {
        
        /// <summary>
        /// The original position of the Text (init. before animating)
        /// </summary>
        private Vector3 originalTextPos;

        private bool persistHighlight;
        
        /// <summary>
        /// The duration for each tween animation
        /// </summary>
        public float duration = 0.15f;
        
        /// <summary>
        /// Stay highlighted
        /// </summary>
        public bool PersistHighlight
        {
            get => persistHighlight;
            set
            {
                persistHighlight = value;
                if (value)
                    Highlight();
                else Normal();
            }
        }
        

        #region Has Booleans

        public bool hasSlider;
        
        public bool hasHoverSound;
        public bool hasClickSound;

        #endregion


        #region Normal

        public List<ButtonState> normalStates;

        #endregion


        #region On Highlight

        public List<ButtonState> highlightStates;

        #endregion


        #region On Click
        
        public List<ButtonState> clickStates;

        #endregion


        #region Slider Effect
        
        public Slider slider;

        #endregion


        #region Sounds
        
        public AudioClip onHoverAudio;
        public AudioClip onClickAudio;

        #endregion


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            foreach (var state in normalStates)
            {
                var image = state.image;
                if (!image) continue;
                if (state.color == default)
                {
                    state.color = image.color;
                }
                
                image.color = state.color;
            }
            if (hasSlider) slider.value = 0;
            
            
            // Set highlight state to copy normal state
            int i;
            for (i = 0; i < highlightStates.Count; i++)
            {
                if (normalStates.Count > i && highlightStates[i].image == null)
                {
                    highlightStates[i].image = normalStates[i].image;
                    highlightStates[i].color = normalStates[i].color;
                }
            }

            if (i < normalStates.Count)
                for (; i < normalStates.Count; i++)
                    highlightStates.Add(new ButtonState());
            
            // Set click state to copy normal state
            for (i = 0; i < clickStates.Count; i++)
            {
                if (normalStates.Count > i && clickStates[i].image == null)
                {
                    clickStates[i].image = normalStates[i].image;
                    clickStates[i].color = normalStates[i].color;
                }
            }
            
            if (i < normalStates.Count)
                for (; i < normalStates.Count; i++)
                    clickStates.Add(new ButtonState());
        }
#endif

        public void Set(ButtonState state)
        {
            state.image.TweenGraphicColor(state.color, duration).SetEase(EaseType.ExpoOut);
        }


        public void Normal()
        {
            if (persistHighlight) return;

            foreach (var state in normalStates)
            {
                Set(state);
            }
            
            // Tween slider value
            if (hasSlider)
            {
                this.TweenValueFloat(0, 0.2f, f =>
                {
                    slider.value = f;
                }).SetEase(EaseType.ExpoOut).SetFrom(slider.value);
            }
        }

        public void Highlight()
        {
            foreach (var state in highlightStates)
            {
                Set(state);
            }
            
            // Tween slider value
            if (hasSlider)
            {
                this.TweenValueFloat(1, 0.2f, f =>
                {
                    slider.value = f;
                }).SetEase(EaseType.ExpoOut);
            }
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            
            if (!interactable) return;
            
            Highlight();
            
            if (hasHoverSound) AudioManager.Play(onHoverAudio);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            
            if (!interactable || persistHighlight) return;

            foreach (var state in clickStates)
            {
                Set(state);
            }

            if (hasClickSound) AudioManager.Play(onClickAudio);
            Invoke(nameof(Normal), duration);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            
            if (!interactable) return;
            
            Normal();
        }
    }

    [Serializable]
    public class ButtonState
    {
        public Graphic image;
        public Color color;
    }
}