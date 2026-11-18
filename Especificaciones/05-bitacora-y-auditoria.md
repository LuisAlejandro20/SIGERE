# 05 — Bitácora de prompts y auditoría del código

Este archivo lo llena **el equipo con la información real**: qué se pidió, qué respondió la IA y qué se corrigió. Lo que se califica es el criterio humano, así que la columna de correcciones es la más importante.

## A. Bitácora de prompts
| # | Fecha | Integrante | Herramienta | Prompt (resumen o texto) | Qué entregó la IA | Qué se usó / se descartó |
|---|---|---|---|---|---|---|
| 1 | | | | | | |

## B. Auditoría y corrección humana del código
Por cada archivo generado con IA: revisar con esta lista y anotar lo encontrado.

**Lista de revisión**
- [ ] ¿Hay credenciales o secretos en el código?
- [ ] ¿Todo SQL usa parámetros?
- [ ] ¿La validación está en Services y no solo en la vista?
- [ ] ¿Maneja `null`/`DBNull` y errores sin `catch` vacío?
- [ ] ¿Respeta las capas (la vista no toca la BD)?
- [ ] ¿Cumple los contratos @pre/@post/@inv del plan?
- [ ] ¿Compila y pasa los casos de `04-casos-tdd.md`?
- [ ] ¿Cumple accesibilidad (teclado, etiquetas, contraste)?

| # | Fecha | Revisó | Archivo | Problema encontrado | Corrección aplicada | Evidencia (commit / captura) |
|---|---|---|---|---|---|---|
| 1 | | | | | | |

## C. Bitácora semanal de trabajo
| Semana | Integrante | Tareas (IDs de `03-tareas.md`) | Horas | Observaciones |
|---|---|---|---|---|
| 1 | | | | |