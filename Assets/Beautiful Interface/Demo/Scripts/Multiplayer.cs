using System.Collections.Generic;
using Interface.Elements.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Demo.Scripts
{
    public class Multiplayer : MonoBehaviour
    {
        public Text textTeam;
        public Image imageTeam;
        public Text textMap;
        public Image imageMap;
        public ButtonGroup teamSelection;
        public ButtonGroup mapSelection;

        public List<Sprite> teams;
        public List<Sprite> maps;

        private void Awake()
        {
            teamSelection.OnSelected += OnTeamSelected;
            mapSelection.OnSelected += OnMapSelected;
        }

        private void OnTeamSelected(IList<int> selectedIndices)
        {
            textTeam.text = teams[selectedIndices[0]].name.ToUpper();
            imageTeam.sprite = teams[selectedIndices[0]];
        }

        private void OnMapSelected(IList<int> selectedIndices)
        {
            imageMap.sprite = this.maps[selectedIndices[0]];
            
            var maps = "";
            foreach (var i in selectedIndices)
            {
                maps += this.maps[i].name.ToUpper() + "\n";
            }

            textMap.text = maps;
        }
    }
}