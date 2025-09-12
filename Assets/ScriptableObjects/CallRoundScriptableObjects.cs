using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "CallRoundScriptableObjects", menuName = "Scriptable Objects/CallRoundScriptableObjects")]
public class CallRoundScriptableObjects : ScriptableObject
{
    public List<Button>_callRounds = new List<Button>();
}
