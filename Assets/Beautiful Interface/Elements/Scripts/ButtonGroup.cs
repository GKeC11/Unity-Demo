using System.Collections.Generic;
using UnityEngine;

namespace Interface.Elements.Scripts
{
    public class ButtonGroup : MonoBehaviour
    {
        [Tooltip("The buttons that are grouped together")]
        public List<ButtonUI> buttons;

        [Tooltip("The selected buttons index on start. Set empty for none selected")] 
        public List<int> selectedIndex = new List<int>();

        [Tooltip("Does this have multiple selection")]
        public bool multipleSelection;

        [Tooltip("Can have 0 selections")] 
        public bool canHaveNoSelection;

        
        /// <summary>
        /// Accessor for the selected index
        /// </summary>
        public List<int> SelectedIndex => selectedIndex;

        public delegate void OnSelectedDelegate(IList<int> selectedIndices);

        public OnSelectedDelegate OnSelected;

        private void Start()
        {
            for (var i = 0; i < buttons.Count; i++)
            {
                var index = i;
                buttons[index].onClick.AddListener(() => Select(index));
            }

            if (!canHaveNoSelection && selectedIndex.Count == 0) 
                selectedIndex.Add(0);


            foreach (var i in selectedIndex)
            {
                buttons[i].PersistHighlight = true;
            }
            
            OnSelected?.Invoke(selectedIndex);
        }

        public void Select(int index)
        {
            if (!multipleSelection)
            {
                foreach (var button in buttons)
                {
                    button.PersistHighlight = false;
                }
                selectedIndex.Clear();
            }
            
            if (selectedIndex.Contains(index))
            {
                if (canHaveNoSelection || selectedIndex.Count > 1)
                {
                    selectedIndex.Remove(index);
                    buttons[index].PersistHighlight = false;
                }
            }
            else
            {
                selectedIndex.Add(index);
                buttons[index].PersistHighlight = true;
            }
            
            OnSelected?.Invoke(selectedIndex);
        }
    }
}