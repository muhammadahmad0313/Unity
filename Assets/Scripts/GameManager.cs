using System;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour

{

    public static GameManager instance;


    [SerializeField] private GameObject[] players;


    public int index;


    public void Awake()

    {

        if (!instance)

        {

            instance = this;

            DontDestroyOnLoad(gameObject);

        }

        else

            Destroy(gameObject);

    }


    private void OnEnable()

    {

        SceneManager.sceneLoaded += SceneLoadedFunction;

    }

    private void SceneLoadedFunction(UnityEngine.SceneManagement.Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "SampleScene")

        {

            Instantiate(players[index]);

        }
    }


    private void OnDisable()

    {

        SceneManager.sceneLoaded -= SceneLoadedFunction;

    }


    

}