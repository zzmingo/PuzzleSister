using UnityEngine;
using UnityEngine.UI;
using TinyLocalization;

[RequireComponent(typeof(Text))]
public class Score : MonoBehaviour {
    //Some game score
	[SerializeField] int scoreCount = 0;
    public int ScoreCount {
        set {
            scoreCount = value;
        
            UpdateScoreText();
        }
        get {
            return scoreCount;
        }
    }
    
    //Subscribe on events
    void OnEnable () {
        LocalizationManager.OnChangeLanguage += OnChangeLanguage;
        Language.OnKeySet += OnKeySet;
        
        UpdateScoreText();
    }
    
    //Unsubscribe on events
    void OnDisable () {
        LocalizationManager.OnChangeLanguage -= OnChangeLanguage;
        Language.OnKeySet -= OnKeySet;
    }
    
    //OnChangeLanguage event
    void OnChangeLanguage(string languageCode){
        UpdateScoreText();
    }
    //OnKeySet event. When some key changed this value
    void OnKeySet(string languageCode, LocalizationKey changedKey){         
        UpdateScoreText();
    }
    
    //Called when you change value in editor
    [ExecuteInEditMode]
    void OnValidate(){
        ScoreCount = scoreCount;
    }
    
    //Update text in text component depend on "score"
    void UpdateScoreText() {
        GetComponent<Text>().text = LocalizationManager.Instance.GetLocalizedText("SCORE") + " " + ScoreCount.ToString();
    }   
}
