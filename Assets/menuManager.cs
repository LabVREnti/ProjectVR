using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuManager : MonoBehaviour
{

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;

    Vector3 spawnPos;
    bool open;

    private void Start()
    {
        spawnPos = transform.position;
    }
    public void OpenQuitMenu()
    {
        open = !open;

        if (open)
        {
           gameObject.SetActive(true);
           settingsMenu.transform.position = spawnPos + new Vector3(-0.03f, 0, -0.3f);
           mainMenu.transform.position = spawnPos + new Vector3(0.02f, 0.46f, 0.77f);
           settingsMenu.SetActive(false);
        }
        else
        {
            transform.position = spawnPos;
            mainMenu.transform.position = spawnPos + new Vector3(0.02f, 0.46f, 0.77f);
            settingsMenu.transform.position = spawnPos + new Vector3(-0.03f, 0, -0.3f);
            settingsMenu.SetActive(false);
            gameObject.SetActive(false);
        }
    }

}
