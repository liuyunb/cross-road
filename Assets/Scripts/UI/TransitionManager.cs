using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;
    
    private CanvasGroup _cg;

    public Text playerName;

    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(this);
    }

    public void TransitionTo(string sceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        playerName.text = PlayFabLogin.Instance.playerName + " 你好呀！";
        
        yield return Fade(1);

        yield return SceneManager.LoadSceneAsync(sceneName);

        yield return Fade(0);
    }
    
    IEnumerator Fade(int amount)
    {
        _cg.blocksRaycasts = true;
        float flag = Mathf.Sign(amount - _cg.alpha);
        while (!Mathf.Approximately(amount, _cg.alpha))
        {
            // _cg.alpha = Mathf.Lerp(_cg.alpha, amount, 0.1f);
            _cg.alpha += Time.deltaTime * flag;
            yield return null;
        }

        _cg.blocksRaycasts = false;
    }
}
