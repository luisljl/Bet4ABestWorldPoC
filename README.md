# Bet4ABestWorldPoC

Aplicacion creada para el RFranco Challenge, la aplicacion permite realizar las siguientes acciones:

* Registrar un usuario
* Hacer login con un usuario
* Hacer logout con un usuario
* Hacer depositos y ver el historico de depositos
* Obtener el perfil del usuario logeado  y ver su historial de apuestas, historial de depositos, informacion de registro y balance actual
* Obtener un catalogo de slots, se pueden filtrar por nombre y obtener un slot por id
* Obtener el balance actual del jugador
* Apostar en slot y recibir ganancias
* Obtener un historico de apuestas, se puede obtener un historico de apuestas por slot y un historico de apuestas ganadoras por slot



#Pros y Contras

Una de las ventajas de usar un servicio api vs mvc es que la api utiliza menos recursos para mostrar datos, ya que al utilizar por ejemplo ficheros json y no tener la neccesidad
de renderizar la vista en el servidor se hace mucho mas ligero.

Otra ventaja es poder separar la vista de la logica de la aplicacion, pudiendo crear una aplicacion cliente con otro framework que conecte con el servicio, crear una aplicacion movil
o incluso que un tercero pueda tener acceso al servicio sin tener que volver a escribir mas codigo en la aplicacion.

Con  los servicios api es posible utilizar acciones basadas en verbos HTTP pudiendo utilizar standard como OpenApi

Por otro lado una ventaja de las aplicaciones MVC es que pueden ahorrar tiempo al crear las vistas en la misma aplicacion gracias a las herramientas que proveen los
framework como el motor de vistas Razor de .NET y no tener que crearlas en otro proyecto.


#Desglose de horas

Aproximadamente unas 25-26 horas totales

##Test y Servicios

Lo que mas tiempo se ha llevado del desarrollo es la creacion de test y la implementacion de los servicios con aproximadamente 18 horas,
pero una vez creados, las demas implementaciones eran mucho mas rapidas.

##Repositorios

La implementacion de los repositorios costo aproximadamente 2 horas.

##Controllers

La creacion de controllers con sus request y responses costo aproximadamente 2 horas.

##Autenticacion

La autenticacion a tenido un costo de unas 3 horas, debido a un cambio de ultima hora en la autenticacion hizo que se tardara mas de lo esperado.

##Despliege con docker

El despliegue con contenedores en docker costo aproximadamente 1 horas.