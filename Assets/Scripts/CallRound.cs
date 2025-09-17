
using UnityEngine;
public class CallRound : MonoBehaviour
{
    //[SerializeField]private int _numberOfRounds;
    [SerializeField]private CallRoundScriptableObjects callRoundScriptableObjects;
    [SerializeField] private Canvas _canvas;
    private HumanPlayer _humanPlayer;
    private bool isTheRoundChosen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
 
        for (int i = 0; i < callRoundScriptableObjects.callRounds.Count; i++)
        {
            int index = i;
            callRoundScriptableObjects.callRounds[i].onClick.AddListener(()=>OnButtonClicked(index));
        } 
        // _humanPlayer = FindFirstObjectByType<HumanPlayer>();

        //
        // for (int i = 0; i < callRoundScriptableObjects.callRounds.Count; i++)
        // {
        //     int index = i; 
        //     callRoundScriptableObjects.callRounds[i].onClick.AddListener(() => OnButtonClicked(index));
        // }
    }

    // Update is called once per frame
  
    public void Show()
    {
        if(_canvas!=null)   _canvas.gameObject.SetActive(true);
       // _isTheRoundChosen = false;
            //Time.timeScale = 0f;

        
        Debug.Log("Call Round Showing");
    }

    public void Hide()
    {
        //Time.timeScale = 1f;
        if(_canvas!=null) _canvas.gameObject.SetActive(false);
        Debug.Log("Call Round Hiding");
    }
    public void OnButtonClicked(int index)
    {
        DoActionRound(index);

        Debug.Log("Button clicked"+index+2);
        isTheRoundChosen = true;
        Hide();
        //Time.timeScale = 1f;
    
    }

    public void DoActionRound(int index)
    {
        if (_humanPlayer == null) _humanPlayer = FindAnyObjectByType<HumanPlayer>();
        if(_humanPlayer!=null) _humanPlayer.Call= index+1; Debug.Log("human is setted to "+_humanPlayer.Call);
        isTheRoundChosen = true;
       

    }  
    
    
}
