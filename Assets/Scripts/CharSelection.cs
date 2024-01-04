using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelection : MonoBehaviour
{
    public void SelectChar(int num)
    {
        PlayerPrefs.SetInt("char", num);
        SceneManager.LoadScene(1);
    }
}
