# Protocolo de Validación SENASA — Res. 1019/2021

## Requisitos mínimos para caravanas electrónicas

1. **Formato del número SENASA**  
   - 12 caracteres  
   - Prefijo provincial: `LP` para La Pampa  
   - Ej: `LP0025481937`

2. **Tecnología UHF**  
   - ISO/IEC 18000-63 (Clase 1, Gen 2)  
   - Frecuencia: 902–928 MHz (Argentina)

3. **Registro en SNT**  
   - Caravana debe figurar en padrón oficial al momento de la lectura  
   - No debe estar asociada a más de un animal activo

4. **Integridad física**  
   - Sin daños visibles que impidan lectura  
   - Batería > 2.7V (para tags activas, si aplica)

## Hallazgos críticos (rechazo automático)

- Caravana no registrada  
- Duplicado (mismo UID en >1 animal vivo)  
- Número SENASA inválido o fuera de provincia  
- UID con formato incorrecto (<24 hex chars)

> Documento basado en: SENASA Res. 1019/2021, Disposición DNG 07/2023, Normativa Provincial LP N°12/2024
