using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public sealed class Navigator : ScriptableObject
{
    public void NavigateToMainMenu() => LoadScene("StartScreen");

    private void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
