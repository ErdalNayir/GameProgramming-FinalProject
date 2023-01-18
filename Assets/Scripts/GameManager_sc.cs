using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager_sc : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject restartGamePanel;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject finishedGamePanel;
    [SerializeField] private GameObject pauseGamePanel;
    [SerializeField] private GameObject UIManagerObject; 
    [SerializeField] private GameObject indicatorPanel;
    private UI_Manager_sc uiManager;
    [SerializeField] private AudioClip happySound;
    private AudioSource audioSource;
    public bool isDead = false;
    public bool isFinished = false;
    public int Level=0;
    private bool isRunning=false;
    public bool paused=false;
    private IEnumerator checkMovement;
    private IEnumerator updateText;
    void Start()
    {
        finishedGamePanel.SetActive(false);
        pauseGamePanel.SetActive(false);
        restartGamePanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();


        uiManager = UIManagerObject.GetComponent<UI_Manager_sc>();

        updateText = uiManager.UpdateSecondScore();

    }
    // Update is called once per frame
    void Update()
    {
        bool pause = Input.GetButtonDown("Pause");
        bool resume = Input.GetButtonDown("Resume");

        if(pause && isDead == false && paused == false){
            PauseGame();
        }

        if(resume && isDead == false && paused == true){
            ContinueGame();
        }        
    }
    public void LevelUp(){
        if(Level<3){
            Level+=1;
        }
    }
    public IEnumerator GoNextLevel(){
        yield return new WaitForSeconds(5.0f); // opsiyonel
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1);
// Sahne yüklenmiş olana kadar bekle
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

     public IEnumerator ReturnMainMenu(){
        yield return new WaitForSeconds(1.0f); // opsiyonel
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
// Sahne yüklenmiş olana kadar bekle
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void ReturnMenu(){
        audioSource.Stop();
        string name = PlayerPrefs.GetString("Level");
        if(name == "SecondLevel"){
            Time.timeScale = 1.0f;
            StartCoroutine(ReturnMainMenu());
        }
        else{
            Time.timeScale = 1.0f;
            PlayerPrefs.SetString("Level","FirstLevel");
            PlayerPrefs.Save();   
            StartCoroutine(ReturnMainMenu());
        }      
    }
    private void Death(){
        isDead=true;
        //uiManager.UpdateTheBestScore();
        restartGamePanel.SetActive(true);
    }
    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PauseGame(){
        Time.timeScale= 0.0f;
        paused = true;
        audioSource.Pause();
        pauseGamePanel.SetActive(true);
    }
    public void ContinueGame(){
        Time.timeScale =1.0f;
        paused=false;
        audioSource.Play();
        pauseGamePanel.SetActive(false);
    }
    public void GameFinished(){
        isFinished =true;
        StopCoroutine(updateText);
        //uiManager.UpdateTheBestScore();
        finishedGamePanel.SetActive(true);
        StartCoroutine(GoNextLevel());
    }
    private void startCustomCoroutine(){
        if(!isRunning){
            StartCoroutine(PlaySongAndStop());
        }      
    }
    public void GameOver(){
        StopCoroutine(updateText);
        StopCoroutine(PlaySongAndStop());
        StopCoroutine(CheckMovement());
    }
    private IEnumerator CheckMovement(){
        float VerticalInput = Input.GetAxis("Vertical");
        indicatorPanel.GetComponent<Renderer>().material.color = new Color(255,0,0);

        if(VerticalInput != 0.0f){
            Death();
        }
        else{
            yield return new WaitForSeconds(1);
            indicatorPanel.GetComponent<Renderer>().material.color = new Color(0,255,0);
            startCustomCoroutine();
        }
    }
    private IEnumerator PlaySongAndStop(){
        isRunning=true;
        int secondsToPlay;
        if(Level == 0){
            secondsToPlay = Random.Range(9,12); //9-11
            configureSound(1.0f, 0.05f);
            audioSource.Play();
            yield return new WaitForSeconds(secondsToPlay);
            audioSource.Stop();
            StartCoroutine(CheckMovement());
            isRunning =false;
        }
        else if(Level == 1){
            secondsToPlay = Random.Range(7,10); //7-9
            configureSound(1.0f, 0.05f);
            audioSource.Play();
            yield return new WaitForSeconds(secondsToPlay);
            audioSource.Stop();
            StartCoroutine(CheckMovement());
            isRunning =false;
        }
        else if(Level == 2){
            secondsToPlay = Random.Range(5,8); //5-7
            configureSound(1.0f, 0.05f);
            audioSource.Play();
            yield return new WaitForSeconds(secondsToPlay);
            audioSource.Stop();
            StartCoroutine(CheckMovement());
            isRunning =false;
        }
        else if(Level == 3){
            secondsToPlay = Random.Range(3,6); //3-5
            configureSound(1.0f, 0.05f);
            audioSource.Play();
            yield return new WaitForSeconds(secondsToPlay);
            audioSource.Stop();
            StartCoroutine(CheckMovement());
            isRunning =false;
        } 
    }
    public void StartGame(){
        if(!isDead){
            if (!audioSource.isPlaying)
            {
                 StartCoroutine(PlaySongAndStop());
            }
            StartCoroutine(updateText); 
        }  
    }
    private void configureSound(float speed,float volume){
        audioSource.clip = happySound;
        audioSource.loop = true;
        audioSource.pitch = speed;
        audioSource.volume = volume;
    }
}
