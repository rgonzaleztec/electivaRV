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


## Como desplazarse en movil VR sin controles
