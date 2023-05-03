using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _mainBtn;

    private Animator _anim;

    private bool _isEnter;

    public GameObject namePanel;
    public InputField nameInput;
    public Button confirmBtn;

    private void Awake()
    {
        _mainBtn = GetComponent<Button>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _mainBtn.onClick.AddListener(OnBtnClick);
        confirmBtn.onClick.AddListener(OnConfirmClick);
    }

    public void OnBtnClick()
    {
        if (PlayFabLogin.Instance.playerName == string.Empty)
        {
            namePanel.SetActive(true);
        }
        else
        {
            namePanel.SetActive(false);
            TransitionManager.instance.TransitionTo("GamePlay");
        }
    }

    public void OnConfirmClick()
    {
        PlayFabLogin.Instance.SubmitName(nameInput.text);
        namePanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isEnter = true;
        _anim.SetBool("isEnter", _isEnter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isEnter = false;
        _anim.SetBool("isEnter", _isEnter);
    }
}
