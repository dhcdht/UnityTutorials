﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _2SpaceShooter
{
    public class Done_GameController : MonoBehaviour
    {
        public GameObject[] hazards;
        public Vector3 spawnValues;
        public int hazardCount;
        public float spawnWait;
        public float startWait;
        public float waveWait;

        public Text gameOverText;
        public Text restartText;
        public Text scoreText;

        private bool gameOver;
        private bool restart;
        private int score;

        private void Start()
        {
            gameOver = false;
            restart = false;
            score = 0;

            gameOverText.text = "";
            restartText.text = "";
            UpdateScore();

            StartCoroutine(SpawnWaves());
        }

        private void Update()
        {
            if (restart)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }

        private IEnumerator SpawnWaves()
        {
            yield return new WaitForSeconds(startWait);
            while (true)
            {
                for (int i = 0; i < hazardCount; i++)
                {
                    GameObject hazard =
                        hazards[Random.Range(0, hazards.Length)];
                    Vector3 spawnPosition =
                        new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y,
                            spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(hazard, spawnPosition, spawnRotation);
                    yield return new WaitForSeconds(spawnWait);
                }
                yield return new WaitForSeconds(waveWait);

                if (gameOver)
                {
                    restartText.text = "Press 'R' for Restart";
                    restart = true;
                    break;
                }
            }
        }

        private void UpdateScore()
        {
            scoreText.text = "Score: " + score;
        }

        // MARK: - Public Methods

        public void AddScore(int newScoreValue)
        {
            score += newScoreValue;
            UpdateScore();
        }

        public void GameOver()
        {
            gameOverText.text = "Game Over!";
            gameOver = true;
        }
    }
}