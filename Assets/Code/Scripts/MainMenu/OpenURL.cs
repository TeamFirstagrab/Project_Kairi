using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public void OpenSite(string url)
    {
        Application.OpenURL(url);
    }
}
