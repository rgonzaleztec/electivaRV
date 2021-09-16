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
Este script lo que hace es disparar el rayo para calcular si golpea a un objeto que es golpeable mediante un collinder, si lo logra porque esta en el rango de alcance,
calcula su hit e indica que lo golpeo. Con esto podemos enviar un mensaje mediante el controlador de eventos de unity para decir que estamos tocando un objeto.
Lo anterior se ve en la linea donde se indica "OnPointEnter". Este es el nombre de la funcion que va a ser implementada en el script de ObjectController.

El script de ObjectCrontroller contiene algo asi:
```c#
public class ObjectController : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;

    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;

    public AudioSource audioCapsula;
    public TextMeshPro hitText;

    // The objects are about 1 meter in radius, so the min/max target distance are
    // set so that the objects are always within the room (which is about 5 meters
    // across).
    private const float _minObjectDistance = 2.5f;
    private const float _maxObjectDistance = 3.5f;
    private const float _minObjectHeight = 0.5f;
    private const float _maxObjectHeight = 3.5f;

    private Renderer _myRenderer;
    private Vector3 _startingPosition;

    private GameObject myCapsule;
    private GameObject myTextPRO;
    
    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        _startingPosition = transform.parent.localPosition;
        _myRenderer = GetComponent<Renderer>();
        SetMaterial(false);
        myCapsule = GameObject.Find("Capsule");
        myTextPRO = GameObject.Find("Hits");
        hitText = myTextPRO.GetComponent<TextMeshPro>();
    }

    /// <summary>
    /// Teleports this instance randomly when triggered by a pointer click.
    /// </summary>
    public void TeleportRandomly()
    {
        // Picks a random sibling, activates it and deactivates itself.
        int sibIdx = transform.GetSiblingIndex();
        int numSibs = transform.parent.childCount;
        sibIdx = (sibIdx + Random.Range(1, numSibs)) % numSibs;
        GameObject randomSib = transform.parent.GetChild(sibIdx).gameObject;

        // Computes new object's location.
        float angle = Random.Range(-Mathf.PI, Mathf.PI);
        float distance = Random.Range(_minObjectDistance, _maxObjectDistance);
        float height = Random.Range(_minObjectHeight, _maxObjectHeight);
        Vector3 newPos = new Vector3(Mathf.Cos(angle) * distance, height,
                                     Mathf.Sin(angle) * distance);

        // Moves the parent to the new position (siblings relative distance from their parent is 0).
        transform.parent.localPosition = newPos;

        randomSib.SetActive(true);
        gameObject.SetActive(false);
        SetMaterial(false);
    }

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
        SetMaterial(true);
        hitText.text = "# de Hits: Incrementando"; 
    }

    // <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject Capsule.
    /// </summary>
     public void OnPointerEnter2()
    {
        SetMaterial(true);
        audioCapsula.Play();
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        SetMaterial(false);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject Capsule
    /// </summary>
    public void OnPointerExit2()
    {
        SetMaterial(false);
        audioCapsula.Stop();

    }
    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {
        TeleportRandomly();
    }

    /// <summary>
    /// Sets this instance's material according to gazedAt status.
    /// </summary>
    ///
    /// <param name="gazedAt">
    /// Value `true` if this object is being gazed at, `false` otherwise.
    /// </param>
    private void SetMaterial(bool gazedAt)
    {
        if (InactiveMaterial != null && GazedAtMaterial != null)
        {
            _myRenderer.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
        }
    }

  
}
```
Como se puede observar en el script se implementan funciones para realizar acciones segun el tipo de evento que se invoco. Los eventos coinsiden con el nombre de la funcion, asi que si tienes o requieres muchos objetos con que interactuar aqui deberas implementarlos.




## Como desplazarse en movil VR sin controles
Debemos crear un proyecto 3D con una escena en la que nos podamos desplazar.
Para poder hacer un movimiento sin controles osea hacer locomotion podemos utilizar la posibilidad que tiene Uniti de decirnos si la camara se mueve, para el caso de nosotros necesitamos controlar si rota sobre el eje X. Con esto podremos determinar si la persona esta observando hacia abajo y utilizar esa interaccion para indicar que nos tenemos que desplazar.

1 Requerimos adjuntar a la camara con CharacterController
2 Tenemos que configurar el colinder para que este en una posicion funcional para la camara
3 Debemos estimar cuantos grados de movimiento se pueden hacer
4 Debemos crear un script para controlar ese movimiento y poderselo adjuntar al GameObject de la camara como componente

El script es:
```c#
public class lookWalk : MonoBehaviour
{
    public Transform vrCamera;
    public float puntolimite = 10.0f;

    public float speed = 3.0f;

    public bool moviendose;

    public CharacterController myPersonaje;

    // Start is called before the first frame update
    void Start()
    {
        myPersonaje = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
       if (vrCamera.eulerAngles.x >= puntolimite && vrCamera.eulerAngles.x < 90.0f)
        {
            moviendose = true;
        }
       else
        { 
            moviendose = false;
        }

       if (moviendose)
        {
            Vector3 forward = vrCamera.TransformDirection(Vector3.forward);
            myPersonaje.SimpleMove(forward * speed);
        }
    }
}
```


