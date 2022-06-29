using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-10)]
public class MoveSceneManager : SingletonMonoBehaviour<MoveSceneManager>
{

	private GameManager gameManager;

	public string SceneName
	{
		get
		{
			return SceneManager.GetActiveScene().name;
		}
	}

	void Start()
	{
		gameManager = GameManager.Instance;

		//シーンが変わったらOnSceneLoadedが呼ばれる
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	//シーンのロード時に実行（最初は実行されない）
	protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		gameManager.InstantiateWhenLoadScene();
		gameManager.InitGame();
	}

	public void GoToTitle()
	{
		SceneManager.LoadScene("Title");
	}

	public void GoToMain()
	{
		SceneManager.LoadScene("Main");
	}

	public void LoadCurrentScene()
	{
		//現在のシーン番号を取得
		int sceneIndex = SceneManager.GetActiveScene().buildIndex;

		//現在のシーンをリセットする
		SceneManager.LoadScene(sceneIndex);
	}
}

