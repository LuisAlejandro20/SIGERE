# 02 — Plan técnico

## Capas y responsabilidades
| Capa | Ejemplos | Qué NO hace |
|---|---|---|
| Views | `FrmLogin`, `FrmPuntoVenta`, `FrmInventario`, `FrmOrdenCompra`, `FrmUsuarios` | No tiene SQL ni reglas de negocio |
| Controllers | `LoginController`, `VentaController`, `InventarioController`, ... | No accede a datos directamente |
| Services | `AutenticacionService`, `AutorizacionService`, `VentaService`, `InventarioService`, `CompraService`, `UsuarioService` | No conoce formularios |
| DataAccess | `UsuarioRepositorio`, `ProductoRepositorio`, `VentaRepositorio`, ... | No valida reglas de negocio |
| Models | `Usuario`, `Producto`, `Venta`, `DetalleVenta`, `OrdenCompra` | No tiene comportamiento de BD ni de UI |

## Patrones de diseño GoF
| Patrón | Tipo | Problema del proyecto que resuelve |
|---|---|---|
| **Strategy** (`IEstrategiaPago`: `PagoEfectivo`, `PagoTarjeta`, `PagoOtro`) | Comportamiento | Cada método de pago valida y calcula distinto (el efectivo necesita cambio, la tarjeta no). Evita `if/else` repetidos en `VentaService` y permite agregar métodos sin modificarlo. |
| **Observer** (`Producto` notifica a `AlertaStockObserver`) | Comportamiento | RF10: cuando la existencia baja del stock mínimo, el gerente debe enterarse sin que `VentaService` conozca a quién avisar. |
| **Singleton** (`ConfiguracionBD`) | Creacional | La cadena de conexión se lee una sola vez de la configuración segura y todos los repositorios usan la misma fuente, sin credenciales en el código. |

## Contratos en métodos clave
| Método | @pre | @post | @inv |
|---|---|---|---|
| `PagoEfectivo.CalcularCambio(total, monto)` | `total >= 0`, `monto >= total` | `cambio == monto - total` | `cambio >= 0` |
| `VentaService.AgregarProducto(venta, producto, cantidad)` | `cantidad > 0`, `cantidad <= producto.Existencia` | el detalle queda en la venta; el total se recalcula | `venta.Total >= 0` |
| `VentaService.Cobrar(venta, estrategia, monto)` | venta en `Pendiente de cobro` y con al menos un detalle | venta `Cerrada`; existencias descontadas | existencia nunca `< 0` |
| `InventarioService.RecibirMercancia(orden, cantidades)` | orden `Confirmada` | existencia aumentada; orden `Recibida` | existencia `>= 0` |
| `CompraService.GenerarOrden(productos, proveedor)` | al menos un producto; proveedor `Activo` | orden `Pendiente` con folio único | folio no se repite |
| `AutenticacionService.Autenticar(usuario, password)` | ambos no vacíos | devuelve `Usuario` activo o `null` | la contraseña no sale de la capa |

## Fases
1. **Base:** crear BD (`database/sigere_db.sql`), corregir `Usuario` y `UsuarioDAO`, `ConfiguracionBD`, bitácora.
2. **Login y CU7:** `FrmLogin`, menú principal por rol, CRUD de usuarios.
3. **Punto de venta:** CU3, CU1, CU2 (Strategy), ticket.
4. **Almacén y compras:** CU4, CU5 (Observer).
5. **Media:** CU6, CU8.
6. **Cierre:** evidencias TDD, Gantt final, revisión WCAG, coevaluación.

## Correcciones a los archivos entregados
| Archivo | Hallazgo | Acción |
|---|---|---|
| `UsuarioDAO.cs` | Cadena de conexión con usuario y contraseña en el código | Mover a `ConfiguracionBD` (variable de entorno) |
| `Usuario.cs` | `Telefono` es `int` | Cambiar a `string` |
| `UsuarioDAO.cs` | Autentica, verifica hash y autoriza en una sola clase | Separar en repositorio + `AutenticacionService` + `AutorizacionService` |
| `UsuarioDAO.cs` | Vendedor accede a "Inventario"; falta Almacenista y Proveedor | Aplicar la matriz de roles de `01-especificaciones.md` |
| `UsuarioDAO.cs` | `System.Data.SqlClient` en desuso | Usar `Microsoft.Data.SqlClient` |
| `UsuarioDAO.cs` | `Convert.ToInt32` sobre teléfono y `FECHA_ALTA` sin validar nulos | Manejo explícito de `DBNull` |