# CU4 — Actualizar inventario (Recepción de mercancía)

## Caso de uso
- **Actor principal:** Almacenista.
- **Precondición:** El almacenista debe haber iniciado sesión con el rol de 'Almacenista'[cite: 38]. Debe existir una orden de compra generada previamente por el gerente con estatus `Confirmada`[cite: 38].
- **Postcondición:** El inventario físico ha sido incrementado con las cantidades recibidas, la orden de compra pasa a estatus `Recibida` y se genera el registro correspondiente en la bitácora del sistema[cite: 38].

## Escenario principal
1. El almacenista selecciona la opción 'Recepción de mercancía' en el sistema.
2. El sistema solicita al repositorio y despliega la lista de órdenes de compra con estatus `Confirmada`.
3. El almacenista selecciona la orden de compra correspondiente a la entrega física del proveedor.
4. El sistema despliega el detalle de los productos esperados (`DetalleOrdenCompra`).
5. El almacenista verifica físicamente la mercancía e ingresa en el sistema las cantidades reales recibidas por cada artículo.
6. El sistema compara automáticamente lo recibido contra lo solicitado y señala cualquier diferencia visualmente.
7. El almacenista confirma la recepción de la mercancía.
8. El sistema ejecuta una transacción ACID: actualiza las existencias sumando lo recibido a la tabla `Productos`, cambia el estatus de la orden a `Recibida` y registra la operación en la `Bitacora`.

## Escenarios alternos
- **6a. Diferencia en cantidades:** Si la cantidad física recibida es menor a la solicitada, el sistema advierte la diferencia para su respectiva aclaración administrativa, permitiendo continuar con el registro parcial o total según la regla de negocio.
- **3a. Orden no válida:** Si se intenta procesar una orden que se encuentra en estatus `Pendiente` o `Cancelada`, el sistema bloquea la acción y muestra un mensaje de restricción.

## Reglas de negocio
- Solo se pueden recibir mercancías asociadas a órdenes que tengan estrictamente el estatus `Confirmada`.
- La actualización de existencias y el cambio de estatus de la orden deben ocurrir dentro de una única transacción de base de datos (todo se guarda o se revierte).

## Criterios de aceptación (Given-When-Then)
- **INV-01 (Camino feliz - Recepción completa):**
  - **Dado** una orden `Confirmada` con 10 unidades solicitadas de un producto
  - **Cuando** el almacenista captura 10 unidades recibidas y confirma
  - **Entonces** la existencia del producto aumenta en +10 y la orden cambia a estatus `Recibida`.

- **INV-02 (Escenario alterno - Diferencia de cantidades):**
  - **Dado** una orden `Confirmada` de 10 unidades
  - **Cuando** el almacenista captura 8 unidades recibidas
  - **Entonces** el sistema señala la diferencia de 2 unidades antes de aplicar la confirmación final.

- **INV-03 (Caso límite - Orden en estatus incorrecto):**
  - **Dado** una orden de compra con estatus `Pendiente`
  - **Cuando** el almacenista intenta seleccionarla para recepción
  - **Entonces** el sistema lo impide y muestra un mensaje indicando que solo se reciben órdenes confirmadas.
