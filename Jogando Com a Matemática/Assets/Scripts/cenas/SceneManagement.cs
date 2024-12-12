using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : Singleton<SceneManagement>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject); // Persiste o objeto entre cenas
    }

    public string SceneTransitionName { get; private set; }

    public void SetTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
    }
}
