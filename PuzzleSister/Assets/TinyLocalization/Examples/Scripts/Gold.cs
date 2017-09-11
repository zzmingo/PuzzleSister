using UnityEngine;
using TinyLocalization;

[RequireComponent(typeof(Localize))]
public class Gold : MonoBehaviour {
    [SerializeField] int goldCount = 0;
    
    //Gold count with updating
    public int GoldCount {
        set {
            goldCount = value;
        
            UpdatePostfix();
        }
        get {
            return goldCount;
        }
    }
    
    //Update postfix
    void UpdatePostfix() {
        GetComponent<Localize>().Postfix = " " + goldCount.ToString();
    }    
    
    //Called when you change value in editor
    [ExecuteInEditMode]
    void OnValidate(){
        UpdatePostfix();
    }
}
