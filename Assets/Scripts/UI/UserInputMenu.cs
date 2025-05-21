using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInputMenu : MonoBehaviour
{
    public MockHttpProvider GameHttpApiProvider;
    public TMP_InputField login;
    public TMP_InputField password;
    public GameObject MainMenu;

    public void Start()
    {
        if (PlayerPrefs.HasKey("token"))
        {
            gameObject.SetActive(false);
            MainMenu.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
            MainMenu.SetActive(false);
        }
    }

    public async void OnLogin ()
    {
        var response = await GameHttpApiProvider.Login(login.text, password.text);
        PlayerPrefs.SetString("token", response.Data.Token);
        PlayerPrefs.SetString("login", login.text);
        PlayerPrefs.SetInt("id", response.Data.Id);
        PlayerPrefs.Save();
        gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }

    public async void OnRegister()
    {
        var response = await GameHttpApiProvider.Register(login.text, password.text);
        PlayerPrefs.SetString("token", response.Data.Token);
        PlayerPrefs.SetString("login", login.text);
        PlayerPrefs.SetInt("id", response.Data.Id);
        PlayerPrefs.Save();
        gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }

}
