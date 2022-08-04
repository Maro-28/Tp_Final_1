Red Social Logo

Requerimientos:

Abrir con Microsoft Visual Studio 2019 o superior (se necesita de una version de Net Core para acceder a los diseños desde el IDE)

Se debe abrir el proyecto desde el archivo Tp_Final_1 y ejecutar usando el boton Ejecutar o con la tecla F5.

(Guía de uso)
1. Al ejecutar, el usuario observa una pantalla de inicio de sesión.
a. Existe un botón que permite registrarse, esto lleva al usuario a una nueva
pantalla donde se piden sus datos y puede crear una nueva cuenta.
b. Si el usuario es válido pero la clave es incorrecta, se suma un intento fallido.
c. Si el usuario llega a 3 intentos fallidos, el mismo queda bloqueado y no puede
acceder aunque coloque la clave correcta.

2. Si las credenciales son correctas, el usuario ingresa a la pantalla principal donde
observa:
a. Su lista de amigos
i. Aquí puede quitarlos de la lista

b. Sus Posts (con comentarios y reacciones asociados)
i. En este menú puede crear un nuevo post
ii. Al crear un post, también hay un recuadro extra para agregar hastags
al post, los mismos comienzan con #
iii. También tiene la opción de eliminarlos.

c. Los Posts de sus amigos
i. Podrá comentar y reaccionar en cada post
ii. Podrá editar y eliminar sus comentarios y reacciones.

d. Un campo para buscar amigos
i. Tendrá la opción de elegir usuarios de la lista y agregar a amigos, se
agregan automáticamente para ambos usuarios. Por ejemplo, usuario A busca amigos con nombre B,
selecciona a usuario B, presiona el botón agregar amigo y en ese
momento, B se agrega a la lista de amigos de A y A se agrega a la lista
de amigos de B.

e. Un campo para buscar Posts (por contenido, fechas y/o tags)
i. Todo lo que se publica en esta red es “público” por ende, los resultados contienen posts de usuarios que no
son amigos también.

3. Listado de usuarios: 
mail: mariano@mail.com
contraseña: 111
condicion: Admin

mail: alan@mail.com
contraseña: 222
condicion: usuario comun

mail: paula@mail.com
contraseña: 444
condicion: usuario bloqueado

mail: marianog@mail.com
contraseña: 555
condicion: usuario con dos intentos fallidos