using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuManager : MonoBehaviour
{
    [SerializeField] Vector3 spawnPos;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;

    bool open;
    public void OpenQuitMenu()
    {
        open = !open;

        if (open)
        {
           gameObject.SetActive(true);
           settingsMenu.transform.position = spawnPos + new Vector3(-0.03f, 0, -0.3f);
            settingsMenu.SetActive(false);
        }
        else
        {
            transform.position = spawnPos;
            settingsMenu.transform.position = spawnPos + new Vector3(-0.03f, 0, -0.3f);
            settingsMenu.SetActive(false);
            gameObject.SetActive(false);
        }
    }

}
