using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject restartGamePanel;
    [SerializeField] private GameObject finishedGamePanel;
    [SerializeField] private GameObject pauseGamePanel;
    Animator animator;
    System.Random rastgele = new System.Random();
    int kirikcam;
    int[] kiriklar=new int[20];
    public AudioClip sound_fire;
    public AudioClip backmusic;
    private AudioSource audioSource;
    public int kontrol=0;
    public Rigidbody rbody;
    float  movementSpeed = 10.0f;
    private bool isDead = false;
    private bool paused = false;
    public float fireRate = 0.5f;
    public float nextFire = 0f;
void Start()
{

    finishedGamePanel.SetActive(false);
    pauseGamePanel.SetActive(false);
    restartGamePanel.SetActive(false);

    rbody = GetComponent<Rigidbody>(); //ziplama için rigidbody e erişim
    animator = GetComponent<Animator>(); //animasyonlara erişim için
    for (int x=0;x<11;x++){
        kiriklar[x]=rastgele.Next(0,2); //sağlam olmayan camların hangisi olacaklarını random ile tutan dizi
    }
    audioSource = GetComponent<AudioSource>(); //başlangıç müziği başlatma
                audioSource.clip = backmusic;
                audioSource.volume = 0.07f;
                audioSource.loop = true;
                audioSource.Play();
}
void Update()
{
    Hareket();    

    bool pause = Input.GetButtonDown("Pause");
    bool resume = Input.GetButtonDown("Resume");

    if(pause &&  paused == false && isDead == false){
        PauseGame();
    }

    if(resume && paused == true && isDead == false){
        ContinueGame();
    }        
}
    void OnTriggerEnter(Collider other){ //oyuncunun camlara değdiğini anlamak için
        if(other.transform.tag =="Yer") // eğerki yere düşerse ölüm animasyonu için
        {
            animator.SetInteger("dusme",0);
            isDead = true;
            restartGamePanel.SetActive(true);
        }
        if(other.transform.tag=="Bitis") //sona ulaştığında sevinme animasyonu için
        {
            animator.SetInteger("kutlama", 1);
            finishedGamePanel.SetActive(true);
            StartCoroutine(FinishReturn());
        }
        kirikcam=int.Parse(other.transform.name); //camların taglarını alıp int cast etme
        if(kiriklar[kirikcam].ToString() == other.transform.tag){ // eğer camın tag i dizinin elemanına şit ise
            isDead = true;
            audioSource.clip = sound_fire; //cam kırılma sesi
            audioSource.loop = false;
            audioSource.Play(); //başlatma
            Destroy(other.gameObject); //camın kırılma anı
            animator.SetInteger("dusme", 1); //oyuncunun düşme efekti başlangıcı
        }
        else{
        animator.SetInteger("korku", 1);
        }
    }
    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        PlayerPrefs.SetString("Level","SecondLevel");
        PlayerPrefs.Save();
        Time.timeScale = 1.0f;
        StartCoroutine(ReturnMainMenu());
    }

    public IEnumerator FinishReturn(){
        yield return new WaitForSeconds(5.0f); // opsiyonel
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
         PlayerPrefs.SetString("Level","");
// Sahne yüklenmiş olana kadar bekle
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void PauseGame(){
        Time.timeScale= 0.0f;
        audioSource.Pause();
        paused= true;
        pauseGamePanel.SetActive(true);
    }
    public void ContinueGame(){
        Time.timeScale =1.0f;
        audioSource.Play();
        paused= false;
        pauseGamePanel.SetActive(false);
    }

    void Hareket()
    {
        mouse(); // oyunu hareketinin mouse ile konrol kısmı
        if (transform.position.y<584f && kontrol==0){
        fall(); // oyuncunun yüksekliğinin camların altında olduğu zaman duşme efektinin kontrolu için
        }
        else{
            
        if (Input.GetKey(KeyCode.W)) // w tuşu basıldığında ileri yönde hareket
        {
        animator.SetInteger("status", 1);
        transform.position +=  Vector3.forward * Time.deltaTime * movementSpeed;
        Ziplama();
        }
        else if(Input.GetKey(KeyCode.S))// s tuşu basıldığında geri yönde hareket
        {
            rbody.position += Vector3.back * Time.deltaTime * movementSpeed;
            animator.SetInteger("back", 1);
        }
        else if (Input.GetAxis("Vertical") == 0.0f) // ileri veya geri yönde tuşa basılmadığı zaman animasyonların durması için
        {
        animator.SetInteger("status", 0);
        animator.SetInteger("back", 0);
        }

        if (Input.GetKey(KeyCode.D)) // d tuşu basıldığında sağ yönde hareket
        {
        animator.SetInteger("right", 1);
        rbody.position += Vector3.right * Time.deltaTime * movementSpeed;
        }
        else if(Input.GetKey(KeyCode.A)){ // a tuşu basıldığında sol yönde hareket
            rbody.position += Vector3.left * Time.deltaTime * movementSpeed;
            animator.SetInteger("left", 1);
        }
        else if (Input.GetAxis("Horizontal") == 0.0f ) //sağ ve sol hareketi olmadığında sağ ve sol hareket efektreinin durması için
        {
        animator.SetInteger("left", 0);
        animator.SetInteger("right", 0);
        }
        }
    }
    void Ziplama(){ 
        if(Input.GetKey(KeyCode.Space) && Time.time > nextFire){ // oyuncu zıplamak için space bastığında ve önceki basması ile arasında fark olduğu zaman çalışır
        animator.SetInteger("jump",1); // zıplama animasyonu çalıştırılması
        rbody.AddForce(transform.up * 45000000.0f); //zıplamanın gerçekleşmeisi
        nextFire = Time.time + fireRate;
        }
        else{
            animator.SetInteger("jump",0); // spaceye basılmadığı zaman zıplamanın durması
        }
    }
    void mouse(){
        float donus=Input.GetAxis("Mouse X"); // mouse hareketine göre kameranın döneceği ekseni alma
        if(paused == false && isDead == false ){
            transform.rotation *=Quaternion.Euler(0,donus,0); //oyuncuyu döndürme kısmı
        }
        
    }
    void fall(){ //oyuncunun yüksekliği camlardan az olduğu zaman düşme animasyonunun gerçekleşmesi
        kontrol=1;
        animator.SetInteger("status", 0); // yüm hareketanimasyonlarının  durması
        animator.SetInteger("back", 0);
        animator.SetInteger("left", 0);
        animator.SetInteger("right", 0);
        animator.SetInteger("dusme", 1); // düşme animasyonunun başlaması
    }
}
