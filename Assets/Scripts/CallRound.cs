using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class CallRound : MonoBehaviour
{
    //[SerializeField]private int _numberOfRounds;
    [SerializeField]private CallRoundScriptableObjects _callRoundScriptableObjects;
    [SerializeField] private Canvas _canvas;
    [SerializeField]private Player _player;

    private bool _isTheRoundChosen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        _player = FindFirstObjectByType<Player>();
        if(_canvas!=null)
            _canvas.gameObject.SetActive(false);
        
        for (int i = 0; i < _callRoundScriptableObjects._callRounds.Count; i++)
        {
            int index = i; 
            _callRoundScriptableObjects._callRounds[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isTheRoundChosen && Input.GetKeyDown(KeyCode.Space))
        {
            _canvas.gameObject.SetActive(!_canvas.gameObject.activeSelf);
        }
    }
    
    public void OnButtonClicked(int index)
    {
        DoActionRound(index);
        Debug.Log("Button clicked"+index+2);
        _isTheRoundChosen = true;

    }

    public void DoActionRound(int index)
    {
        _player.Call = index+2;
        Debug.Log(_player.Call);
        _canvas.gameObject.SetActive(false);
        
    }  
    
    
}
