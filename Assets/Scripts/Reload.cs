using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour {

    float reloadTimer = 5;

    [ContextMenu("Reload")]
    public void ReloadApp()
    {
        if (reloadTimer > 0)
            return;

        Debug.Log("ReloadScene");
        reloadTimer = 5;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        reloadTimer -= Time.deltaTime;
    }
}
