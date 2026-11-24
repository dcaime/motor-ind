# API EvaluaIndividuos

## Descripción general

API EvaluaIndividuos provee una interfaz para invocar la política crediticia homónima del Motor de Evaluación, extendiendo las capacidades con enriquecimiento de datos, manejo de errores y registro transaccional. Los endpoints de la API sirven para analizar la situación y capacidad de pago de un individuo en el contexto de la adquisición o modificación de un producto de crédito: Préstamo Personal, Hipotecario, Prendario, Tarjeta de Crédito o Acuerdo en Cuenta.

### URL Base

La URL varía según el ambiente:
| Ambiente | URL           |
|-------------|-------------------|
| TEST    | https://srvmebiwb-test:50008/EvaluaIndividuos |
| PREP    | https://srvmebiwb-pre:50008/EvaluaIndividuos |
| PROD    | https://srvmebiwb-prod:50008/EvaluaIndividuos |

### Versión

La versión actual es 0.0.1, disponible solamente en el ambiente de TEST.

### Concurrencia máxima y tiempos

El servicio soporta hasta 28 transacciones concurrentes (límite compartido con los demás servicios del Motor de Evaluación). El tiempo medio de procesamiento de las invocaciones es 6 segundos, dependiendo la casuística del caso. El timeout se encuentra definido en 50 segundos.

### Códigos de estado HTTP

La API EvaluaIndividuos usa los sigueintes códigos HTTP standard:

| Status code | Description   |
|-------------|---------------|
| `200 OK`    | La invocación fue exitosa
| `400 Bad Request` | Los datos del request no se ajustan al contrato expuesto
| `405 Method Not Allowed` | Se efectuó una invocación utilizando un método no habilitado
| `408 Rquest Timeout` | El procesamiento de la solicitud excedió el Timeout definido
| `500 Internal Server Error` | El Motor de Evaluación tuvo un error fatal no controlado

### Errores

El servicio contempla una serie de errores controlados que reflejan que la evalución crediticia no logró efectuarse. Según el caso, estos errores pueden resolverse con un reintento.

| Error                                   | Description      |
|-----------------------------------------|------------------|
| No se obtuvo informe | Falló la conexión un Bureau externo  |

### Códigos de Rechazo

En caso que la evaluación se efectúe, los resultados pueden ser Aprobado y Rechazado. El rechazo es deterministico y en este caso la respuesta no cambiará con un mero reintento. Según el caso, el rechazo puede convertirse en aprobación ajustando valores del Request. Para mayor detalle consultar el [listado de códigos](https://docs.google.com/spreadsheets/d/1T773KoV2D2i6iR7qh7Qx4T6586u7fS_kBQw5G3yR5UQ/edit?gid=1357485019#gid=1357485019) que provee una descripción y acción sugerida (si aplica).

### Endpoints

Lo siguientes endpoints se encuentran habilitados para invocar el servicio:

| Method | Endpoint name                            | Description             |
|--------|------------------------------------------|-------------------------|
| POST   | /Riesgos_ValidaOfertaCrediticia	   		| Exposicion del servicio completo


## Riesgos_ValidaOfertaCrediticia

Evalúa un individuo por CUIL para Préstamo Personal o Tarjeta de Crédito. Provee una interfaz completa de todos los campos que admite la política crediticia, manteniendo  la posibilidad de efectuar una invocación con un set mínimo. El contrato del servicio clona la contrapartida publicada en el ESB para simplificar la integración. 

### Endpoint

```
POST /Riesgos_ValidaOfertaCrediticia
```

### Esquema
Puede visualizar el esquema del Request y Response e incluso probar con  [Swagger](https://srvmebiwb-test:50008/EvaluaIndividuos/swagger/index.html)

#### Ejemplos

### Request de ejemplo

```
{
    "element": {
        "OrigenId": 3,
        "NumeroOportunidad": 565,
        "NumeroEvaluacion": 1,
        "ComercioId": 4,
        "ComercioSucursal": "4",
        "ComercioUsuario": "",
        "Provincia": "Tucuman",
        "SucursalMacroAsociada": 600,
        "Titular": {
            "NumeroCUIL": 20273870297,
            "Apellido": "CHAVES",
            "Nombre": "Nuria Maria Marisol",
            "FechaNacimiento": "1983-07-14"
        },
        "SolicitaTarjeta": "SI"
    }
}
```

### Response de ejemplo

```
{
    "elements": [
        {
            "OrigenId": 3,
            "NumeroOportunidad": 1235,
            "NumeroEvaluacion": 1,
            "EsCliente": "SI",
            "Cobis": 127,
            "NumeroCUIL": 23065412454,
            "Rama": "CLIENTE PRECA",
            "Dictamen": "Aprobar",
            "Codigos_Rechazos": "",
            "Explicacion": "",
            "PrestamoMontoMaximo": 0,
            "PrestamoCuotaMaxima": 0,
            "PrestamoPlazoMaximo": 0,
            "PrestamoTipoTasa": "",
            "PrestamoTNA": 0,
            "TarjetaCreditoLimite": 3710000,
            "CuentaCorrienteLimite": 0,
            "ReportePolitica": "",
            "OutputAdicional": "[Descripcion:0|0|01|004|OFERTA_PORTAL|618593||PRECALIF013][ConveniosBeneficios:0][Segmento:19][Subsegmento:17][Categoria:02][SegmentoCRM:04][SubSegmentoCRM:17][FechaNacimiento:06/05/1951][MarcaPEP:N][SujetoFATCAOCDE:][SujetoObligado:]"
        }
    ]
}
```

---
