# PampaGanadero.Check

Validador *in situ* de caravanas electrónicas para ganado — offline-first.  
Diseñado para técnicos, veterinarios y productores de La Pampa.

## Características

- Lectura UHF (ISO 18000-6C) de caravanas SENASA  
- Validación offline contra padrón local (SQLite)  
- Detección de: duplicados, batería baja, no registradas, fuera de provincia  
- Generación de reportes PDF con QR para subir luego  
- Modo simulado para entrenamiento  

## Requisitos

- .NET 8 Runtime  
- (Opcional) Lector UHF Feig OBID, Impinj, o adaptador serie genérico  

## Uso

```bash
dotnet run --project src/Presentation.Cli
```

Licencia
MIT — Uso libre en entidades públicas y privadas de La Pampa.
