using System.Collections.Generic;
using Interface.Elements.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Editor
{
    public static class ReplaceElements
    {
        [MenuItem("Tools/Beautiful Interface/Upgrade/All")]
        public static void UpgradeAll()
        {
            UpdgradeTexts();
            UpdgradeButtons();
            UpgradeInputs();
            Debug.Log("Upgrade all finished");
        }
        
        [MenuItem("Tools/Beautiful Interface/Upgrade/Texts Only")]
        public static void UpdgradeTexts()
        {
            var count = UpgradeConfirmed<Text, TextUI>();
            Debug.Log("Upgraded " + count + " Texts");
        }
        
        [MenuItem("Tools/Beautiful Interface/Upgrade/Buttons Only")]
        public static void UpdgradeButtons()
        {
            var count = UpgradeConfirmed<Button, ButtonUI>();
            Debug.Log("Upgraded " + count + " Buttons");
        }
        
        [MenuItem("Tools/Beautiful Interface/Upgrade/Input Fields Only")]
        public static void UpgradeInputs()
        {
            var count = UpgradeConfirmed<InputField, InputUI>();
            Debug.Log("Upgraded " + count + " Input Fields");
        }

        
        [MenuItem("Tools/Beautiful Interface/Revert/All")]
        public static void RevertAll()
        {
            RevertTexts();
            RevertButtons();
            RevertInputs();
        }
        
        [MenuItem("Tools/Beautiful Interface/Revert/Texts Only")]
        public static void RevertTexts()
        {
            var count = UpgradeConfirmed<TextUI, Text>();
            Debug.Log("Reverted " + count + " Texts");
        }
        
        [MenuItem("Tools/Beautiful Interface/Revert/Buttons Only")]
        public static void RevertButtons()
        {
            var count = UpgradeConfirmed<ButtonUI, Button>();
            Debug.Log("Reverted " + count + " Buttons");
        }
        
        [MenuItem("Tools/Beautiful Interface/Revert/Input Fields Only")]
        public static void RevertInputs()
        {
            var count = UpgradeConfirmed<InputUI, InputField>();
            Debug.Log("Reverted " + count + " Input Fields");
        }
        
        
        
        /// <summary>
        /// Remove the TExisting component and replace it with TNew
        /// TExisting must be a parent or same type of TNew
        /// </summary>
        /// <typeparam name="TExisting"></typeparam>
        /// <typeparam name="TNew"></typeparam>
        /// <returns></returns>
        private static int UpgradeConfirmed<TExisting, TNew>() where TExisting: MonoBehaviour where TNew: MonoBehaviour
        {
            var texts = GetComponents<TExisting>();
            foreach (var tComp in texts)
            {
                var newObject = GameObject.Instantiate(tComp.gameObject, tComp.transform.parent);
                newObject.transform.SetSiblingIndex(tComp.transform.GetSiblingIndex());
                
                Undo.DestroyObjectImmediate(newObject.GetComponent<TExisting>());
                var newTComp = Undo.AddComponent<TNew>(newObject);
                PropertyCopier<TExisting, TNew>.Copy(tComp, newTComp);
                GameObject.DestroyImmediate(tComp.gameObject);
            }

            return texts.Count;
        }

        
        /// <summary>
        /// Get the root objects in the scene
        /// </summary>
        /// <returns></returns>
        private static GameObject[] GetRootObjects()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        }
        
        /// <summary>
        /// Get all the components of Type T in the scene
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static List<T> GetComponents<T>()
        {
            var comps = new List<T>();
            var gameObjects = GetRootObjects();
            foreach (var o in gameObjects)
            {
                comps.AddRange(GetComponentsAux<T>(o.transform));
            }
            
            return comps;
        }
        
        /// <summary>
        /// Get all the components of Type T under parent gameobject
        /// </summary>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static List<T> GetComponentsAux<T>(Transform parent)
        {
            var comps = new List<T>();
            foreach (Transform child in parent)
            {
                var c = child.GetComponent<T>();
                if (c != null) comps.Add(c);

                comps.AddRange(GetComponentsAux<T>(child));
            }
            
            return comps;
        }
        
        
        /// <summary>
        /// Copy the properties of one Component to another
        /// </summary>
        /// <typeparam name="TParent">The parent copy FROM</typeparam>
        /// <typeparam name="TChild">The child copy TO</typeparam>
        public static class PropertyCopier<TParent, TChild> where TParent : MonoBehaviour where TChild : MonoBehaviour
        {
            public static void Copy(TParent parent, TChild child)
            {
                var parentProperties = parent.GetType().GetProperties();
                var childProperties = child.GetType().GetProperties();

                foreach (var parentProperty in parentProperties)
                {
                    foreach (var childProperty in childProperties)
                    {
                        if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                        {
                            if (childProperty.CanWrite)
                                childProperty.SetValue(child, parentProperty.GetValue(parent));
                            break;
                        }
                    }
                }
            }
        }
    }
}