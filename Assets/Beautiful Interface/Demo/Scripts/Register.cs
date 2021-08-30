using Interface.Elements.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Demo.Scripts
{
    public class Register : BasePanel
    {
        private bool termsAccepted;

        public TextUI txtTitle;
        
        [Space] 
        
        public InputField inpUser;
        public InputField inpPass;
        public InputField inpRePass;

        [Space] 
        
        public Toggle togTerms;
        
        [Space]
        
        public Button btnContinue;
        public Button btnBack;

        private void Start()
        {
            togTerms.onValueChanged.AddListener(Terms);
            
            btnContinue.onClick.AddListener(Launch);
            btnBack.onClick.AddListener(Back);
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (inpUser.isFocused)
                {
                    inpPass.ActivateInputField();
                }
                else if (inpPass.isFocused)
                {
                    inpRePass.ActivateInputField();
                }
                else if (inpRePass.isFocused)
                {
                    btnContinue.Select();
                }
            }
        }

        private void Terms(bool accepted) => termsAccepted = accepted;

        private void ClearFields()
        {
            inpUser.text = "";
            inpPass.text = "";
            inpRePass.text = "";
        }

        private void Launch()
        {
            if (inpUser.text == "" || inpPass.text == "")
            {
                var description = "Fill in all the details".ToUpper();
                Error(description);
                return;
            }
            
            if (inpPass.text != inpRePass.text)
            {
                var description = "Password confirmation do not match".ToUpper();
                Error(description);
                return;
            }
            
            if (!termsAccepted)
            {
                var description = "Accept the terms and conditions".ToUpper();
                Error(description);
                return;
            }


            // Todo: send details to server and wait for callback
            // Remove this hardcoded callback
            var result = new Result
            {
                Success = true,
                Message = "",
                Data = new[] {inpUser.text, "EGA134"}
            };
            
            if (result.Success)
            {
                Success(result.Data);
            }
            else
            {
                Error(result.Message);
            }
        }

        private void Success(string[] data)
        {
            ClearFields();
            var title = "Registration successful".ToUpper();
            var description = ("You can log in now, <b>" + data[0] + "</b>").ToUpper();
            Notification.Show(title, description);
            AudioManager.Play(SoundEffects.Success);
            MainMenu.Instance.login.OnLoginSuccess += username =>
            {
                ShowResetCode(data[1]);
            };
        }

        private void Error(string message)
        {
            var title = "Cannot register".ToUpper();
            var description = message.ToUpper();
            Notification.Show(title, description);
            AudioManager.Play(SoundEffects.Error);
        }

        private void ShowResetCode(string code)
        {
            var title = "Reset Code".ToUpper();
            var description = ("Your Reset Code is <b>" + code + "</b>" +
                               "\nYou will need this to reset your password").ToUpper();
            Notification.Show(title, description, null, 5, NotifPosition.MidCenter);
        }

        private void Back()
        {
            Hide(CanvasSide.Left);
            MainMenu.Instance.login.Show(CanvasSide.Right);
        }

        public override void Show(CanvasSide side)
        {
            base.Show(side);
            txtTitle.StartAnimation();
        }

        
    }
}