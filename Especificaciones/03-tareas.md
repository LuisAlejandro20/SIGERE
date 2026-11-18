# 03 — Tareas

Marcar con `[x]` al terminar. Cada tarea se hace en su rama y con commits propios.

## Fase 1 — Base
- [ ] T01 Crear la BD con `database/sigere_db.sql` y verificar que corre sin errores
- [ ] T02 Crear solución y carpetas MVC (Models, Views, Controllers, Services, DataAccess, Patterns, Config)
- [ ] T03 `ConfiguracionBD` (Singleton) leyendo `SIGERE_DB_CONNECTION`
- [ ] T04 Corregir `Usuario` (teléfono `string`) y migrar `UsuarioDAO` a `UsuarioRepositorio`
- [ ] T05 `BitacoraRepositorio` + `BitacoraService`

## Fase 2 — Login y usuarios (CU7)
- [ ] T06 `AutenticacionService` + `AutorizacionService` (matriz de roles)
- [ ] T07 `FrmLogin` (accesible, con teclado)
- [ ] T08 `FrmPrincipal` con menú según rol
- [ ] T09 CRUD de usuarios: `FrmUsuarios`, `UsuarioService`
- [ ] T10 Casos TDD de Login y CU7 ejecutados con captura

## Fase 3 — Punto de venta
- [ ] T11 CU3 Consultar disponibilidad
- [ ] T12 CU1 Registrar venta
- [ ] T13 Strategy de pago (`IEstrategiaPago` + 3 implementaciones)
- [ ] T14 CU2 Cobrar venta con transacción de BD + corte de caja
- [ ] T15 Ticket de venta (impresión o vista previa)
- [ ] T16 Casos TDD de CU3, CU1, CU2 con captura

## Fase 4 — Almacén y compras
- [ ] T17 Observer de stock mínimo
- [ ] T18 CU5 Generar orden de compra
- [ ] T19 CU4 Actualizar inventario
- [ ] T20 Casos TDD de CU5 y CU4 con captura

## Fase 5 — Media
- [ ] T21 CU6 Reportes (con exportación)
- [ ] T22 CU8 Confirmar surtido

## Fase 6 — Cierre
- [ ] T23 Revisión WCAG 2.2 (contraste, teclado, etiquetas)
- [ ] T24 Gantt planeado vs real con desfases justificados y 30 % restante proyectado
- [ ] T25 Bitácora semanal con carga equilibrada
- [ ] T26 Coevaluación individual (Anexo B)
- [ ] T27 Revisión de código entre integrantes y registro de correcciones