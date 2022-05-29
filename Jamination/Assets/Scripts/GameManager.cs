using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hopSound;
    [SerializeField] AudioClip gameOver;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip portal;
    [SerializeField] GameObject controls;
    bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Move(){
        audioSource.PlayOneShot(hopSound);
    }
    public void GameOver(){
        audioSource.PlayOneShot(gameOver);
    }
    public void Win(){
        audioSource.PlayOneShot(winSound);
    }
    public void Crash(){
        audioSource.PlayOneShot(crash);
    }
    public void Portal(){
        audioSource.PlayOneShot(portal);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pausePanel.activeInHierarchy)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
    void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit(){
        Application.Quit();
        Debug.Log("Quit");
    }
    public void Controls(){
        if(!isOpen){
        controls.SetActive(true);
        isOpen =true;

        }
        else if(isOpen){
            controls.SetActive(false);
            isOpen = false;
        }
    }
}
