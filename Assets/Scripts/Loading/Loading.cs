using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public string LoadToSceneName;

    public Slider progressbar;
    public TMPro.TMP_Text loadText;
    private void Start()
    {
        LoadToSceneName = PlayerPrefs.GetString("SceneToLoad");
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(LoadToSceneName);
        operation.allowSceneActivation = true;

        while(!operation.isDone)
        {
            yield return null;
            if(progressbar.value < 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }
            else if(operation.progress >= 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1.0f, Time.deltaTime);
            }
            if(progressbar.value >= 1f)
            {
                loadText.text = "로딩 완료..!";
            }
            if (Input.GetKeyDown(KeyCode.Space) && progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                //operation.allowSceneActivation = true;
            }
        }
    }
}
