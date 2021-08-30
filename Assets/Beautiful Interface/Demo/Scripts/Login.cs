using System;
using System.Collections.Generic;
using Interface.Elements.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Demo.Scripts
{
    public class Login : BasePanel
    {
        /// <summary>
        /// The PlayerPrefs key for checking whether there are saved credentials 
        /// </summary>
        private const string SaveKey = "SavedCredentials";
        /// <summary>
        /// The PlayerPrefs key for saving username 
        /// </summary>
        private const string UserKey = "SavedUsername";
        /// <summary>
        /// The PlayerPrefs key for saving the secure login token
        /// </summary>
        private const string TokenKey = "SavedToken";
        
        private bool saveDetails = true;
        public static bool IsLoggedIn;
        
        public TextUI txtTitle;
        
        [Space]
        
        public InputField inpUser;
        public InputField inpPass;

        [Space]
        
        public Toggle togSave;
        
        [Space]
        
        public Button btnContinue;
        public Button btnForgot;
        public Button btnRegister;
        
        
        public delegate void SimpleDelegate();
        public delegate void StringDelegate(string username);
        public event StringDelegate OnLoginSuccess;
        public event SimpleDelegate OnLogout;



        private void Start()
        {
            
            togSave.onValueChanged.AddListener(Save);

            btnContinue.onClick.AddListener(Continue);
            btnForgot.onClick.AddListener(Forgot);
            btnRegister.onClick.AddListener(NoAccount);

            // Auto login after a brief delay, for all connections to finish connecting
            Invoke(nameof(Autologin), 1);
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
                    btnContinue.Select();
                }
            }
        }

        private void Save(bool value) => saveDetails = value;

        /// <summary>
        /// Try to auto login
        /// </summary>
        private void Autologin()
        {
            var saveDetails = GetSavedLogin(out var user, out var pass);
            if (saveDetails)
            {
                this.saveDetails = true;
                Launch(user, pass);
            }
        }
        
        private void ClearFields()
        {
            inpUser.text = "";
            inpPass.text = "";
        }

        private void Continue()
        {
            Launch(inpUser.text, inpPass.text);
        }

        private void Forgot()
        {
            Hide(CanvasSide.Right);
            MainMenu.Instance.reset.Show(CanvasSide.Left);
        }

        private void NoAccount()
        {
            Hide(CanvasSide.Right);
            MainMenu.Instance.register.Show(CanvasSide.Left);
        }

        public void Show()
        {
            if (IsLoggedIn) return;
            cg.Show();
        }
        
        public override void Show(CanvasSide side)
        {
            base.Show(side);
            txtTitle.StartAnimation();
        }

        public void Hide()
        {
            Hide(CanvasSide.Centre);
        }

        public void Launch(string username, string password)
        {
            if (IsLoggedIn) return;
            if (string.IsNullOrWhiteSpace(username))
            {
                var description = "Fill in all the details".ToUpper();
                Error(description);
                return;
            }
            
            // Todo: send details to server and wait for callback
            // Remove this hardcoded callback
            // You should save a session token (rather than clear password text) for security
            var token = password;
            var result = new Result
            {
                Success = true,
                Message = "",
                Data = new[] {username, token}
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

        private void Success(IReadOnlyList<string> data)
        {
            ClearFields();
            
            var username = data[0];
            var token = data[1];
            SetSavedLogin(saveDetails, username, token);
            
            var title = "Login successful".ToUpper();
            var description = ("Welcome <b>" + username + "</b>").ToUpper();
            Notification.Show(title, description);
            AudioManager.Play(SoundEffects.Success);

            IsLoggedIn = true;
            OnLoginSuccess?.Invoke(username);
        }

        private void Error(string message)
        {
            var title = "Cannot login".ToUpper();
            var description = message.ToUpper();
            Notification.Show(title, description);
            AudioManager.Play(SoundEffects.Error);
        }

        public void Logout()
        {
            IsLoggedIn = false;
            SetSavedLogin(false);
            OnLogout?.Invoke();
        }
        
        /// <summary>
        /// Save Login credentials in the playerprefs
        /// </summary>
        /// <param name="save">Enable saving credentials</param>
        /// <param name="user">The username</param>
        /// <param name="secure">The secure token</param>
        private void SetSavedLogin(bool save, string user = "", string secure = "")
        {
            PlayerPrefs.SetString(SaveKey, save.ToString());
            if (user == "" && secure == "")
                return;
            
            PlayerPrefs.SetString(UserKey, user);
            PlayerPrefs.SetString(TokenKey, secure);
        }
        
        private bool GetSavedLogin(out string user, out string pass)
        {
            user = PlayerPrefs.GetString(UserKey);
            pass = PlayerPrefs.GetString(TokenKey);
            return Convert.ToBoolean(PlayerPrefs.GetString(SaveKey, "false"));
        }
    }
}