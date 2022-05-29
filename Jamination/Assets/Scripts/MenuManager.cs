using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject levels;
    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject infoPanel;
    
    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClick);
    }


   public void PlayGame(){
        infoPanel.SetActive(true);
   }
   public void QuitGame(){
       Debug.Log("Quit");
       Application.Quit();
   }
   public void Levels()
   {
         menu.SetActive(false);
         levels.SetActive(true);
   }
    public void Back()
    {
        menu.SetActive(true);
        levels.SetActive(false);
    }

public void Skip(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}
}
