using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void EasyTacticalGame()
    {
        PlayerPrefs.SetInt("agent_type",1);
      
        SceneManager.LoadScene("EasyLevel");
    }

    public void EasyNaiveGame()
    {
        PlayerPrefs.SetInt("agent_type", 3);
      
        SceneManager.LoadScene("EasyLevel");
    }

    public void EasySmartGame()
    {
        PlayerPrefs.SetInt("agent_type", 2);
      
        SceneManager.LoadScene("EasyLevel");
    }

    public void EasySafeGame()
    {
        PlayerPrefs.SetInt("agent_type", 4);
      
        SceneManager.LoadScene("EasyLevel");
    }

    public void HardTacticalGame()
    {
        PlayerPrefs.SetInt("agent_type", 1);
    
        SceneManager.LoadScene("HardLevel");
    }

    public void HardNaiveGame()
    {
        PlayerPrefs.SetInt("agent_type", 3);
    
        SceneManager.LoadScene("HardLevel");
    }

    public void HardSmartGame()
    {
        PlayerPrefs.SetInt("agent_type", 2);
     
        SceneManager.LoadScene("HardLevel");
    }

    public void HardSafeGame()
    {
        PlayerPrefs.SetInt("agent_type", 4);
       
        SceneManager.LoadScene("HardLevel");
    }
}
