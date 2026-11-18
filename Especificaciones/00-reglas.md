# 00 — Reglas del proyecto SIGERE

Reglas que gobiernan **todo** el código del repositorio. Si una tarea contradice una regla, se corrige la tarea, no la regla.

## 1. Stack
- C# sobre .NET 8, Windows Forms, Visual Studio 2022.
- SQL Server. Acceso a datos con `Microsoft.Data.SqlClient` (no `System.Data.SqlClient`, en desuso).
- Contraseñas con `BCrypt.Net-Next`.
- Pruebas: TDD manual (casos Given-When-Then en `04-casos-tdd.md`, con captura de la ejecución).

## 2. Arquitectura (MVC multicapa)
```
SIGERE/
├── Models/          Entidades (Usuario, Producto, Venta, ...). Sin lógica de BD ni de UI.
├── Views/           Formularios WinForms. Sin lógica de negocio ni SQL.
├── Controllers/     Reciben eventos de la vista, llaman a Services y devuelven resultados.
├── Services/        Reglas de negocio y validación (la validación vive aquí, no en la vista).
├── DataAccess/      DAOs/repositorios: único lugar donde hay SQL.
├── Patterns/        Strategy, Observer, Singleton.
├── Config/          Lectura de configuración (cadena de conexión).
└── Tests/           Casos TDD manuales y evidencias.
```
Dependencias permitidas: Views → Controllers → Services → DataAccess → Models. Nunca al revés.

## 3. Seguridad
- **Prohibido** escribir credenciales o cadenas de conexión con usuario/contraseña en el código.
- La cadena de conexión se lee de la variable de entorno `SIGERE_DB_CONNECTION` (o de `appsettings.json` local, ignorado por Git).
- Todo SQL con parámetros (`@param`); nunca concatenar texto del usuario.
- La validación de datos se hace en `Services` (servidor), aunque la vista también valide por comodidad.
- Contraseñas siempre con hash BCrypt; el modelo nunca se muestra ni se registra en bitácora con la contraseña.
- Los mensajes de error al usuario no revelan detalles internos (ni SQL, ni rutas, ni si el usuario existe).
- Cada operación crítica se registra en `Bitacora` (RF18).

## 4. Código limpio
- Nombres descriptivos en español, `PascalCase` para tipos y métodos, `camelCase` para variables locales.
- Un método hace una cosa. Sin código duplicado: si se repite, se extrae.
- Manejo de errores con excepciones propias o resultados explícitos; nunca `catch` vacío.
- Comentarios solo para explicar el porqué, y para los contratos (ver sección 6).

## 5. SOLID (cómo se aplica aquí)
- **S:** `UsuarioDAO` solo accede a datos; autenticación en `AutenticacionService`; permisos en `AutorizacionService`.
- **O:** nuevos métodos de pago se agregan como nuevas clases (Strategy), sin modificar `VentaService`.
- **L:** toda `IEstrategiaPago` es intercambiable sin romper el cobro.
- **I:** interfaces pequeñas (`IUsuarioRepositorio`, `IProductoRepositorio`, ...).
- **D:** los Services dependen de interfaces, no de DAOs concretos.

## 6. Contratos
En métodos clave, en el comentario XML:
```csharp
/// @pre   monto >= total
/// @post  resultado == monto - total && resultado >= 0
/// @inv   total >= 0
```
Se hacen cumplir con guard clauses (`ArgumentException`) y `Debug.Assert`.

## 7. Interfaz y accesibilidad (WCAG 2.2)
- Paleta azul: fondo de encabezado `#0D1B40`, acento `#1F6FB2`, texto oscuro `#1E293B`, fondo claro `#F0F6F8`. Texto blanco sobre `#1F6FB2` tiene contraste aproximado de 5.3:1 (mínimo AA = 4.5:1); verificar con una herramienta antes de entregar.
- Todo control con etiqueta visible y `AccessibleName` / `AccessibleDescription`.
- Uso completo con teclado: `TabIndex` lógico, atajos con `&` (Alt+letra), `AcceptButton` y `CancelButton`, foco visible.
- No transmitir información solo con color: los errores llevan texto e ícono.
- Consistencia: mismos botones, mismos nombres y misma ubicación en todas las pantallas.
- Retroalimentación: mensaje claro tras cada acción (éxito o error) y confirmación antes de acciones destructivas.
- Prevención de errores: deshabilitar acciones no válidas, validar mientras se escribe.
- Texto alternativo para imágenes e íconos (`AccessibleName`).

## 8. Git
- Un repositorio, ramas por funcionalidad (`feature/cu1-registrar-venta`).
- Cada integrante hace commits propios y frecuentes durante las semanas del proyecto.
- Mensajes en el formato `tipo: descripción` (`feat:`, `fix:`, `test:`, `docs:`).
- `.gitignore` debe excluir `appsettings.json`, `bin/`, `obj/` y `.vs/`.

## 9. Proceso con IA (Opción B)
- Todo prompt relevante se registra en `05-bitacora-y-auditoria.md`.
- Todo código generado con IA se revisa, se prueba y se corrige por una persona; las correcciones se documentan en la misma bitácora con evidencia.
