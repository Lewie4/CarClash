using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugLoadLevel : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DebugOptions.LoadSceneAndSetActive(SceneManager.GetActiveScene().buildIndex + 1));
    }
}
