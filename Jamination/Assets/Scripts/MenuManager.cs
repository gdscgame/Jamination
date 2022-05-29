using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject levels;


   public void PlayGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
}
