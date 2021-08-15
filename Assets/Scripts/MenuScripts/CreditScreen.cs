using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScreen : MonoBehaviour
{
    public void playAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
