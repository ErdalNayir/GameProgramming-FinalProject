using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_sc : MonoBehaviour
{
    // Start is called before the first frame update
    private float _speed;
    [SerializeField] private GameObject GameManagerObject; 
    private GameManager_sc gameManager;
    [SerializeField] private AudioClip walkinSound;

    private AudioSource audioSource;

    private Animator animator;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManagerObject.GetComponent<GameManager_sc>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(gameManager.paused){
            PauseSound();
        }
        else{
            HaraketEt();
        }    
    }

    private void configureSound(float speed,float volume){
        audioSource.clip = walkinSound;
        audioSource.loop = true;
        audioSource.pitch = speed;
        audioSource.volume = volume;
    }
    private void PauseSound(){
        audioSource.Pause();
    }
    private void HaraketEt(){
        //yürümek için
        if(!gameManager.isDead ){
            float VerticalInput = Input.GetAxis("Vertical"); //normal yürüme ve hızlanma
            float isSpeed = Input.GetAxis("SpeedUp"); //Hızlanmak için

            if(transform.position.z>1.0f && VerticalInput>0){ //z birden büyük ise haraket et
                if(isSpeed == 0.0f)
                {
                     ChangeSpeedNSound(1,0.75f,0.05f,20.0f,VerticalInput); 
                }
                else if(isSpeed == 1.0f)
                {
                     ChangeSpeedNSound(2,0.75f,0.05f,25.5f,VerticalInput);           
                }     
            }
       
            else{
                if(!gameManager.isFinished){
                    animator.SetInteger("status",0);
                    audioSource.Stop();
                }
                else{
                    animator.SetInteger("status",0);
                    audioSource.Stop();
                    animator.SetInteger("status",5);
                }    
            }
        }
        else{
            gameManager.GameOver();
            audioSource.Stop();
            animator.SetInteger("status",4);
        }
        
    }
    private void ChangeSpeedNSound(int status, float Soundspeed, float volume,float PlayerSpeed ,float VerticalInput)
    {
        animator.SetInteger("status", status);
        _speed = PlayerSpeed;
        transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * VerticalInput * _speed);
        if (!audioSource.isPlaying)
        {
            configureSound(Soundspeed, volume);
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other) {

        if(other.tag =="Start"){
            gameManager.StartGame();
        }    
        else if(other.tag == "Finish"){
            animator.SetInteger("status", 5);
            gameManager.GameFinished();
        }
        else{
            Debug.Log("Leveled Up");
            gameManager.LevelUp();
        }
    }



}

