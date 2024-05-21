using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic; // Necesario para usar List

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    private List<AudioSource> audioSources = new List<AudioSource>();
    private float originalVolume = 1f; 

    public void ShowGameOverPanel()
    {
        // Encuentra y silencia todos los AudioSource
        audioSources.Clear();
        audioSources.AddRange(FindObjectsOfType<AudioSource>());
        foreach (var source in audioSources)
        {
            source.volume = 0; // Silencia el AudioSource
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
    }

    public void RestartGame()
    {
        // Restaurar el volumen de los AudioSource
        foreach (var source in audioSources)
        {
            source.volume = originalVolume; // Restablece el volumen original
        }

        Time.timeScale = 1f; // Reanuda el juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Carga la escena actual de nuevo
    }
}
