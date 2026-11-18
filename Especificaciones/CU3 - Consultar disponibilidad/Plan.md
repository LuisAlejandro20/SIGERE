# Plan Técnico: CU3 — Consultar disponibilidad

## 1. Arquitectura por Capas (MVC y Multicapa)
- **Views (`Views/FrmInventario.cs`):** Formulario WinForms con un `TextBox` para criterios de búsqueda, un botón de consulta y un `DataGridView` para los resultados. Sin lógica de negocio ni consultas SQL.
- **Controllers (`Controllers/InventarioController.cs`):** Recibe el evento de búsqueda de la vista, valida que el criterio no esté vacío y solicita los datos al servicio.
- **Services (`Services/InventarioService.cs`):** Contiene la lógica de negocio para filtrar y procesar la consulta de productos.
- **DataAccess (`DataAccess/ProductoRepositorio.cs`):** Ejecuta la sentencia `SELECT` parametrizada contra la tabla `Productos` y `Categorias` en SQL Server.
- **Models (`Models/Producto.cs`):** Entidad plana de mapeo de datos.

## 3. Contratos de Diseño (DbC)
Método clave: `BuscarProductos(string criterio)` en `ProductoRepositorio.cs`
- **@pre (Precondición):** `criterio` no puede ser nulo o vacío (se valida con guard clauses).
- **@post (Postcondición):** Retorna una lista genérica de objetos `Producto` (vacía si no hay coincidencias, nunca nula).
- **@inv (Invariante):** Las conexiones a la base de datos se abren y cierran de manera segura mediante bloques `using`.