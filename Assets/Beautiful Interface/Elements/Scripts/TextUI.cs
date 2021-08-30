using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Interface.Elements.Scripts
{
    public class TextUI : Text
    {
        private bool isAnimating;
        private Coroutine animRoutine;
        private readonly char[] randomChars = {'#','%','$','&','A','G','R','F','T','P'};

        /// <summary>
        /// The text is capitalized
        /// </summary>
        [SerializeField] private bool isCapitalized;

        public bool Capitalize
        {
            get => isCapitalized;
            set
            {
                isCapitalized = value;
                UpdateText();
            }
        }

        /// <summary>
        /// The original unchanged text
        /// </summary>
        private string originalText = "";

        public override string text
        {
            get => base.text;
            set
            {
                originalText = value;
                UpdateText();
            }
        }
        
        [Header("Animation")]
        
        [Tooltip("Animate the text to look as if it is typing")]
        public bool animateTypingOnStart;
        [Tooltip("Adds a trailing underscore while animating")]
        public bool addTrailingUnderscore;
        [Tooltip("Adds random characters while animating")]
        public bool addRandomCharacters;

        [Tooltip("The speed at which the typing animation plays")] 
        public float typingSpeed = 12;

        [Header("Sounds")] 
        public AudioClip typingSound;


#if UNITY_EDITOR
        
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (!string.Equals(text, originalText, StringComparison.CurrentCultureIgnoreCase))
            {
                originalText = text;
            }
        }
        
#endif
        

        protected override void Start()
        {
            if (Application.isPlaying)
            {
                if (animateTypingOnStart)
                    StartAnimation();
            }
        }
        
        

        private void UpdateText()
        {
            // Make sure you are setting the base text (to prevent infinite recursion)
            base.text = Capitalize ? originalText.ToUpper() : originalText;
        }

        
        public void StartAnimation()
        {
            if (isAnimating)
            {
                // Stop the previous animation if already animating
                isAnimating = false;
                if (animRoutine != null) StopCoroutine(animRoutine);
                UpdateText();
            }

            animRoutine = StartCoroutine(AnimateTypingEffect(typingSpeed));
        }

        private IEnumerator AnimateTypingEffect(float speed)
        {
            isAnimating = true;
            
            var original = text;
            var modified = new StringBuilder();
            base.text = "";
            var i = 0;

            foreach (var c in original)
            {
                modified.Append(" ");
            }

            modified.Append(" ");
            if (addRandomCharacters && addTrailingUnderscore) modified.Append(" ");
            var time = 1 / speed;
            
            while (i < original.Length)
            {
                modified[i] = original[i];
                var incr = 1;
                if (addTrailingUnderscore) modified[i + incr++] = '_';
                if (addRandomCharacters) modified[i + incr] = randomChars[Random.Range(0, randomChars.Length)];
                base.text = modified.ToString();
                
                if (addRandomCharacters)
                {
                    float counter = 0;
                    while (counter < time)
                    {
                        modified[i + incr] = randomChars[Random.Range(0, randomChars.Length)];
                        base.text = modified.ToString();
                        yield return new WaitForSeconds(0.02f);
                        counter += 0.02f;
                    }
                }
                else
                {
                    yield return new WaitForSeconds(time);
                }

                AudioManager.Play(typingSound);
                i++;
            }

            if (addTrailingUnderscore) modified.Remove(i, 1);
            if (addRandomCharacters) modified.Remove(i, 1);
            base.text = modified.ToString();

            isAnimating = false;
        }
    }
}