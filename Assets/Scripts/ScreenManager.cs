using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Classe para troca de telas e cenas
public class ScreenManager : MonoBehaviour
{

    public List<GameObject> screens = new List<GameObject>();
    
    public void ChangeScreen(GameObject newScreen) { // Ativa a nova tela especificada e desativa as outras

        foreach (var screen in screens) {

            if (screen == newScreen) {

                screen.SetActive(true);
                continue;

            }

            screen.SetActive(false);

        }

    }

    public void ChangeScene(string scene) { // Função para mudar de cena

        SceneManager.LoadScene(scene);

    }

}
