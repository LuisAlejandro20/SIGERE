# Plan de Diseño Técnico: F04 – Consultar disponibilidad

## 1. Arquitectura por Capas (MVC)
- **Vista (`Vista/CatalogoForm.cs`):** Formulario de Windows Forms con una barra de búsqueda (`TextBox`), un botón de búsqueda, una tabla (`DataGridView`) para listar resultados y etiquetas informativas de detalle. No contiene lógica SQL ni de negocio.
- **Controlador (`Controlador/ProductoController.cs`):** Recibe el texto de búsqueda desde la vista, valida que no esté vacío, e invoca al modelo. Retorna los resultados formateados.
- **Modelo / DAO (`Modelo/ProductoDAO.cs` y `Modelo/Producto.cs`):** Contiene la entidad `Producto` y el método de acceso a datos que ejecuta la consulta `SELECT` parametrizada contra SQL Server (`SIGERE_DB`).

## 2. Decisión sobre Patrones de Diseño GoF
- **Patrón seleccionado:** *Ninguno (No aplica)*.
- **Justificación:** Al tratarse de una operación de consulta de solo lectura directa (`SELECT` simple y mapeo a lista), aplicar un patrón complejo (como Strategy o Factory) añadiría sobreingeniería innecesaria. Se mantiene el principio KISS (Keep It Simple, Stupid) cumpliendo perfectamente con la separación MVC y SOLID.

## 3. Contratos de Diseño (Diseño por Contrato)
Método clave: `BuscarProductos(string criterio)` en `ProductoDAO.cs`
- **@pre (Precondición):** `criterio` no puede ser nulo (`string.IsNullOrWhiteSpace` debe ser manejado controlando entradas vacías).
- **@post (Postcondición):** Retorna una lista de objetos `Producto` (vacía si no hay coincidencias, nunca nula).
- **@inv (Invariante):** La conexión a la base de datos se abre y se cierra correctamente mediante bloques `using` para evitar fugas de recursos.

## 4. Principios SOLID Aplicados
- **Responsabilidad Única (SRP):** `ProductoDAO` maneja exclusivamente la persistencia de productos; `ProductoController` gestiona las reglas de flujo; `CatalogoForm` solo dibuja la interfaz y captura eventos del usuario.