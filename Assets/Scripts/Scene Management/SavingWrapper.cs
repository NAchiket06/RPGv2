using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {

        const string defaultSaveFile = "save1";


        //private void Awake()
        //{
        //   StartCoroutine(LoadLastScene());
        //}

        //IEnumerator LoadLastScene()
        //{
        //    Fader fader = FindObjectOfType<Fader>();
        //    fader.FadeOutImmediate();
        //    yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        //    //yield return null;
        //    yield return fader.FadeIn(0.2f);
        //}
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        { 
            GetComponent<SavingSystem>().Load(defaultSaveFile);

        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }

}