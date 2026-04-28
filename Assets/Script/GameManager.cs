using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    bool isGameOver;

    private void Awake()
    {
        if (Instance == null)
            Destroy(this);
        else
            Instance = this;
    }

    public void GameOver()
    {

    }
}
