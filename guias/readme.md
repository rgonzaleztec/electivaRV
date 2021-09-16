# Guias desarrolladas para temas relacionadas al curso o problemas planteandos
## Utilizando RayCast en movil VR
Vamos a ver los componentes principales para utilizar el RayCast para interactuar con objetos dentro del mundo VR sin tener controles conectados al comando.
De esta forma podemos crear aplicaciones sin mandos y que ademas puedan ser para cualquier usuario que tenga simplemente una gafas.

### Pasos Iniciales
1. Crear un proyecto 3D
2. Agregar el pluggin de Google Cardboard como se indica en el manual en la seccion de guias de [terceros](/guiaterceros/readme.md)
3. Crear una escena inicial con elementos como piso, paredes y elementos para interactuar

### Scripts Principales
Ahora vamos a agregar como componentes los scripts de interaccion.

1. Para la camara agregar CameraPointer.cs
2. Para los objetos con los que vas a interactuar ObjectController.cs
3. Recuerda iniciar el modo VR de Cardboard con el script CardboardStartup.cs

**El RayCast** es un funcion que existe en Unity que se puede utilizar para muchas interacciones. 
No es especificamente para VR pero nos sera util para interactuar sin mouse o controles.

El script en C# es:
```c#
public class CameraPointer : MonoBehaviour
{
    private const float _maxDistance = 6;
    private GameObject _gazedAtObject = null;

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    public void Update()
    {
        // Casts ray towards camera's forward direction, to detect if a GameObject is being gazed
        // at.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            // GameObject detected in front of the camera.
            if (_gazedAtObject != hit.transform.gameObject)
            {
                // New GameObject.
                if (hit.transform.gameObject.name == "Cube")
                {
                    _gazedAtObject?.SendMessage("OnPointerExit");
                    _gazedAtObject = hit.transform.gameObject;
                    _gazedAtObject.SendMessage("OnPointerEnter");
                }
                if (hit.transform.gameObject.name == "Capsule")
                {
                    _gazedAtObject?.SendMessage("OnPointerExit2");
                    _gazedAtObject = hit.transform.gameObject;
                    _gazedAtObject.SendMessage("OnPointerEnter2");

                }
                if (hit.transform.gameObject.name=="Salida")
                {
                    SceneManager.LoadScene(1);
                }
            }
        }
        else
        {
            // No GameObject detected in front of the camera.
            _gazedAtObject?.SendMessage("OnPointerExit");
            _gazedAtObject?.SendMessage("OnPointerExit2");
            _gazedAtObject = null;
        }

        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            _gazedAtObject?.SendMessage("OnPointerClick");
        }
    }
}
```


## Como desplazarse en movil VR sin controles
