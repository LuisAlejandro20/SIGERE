# F06 – Gestionar usuarios del sistema, roles y permisos

## Caso de uso (Basado en CU07)
- **Actor principal:** Gerente (con perfil de administrador).
- **Precondición:** El gerente debe haber iniciado sesión con credenciales válidas y rol de Gerente.
- **Postcondición:** Los cambios en los usuarios (creación, modificación o desactivación) han sido guardados en la base de datos y registrados en la bitácora.

## Escenario principal
1. El gerente selecciona la opción 'Gestión de usuarios' en el menú administrativo.
2. El sistema despliega el listado de usuarios registrados en la base de datos.
3. El gerente selecciona una acción: Crear nuevo usuario, Modificar usuario existente o Desactivar cuenta.
4. El sistema habilita el formulario correspondiente con los campos requeridos (Nombre, Usuario, Contraseña, Rol, Correo, Teléfono).
5. El gerente ingresa o actualiza los datos y selecciona un rol permitido (`Gerente`, `Vendedor`, `Almacenista`).
6. El sistema valida que el nombre de usuario no esté duplicado en la base de datos.
7. El gerente confirma la operación.
8. El sistema cifra la contraseña (si es nueva), guarda los cambios en SQL Server y registra la operación en la bitácora.

## Escenarios alternos
- **6a. Nombre de usuario duplicado:** Si el usuario ingresa un nombre de acceso que ya existe en la BD al crear una cuenta, el sistema bloquea el registro, muestra el mensaje "Nombre de usuario ya existe" y solicita uno diferente.
- **3a. Intento de baja del único administrador:** Si el gerente intenta desactivar el único usuario con rol de Gerente activo en el sistema, el sistema bloquea la acción y muestra un mensaje de advertencia.

## Reglas de negocio
- Ninguna contraseña se almacena en texto plano; toda alta o cambio de contraseña pasa por hashing BCrypt.
- Solo los usuarios con rol `Gerente` pueden acceder a este módulo de administración.

## Criterios de aceptación (Given-When-Then)
- **PT-01 (Camino feliz - Crear usuario):**
  - **Dado** que el gerente está en el panel de usuarios y define un nuevo usuario válido (Usuario: "carlos_v", Rol: "Vendedor")
  - **Cuando** ingresa los datos completos y presiona "Guardar"
  - **Entonces** el sistema registra la cuenta con contraseña cifrada, muestra "Usuario creado con éxito" y actualiza la lista.

- **PT-02 (Escenario alterno - Usuario duplicado):**
  - **Dado** que el nombre de usuario "admin" ya existe en la base de datos
  - **Cuando** el gerente intenta crear otro usuario utilizando exactamente el nombre "admin"
  - **Entonces** el sistema rechaza la operación y despliega el mensaje "Nombre de usuario ya existe".

- **PT-03 (Caso límite - Bloqueo de baja de administrador):**
  - **Dado** que solo existe un Gerente activo en el sistema
  - **Когда / Cuando** se intenta presionar el botón de desactivar sobre ese único usuario administrador
  - **Entonces** el sistema impide la acción y muestra "No es posible desactivar al único administrador del sistema".