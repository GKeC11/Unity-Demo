using System.Collections;
using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using Interface.Elements.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Demo.Scripts
{
    public class Title : BasePanel
    {
        [Tooltip("Wait for user to click to proceed from the title screen")]
        public bool clickToContinue = true;
        
        [Tooltip("The text hint to display when ClickToContinue is enabled")]
        public Text textClickToContinue;
        
        [Tooltip("The title image to display at Title screen. Can be the Game Title image")]
        public Image imageTitle;


        private void Start()
        {
            MainMenu.Instance.login.OnLoginSuccess += username => Hide();
        }


        public void Show()
        {
            Show(CanvasSide.Centre);
            if (imageTitle)
            {
                if (textClickToContinue)
                {
                    var color = textClickToContinue.color;
                    textClickToContinue.color = new Color(color.r, color.g, color.b, 0);
                }
                imageTitle.color = Color.clear;
                imageTitle.transform.localPosition = Vector3.down * 200;
                imageTitle.TweenGraphicColor(Color.white, 1);
                imageTitle.TweenLocalPosition(Vector3.zero, 1).SetEase(EaseType.ExpoOut);
                StartCoroutine(ShowAccount());
            }
            else
            {
                Hide();
            }
        }

        public void Hide()
        {
            if (imageTitle) imageTitle.TweenGraphicAlpha(0, 0.5f);
            if (textClickToContinue) textClickToContinue.TweenGraphicAlpha(0, 0.5f);
            MainMenu.Instance.login.Show();
        }

        private IEnumerator ShowAccount()
        {
            yield return new WaitForSeconds(1);
            
            if (clickToContinue)
            {
                if (textClickToContinue)
                    textClickToContinue.TweenGraphicAlpha(1, 0.1f);
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            }
            else
            {
                yield return new WaitForSeconds(3);
            }
            
            Hide();
        }
    }
}