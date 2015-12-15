using UnityEngine;
using System.Collections;

public class WebcamTestBehaviourScript : MonoBehaviour {


    IEnumerator Start ()
    {
            //Show Authrizatton dialog box.
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            //è¨±å¯ãŒå‡ºã‚Œã°WebCamTextureã‚’ä½¿ç”¨ã™ã‚‹
            if (Application.HasUserAuthorization (UserAuthorization.WebCam)) {
                    WebCamTexture w = new WebCamTexture ();
                    //Materialã«ãƒ†ã‚¯ã‚¹ãƒãƒ£ã‚’è²¼ã‚Šä»˜ã‘
                    GetComponent<Renderer>().material.mainTexture = w;
                    //å†ç”Ÿ
                    w.Play ();
            }
    }
}
