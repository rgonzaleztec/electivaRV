using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class managerMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public void CerrarApplicacion()
    {
        Application.Quit();
    }

    public void CargarEscena(int numescena)
    {
        SceneManager.LoadScene(numescena);

    }
 
}
