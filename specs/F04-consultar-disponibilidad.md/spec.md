# F04 – Consultar disponibilidad y catálogo de refacciones

## Caso de uso (Basado en CU03)
- **Actor principal:** Vendedor (consultando en nombre del cliente en mostrador).
- **Precondición:** El usuario (vendedor) debe haber iniciado sesión en el sistema. El producto debe estar registrado en el catálogo.
- **Postcondición:** El usuario ha consultado la disponibilidad del producto sin modificar ningún dato del inventario.

## Escenario principal
1. El usuario selecciona la opción 'Consultar inventario' en el sistema.
2. El sistema habilita el buscador de refacciones.
3. El usuario ingresa el nombre, clave o descripción parcial/total del producto.
4. El sistema busca en la base de datos y despliega los resultados coincidentes.
5. El usuario selecciona el producto de interés para ver su detalle.
6. El sistema muestra el detalle completo: nombre, precio, existencia actual y ubicación física en almacén.

## Escenarios alternos
- **3a. Búsqueda sin resultados:** Si el término ingresado no coincide con ningún registro, el sistema muestra el mensaje 'Producto no encontrado' y permite reintentar la búsqueda.
- **6a. Producto agotado:** Si la existencia numérica es igual a cero (0), el sistema resalta el estatus indicando 'Producto agotado'.

## Reglas de negocio
- La consulta es una operación de solo lectura; bajo ninguna circunstancia modifica existencias en esta pantalla.
- Se puede buscar tanto por coincidencia exacta de clave interna como por coincidencia parcial (LIKE) en el nombre de la refacción.

## Criterios de aceptación (Given-When-Then)
- **PT-01 (Camino feliz):**
  - **Dado** que el vendedor inició sesión y existe un producto registrado (Ej. "Resistencia 10k", Clave: "RES-10K", Stock: 15, Ubicación: "Estante A-2")
  - **Cuando** el vendedor ingresa "RES-10K" en el buscador y presiona buscar
  - **Entonces** el sistema muestra los detalles del producto con su precio, existencia (15) y ubicación ("Estante A-2").

- **PT-02 (Escenario alterno - No encontrado):**
  - **Dado** que el vendedor se encuentra en el buscador de inventario
  - **Cuando** ingresa un código o nombre que no existe en la base de datos (Ej. "XYZ-999")
  - **Entonces** el sistema muestra el mensaje claro "Producto no encontrado".

- **PT-03 (Caso límite - Stock en cero):**
  - **Dado** que un producto tiene 0 piezas en existencia en la base de datos
  - **Когда / Cuando** el vendedor realiza la consulta de dicho producto
  - **Entonces** el sistema despliega la información del producto indicando explícitamente "Producto agotado".