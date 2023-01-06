namespace UmbralRealm.Login.Service
{
    public enum LoginState : int
    {
        PromptCredentials = 0,
        VerifyCredentials = 1,
        PromptPin = 2,
        VerifyPin = 3,
        Authenticated = 4,
    }
}
