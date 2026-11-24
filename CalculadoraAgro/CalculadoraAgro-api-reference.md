# API CalculadoraAgro

## Descripción general

API CalculadoraAgro provee una interfaz para invocar la política crediticia homónima del Motor de Evaluación, extendiendo las capacidades con enriquecimiento de datos, manejo de errores y registro transaccional. Los endpoints de la API sirven para analizar el flujo de fondos de clientes del Agro.

### URL Base

La URL varía según el ambiente:
| Ambiente | URL           |
|-------------|-------------------|
| TEST    | https://srvmebiwb-test:50011/CalculadoraAgro |
| PREP    | https://srvmebiwb-pre:50011/CalculadoraAgro |
| PROD    | https://srvmebiwb-prod:50011/CalculadoraAgro |

### Versión

La versión actual es 0.0.1, disponible solamente en el ambiente de TEST.

### Concurrencia máxima y tiempos

El servicio soporta hasta 28 transacciones concurrentes (límite compartido con los demás servicios del Motor de Evaluación). El tiempo medio de procesamiento de las invocaciones es 2 segundos, dependiendo la casuística del caso. El timeout se encuentra definido en 30 segundos.

### Códigos de estado HTTP

La API CalculadoraAgro usa los sigueintes códigos HTTP standard:

| Status code | Description   |
|-------------|---------------|
| `200 OK`    | La invocación fue exitosa
| `400 Bad Request` | Los datos del request no se ajustan al contrato expuesto
| `403 Forbidden` | El origen no se encuentra autorizado para invocar este servicio
| `405 Method Not Allowed` | Se efectuó una invocación utilizando un método no habilitado
| `408 Request Timeout` | El procesamiento de la solicitud excedió el Timeout definido
| `500 Internal Server Error` | El Motor de Evaluación tuvo un error fatal no controlado

### Errores

El servicio contempla una serie de errores controlados que reflejan que la evalución crediticia no logró efectuarse. Según el caso, estos errores pueden resolverse con un reintento.

| Error                                   | Description      |
|-----------------------------------------|------------------|
| Error Tipo de Cambio | Falló el servicio Inversiones_ConsultaCotizacionesMonedaExtranjeraBCRA |

### Códigos de Rechazo

Pendiente

### Endpoints

Lo siguientes endpoints se encuentran habilitados para invocar el servicio:

| Method | Endpoint name                            | Description             |
|--------|------------------------------------------|-------------------------|
| POST   | /Riesgos_CalculaFlujoFondosAgro	   		| Exposicion del servicio completo


## Riesgos_CalculaFlujoFondosAgro

Pendiente

### Endpoint

```
POST /Riesgos_CalculaFlujoFondosAgro
```

### Esquema
Puede visualizar el esquema del Request y Response e incluso probar con  [Swagger](https://srvmebiwb-test:50011/CalculadoraAgro/swagger/index.html)

#### Ejemplos

### Request de ejemplo

```
{
    "element": {
        "FechaSolicitud": "2024-08-16 16:22:45",
        "Aplicacion": 4,
        "NumeroCalificacion": "31882",
        "NumeroEvaluacion": "1",
        "MetodoCalificacion": "Calculadora Agro",
        "CUIT": "20134619776",
        "Segmento": "AGRO",
        "Subsegmento": "AGRO",
        "Sucursal": "302",
        "FechaHaciendaSementeras": "1970-01-01",
        "Hacienda": {
            "ModalidadCargaHacienda": "Detalle",
            "PoseeCuentaHacienda": "NO",
            "DestinoTambo": {
                "CantidadVacas": "0",
                "CantidadVaquillonas": "0",
                "CantidadToros": "0",
                "MontoDeclaradoVacas": "0",
                "MontoDeclaradoVaquillonas": "0",
                "MontoDeclaradoToros": "0"
            },
            "DestinoCriaInvernada": {
                "CantidadVacas": "0",
                "CantidadToros": "0",
                "CantidadOtros": "0",
                "MontoDeclaradoVacas": "0",
                "MontoDeclaradoToros": "0",
                "MontoDeclaradoOtros": "0"
            },
            "TotalBienesUsoBovinosTamboCriaInvernadaPorTotales": "0",
            "MontoTotalBienesUsoBovinosTamboCriaInvernadaPorTotales": "0",
            "DestinoVenta": {
                "TamboCriaInvernada": {
                    "CantidadVacas": "0",
                    "CantidadVaquillonas": "0",
                    "CantidadTernerosas": "0",
                    "CantidadNovillos": "0",
                    "CantidadNovillitos": "0",
                    "SumaTotalBienesCambioBovinoPorTotales": "0",
                    "CantidadOtros": "0"
                },
                "FeedLot": {
                    "CantidadVacas": "0",
                    "CantidadVaquillonas": "0",
                    "CantidadTernerosas": "0",
                    "CantidadNovillos": "0",
                    "CantidadNovillitos": "0",
                    "SumaTotalBienesCambioBovino": "0"
                },
                "MontosDeclarados": {
                    "Vacas": "0",
                    "Vaquillonas": "0",
                    "Terneros": "0",
                    "Novillos": "0",
                    "Novillitos": "0",
                    "Otros": "0"
                },
                "TotalBienesCambioBovinosTamboCriaInvernadaFeedLotPorTotales": "0",
                "CantidadCabezasSegunActaVacunacion": "0"
            }
        },
        "Sementeras": {
            "PoseeCuentaSementeras": "NO",
            "RegistrosSementeras": [
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                },
                {
                    "CultivoCereal": "0",
                    "CantidadCereal": "0",
                    "MedidaCereal": "0",
                    "MontoCereal": "0"
                }
            ],
            "TotalCerealesMontoPorTotales": "0",
            "TotalSementerasMontoPorTotales": "0",
            "FechaEstructuraProductivaDeclarada": "2024-08-16"
        },
        "Agricultura": {
            "DesarrollaAgricultura": "SI",
            "RegistrosAgricultura": [
                {
                    "Provincia": "Córdoba",
                    "Zona": "MARGINAL",
                    "PrimerCultivo": "MAIZ",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "65",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "59"
                },
                {
                    "Provincia": "Córdoba",
                    "Zona": "MARGINAL",
                    "PrimerCultivo": "SOJA",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "90",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "Córdoba",
                    "Zona": "MARGINAL",
                    "PrimerCultivo": "SORGO",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "25",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "0",
                    "Zona": "0",
                    "PrimerCultivo": "0",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "0",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "0",
                    "Zona": "0",
                    "PrimerCultivo": "0",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "0",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "0",
                    "Zona": "0",
                    "PrimerCultivo": "0",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "0",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "0",
                    "Zona": "0",
                    "PrimerCultivo": "0",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "0",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "0",
                    "Zona": "0",
                    "PrimerCultivo": "0",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "0",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "0",
                    "Zona": "0",
                    "PrimerCultivo": "0",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "0",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "0",
                    "Zona": "0",
                    "PrimerCultivo": "0",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "0",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "0",
                    "Zona": "0",
                    "PrimerCultivo": "0",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "0",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "0",
                    "Zona": "0",
                    "PrimerCultivo": "0",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "0",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "0",
                    "Zona": "0",
                    "PrimerCultivo": "0",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "0",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "0",
                    "Zona": "0",
                    "PrimerCultivo": "0",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "0",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                },
                {
                    "Provincia": "0",
                    "Zona": "0",
                    "PrimerCultivo": "0",
                    "SegundoCultivo": "0",
                    "HectareasPropias": "0",
                    "HectareasAvalistas": "0",
                    "HectareasTerceros": "0"
                }
            ],
            "HectareasContratoDestinadasAgriculturaAvalistas": "0",
            "HectareasContratoDestinadasAgriculturaTerceros": "0"
        },
        "ActividadTambo": {
            "PoseeDesarrollaActividadTambo": "NO",
            "CantidadVacasNumeroOrdenie": "0",
            "UsinaLacteaPyME": "0",
            "HectareasDestinadasAvalistas": "0",
            "HectareasDestinadasTerceros": "0",
            "HectareasContratoDestinadasAvalistas": "0",
            "HectareasContratoDestinadasTerceros": "0",
            "HectaresPropiasDestinadas": "0",
            "Tambos": [
                {
                    "FechaLiquidacion": null,
                    "LitrosMensualesSegunLiquidacion": "0"
                },
                {
                    "FechaLiquidacion": null,
                    "LitrosMensualesSegunLiquidacion": "0"
                },
                {
                    "FechaLiquidacion": null,
                    "LitrosMensualesSegunLiquidacion": "0"
                }
            ]
        },
        "ActividadGanaderia": {
            "DesarrollaActividadGanaderia": "NO",
            "DesarrollaActividadGanaderiaCriaCompleto": "NO",
            "DesarrollaActividadGanaderiaInvernada": "NO",
            "DesarrollaActividadGanaderiaFeedLot": "NO",
            "CantidadCabezasCapacidadFeedLot": "0",
            "HectareasDestinadasAvalistas": "0",
            "HectareasDestinadasTerceros": "0",
            "HectareasContratoDestinadasAvalistas": "0",
            "HectareasContratoDestinadasTerceros": "0",
            "HectaresPropiasDestinadas": "0"
        },
        "ActividadContratista": {
            "DesarrollaActividadComoContratistaRural": "NO",
            "RegistrosContratista": [
                {
                    "ServicioComoContratista": "0",
                    "CantidadHectareasToneladasRollos": "0"
                },
                {
                    "ServicioComoContratista": "0",
                    "CantidadHectareasToneladasRollos": "0"
                },
                {
                    "ServicioComoContratista": "0",
                    "CantidadHectareasToneladasRollos": "0"
                },
                {
                    "ServicioComoContratista": "0",
                    "CantidadHectareasToneladasRollos": "0"
                },
                {
                    "ServicioComoContratista": "0",
                    "CantidadHectareasToneladasRollos": "0"
                },
                {
                    "ServicioComoContratista": "0",
                    "CantidadHectareasToneladasRollos": "0"
                },
                {
                    "ServicioComoContratista": "0",
                    "CantidadHectareasToneladasRollos": "0"
                },
                {
                    "ServicioComoContratista": "0",
                    "CantidadHectareasToneladasRollos": "0"
                },
                {
                    "ServicioComoContratista": "0",
                    "CantidadHectareasToneladasRollos": "0"
                },
                {
                    "ServicioComoContratista": "0",
                    "CantidadHectareasToneladasRollos": "0"
                },
                {
                    "ServicioComoContratista": "0",
                    "CantidadHectareasToneladasRollos": "0"
                }
            ]
        },
        "ParametrosModificadosManualmente": "",
        "InformacionAdicional": "Peticion a la calculadora desde Api Calificacion - SISA"
    }
}
```

### Response de ejemplo

```
{
    "elements": [
        {
            "Aplicacion": "4",
            "NumeroCalificacion": 31882,
            "NumeroEvaluacion": 1,
            "EstructuraProductivaCampaniaFecha": "2024-08-16",
            "EstructuraProductivaUltimaActualizacionParametros": "2025-01-15 00:00:00",
            "TipoCambioCOBIS": 1000,
            "Totales": {
                "Montos": {
                    "Hacienda": 0,
                    "Cereales": 0,
                    "Sementeras": 0
                },
                "HectareasExplotadas": {
                    "HectareasAgricultura": {
                        "Propias": 180,
                        "Avalistas": 0,
                        "Terceros": 59,
                        "TotalAgricultura": 239
                    },
                    "HectareasGanaderia": {
                        "Propias": 0,
                        "Avalistas": 0,
                        "Terceros": 0,
                        "TotalGanaderia": 0
                    },
                    "HectareasTambo": {
                        "Propias": 0,
                        "Avalistas": 0,
                        "Terceros": 0,
                        "TotalTambo": 0
                    },
                    "HectareasDeclaradas": {
                        "Propias": 180,
                        "Avalistas": 0,
                        "Terceros": 59,
                        "TotalDeclaradas": 239
                    }
                },
                "HectareasCamposArrendados": {
                    "ConContratoAvalistas": 0,
                    "ConContratoTerceros": 0,
                    "TotalConContrato": 0,
                    "TotalesDeclaradasAvalistasVinculadosTerceros": 59
                },
                "HectareasAgricultura": {
                    "Soja": 90,
                    "SojaSegunda": 0,
                    "Trigo": 0,
                    "Maiz": 124,
                    "Girasol": 0,
                    "Cebada": 0,
                    "Sorgo": 25,
                    "NucleoPropias": 0,
                    "NucleoAvalistas": 0,
                    "NucleoTerceros": 0,
                    "TotalNucleo": 0,
                    "Perimetrales": [
                        {
                            "Propias": 0,
                            "Avalistas": 0,
                            "Terceros": 0,
                            "TotalHectareas": 0
                        },
                        {
                            "Propias": 0,
                            "Avalistas": 0,
                            "Terceros": 0,
                            "TotalHectareas": 0
                        }
                    ],
                    "SudoesteTriguera": {
                        "Propias": 0,
                        "Avalistas": 0,
                        "Terceros": 0,
                        "TotalHectareas": 0,
                        "TotalMarginal": 239
                    },
                    "Marginal": {
                        "Propias": 180,
                        "Avalistas": 0,
                        "Terceros": 59
                    }
                }
            },
            "HectareasContratistaRural": {
                "Cosecha": 0,
                "Siembra": 0,
                "PicadoFino": 0,
                "PicadoGrueso": 0,
                "FumigacionAerea": 0,
                "FumigacionTerrestre": 0,
                "Laboreo": 0,
                "EmbolsadoGranos": 0,
                "Rotoenfardado": 0
            },
            "Tambo": {
                "TotalVacas": 0,
                "VacasEnOrdenie": 0,
                "PorcentajeVacasEnOrdenie": 0,
                "LitrosEstimadosMensuales": 0,
                "LitrosRealDiarioSobreLiquidacion": 0,
                "LitrosRealMensualSobreLiquidacion": 0,
                "Usina": "0"
            },
            "Ganaderia": {
                "MarcaCria": "NO",
                "MarcaInvernada": "NO",
                "MarcaFeedLot": "NO",
                "CapacidadCabezasFeedLot": 0,
                "Vientres": 0,
                "RestoCriaInvernada": 0,
                "CabezasFeedLot": 0,
                "CabezasSobreHectareas": 0,
                "PorcentajeOcupacion": 0,
                "TotalCabezas": 0,
                "TotalSobreActasVacunacion": 0
            },
            "EERRGlobalProyectado": {
                "ProyectadoAgricultura": {
                    "Ingresos": 204030000,
                    "GastosComercialesConCosecha": -93716100,
                    "IngresosNetos": 110313900,
                    "GastosDirectosConArrendamiento": -85638000,
                    "ResultadosOperacionPotenciales": 24675900,
                    "IngresosParticulares": 1,
                    "IngresosPromedio": 17002500,
                    "IngresosDolares": 204030,
                    "GastosComercialesConCosechaDolares": -937161,
                    "IngresosNetosDolares": 1103139,
                    "GastosDirectosConArrendamientoDolares": -85638,
                    "ResultadosOperacionPotencialesDolares": 246759
                },
                "ProyectadoContratistaRural": {
                    "Ingresos": 0,
                    "GastosDirectosConArrendamiento": 0,
                    "ResultadosOperacionPotenciales": 0,
                    "IngresosParticulares": 0,
                    "IngresosPromedio": 0
                },
                "ProyectadoTambo": {
                    "Ingresos": 0,
                    "GastosDirectosConArrendamiento": 0,
                    "ResultadosOperacionPotenciales": 0,
                    "IngresosParticulares": 0,
                    "IngresosPromedio": 0
                },
                "ProyectadoGanaderia": {
                    "Ingresos": 0,
                    "GastosDirectosConArrendamiento": 0,
                    "ResultadosOperacionPotenciales": 0,
                    "IngresosParticulares": 0,
                    "IngresosPromedio": 0
                },
                "ProyectadoTotal": {
                    "Ingresos": 204030000,
                    "GastosDirectosConArrendamiento": -85638000,
                    "ResultadosOperacionPotenciales": 24675900,
                    "IngresosPromedio": 17002500,
                    "GastosComercialesConCosecha": -93716100,
                    "IngresosNetos": 110313900
                }
            },
            "InformacionAdicional": "Peticion a la calculadora desde Api Calificacion - SISA",
            "Version": "CALCULADORA_v7.0_publicada.20241009"
        }
    ]
}
```

---
