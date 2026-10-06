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
   `git clone https://github.com/Venatus17/SIGERE.git`
2. Abrir la solución `.sln` en Visual Studio 2022.
3. Restaurar los paquetes NuGet necesarios.
4. Configurar la cadena de conexión:
   * Abrir el archivo `appsettings.json` (o `App.config`).
   * Modificar el `ConnectionString` con las credenciales de tu instancia local de SQL Server.
5. Ejecutar el script SQL proporcionado en la carpeta `/Database` para crear las tablas y datos de prueba.
6. Compilar y ejecutar el proyecto (F5).

## Integrantes del Equipo
* **Jose Miguel Nuñez Garcia** 
* **Sebastian Nuño Melendrez** 
* **Reyes Chávez Luis Alejandro**
* **Ortega Coronado César David**

## Estructura de Ramas
* `main`: Código de producción estable.
* `develop`: Rama de integración para pruebas.
* `feature/*`: Ramas individuales para el desarrollo de nuevas características.
