using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {

    public static LevelManager instance = null;
    public float autoLoadNextSceneAfter;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        if (autoLoadNextSceneAfter <= 0) {
            Debug.Log("Level auto load disabled, use a postive number is seconds");
        } else {
            Invoke("LoadNextScene", autoLoadNextSceneAfter);
        }
    }

	public void LoadNextScene() {
		Debug.Log ("----- Loading Next Scene -----");
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }

	
    public void LoadScene(string sceneName) {

		Debug.Log ("----- Loading " + sceneName + " Scene -----");
        SceneManager.LoadScene(sceneName);

		Scene nextSceneInstance = SceneManager.GetSceneByName (sceneName);
		Debug.Log ("nextSceneInstance.buildIndex: " + nextSceneInstance.buildIndex);
		
    }

	

    public void LoadPreviousScene() {

		Debug.Log ("----- Loading Previous Scene -----");
        int currentIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex - 1);
		
    }

    public void Quit() {

        Application.Quit();
    }

		
}
