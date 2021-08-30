using Interface.Elements.Scripts;
using UnityEngine;

namespace Interface.Demo.Scripts
{
    
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// Exit notification ID. Used to hide the notification after response
        /// </summary>
        private int exitNotifID;

        [Tooltip("The main game menu to set active when user is authenitcated. Must have the CanvasGroup component")]
        public CanvasGroup gameMenu;

        [Tooltip("The title menu")]
        public Title title;
        
        [Tooltip("The login screen")]
        public Login login;
        
        [Tooltip("The register screen")]
        public Register register;
        
        [Tooltip("The reset password scree")]
        public Reset reset;

        public static MainMenu Instance;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        private void Start()
        {
            login.OnLoginSuccess += ShowGameMenu;
            login.OnLogout += HideGameMenu;
            gameMenu.Hide();
            
            if (title)
            {
                title.Show();
            }
            else if (login)
            {
                login.Show();
            }
            else
            {
                gameMenu.Show();
            }
        }

        public void Logout()
        {
            AudioManager.Play(SoundEffects.Logout);
            login.Logout();
        }

        private void ShowGameMenu(string username)
        {
            if (login) login.Hide();
            if (gameMenu) gameMenu.Show();
        }

        private void HideGameMenu()
        {
            if (login) login.Show(CanvasSide.Centre);
            if (gameMenu) gameMenu.Hide();
        }

        public void Exit()
        {
            var title = "Exit Game".ToUpper();
            var description = "Are you sure you want to exit".ToUpper();
            exitNotifID = Notification.Show(title, description, null,
                20, NotifPosition.MidCenter,
                NotifStyle.Rectangle, false, Color.clear, true,
                () => ExitResponse(true), () => ExitResponse(false), "EXIT");
            
            ExitResponse(true);
        }

        /// <summary>
        /// Response to exit notification
        /// </summary>
        /// <param name="response"></param>
        private void ExitResponse(bool response)
        {
            Notification.BackgroundClicked();
            Notification.Destroy(exitNotifID);
            if (response)
            {
                Application.Quit();
            }
        }
    }
    
    public struct Result
    {
        public bool Success;
        public string Message;
        public string[] Data;
    }
}