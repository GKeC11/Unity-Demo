using Interface.Elements.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Demo.Scripts
{
    public class Reset : BasePanel
    {
        public TextUI txtTitle;
        
        [Space] 
        
        public InputField inpCode;
        public InputField inpNewPass;

        [Space] 
        
        public Button btnContinue;
        public Button btnBack;

        private void Start()
        {
            btnContinue.onClick.AddListener(Launch);
            btnBack.onClick.AddListener(Launch);
        }

        private void Launch()
        {
            Hide(CanvasSide.Left);

            var code = inpCode.text;
            var pass = inpNewPass.text;
            
            MainMenu.Instance.login.Show(CanvasSide.Right);
        }

        public override void Show(CanvasSide side)
        {
            base.Show(side);
            txtTitle.StartAnimation();
        }
    }
}