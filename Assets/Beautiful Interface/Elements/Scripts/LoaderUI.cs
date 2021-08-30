using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Elements.Scripts
{
    public class LoaderUI : MonoBehaviour
    {
        public Image loaderImage;
        public Text loaderText;
        
        [InspectorName("Start Playing")] 
        public bool isPlaying;

        public EaseType easeStart = EaseType.QuadIn;
        public EaseType easeEnd = EaseType.QuadOut;


        private void Start()
        {
            PlayAnimation(isPlaying);
        }

        private void PlayAnimation(bool isPlaying)
        {
            if (!isPlaying)
            {
                StopAnimation();
            }
            else
            {
                PlayAnimation();
            }
        }

        public void PlayAnimation()
        {
            isPlaying = true;
            var clockwise = loaderImage.fillClockwise;
            var duration = 1;
            if (loaderText) loaderText.color += Color.black;

            loaderImage.TweenImageFillAmount(1, duration).SetFrom(0).SetEase(easeStart)
                .SetOnComplete(() =>
                {
                    loaderImage.fillClockwise = !clockwise;
                    loaderImage.TweenImageFillAmount(0, duration).SetFrom(1).SetEase(easeEnd)
                        .SetOnComplete(() =>
                        {
                            loaderImage.fillClockwise = clockwise;
                            PlayAnimation(isPlaying);
                        });
                });

            this.TweenValueFloat(360, 2,
                f => loaderImage.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, f)));
        }

        public void StopAnimation()
        {
            isPlaying = false;
            loaderImage.fillAmount = 0;
            if (loaderText) loaderText.color -= Color.black;
        }
    }
}