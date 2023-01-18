using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager_sc : MonoBehaviour
{
    // Start is called before the first frame update
     [SerializeField] private TextMeshProUGUI scoreText;
     [SerializeField] private TextMeshProUGUI  bestScore;
     private int SecondScore=0;
     private int oldScore;
    void Start()
    {
        oldScore = PlayerPrefs.GetInt("bestScore");  
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    // public void UpdateTheBestScore(){
    //     if(oldScore!=0){
    //         if(SecondScore < oldScore){
    //             PlayerPrefs.SetInt("bestScore",SecondScore);
    //             bestScore.text = SecondScore.ToString();
    //         }
    //         else{
    //             bestScore.text = SecondScore.ToString();
    //         }
    //     }
        
    // }
    public IEnumerator UpdateSecondScore(){
        while(true){
            scoreText.text = SecondScore.ToString();
            yield return new WaitForSeconds(1); 
            SecondScore+=1;
        }
    } 
}
