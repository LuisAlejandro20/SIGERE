# CU3 — Consultar disponibilidad de productos

## Caso de uso
- **Actor principal:** Vendedor, Almacenista, Gerente (consultando en mostrador o almacén).
- **Precondición:** El usuario debe haber iniciado sesión en el sistema con un rol activo. El producto debe estar registrado en el catálogo (`Productos`).
- **Postcondición:** El usuario ha consultado la existencia, precio y ubicación del producto sin modificar ningún dato en la base de datos (operación de solo lectura).

## Escenario principal
1. El usuario selecciona la opción 'Consultar inventario' en la interfaz principal.
2. El sistema habilita el formulario de búsqueda de refacciones.
3. El usuario ingresa el nombre, clave o descripción parcial del producto.
4. El sistema busca en la base de datos a través del repositorio y despliega los resultados coincidentes en una tabla.
5. El sistema muestra los detalles completos de la refacción: Clave, Nombre, Precio, Existencia actual y Ubicación física en almacén.

## Escenarios alternos
- **3a. Búsqueda sin resultados:** Si el término ingresado no coincide con ningún registro en la base de datos, el sistema muestra el mensaje claro "Producto no encontrado" y permite reintentar la búsqueda sin arrojar excepciones.
- **5a. Existencia en ceros:** Si la existencia numérica es igual a cero, el sistema despliega el detalle indicando explícitamente el stock agotado.

## Reglas de negocio
- La consulta es estrictamente de solo lectura; bajo ninguna circunstancia altera los registros de inventario.
- Se soporta búsqueda tanto por coincidencia exacta de clave interna (`CLAVE`) como por coincidencia parcial (`LIKE`) en el nombre comercial.

## Criterios de aceptación (Given-When-Then)
- **DIS-01 (Camino feliz - Búsqueda por clave):**
  - **Dado** que el producto con clave `RES-1K` existe en la base de datos con precio y ubicación
  - **Cuando** el usuario ingresa `RES-1K` en el buscador y presiona buscar
  - **Entonces** el sistema muestra su existencia, precio y ubicación en almacén.

- **DIS-02 (Camino feliz - Búsqueda por nombre parcial):**
  - **Dado** que existen productos que contienen la cadena "Resistencia" en su nombre
  - **Когда / Cuando** el usuario busca por parte del nombre
  - **Entonces** el sistema lista todas las coincidencias correspondientes.

- **DIS-03 (Escenario alterno - Producto no encontrado):**
  - **Dado** que el usuario se encuentra en el buscador de inventario
  - **Cuando** busca un texto o clave sin coincidencias en la BD
  - **Entonces** el sistema muestra el mensaje "Producto no encontrado" de forma limpia.