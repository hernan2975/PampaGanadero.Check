# Guía de Campo — Pampa Ganadero.Check

## Zonas de uso típico en La Pampa

| Localidad         | Conectividad | Recomendación               |
|-------------------|--------------|-----------------------------|
| Santa Rosa        | 4G estable   | Modo normal + sync API      |
| General Pico      | 4G intermitente | USB + modo offline       |
| Eduardo Castex    | Sin 4G       | Solo USB / modo simulado    |
| Lonquimay         | Sin 4G       | Solo USB / modo simulado    |
| 25 de Mayo        | 3G débil     | USB primario, API secundario|

## Procedimiento en campo

1. **Preparación**  
   - Cargar última base SENASA vía USB (usar `SenasaDbUpdater.exe`)  
   - Verificar batería del dispositivo (>50%)

2. **Escaneo**  
   - Activar lector UHF  
   - Acercar a la oreja del animal (distancia < 30 cm)  
   - Esperar confirmación visual/auditiva

3. **Interpretación**  
   - ✅ Verde: Aceptada  
   - ⚠️ Amarillo: Revisar (ej: batería baja)  
   - ❌ Rojo: Rechazada (no movilizar)

4. **Registro**  
   - Si hay alertas, generar PDF con botón "Exportar"  
   - Guardar en carpeta `reports/`  
   - Subir al SNT al llegar a zona con red

## Soporte técnico  
📞 02954-123456 (Dirección de Ganadería, La Pampa)  
📧 soporte.ganadero@produccion.lapampa.gob.ar

> Versión: 1.0 — Noviembre 2025
