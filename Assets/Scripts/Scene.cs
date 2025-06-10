using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    // Start is called before the first frame update
    public void playGame()
    {
        string selectedPlayer = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        if (selectedPlayer == "P1") GameManager.instance.index = 0;
        else if (selectedPlayer == "P2") GameManager.instance.index = 1;
        
        SceneManager.LoadScene("SampleScene");
    }
}
