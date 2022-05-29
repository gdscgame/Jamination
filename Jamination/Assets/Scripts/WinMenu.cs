using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
