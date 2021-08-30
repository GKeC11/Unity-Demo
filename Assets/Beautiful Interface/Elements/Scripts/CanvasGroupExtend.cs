using System;
using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using UnityEngine;

namespace Interface.Elements.Scripts
{
    /// <summary>
    /// Animate Menu Canvas groups using Tween
    /// </summary>
    public static class CanvasGroupExtend
    {
        /// <summary>
        /// Fades in CanvasGroup
        /// </summary>
        /// <param name="canvasGroup"></param>
        /// <param name="duration"></param>
        /// <param name="ease">Recommended: ExpoOut</param>
        /// <param name="affectInteraction"></param>
        public static void Show(this CanvasGroup canvasGroup, float duration, EaseType ease, bool affectInteraction = true)
        {
            canvasGroup.TweenValueFloat(1, duration, f => canvasGroup.alpha = f).SetEase(ease)
                .SetFrom(canvasGroup.alpha);
            if (affectInteraction)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        /// <summary>
        /// Fades out CanvasGroup
        /// </summary>
        /// <param name="canvasGroup"></param>
        /// <param name="duration"></param>
        /// <param name="ease">Recommended: ExpoOut</param>
        /// <param name="affectInteraction"></param>
        public static void Hide(this CanvasGroup canvasGroup, float duration, EaseType ease, bool affectInteraction = true)
        {
            canvasGroup.TweenValueFloat(0, duration, f => canvasGroup.alpha = f).SetEase(ease)
                .SetFrom(canvasGroup.alpha);

            if (affectInteraction)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }


        /// <summary>
        /// Fades in canvas group along with resizing localscale from 0.75 to 1
        /// </summary>
        /// <param name="cg"></param>
        /// <param name="duration"></param>
        public static void Show(this CanvasGroup cg, float duration = 0.2f)
        {
            cg.Show(duration, EaseType.ExpoOut);
            
            cg.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            cg.transform.TweenLocalScale(Vector3.one, duration).SetEase(EaseType.QuadOut);
        }

        /// <summary>
        /// Fades out canvas group along with resizing localscale from 1 to 0.75
        /// </summary>
        /// <param name="cg"></param>
        /// <param name="duration"></param>
        public static void Hide(this CanvasGroup cg, float duration = 0.2f)
        {
            cg.Hide(duration, EaseType.ExpoOut);
            
            var endValue = new Vector3(0.75f, 0.75f, 0.75f);
            cg.transform.TweenLocalScale(endValue, duration).SetEase(EaseType.QuadIn);
        }


        /// <summary>
        /// Fades in canvas group along with moving on the axis by 100 units
        /// </summary>
        /// <param name="cg"></param>
        /// <param name="side">The side from where the panel will fade in</param>
        /// <param name="duration">The time to fade in</param>
        public static void Show(this CanvasGroup cg, CanvasSide side, float duration = 0.5f)
        {
            const float offset = 150;
            var pos = cg.transform.localPosition;
            float value;
            switch (side)
            {
                case CanvasSide.Left:
                    value = -offset;
                    break;
                case CanvasSide.Right:
                    value = offset;
                    break;
                case CanvasSide.Centre:
                    value = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }

            cg.alpha = 0;
            cg.Show(duration, EaseType.ExpoOut);

            cg.transform.localPosition = new Vector3(pos.x + value, pos.y, pos.z);
            cg.transform.TweenLocalPositionX(0, duration).SetEase(EaseType.QuadOut);
        }

        /// <summary>
        /// Fades out canvas group along with moving on the axis by 100 units
        /// </summary>
        /// <param name="cg"></param>
        /// <param name="side">The side to where the panel will fade out</param>
        /// <param name="duration">The time to fade out</param>
        public static void Hide(this CanvasGroup cg, CanvasSide side, float duration = 0.5f)
        {
            const float offset = 150;
            var pos = cg.transform.localPosition;
            float value;
            switch (side)
            {
                case CanvasSide.Left:
                    value = -offset;
                    break;
                case CanvasSide.Right:
                    value = offset;
                    break;
                case CanvasSide.Centre:
                    value = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }

            cg.Hide(duration, EaseType.ExpoOut);

            cg.transform.TweenLocalPositionX(pos.x + value, duration).SetEase(EaseType.QuadOut);
        }
    }

    public enum CanvasSide
    {
        Left, Right, Centre
    }
}