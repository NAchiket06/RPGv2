using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        Coroutine CurrentActiveFadeOut = null;
        Coroutine currentActiveFadeIn = null;


        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        private void Start()
        {
        }

        IEnumerator FadeOutIn()
        {
            yield return FadeOut(3f);
            print("Faded out");
            yield return FadeIn(1f);
            print("Faded in");
        }



        public IEnumerator FadeOut(float time)
        {

            if(CurrentActiveFadeOut != null)
            {
                StopCoroutine(CurrentActiveFadeOut);
            }
            CurrentActiveFadeOut = StartCoroutine(FadeOutRoutine(time));

            yield return CurrentActiveFadeOut;
        }

        private IEnumerator FadeOutRoutine(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }


        public IEnumerator FadeIn(float time)
        {
           
            if (currentActiveFadeIn != null)
            {
                StopCoroutine(currentActiveFadeIn);
            }

            currentActiveFadeIn = StartCoroutine(FadeInRoutine(time));

            yield return currentActiveFadeIn;
        }
        private IEnumerator FadeInRoutine(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }
    }
}