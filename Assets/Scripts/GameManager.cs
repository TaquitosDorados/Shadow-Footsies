using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] playerChar;
    public GameObject[] AIChar;
    void Start()
    {
        if(PlayerPrefs.GetInt("char") == 1)
        {
            Instantiate(playerChar[0]);
            Instantiate(AIChar[1]);
        } else
        {
            Instantiate(playerChar[1]);
            Instantiate(AIChar[0]);
        }
    }
}
