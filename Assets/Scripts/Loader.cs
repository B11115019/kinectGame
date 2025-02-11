using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class Loader
{
	public enum Scene
	{
		GameScene,
		Loading,
		Begin,
	}
	private class LoadingMonoBehaviour : MonoBehaviour { };
	private static Action onLoaderCallback;
	private static AsyncOperation loadingAsyncOperation;

	private static Scene curScene = Scene.Begin;
	public static Scene CurScene { get => curScene; }
    public static void Load(Scene scene)
	{
		onLoaderCallback = () =>
		{
			GameObject loadingGameObject = new GameObject("Loading Game Object");
			loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
		};
		SceneManager.LoadScene(Scene.Loading.ToString());
	}

	private static IEnumerator LoadSceneAsync(Scene scene)
	{
		yield return null;
		loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
		curScene = scene;
		while (!loadingAsyncOperation.isDone)
		{
			yield return null;
		}
	}

	public static float GetLoadingProgress()
	{
		if(loadingAsyncOperation != null)
		{
			return loadingAsyncOperation.progress;
		}
		return 0f;
	}

	public static void LoaderCallback()
	{
		if(onLoaderCallback != null)
		{
			onLoaderCallback();
			onLoaderCallback = null;
		}
	}
}
