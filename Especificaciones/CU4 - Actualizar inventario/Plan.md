# Plan Técnico: CU4 — Actualizar inventario

## 1. Arquitectura por Capas (MVC y Multicapa)
- **Views (`Views/FrmRecepcionOrden.cs`):** Interfaz para listar órdenes confirmadas, mostrar detalles de productos y capturar cantidades reales.
- **Controllers (`Controllers/InventarioController.cs` o `CompraController.cs`):** Coordina la validación de diferencias y la llamada al servicio de recepción.
- **Services (`Services/InventarioService.cs`):** Orquesta la transacción ACID que involucra actualizar el stock y cerrar la orden.
- **DataAccess (`DataAccess/OrdenCompraRepositorio.cs` y `ProductoRepositorio.cs`):** Ejecuta las operaciones de actualización transaccional en SQL Server.
- **Models (`Models/OrdenCompra.cs`, `Models/DetalleOrdenCompra.cs`):** Entidades del dominio.

## 3. Contratos de Diseño (DbC)
Método clave: `RecibirMercancia(int idOrden, Dictionary<int, int> cantidadesRecibidas)` en `InventarioService.cs`
- **@pre (Precondición):** `idOrden > 0`, la orden debe existir y estar en estatus `Confirmada`.
- **@post (Postcondición):** Las existencias en la tabla `Productos` se incrementan correctamente y el estatus de la orden cambia a `Recibida`.
- **@inv (Invariante):** Las existencias de productos nunca pueden volverse negativas (`Existencia >= 0`).