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
        if (string.IsNullOrEmpty(UserData.Token))
        {
            gameObject.SetActive(true);
            MainMenu.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
            MainMenu.SetActive(true);
        }
    }

    public async void OnLogin ()
    {
        var response = await GameHttpApiProvider.Login(login.text, password.text);

        if (response.Data.Id < 0)
        {
            return;
        }
        
        UserData.Id = response.Data.Id;
        UserData.Token = response.Data.Token;
        UserData.Name = login.text;
        gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }

    public async void OnRegister()
    {
        var response = await GameHttpApiProvider.Register(login.text, password.text);
        UserData.Id = response.Data.Id;
        UserData.Token = response.Data.Token;
        UserData.Name = login.text;
        gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }

}
