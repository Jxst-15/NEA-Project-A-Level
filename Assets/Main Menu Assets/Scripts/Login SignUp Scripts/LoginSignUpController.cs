using UnityEngine;

public class LoginSignUpController : MonoBehaviour
{
    #region Fields
    #region Gameobjects
    public GameObject logSuGO;

    public GameObject logFields;
    public GameObject signFields;
    #endregion

    #region Script References
    private ConnectionHandler logSU;
    #endregion
    #endregion
    
    public void LoginButton()
    {
        logSuGO.SetActive(false);
        logFields.SetActive(true);
    }

    public void SignUpButton()
    {
        logSuGO.SetActive(false);
        signFields.SetActive(true);
    }

    public void AccountLoginButton()
    {
        
    }

    public void AccountSignInButton()
    {

    }
}
