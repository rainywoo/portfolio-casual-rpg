using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;
    public string exitName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerPrefs.SetString("LastExitName", exitName);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.V))
        //{
        //    SceneManager.LoadScene("DemoScene");
        //}
    }
}
