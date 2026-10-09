# Sistema de Punto de Venta - Equipo 5

## Descripción
Este proyecto es un sistema de Punto de Venta desarrollado para el Avance 2 de la materia de Desarrollo de Software. El sistema aplica una arquitectura MVC (Modelo-Vista-Controlador), principios SOLID y Diseño por Contrato para asegurar un código mantenible y escalable. 

El módulo principal de este avance es el login de usuario el cual se utilizara para tener una clara división de los permisos que podrá tener cada usuario, por ejemplos, los trabajadores solo tendrán permitido el acceso a el cobro y el inventario.

## Stack Tecnológico
* **Lenguaje:** C#
* **Framework:** Windows Forms .NET
* **Base de Datos:** SQL Server
* **IDE:** Visual Studio 2022

## Instrucciones para ejecutar el proyecto
1. Clonar este repositorio en tu máquina local:
   `git clone https://github.com/LuisAlejandro20/SIGERE.git`
2. Abrir la carpeta del proyecto en **Visual Studio Code** o tu IDE de preferencia para .NET 8.
3. Restaurar los paquetes NuGet necesarios, asegurándote de instalar la librería de cifrado:
   * Ejecutar en la terminal: `dotnet add package BCrypt.Net-Next`
4. Configurar la cadena de conexión:
   * Abrir el archivo donde se define la conexión (en `UsuarioDAO.cs` o el archivo de configuración local).
   * Modificar el `connectionString` con el nombre de tu servidor y credenciales de tu instancia local de SQL Server.
5. Ejecutar el script SQL proporcionado en el servidor para crear la base de datos `SIGERE_DB`, la tabla `Usuarios` y cargar los datos de prueba y roles.
6. Compilar y ejecutar el proyecto mediante la terminal con `dotnet run` o directamente desde tu entorno de desarrollo para abrir la ventana de login (`LoginForm.cs`).

## Integrantes del Equipo
* **Jose Miguel Nuñez Garcia** 
* **Sebastian Nuño Melendrez** 
* **Reyes Chávez Luis Alejandro**
* **Ortega Coronado César David**

## Estructura de Ramas
* `main`: Código de producción estable.
* `develop`: Rama de integración para pruebas.
* `feature/*`: Ramas individuales para el desarrollo de nuevas características.
