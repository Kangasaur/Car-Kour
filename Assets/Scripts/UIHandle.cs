using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIHandle : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI sideText;
    public bool dead = false;
    public bool win = false;
    float time = 0f;

    private void Update()
    {
        if (!dead && !win)
        {
            time += Time.deltaTime;
            timeText.text = string.Format("{0:N2}", time);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1f;
        }
    }

    void DoDeath()
    {
        dead = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MovePlayer>().enabled = false;
        Time.timeScale = 0f;
        sideText.text = "Press Space to restart";
    }

    void DoWin()
    {
        win = true;
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        sideText.text = "You won!\nCan you get a better time?";
    }
    
}
