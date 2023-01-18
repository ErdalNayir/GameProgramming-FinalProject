using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using TMPro;
public class MainMenu_sc : MonoBehaviour
{

    [SerializeField] private Button resumeButton;
    [SerializeField] private TextMeshProUGUI  bttnText;
    [SerializeField] private AudioClip backgroundSound;
    private AudioSource audioSource;
    public bool isResumed=false;
    private string levelName;
    // Start is called before the first frame update

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if(audioSource!=null){
          configureSound(1.0f,0.05f);
          if (!audioSource.isPlaying)
          {
            audioSource.Play();
          }
        }

        DontDestroyOnLoad(audioSource);
    }
    void Start()
    {
        levelName = PlayerPrefs.GetString("Level");

        if(levelName.Length == 0 ){
            resumeButton.enabled = false;
            bttnText.color = Color.grey;
        }
        else{
          resumeButton.enabled = true;
        }  
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator NewGame(){
        yield return new WaitForSeconds(1.0f); // opsiyonel
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("FirstLevel");
// Sahne yüklenmiş olana kadar bekle
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void setIsResumed(bool val){
      isResumed = val;
    }

    public void OyunaDevamEt(){

      string name = PlayerPrefs.GetString("Level");
      SceneManager.LoadScene(name);
    }


    public void OyunaBasla(){
      StartCoroutine(NewGame());
    }

    private void configureSound(float speed,float volume){
      audioSource.clip = backgroundSound;
      audioSource.loop = true;
      audioSource.pitch = speed;
      audioSource.volume = volume;
    }
}
