using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void Teleport()
    {
        if(sceneName != null)
        SceneManager.LoadScene(sceneName);
    }
}
