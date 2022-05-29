using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    [SerializeField] Animator[] anims;
    bool isFirstTime = true;
    public Animator changeLevel;
    private void Start() 
    {
        // changeLevel = GameObject.Find("ChangeLevel").GetComponent<Animator>();
    }

    public void SetAnim(int index)
    
    {
        int i =index%anims.Length;
        if(isFirstTime)
        {
            anims[i].SetBool("isWalking", true);
            isFirstTime = false;
        }
        else
        {
            if(i==0)
            {
                anims[i].SetBool("isWalking", true);
                anims[anims.Length-1].SetBool("isWalking", false);
            }
            else
            {
                anims[i].SetBool("isWalking", true);
                anims[i-1].SetBool("isWalking", false);
            }
        }
    }

    public void ChangeLevel()
    {
        changeLevel.SetTrigger("IsWin");
    }
}
