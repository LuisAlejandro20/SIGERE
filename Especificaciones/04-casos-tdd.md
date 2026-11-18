# 04 — Casos TDD manual (Given-When-Then)

Escribir **antes** de programar. Después de ejecutar cada caso, llenar "Resultado obtenido" y adjuntar captura en `Tests/evidencias/` con el nombre del ID (ej. `LOG-01.png`).

Estados: ⬜ pendiente · ✅ pasó · ❌ falló (anotar la corrección en `05-bitacora-y-auditoria.md`).

## Login
| ID | Given | When | Then (esperado) | Resultado obtenido | Estado |
|---|---|---|---|---|---|
| LOG-01 | Existe el usuario activo `gerente1` con su contraseña | Ingresa credenciales correctas | Entra al menú principal con opciones del Gerente | | ⬜ |
| LOG-02 | Existe `gerente1` | Ingresa contraseña incorrecta | Mensaje genérico "Usuario o contraseña incorrectos"; no entra | | ⬜ |
| LOG-03 | Existe un usuario con estatus `Inactivo` | Ingresa credenciales correctas | No entra; mismo mensaje genérico | | ⬜ |
| LOG-04 | Pantalla de login abierta | Deja vacíos usuario o contraseña y presiona Enter | Se indica qué campo falta; no se consulta la BD | | ⬜ |
| LOG-05 | Sesión iniciada como Vendedor | Revisa el menú | No aparecen Usuarios, Compras ni Reportes | | ⬜ |
| LOG-06 | Cualquier intento de login | Se completa (éxito o fallo) | Queda un registro en `Bitacora` sin contraseña | | ⬜ |

## CU7 — Gestionar usuarios
| ID | Given | When | Then | Resultado obtenido | Estado |
|---|---|---|---|---|---|
| USR-01 | Gerente en `FrmUsuarios` | Crea un usuario con datos válidos | Se guarda; contraseña almacenada con hash BCrypt | | ⬜ |
| USR-02 | Existe el usuario `vendedor1` | Crea otro con `USUARIO = vendedor1` | Se rechaza con mensaje de nombre ya existente | | ⬜ |
| USR-03 | Existe un usuario | Modifica su rol y guarda | El cambio persiste al reiniciar la aplicación | | ⬜ |
| USR-04 | Existe un usuario activo distinto del gerente en sesión | Lo desactiva | Estatus `Inactivo`; el registro no se borra | | ⬜ |
| USR-05 | Gerente en sesión | Intenta desactivarse a sí mismo | Se bloquea con mensaje claro | | ⬜ |
| USR-06 | Teléfono de 10 dígitos, ej. 6621234567 | Guarda el usuario | Se guarda completo sin truncarse | | ⬜ |

## CU3 — Consultar disponibilidad
| ID | Given | When | Then | Resultado obtenido | Estado |
|---|---|---|---|---|---|
| DIS-01 | Producto existente con clave `RES-1K` | Busca por clave | Muestra existencia, precio y ubicación | | ⬜ |
| DIS-02 | Producto existente | Busca por parte del nombre | Lista las coincidencias | | ⬜ |
| DIS-03 | No existe el producto | Busca un texto sin coincidencias | Mensaje "producto no encontrado"; sin errores | | ⬜ |
| DIS-04 | Cualquier búsqueda | Se completa | No cambia ningún dato en la BD | | ⬜ |

## CU1 — Registrar venta
| ID | Given | When | Then | Resultado obtenido | Estado |
|---|---|---|---|---|---|
| VEN-01 | Producto con precio 10.00 y existencia 20 | Agrega 3 unidades | Total de la venta = 30.00 | | ⬜ |
| VEN-02 | Producto con existencia 2 | Intenta agregar 5 | Se rechaza e indica la existencia disponible | | ⬜ |
| VEN-03 | Venta con productos | Agrega el mismo producto otra vez | Se suma la cantidad en el mismo renglón | | ⬜ |
| VEN-04 | Cantidad 0 o negativa | Intenta agregar | Se rechaza | | ⬜ |
| VEN-05 | Venta con productos | Se guarda | Estado `Pendiente de cobro`; el inventario aún no cambia | | ⬜ |

## CU2 — Cobrar venta
| ID | Given | When | Then | Resultado obtenido | Estado |
|---|---|---|---|---|---|
| COB-01 | Venta de 30.00 en efectivo | Recibe 50.00 | Cambio = 20.00; venta `Cerrada` | | ⬜ |
| COB-02 | Venta de 30.00 en efectivo | Recibe 20.00 | Se rechaza por monto insuficiente; la venta sigue pendiente | | ⬜ |
| COB-03 | Venta cobrada | Revisa el inventario | Existencias descontadas según los renglones | | ⬜ |
| COB-04 | Venta cobrada | Revisa corte de caja | El total del día aumentó por el monto de la venta | | ⬜ |
| COB-05 | Pago con tarjeta | Confirma el cobro | Cierra la venta sin pedir monto ni calcular cambio | | ⬜ |
| COB-06 | Falla al descontar inventario (se simula) | Intenta cobrar | Se revierte todo: venta sigue pendiente y sin cobro registrado | | ⬜ |
| COB-07 | Venta cobrada | Se completa | Se genera el ticket y queda en bitácora | | ⬜ |

## CU5 — Generar orden de compra
| ID | Given | When | Then | Resultado obtenido | Estado |
|---|---|---|---|---|---|
| ORD-01 | Producto con existencia 3 y stock mínimo 5 | Abre generar orden | Aparece en la lista sugerida | | ⬜ |
| ORD-02 | Producto con existencia 20 y stock mínimo 5 | Abre generar orden | No aparece en la lista sugerida | | ⬜ |
| ORD-03 | Proveedor activo | Confirma la orden | Se genera folio único; estatus `Pendiente` | | ⬜ |
| ORD-04 | Proveedor inactivo | Intenta seleccionarlo | No se permite | | ⬜ |
| ORD-05 | Sesión como Vendedor | Intenta entrar a Compras | Acceso denegado | | ⬜ |
| ORD-06 | Existencia baja del mínimo por una venta | Se cobra la venta | El gerente recibe la alerta (Observer) | | ⬜ |

## CU4 — Actualizar inventario
| ID | Given | When | Then | Resultado obtenido | Estado |
|---|---|---|---|---|---|
| INV-01 | Orden `Confirmada` de 10 unidades | Captura 10 recibidas y confirma | Existencia +10; orden `Recibida` | | ⬜ |
| INV-02 | Orden `Confirmada` de 10 unidades | Captura 8 recibidas | Señala diferencia de 2 antes de confirmar | | ⬜ |
| INV-03 | Orden `Pendiente` | Intenta recibirla | No se permite; solo las confirmadas | | ⬜ |
| INV-04 | Recepción confirmada | Revisa la bitácora | Existe el registro de la operación | | ⬜ |