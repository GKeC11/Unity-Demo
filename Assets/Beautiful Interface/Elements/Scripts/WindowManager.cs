using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Elements.Scripts
{
    public class WindowManager : MonoBehaviour
    {
        [Header("Button List")]
        [Tooltip("The buttons corresponding to each window index")]
        public List<Button> buttons = new List<Button>();
        
        [Header("Panel List")]
        [Tooltip("The windows corresponding to each button index")]
        public List<CanvasGroup> windows = new List<CanvasGroup>();


        [Header("Title List")] 
        [Tooltip("The title to display (leave blank if not applicable)")]
        public TextUI titleText;
        
        [Tooltip("The titles corresponding to each button index")]
        public List<string> titles = new List<string>();
        
        
        private CanvasGroup currentPanel;
        private CanvasGroup nextPanel;

        [Header("Settings")]
        [Tooltip("The starting panel index. Set to -1 to hide all in the beginning")]
        public int currentPanelIndex = -1;

        private void Start()
        {
            Load();
            
            // Hide all panels in the beginning
            Hide();
            
            if (currentPanelIndex != -1)
                Show();
        }

        private void Load()
        {
            if (buttons.Count != windows.Count)
            {
                Debug.LogError("The window count does not match buttons");
                return;
            }
            
            for (var i = 0; i < windows.Count; i++)
            {
                var panelIndex = i;
                buttons[i].onClick.AddListener(() => PanelAnim(panelIndex));
            }
        }


        public void Show()
        {
            var temp = currentPanelIndex;
            currentPanelIndex = -1;
            PanelAnim(temp);
        }
        
        public void Hide()
        {
            foreach (var window in windows)
            {
                window.Hide(0);
            }
        }

        public void PanelAnim(int newPanel)
        {
            if (newPanel == -1) return;
            if (windows.Count == 0) return;

            if (currentPanelIndex != -1)
            {
                if (buttons[currentPanelIndex].GetType() == typeof(ButtonUI))
                    ((ButtonUI) buttons[currentPanelIndex]).Normal();
                buttons[currentPanelIndex].interactable = true;
            }

            if (buttons[newPanel].GetType() == typeof(ButtonUI))
                ((ButtonUI) buttons[newPanel]).Highlight();
            buttons[newPanel].interactable = false;

            if (newPanel != currentPanelIndex)
            {
                if (currentPanelIndex != -1)
                    currentPanel = windows[currentPanelIndex];

                currentPanelIndex = newPanel;
                nextPanel = windows[currentPanelIndex];

                if (titleText)
                {
                    titleText.text = titles[currentPanelIndex];
                    titleText.StartAnimation();
                }
                
                if (currentPanel) currentPanel.Hide();
                if (nextPanel) nextPanel.Show();
            }
        }
    }
}