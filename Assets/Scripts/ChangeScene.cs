using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	public void GoToScene(string sceneName) {
		SceneManager.LoadScene(sceneName);
	}
}
