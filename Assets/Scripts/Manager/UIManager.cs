using Manager.Utils;
using UnityEngine;
using Utils;

namespace Manager
{
    public class UIManager : Singleton<UIManager>, IManager
    {

        public GameObject _gameplayUIPrefab;

        public void Initialize()
        {
            var gameplayUI = Instantiate(_gameplayUIPrefab);
            
            GameplayManager._instance._gameplayUI = gameplayUI;
        }
    }
}
