# Plan de Diseño Técnico: F06 – Gestionar usuarios

## 1. Arquitectura por Capas (MVC)
- **Vista (`Vista/UsuariosForm.cs`):** Formulario de Windows Forms con un `DataGridView` para listar cuentas y controles de entrada para alta/modificación.
- **Controlador (`Controlador/UsuarioAdminController.cs`):** Orquesta las validaciones de duplicidad de nombres, manejo de roles y llamadas al DAO.
- **Modelo / DAO (`Modelo/UsuarioDAO.cs` y `Modelo/Usuario.cs`):** Conecta con la tabla `Usuarios` en SQL Server.

## 2. Aplicación del Patrón de Diseño GoF
- **Patrón seleccionado:** *Factory Method*.
- **Problema que resuelve:** Al momento de crear o instanciar los permisos y restricciones operativas según el rol del empleado (`Gerente`, `Vendedor`, `Almacenista`), evitar bloques gigantes de `switch` o `if-else` en el controlador.
- **Solución (Clases propuestas):**
  - Interfaz `IRolPermiso` con un método `ValidarAccesoModulo()`.
  - Clases concretas `RolGerente`, `RolVendedor`, `RolAlmacenista`.
  - Una clase `RolFactory` que retorna la instancia correcta según el string del rol almacenado en la BD.

## 3. Contratos de Diseño (Diseño por Contrato)
Método clave: `CrearUsuario(Usuario nuevoUsuario)` en `UsuarioDAO.cs`
- **@pre (Precondición):** `nuevoUsuario` no puede ser nulo; el campo `NombreUsuario` y `Password` no deben estar vacíos (`string.IsNullOrWhiteSpace`).
- **@post (Postcondición):** Retorna `true` si el registro se insertó correctamente en SQL Server, afectando al menos 1 fila.
- **@inv (Invariante):** Las contraseñas guardadas mantienen obligatoriamente el formato de hash cifrado.

## 4. Principios SOLID Aplicados
- **Abierto/Cerrado (OCP):** Gracias al patrón *Factory Method*, si en el futuro el negocio requiere agregar un nuevo rol (ej. "Supervisor"), solo creamos una nueva clase que implemente `IRolPermiso` sin modificar el código de los roles existentes.