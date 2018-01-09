using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour {

    bool allowed = false;
    bool reloading = false;

    private void Start()
    {
        Invoke("AllowReload", 3);
    }

    void AllowReload()
    {
        allowed = true;
    }

	public void ReloadApp()
    {
        if (reloading && allowed)
            return;

        reloading = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
