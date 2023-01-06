using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restarter : MonoBehaviour
{
    // Рестартует сцену (я сам в шоке)
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
