# 01 — Especificaciones

Derivadas de los requisitos funcionales (RF1–RF18), no funcionales (RNF), historias (HU01–HU08) y casos de uso (CU1–CU8) ya documentados.

## Prioridades
| Prioridad | Casos de uso | Motivo |
|---|---|---|
| **Alta** | Login, CU1, CU2, CU3, CU4, CU5, CU7 | Cubren el flujo central (vender, cobrar, inventario, compras) y el CRUD de usuarios. 6 de 8 casos de uso = 75 %. |
| Media | CU6 Reportes, CU8 Confirmar surtido | Se construyen después de la Alta. |

> La prioridad es una propuesta del equipo; confirmarla con el profesor.

## Roles y permisos
| Módulo | Vendedor | Almacenista | Gerente | Proveedor |
|---|:-:|:-:|:-:|:-:|
| Registrar venta (CU1) | ✔ | | ✔ | |
| Cobrar venta (CU2) | ✔ | | ✔ | |
| Consultar disponibilidad (CU3) | ✔ | ✔ | ✔ | |
| Actualizar inventario (CU4) | | ✔ | ✔ | |
| Generar orden de compra (CU5) | | | ✔ | |
| Reportes (CU6) | | | ✔ | |
| Gestionar usuarios (CU7) | | | ✔ | |
| Confirmar surtido (CU8) | | | | ✔ |

## Decisiones de diseño que difieren del diccionario de datos
- **D-01 — Acceso del Proveedor.** El diccionario no da credenciales a `Proveedores`, pero CU8 exige que el proveedor inicie sesión. Se resuelve con `Usuarios.ROL = 'Proveedor'` y una columna opcional `Usuarios.IdProveedor` que lo enlaza a su registro. Hay que actualizar el diccionario (campo 77).
- **D-02 — Teléfono.** El diccionario lo define numérico de 10 dígitos; un `int` no lo soporta. Pasa a `VARCHAR(10)` en BD y `string` en el modelo.

## Especificación por caso de uso

### Login (transversal, RNF7–RNF9)
- Entrada: usuario y contraseña. Salida: sesión con `Usuario` y su rol.
- Solo usuarios con `ESTATUS = 'Activo'`.
- Credenciales inválidas → mensaje genérico, sin indicar cuál campo falló.
- Cada inicio de sesión (éxito o fallo) se registra en bitácora.

### CU1 — Registrar venta (RF1, RF2)
- El vendedor busca productos por nombre o clave y agrega cantidades.
- No se puede agregar más cantidad que la existencia.
- El total se calcula con los precios vigentes del catálogo.
- La venta queda en estado `Pendiente de cobro`.

### CU2 — Cobrar venta (RF3–RF5)
- Métodos: Efectivo, Tarjeta, Otro (Strategy).
- Efectivo: el monto recibido debe ser ≥ total; se calcula el cambio.
- Al cobrar: venta `Cerrada`, inventario descontado, corte de caja actualizado, ticket generado, bitácora.
- Todo ocurre en una sola transacción de BD: o se guarda todo, o nada.

### CU3 — Consultar disponibilidad (RF6)
- Búsqueda por nombre o clave; muestra existencia, precio y ubicación.
- Solo lectura; no modifica datos.

### CU4 — Actualizar inventario (RF7–RF9)
- El almacenista elige una orden `Confirmada` y captura las cantidades recibidas.
- El sistema señala diferencias contra lo solicitado.
- Al confirmar: aumenta la existencia, la orden pasa a `Recibida`, se registra en bitácora.

### CU5 — Generar orden de compra (RF10, RF11)
- Sugiere productos con `EXISTENCIA <= STOCK_MINIMO` (Observer).
- El gerente ajusta cantidades y elige un proveedor activo.
- Se genera folio único y fecha; la orden queda `Pendiente`.

### CU7 — Gestionar usuarios (RF14, RF15) — CRUD completo
- Crear, consultar, modificar y desactivar (baja lógica, no se borra).
- `USUARIO` único; contraseña con BCrypt; solo el Gerente accede.
- Un usuario no puede desactivarse a sí mismo.

### CU6 y CU8 (Media)
Se especifican en detalle al iniciar su fase.

## Criterios de aceptación generales
- Todos los datos persisten en SQL Server.
- Ninguna pantalla accede a la BD directamente.
- Todos los casos de `04-casos-tdd.md` están ejecutados y con captura.