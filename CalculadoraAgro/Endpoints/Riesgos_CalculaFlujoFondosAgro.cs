using CalculadoraAgro.Model;
using Carter;

namespace CalculadoraAgro.Endpoints
{
    public class Riesgos_CalculaFlujoFondosAgro : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/Riesgos_CalculaFlujoFondosAgro", async (RequestCalculadoraAgroArray requestArray) =>
            {
                Herramientas.loguear("Riesgos_CalculaFlujoFondosAgro - Version 1.1");
                var guid = Guid.NewGuid(); //Genera ID unico
                Herramientas.loguear("Inicia Id " + guid.ToString());
                Registro registro = new Registro("Riesgos_CalculaFlujoFondosAgro", guid, requestArray.element.CUIT.ToString(), requestArray.element.Aplicacion.ToString(), "", requestArray);

                var politicaCalculadora = new PoliticaCalculadoraAgro(); //Asigna ID unico para trazabalidad
                politicaCalculadora.Entrada = MapeaEntrada(requestArray);

                //Llama modeler para ejecutar reglas crediticias
                EvaluacionCrediticia evaluacion = await EvaluacionCrediticia.EvaluarAsync(politicaCalculadora, registro);

                //Mapea response
                var respuestaArray = new ResponseCalculadoraAgroArray();
                politicaCalculadora.decodificaSalida(evaluacion.ArraySalida);
                var respuesta = MapeaSalida(politicaCalculadora.Salida);
                respuesta.IdInterno = guid;
                respuestaArray.elements.Add(respuesta);
                Herramientas.loguear("Fin Id " + guid.ToString());

                //Devuelve response
                return respuestaArray;
            });
        }

        private DateTime? convertirFecha(string fechaSTR)
        {
            if (string.IsNullOrEmpty(fechaSTR))
            {
                return null;
            }
            var fecha = DateTime.Parse(fechaSTR); //2-11-2024 12:04:52
            return fecha;
        }

        private PoliticaCalculadoraAgroEntrada MapeaEntrada(RequestCalculadoraAgroArray requestArray)
        {
            var request = requestArray.element;
            var entrada = new PoliticaCalculadoraAgroEntrada();
            entrada.fecha_solicitud = convertirFecha(request.FechaSolicitud);
            entrada.ap_origen = request.Aplicacion;
            entrada.nro_calificacion = request.NumeroCalificacion;
            entrada.nro_eval = request.NumeroEvaluacion;
            entrada.metodo_de_calificacion = request.MetodoCalificacion;
            entrada.cuit = request.CUIT;
            entrada.Segmento = request.Segmento;
            entrada.Subsegmento = request.Subsegmento;
            entrada.Sucursal = request.Sucursal;
            entrada.fecha_hacienda_cereales_sementeras = convertirFecha(request.FechaHaciendaSementeras);
            if (request.Hacienda is not null)
            {
                entrada.modalidad_carga_hacienda = request.Hacienda.ModalidadCargaHacienda;
                entrada.posee_hacienda = request.Hacienda.PoseeCuentaHacienda;
                if (request.Hacienda.DestinoTambo is not null)
                {
                    entrada.vacas_tambo_cabezas = request.Hacienda.DestinoTambo.CantidadVacas;
                    entrada.vaquillonas_tambo_cabezas = request.Hacienda.DestinoTambo.CantidadVaquillonas;
                    entrada.toros_tambo_cabezas = request.Hacienda.DestinoTambo.CantidadToros;
                    entrada.vacas_tambo_pesos = request.Hacienda.DestinoTambo.MontoDeclaradoVacas;
                    entrada.vaquillonas_tambo_pesos = request.Hacienda.DestinoTambo.MontoDeclaradoVaquillonas;
                    entrada.toros_tambo_pesos = request.Hacienda.DestinoTambo.MontoDeclaradoToros;
                }
                if (request.Hacienda.DestinoCriaInvernada is not null)
                {
                    entrada.vacas_cria_inv_cabezas = request.Hacienda.DestinoCriaInvernada.CantidadVacas;
                    entrada.toros_cria_inv_cabezas = request.Hacienda.DestinoCriaInvernada.CantidadToros;
                    entrada.otros_cria_inv_cabezas = request.Hacienda.DestinoCriaInvernada.CantidadOtros;
                    entrada.vacas_cria_inv_pesos = request.Hacienda.DestinoCriaInvernada.MontoDeclaradoVacas;
                    entrada.toros_cria_inv_pesos = request.Hacienda.DestinoCriaInvernada.MontoDeclaradoToros;
                    entrada.otros_cria_inv_pesos = request.Hacienda.DestinoCriaInvernada.MontoDeclaradoOtros;
                }

                entrada.total_bs_uso_tci_cabezas_por_totales = request.Hacienda.TotalBienesUsoBovinosTamboCriaInvernadaPorTotales;
                entrada.total_bs_uso_tci_pesos_por_totales = request.Hacienda.MontoTotalBienesUsoBovinosTamboCriaInvernadaPorTotales;

                if (request.Hacienda.DestinoVenta is not null)
                {
                    if (request.Hacienda.DestinoVenta.TamboCriaInvernada is not null)
                    {
                        entrada.vacas_tci_venta_cabezas = request.Hacienda.DestinoVenta.TamboCriaInvernada.CantidadVacas;
                        entrada.vaquillonas_tci_venta_cabezas = request.Hacienda.DestinoVenta.TamboCriaInvernada.CantidadVaquillonas;
                        entrada.terneros_tci_venta_cabezas = request.Hacienda.DestinoVenta.TamboCriaInvernada.CantidadTernerosas;
                        entrada.novillos_tci_venta_cabezas = request.Hacienda.DestinoVenta.TamboCriaInvernada.CantidadNovillos;
                        entrada.novillitos_tci_venta_cabezas = request.Hacienda.DestinoVenta.TamboCriaInvernada.CantidadNovillitos;
                        entrada.total_bs_cambio_tci_cabezas_por_totales = request.Hacienda.DestinoVenta.TamboCriaInvernada.SumaTotalBienesCambioBovinoPorTotales;
                    }
                    if (request.Hacienda.DestinoVenta.FeedLot is not null)
                    {
                        entrada.otros_tci_venta_cabezas = request.Hacienda.DestinoVenta.FeedLot.CantidadOtros;
                        entrada.vacas_feedlot_venta_cabezas = request.Hacienda.DestinoVenta.FeedLot.CantidadVacas;
                        entrada.vaquillonas_feedlot_venta_cabezas = request.Hacienda.DestinoVenta.FeedLot.CantidadVaquillonas;
                        entrada.terneros_feedlot_venta_cabezas = request.Hacienda.DestinoVenta.FeedLot.CantidadTernerosas;
                        entrada.novillos_feedlot_venta_cabezas = request.Hacienda.DestinoVenta.FeedLot.CantidadNovillos;
                        entrada.novillitos_feedlot_venta_cabezas = request.Hacienda.DestinoVenta.FeedLot.CantidadNovillitos;
                        entrada.total_bs_cambio_feedlot_cabezas_por_totales = request.Hacienda.DestinoVenta.FeedLot.SumaTotalBienesCambioBovino;
                    }
                    if (request.Hacienda.DestinoVenta.MontosDeclarados is not null)
                    {
                        entrada.vacas_venta_pesos = request.Hacienda.DestinoVenta.MontosDeclarados.Vacas;
                        entrada.vaquilonas_venta_pesos = request.Hacienda.DestinoVenta.MontosDeclarados.Vaquillonas;
                        entrada.terneros_venta_pesos = request.Hacienda.DestinoVenta.MontosDeclarados.Terneros;
                        entrada.novillos_venta_pesos = request.Hacienda.DestinoVenta.MontosDeclarados.Novillos;
                        entrada.novillitos_venta_pesos = request.Hacienda.DestinoVenta.MontosDeclarados.Novillitos;
                        entrada.otros_venta_pesos = request.Hacienda.DestinoVenta.MontosDeclarados.Otros;
                    }
                    entrada.total_bs_cambio_tci_feedlot_pesos_por_totales = request.Hacienda.DestinoVenta.TotalBienesCambioBovinosTamboCriaInvernadaFeedLotPorTotales;
                    entrada.cant_cabezas_acta_vacunacion = request.Hacienda.DestinoVenta.CantidadCabezasSegunActaVacunacion;
                }
            }

            if (request.Sementeras is not null)
            {
                entrada.posee_cereales_sementeras = request.Sementeras.PoseeCuentaSementeras;
                entrada.modalidad_carga_cereales_y_sementeras = request.Sementeras.ModalidadCargaSementeras;
                if (request.Sementeras.RegistrosSementeras is not null)
                {
                    int idxRegistrosSementeras = 0;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_1 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_1 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_1 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_1 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_2 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_2 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_2 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_2 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_3 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_3 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_3 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_3 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_4 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_4 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_4 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_4 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_5 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_5 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_5 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_5 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_6 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_6 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_6 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_6 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_7 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_7 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_7 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_7 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_8 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_8 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_8 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_8 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_9 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_9 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_9 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_9 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_10 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_10 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_10 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_10 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_11 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_11 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_11 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_11 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_12 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_12 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_12 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_12 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_13 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_13 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_13 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_13 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_14 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_14 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_14 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_14 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_15 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_15 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_15 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_15 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                    idxRegistrosSementeras++;
                    if (request.Sementeras.RegistrosSementeras.Length > idxRegistrosSementeras)
                    {
                        entrada.cultivo_cereal_sementera_16 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CultivoCereal;
                        entrada.cant_cereal_sementera_16 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].CantidadCereal;
                        entrada.medida_cereal_sementera_16 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MedidaCereal;
                        entrada.monto_cereal_sementera_16 = request.Sementeras.RegistrosSementeras[idxRegistrosSementeras].MontoCereal;
                    }
                }

                entrada.total_cereales_pesos_por_totales = request.Sementeras.TotalCerealesMontoPorTotales;
                entrada.total_sementeras_pesos_por_totales = request.Sementeras.TotalSementerasMontoPorTotales;
                entrada.fecha_estructura_productiva = convertirFecha(request.Sementeras.FechaEstructuraProductivaDeclarada);
            }

            if (request.Agricultura is not null)
            {
                entrada.posee_agricultura = request.Agricultura.DesarrollaAgricultura;
                if (request.Agricultura.RegistrosAgricultura is not null)
                {
                    int idxRegistrosAgricultura = 0;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_1 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_1 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_1 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_1 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_1 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_1 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_1 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_2 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_2 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_2 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_2 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_2 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_2 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_2 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_3 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_3 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_3 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_3 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_3 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_3 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_3 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_4 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_4 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_4 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_4 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_4 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_4 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_4 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_5 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_5 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_5 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_5 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_5 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_5 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_5 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_6 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_6 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_6 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_6 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_6 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_6 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_6 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_7 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_7 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_7 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_7 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_7 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_7 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_7 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_8 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_8 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_8 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_8 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_8 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_8 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_8 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_9 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_9 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_9 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_9 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_9 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_9 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_9 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_10 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_10 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_10 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_10 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_10 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_10 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_10 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_11 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_11 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_11 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_11 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_11 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_11 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_11 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_12 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_12 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_12 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_12 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_12 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_12 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_12 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_13 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_13 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_13 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_13 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_13 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_13 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_13 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_14 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_14 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_14 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_14 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_14 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_14 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_14 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                    idxRegistrosAgricultura++;
                    if (request.Agricultura.RegistrosAgricultura.Length > idxRegistrosAgricultura)
                    {
                        entrada.ag_provincia_15 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Provincia;
                        entrada.ag_zona_15 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].Zona;
                        entrada.ag_cultivo_1ro_15 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].PrimerCultivo;
                        entrada.ag_cultivo_2do_15 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].SegundoCultivo;
                        entrada.ag_has_propias_15 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasPropias;
                        entrada.ag_has_avalistas_15 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasAvalistas;
                        entrada.ag_has_terceros_15 = request.Agricultura.RegistrosAgricultura[idxRegistrosAgricultura].HectareasTerceros;
                    }
                }

                entrada.ag_has_con_contrato_avalistas = request.Agricultura.HectareasContratoDestinadasAgriculturaAvalistas;
                entrada.ag_has_con_contrato_terceros = request.Agricultura.HectareasContratoDestinadasAgriculturaTerceros;
            }

            if (request.ActividadTambo is not null)
            {
                entrada.posee_tambo = request.ActividadTambo.PoseeDesarrollaActividadTambo;
                entrada.ta_vacas_en_ordenie = request.ActividadTambo.CantidadVacasNumeroOrdenie;
                entrada.ta_usina_lactea_pyme = request.ActividadTambo.UsinaLacteaPyME;
                entrada.ta_has_avalistas = request.ActividadTambo.HectareasDestinadasAvalistas;
                entrada.ta_has_terceros = request.ActividadTambo.HectareasDestinadasTerceros;
                entrada.ta_has_avalistas_con_contrato = request.ActividadTambo.HectareasContratoDestinadasAvalistas;
                entrada.ta_has_terceros_con_contrato = request.ActividadTambo.HectareasContratoDestinadasTerceros;
                entrada.ta_superficie_propia = request.ActividadTambo.HectaresPropiasDestinadas;
                if (request.ActividadTambo.Tambos is not null)
                {
                    int idxTambos = 0;
                    if (request.ActividadTambo.Tambos.Length > idxTambos)
                    {
                        entrada.ta_fecha_liq_1 = convertirFecha(request.ActividadTambo.Tambos[idxTambos].FechaLiquidacion);
                        entrada.ta_litros_mensuales_1 = request.ActividadTambo.Tambos[idxTambos].LitrosMensualesSegunLiquidacion;
                    }
                    idxTambos++;
                    if (request.ActividadTambo.Tambos.Length > idxTambos)
                    {
                        entrada.ta_fecha_liq_2 = convertirFecha(request.ActividadTambo.Tambos[idxTambos].FechaLiquidacion);
                        entrada.ta_litros_mensuales_2 = request.ActividadTambo.Tambos[idxTambos].LitrosMensualesSegunLiquidacion;
                    }
                    idxTambos++;
                    if (request.ActividadTambo.Tambos.Length > idxTambos)
                    {
                        entrada.ta_fecha_liq_3 = convertirFecha(request.ActividadTambo.Tambos[idxTambos].FechaLiquidacion);
                        entrada.ta_litros_mensuales_3 = request.ActividadTambo.Tambos[idxTambos].LitrosMensualesSegunLiquidacion;
                    }
                }
            }

            if (request.ActividadGanaderia is not null)
            {
                entrada.posee_ganaderia = request.ActividadGanaderia.DesarrollaActividadGanaderia;
                entrada.ga_cria_completo = request.ActividadGanaderia.DesarrollaActividadGanaderiaCriaCompleto;
                entrada.ga_invernada = request.ActividadGanaderia.DesarrollaActividadGanaderiaInvernada;
                entrada.ga_feed_lot = request.ActividadGanaderia.DesarrollaActividadGanaderiaFeedLot;
                entrada.ga_cap_feed_lot = request.ActividadGanaderia.CantidadCabezasCapacidadFeedLot;
                entrada.ga_has_avalistas = request.ActividadGanaderia.HectareasDestinadasAvalistas;
                entrada.ga_has_terceros = request.ActividadGanaderia.HectareasDestinadasTerceros;
                entrada.ga_has_avalistas_con_contrato = request.ActividadGanaderia.HectareasContratoDestinadasAvalistas;
                entrada.ga_has_terceros_con_contrato = request.ActividadGanaderia.HectareasContratoDestinadasTerceros;
                entrada.ga_superficie_propia = request.ActividadGanaderia.HectaresPropiasDestinadas;
            }

            if (request.ActividadContratista is not null)
            {
                entrada.posee_activ_contratista = request.ActividadContratista.DesarrollaActividadComoContratistaRural;
                if (request.ActividadContratista.RegistrosContratista is not null)
                {
                    int idxContratista = 0;
                    if (request.ActividadContratista.RegistrosContratista.Length > idxContratista)
                    {
                        entrada.co_servicio_1 = request.ActividadContratista.RegistrosContratista[idxContratista].ServicioComoContratista;
                        entrada.co_cant_1 = request.ActividadContratista.RegistrosContratista[idxContratista].CantidadHectareasToneladasRollos;
                    }
                    idxContratista++;
                    if (request.ActividadContratista.RegistrosContratista.Length > idxContratista)
                    {
                        entrada.co_servicio_2 = request.ActividadContratista.RegistrosContratista[idxContratista].ServicioComoContratista;
                        entrada.co_cant_2 = request.ActividadContratista.RegistrosContratista[idxContratista].CantidadHectareasToneladasRollos;
                    }
                    idxContratista++;
                    if (request.ActividadContratista.RegistrosContratista.Length > idxContratista)
                    {
                        entrada.co_servicio_3 = request.ActividadContratista.RegistrosContratista[idxContratista].ServicioComoContratista;
                        entrada.co_cant_3 = request.ActividadContratista.RegistrosContratista[idxContratista].CantidadHectareasToneladasRollos;
                    }
                    idxContratista++;
                    if (request.ActividadContratista.RegistrosContratista.Length > idxContratista)
                    {
                        entrada.co_servicio_4 = request.ActividadContratista.RegistrosContratista[idxContratista].ServicioComoContratista;
                        entrada.co_cant_4 = request.ActividadContratista.RegistrosContratista[idxContratista].CantidadHectareasToneladasRollos;
                    }
                    idxContratista++;
                    if (request.ActividadContratista.RegistrosContratista.Length > idxContratista)
                    {
                        entrada.co_servicio_5 = request.ActividadContratista.RegistrosContratista[idxContratista].ServicioComoContratista;
                        entrada.co_cant_5 = request.ActividadContratista.RegistrosContratista[idxContratista].CantidadHectareasToneladasRollos;
                    }
                    idxContratista++;
                    if (request.ActividadContratista.RegistrosContratista.Length > idxContratista)
                    {
                        entrada.co_servicio_6 = request.ActividadContratista.RegistrosContratista[idxContratista].ServicioComoContratista;
                        entrada.co_cant_6 = request.ActividadContratista.RegistrosContratista[idxContratista].CantidadHectareasToneladasRollos;
                    }
                    idxContratista++;
                    if (request.ActividadContratista.RegistrosContratista.Length > idxContratista)
                    {
                        entrada.co_servicio_7 = request.ActividadContratista.RegistrosContratista[idxContratista].ServicioComoContratista;
                        entrada.co_cant_7 = request.ActividadContratista.RegistrosContratista[idxContratista].CantidadHectareasToneladasRollos;
                    }
                    idxContratista++;
                    if (request.ActividadContratista.RegistrosContratista.Length > idxContratista)
                    {
                        entrada.co_servicio_8 = request.ActividadContratista.RegistrosContratista[idxContratista].ServicioComoContratista;
                        entrada.co_cant_8 = request.ActividadContratista.RegistrosContratista[idxContratista].CantidadHectareasToneladasRollos;
                    }
                    idxContratista++;
                    if (request.ActividadContratista.RegistrosContratista.Length > idxContratista)
                    {
                        entrada.co_servicio_9 = request.ActividadContratista.RegistrosContratista[idxContratista].ServicioComoContratista;
                        entrada.co_cant_9 = request.ActividadContratista.RegistrosContratista[idxContratista].CantidadHectareasToneladasRollos;
                    }
                    idxContratista++;
                    if (request.ActividadContratista.RegistrosContratista.Length > idxContratista)
                    {
                        entrada.co_servicio_10 = request.ActividadContratista.RegistrosContratista[idxContratista].ServicioComoContratista;
                        entrada.co_cant_10 = request.ActividadContratista.RegistrosContratista[idxContratista].CantidadHectareasToneladasRollos;
                    }
                    idxContratista++;
                    if (request.ActividadContratista.RegistrosContratista.Length > idxContratista)
                    {
                        entrada.co_servicio_11 = request.ActividadContratista.RegistrosContratista[idxContratista].ServicioComoContratista;
                        entrada.co_cant_11 = request.ActividadContratista.RegistrosContratista[idxContratista].CantidadHectareasToneladasRollos;
                    }

                }
                entrada.parametros_manuales = request.ParametrosModificadosManualmente;
                entrada.extra = request.InformacionAdicional;
            }
            return entrada;
        }

        private ResponseCalculadoraAgro MapeaSalida(PoliticaCalculadoraAgroSalida salida)
        {
            var response = new ResponseCalculadoraAgro();
            response.Aplicacion = salida.ap_origen;
            response.NumeroCalificacion = salida.nro_calificacion;
            response.NumeroEvaluacion = salida.nro_eval;
            response.EstructuraProductivaCampaniaFecha = salida.fecha_estructura_productiva;
            response.EstructuraProductivaUltimaActualizacionParametros = salida.p_calc_fecha_actualizacion;
            response.TipoCambioCOBIS = salida.tipo_de_cambio;
            response.Totales = new ResponseCalculadoraAgroTotales();
            response.Totales.Montos = new ResponseCalculadoraAgroTotalesMontos();
            response.Totales.Montos.Hacienda = salida.total_bovinos_pesos;
            response.Totales.Montos.Cereales = salida.total_cereales_pesos;
            response.Totales.Montos.Sementeras = salida.total_sementeras_pesos;
            response.Totales.HectareasExplotadas = new ResponseCalculadoraAgroTotalesHectareasExplotadas();
            response.Totales.HectareasExplotadas.HectareasAgricultura = new ResponseCalculadoraAgroHectareasAgricultura();
            response.Totales.HectareasExplotadas.HectareasAgricultura.Propias = salida.ag_total_has_propias;
            response.Totales.HectareasExplotadas.HectareasAgricultura.Avalistas = salida.ag_total_has_avalistas;
            response.Totales.HectareasExplotadas.HectareasAgricultura.Terceros = salida.ag_total_has_terceros;
            response.Totales.HectareasExplotadas.HectareasAgricultura.TotalAgricultura = salida.ag_total_has_efectivas;
            response.Totales.HectareasExplotadas.HectareasGanaderia = new ResponseCalculadoraAgroHectareasGanaderia();
            response.Totales.HectareasExplotadas.HectareasGanaderia.Propias = salida.ga_superficie_propia;
            response.Totales.HectareasExplotadas.HectareasGanaderia.Avalistas = salida.ga_has_avalistas;
            response.Totales.HectareasExplotadas.HectareasGanaderia.Terceros = salida.ga_has_terceros;
            response.Totales.HectareasExplotadas.HectareasGanaderia.TotalGanaderia = salida.ga_sup_total;
            response.Totales.HectareasExplotadas.HectareasTambo = new ResponseCalculadoraAgroHectareasTambo();
            response.Totales.HectareasExplotadas.HectareasTambo.Propias = salida.ta_superficie_propia;
            response.Totales.HectareasExplotadas.HectareasTambo.Avalistas = salida.ta_has_avalistas;
            response.Totales.HectareasExplotadas.HectareasTambo.Terceros = salida.ta_has_terceros;
            response.Totales.HectareasExplotadas.HectareasTambo.TotalTambo = salida.ta_sup_total;
            response.Totales.HectareasExplotadas.HectareasDeclaradas = new ResponseCalculadoraAgroHectareasDeclaradas();
            response.Totales.HectareasExplotadas.HectareasDeclaradas.Propias = salida.has_totales_declaradas_cliente;
            response.Totales.HectareasExplotadas.HectareasDeclaradas.Avalistas = salida.has_totales_declaradas_avalistas;
            response.Totales.HectareasExplotadas.HectareasDeclaradas.Terceros = salida.has_totales_declaradas_terceros;
            response.Totales.HectareasExplotadas.HectareasDeclaradas.TotalDeclaradas = salida.has_totales_declaradas;
            response.Totales.HectareasCamposArrendados = new ResponseCalculadoraAgroTotalesHectareasCamposArrendados();
            response.Totales.HectareasCamposArrendados.TotalesDeclaradasAvalistasVinculadosTerceros = salida.has_totales_declaradas_avalistas_vinculados_terceros;
            response.Totales.HectareasCamposArrendados.ConContratoAvalistas = salida.has_totales_con_contrato_avalistas;
            response.Totales.HectareasCamposArrendados.ConContratoTerceros = salida.has_totales_con_contrato_terceros;
            response.Totales.HectareasCamposArrendados.TotalConContrato = salida.has_totales_con_contrato;
            response.Totales.HectareasAgricultura = new ResponseCalculadoraAgroTotalesHectareasAgricultura();
            response.Totales.HectareasAgricultura.Soja = salida.has_totales_soja;
            response.Totales.HectareasAgricultura.SojaSegunda = salida.has_totales_soja_2da;
            response.Totales.HectareasAgricultura.Trigo = salida.has_totales_trigo;
            response.Totales.HectareasAgricultura.Maiz = salida.has_totales_maiz;
            response.Totales.HectareasAgricultura.Girasol = salida.has_totales_girasol;
            response.Totales.HectareasAgricultura.Cebada = salida.has_totales_cebada;
            response.Totales.HectareasAgricultura.Sorgo = salida.has_totales_sorgo;
            response.Totales.HectareasAgricultura.NucleoPropias = salida.ag_total_has_propias_nucleo;
            response.Totales.HectareasAgricultura.NucleoAvalistas = salida.ag_total_has_avalistas_nucleo;
            response.Totales.HectareasAgricultura.NucleoTerceros = salida.ag_total_has_terceros_nucleo;
            response.Totales.HectareasAgricultura.TotalNucleo = salida.ag_total_has_nucleo;
            response.Totales.HectareasAgricultura.Perimetrales = new ResponseCalculadoraAgroTotalesPerimetrales[2];
            response.Totales.HectareasAgricultura.Perimetrales[0] = new ResponseCalculadoraAgroTotalesPerimetrales();
            response.Totales.HectareasAgricultura.Perimetrales[0].Propias = salida.ag_total_has_propias_perimetral_1;
            response.Totales.HectareasAgricultura.Perimetrales[0].Avalistas = salida.ag_total_has_avalistas_perimetral_1;
            response.Totales.HectareasAgricultura.Perimetrales[0].Terceros = salida.ag_total_has_terceros_perimetral_1;
            response.Totales.HectareasAgricultura.Perimetrales[0].TotalHectareas = salida.ag_total_has_perimetral_1;
            response.Totales.HectareasAgricultura.Perimetrales[1] = new ResponseCalculadoraAgroTotalesPerimetrales();
            response.Totales.HectareasAgricultura.Perimetrales[1].Propias = salida.ag_total_has_propias_perimetral_2;
            response.Totales.HectareasAgricultura.Perimetrales[1].Avalistas = salida.ag_total_has_avalistas_perimetral_2;
            response.Totales.HectareasAgricultura.Perimetrales[1].Terceros = salida.ag_total_has_terceros_perimetral_2;
            response.Totales.HectareasAgricultura.Perimetrales[1].TotalHectareas = salida.ag_total_has_perimetral_2;
            response.Totales.HectareasAgricultura.SudoesteTriguera = new ResponseCalculadoraAgroTotalesSudoesteTriguera();
            response.Totales.HectareasAgricultura.SudoesteTriguera.Propias = salida.ag_total_has_propias_se_triguera;
            response.Totales.HectareasAgricultura.SudoesteTriguera.Avalistas = salida.ag_total_has_avalistas_se_triguera;
            response.Totales.HectareasAgricultura.SudoesteTriguera.Terceros = salida.ag_total_has_terceros_se_triguera;
            response.Totales.HectareasAgricultura.SudoesteTriguera.TotalHectareas = salida.ag_total_has_se_triguera;
            response.Totales.HectareasAgricultura.Marginal = new ResponseCalculadoraAgroTotalesMarginal();
            response.Totales.HectareasAgricultura.Marginal.Propias = salida.ag_total_has_propias_marginal;
            response.Totales.HectareasAgricultura.Marginal.Avalistas = salida.ag_total_has_avalistas_marginal;
            response.Totales.HectareasAgricultura.Marginal.Terceros = salida.ag_total_has_terceros_marginal;
            response.Totales.HectareasAgricultura.SudoesteTriguera.TotalMarginal = salida.ag_total_has_marginal;
            response.HectareasContratistaRural = new ResponseCalculadoraAgroHectareasContratistaRural();
            response.HectareasContratistaRural.Cosecha = salida.co_cosecha;
            response.HectareasContratistaRural.Siembra = salida.co_siembra;
            response.HectareasContratistaRural.PicadoFino = salida.co_picado_fino;
            response.HectareasContratistaRural.PicadoGrueso = salida.co_picado_grueso;
            response.HectareasContratistaRural.FumigacionAerea = salida.co_fumig_aerea;
            response.HectareasContratistaRural.FumigacionTerrestre = salida.co_fumig_terrestre;
            response.HectareasContratistaRural.Laboreo = salida.co_laboreo;
            response.HectareasContratistaRural.EmbolsadoGranos = salida.co_embolsado;
            response.HectareasContratistaRural.Rotoenfardado = salida.co_rotoenfardado;
            response.Tambo = new ResponseCalculadoraAgroTambo();
            response.Tambo.TotalVacas = salida.ta_total_vacas;
            response.Tambo.VacasEnOrdenie = salida.ta_vacas_en_ordenie;
            response.Tambo.PorcentajeVacasEnOrdenie = salida.ta_porc_vacas_en_ordenie;
            response.Tambo.LitrosEstimadosMensuales = salida.ta_litros_estimados_mensuales;
            response.Tambo.LitrosRealDiarioSobreLiquidacion = salida.ta_litros_reales_diarios;
            response.Tambo.LitrosRealMensualSobreLiquidacion = salida.ta_litros_prom_mensual_ult_3_liq;
            response.Tambo.Usina = salida.ta_usina_lactea_pyme;
            response.Ganaderia = new ResponseCalculadoraAgroGanaderia();
            response.Ganaderia.MarcaCria = salida.ga_cria_completo;
            response.Ganaderia.MarcaInvernada = salida.ga_invernada;
            response.Ganaderia.MarcaFeedLot = salida.ga_feed_lot;
            response.Ganaderia.CapacidadCabezasFeedLot = salida.ga_total_feedlot;
            response.Ganaderia.Vientres = salida.vacas_cria_inv_cabezas;
            response.Ganaderia.RestoCriaInvernada = salida.ga_stock_resto_cria_inv;
            response.Ganaderia.CabezasFeedLot = salida.ga_cap_feed_lot;
            response.Ganaderia.CabezasSobreHectareas = salida.ga_cab_has;
            response.Ganaderia.PorcentajeOcupacion = salida.ga_porc_ocupacion;
            response.Ganaderia.TotalCabezas = salida.total_bovinos_cabezas;
            response.Ganaderia.TotalSobreActasVacunacion = salida.cant_cabezas_acta_vacunacion;
            response.EERRGlobalProyectado = new ResponseCalculadoraAgroEERRGlobalProyectado();
            response.EERRGlobalProyectado.ProyectadoAgricultura = new ResponseCalculadoraAgroProyectadoAgricultura();
            response.EERRGlobalProyectado.ProyectadoAgricultura.Ingresos = salida.ag_ingresos_brutos;
            response.EERRGlobalProyectado.ProyectadoAgricultura.GastosComercialesConCosecha = salida.ag_gs_com_mas_gs_cosecha;
            response.EERRGlobalProyectado.ProyectadoAgricultura.IngresosNetos = salida.ag_ingresos_netos;
            response.EERRGlobalProyectado.ProyectadoAgricultura.GastosDirectosConArrendamiento = salida.ag_gs_directos_mas_gs_arrend;
            response.EERRGlobalProyectado.ProyectadoAgricultura.ResultadosOperacionPotenciales = salida.ag_rdo_operativo_potencial;
            response.EERRGlobalProyectado.ProyectadoAgricultura.IngresosParticulares = salida.ingresos_brutos_partic_agricultura;
            response.EERRGlobalProyectado.ProyectadoAgricultura.IngresosPromedio = salida.ingresos_brutos_promedio_agricultura;

            response.EERRGlobalProyectado.ProyectadoContratistaRural = new ResponseCalculadoraAgroProyectado();
            response.EERRGlobalProyectado.ProyectadoContratistaRural.Ingresos = salida.co_ingresos_brutos;
            response.EERRGlobalProyectado.ProyectadoContratistaRural.GastosDirectosConArrendamiento = salida.co_costo;
            response.EERRGlobalProyectado.ProyectadoContratistaRural.ResultadosOperacionPotenciales = salida.co_ebitda;
            response.EERRGlobalProyectado.ProyectadoContratistaRural.IngresosParticulares = salida.ingresos_brutos_partic_contratista;
            response.EERRGlobalProyectado.ProyectadoContratistaRural.IngresosPromedio = salida.ingresos_brutos_promedio_contratista;

            response.EERRGlobalProyectado.ProyectadoTambo = new ResponseCalculadoraAgroProyectado();
            response.EERRGlobalProyectado.ProyectadoTambo.Ingresos = salida.ta_ingresos_brutos;
            response.EERRGlobalProyectado.ProyectadoTambo.GastosDirectosConArrendamiento = salida.ta_costo;
            response.EERRGlobalProyectado.ProyectadoTambo.ResultadosOperacionPotenciales = salida.ta_ebitda;
            response.EERRGlobalProyectado.ProyectadoTambo.IngresosParticulares = salida.ingresos_brutos_partic_tambo;
            response.EERRGlobalProyectado.ProyectadoTambo.IngresosPromedio = salida.ingresos_brutos_promedio_tambo;

            response.EERRGlobalProyectado.ProyectadoGanaderia = new ResponseCalculadoraAgroProyectado();
            response.EERRGlobalProyectado.ProyectadoGanaderia.Ingresos = salida.ga_ingresos_brutos;
            response.EERRGlobalProyectado.ProyectadoGanaderia.GastosDirectosConArrendamiento = salida.ga_costo;
            response.EERRGlobalProyectado.ProyectadoGanaderia.ResultadosOperacionPotenciales = salida.ga_ebitda;
            response.EERRGlobalProyectado.ProyectadoGanaderia.IngresosParticulares = salida.ingresos_brutos_partic_ganaderia;
            response.EERRGlobalProyectado.ProyectadoGanaderia.IngresosPromedio = salida.ingresos_brutos_promedio_ganaderia;

            response.EERRGlobalProyectado.ProyectadoTotal = new ResponseCalculadoraAgroProyectadoTotal();
            response.EERRGlobalProyectado.ProyectadoTotal.Ingresos = salida.ingresos_brutos_total;
            response.EERRGlobalProyectado.ProyectadoTotal.GastosComercialesConCosecha = salida.gs_com_mas_gs_cosecha_total;
            response.EERRGlobalProyectado.ProyectadoTotal.IngresosNetos = salida.ingresos_netos_total;
            response.EERRGlobalProyectado.ProyectadoTotal.GastosDirectosConArrendamiento = salida.gs_directos_mas_gs_arrend;
            response.EERRGlobalProyectado.ProyectadoTotal.ResultadosOperacionPotenciales = salida.ebitda_total;
            response.EERRGlobalProyectado.ProyectadoTotal.IngresosPromedio = salida.ingresos_brutos_promedio;
            response.EERRGlobalProyectado.ProyectadoAgricultura.IngresosDolares = salida.ag_ingresos_brutos_usd;
            response.EERRGlobalProyectado.ProyectadoAgricultura.GastosComercialesConCosechaDolares = salida.ag_gs_com_mas_gs_cosecha_usd;
            response.EERRGlobalProyectado.ProyectadoAgricultura.IngresosNetosDolares = salida.ag_ingresos_netos_usd;
            response.EERRGlobalProyectado.ProyectadoAgricultura.GastosDirectosConArrendamientoDolares = salida.ag_gs_directos_mas_gs_arrend_usd;
            response.EERRGlobalProyectado.ProyectadoAgricultura.ResultadosOperacionPotencialesDolares = salida.ag_rdo_operativo_potencial_usd;
            response.InformacionAdicional = salida.extra;
            response.Version = salida.version_politica;

            if (salida.cultivos is not null)
            {
                response.Cultivos = new List<ResponseCalculadoraAgroCultivo>();
                string[] cultivosNombreArray = salida.cultivos.Split('|');
                string[] cultivosHectareasArray = salida.has_totales_cultivo.Split('|');
                if (cultivosNombreArray.Length > 0)
                {
                    for (int i = 0; i < cultivosNombreArray.Length - 1; i++)
                    {
                        var cultivo = new ResponseCalculadoraAgroCultivo();
                        cultivo.cultivo = cultivosNombreArray[i];
                        cultivo.TotalHectareas = decimal.Parse(cultivosHectareasArray[i]);
                        response.Cultivos.Add(cultivo);
                    }
                }
            }

            if (salida.ag_zonas_desagregado is not null)
            {
                response.Zonas = new List<ResponseCalculadoraAgroZona>();
                string[] zonasNombreArray = salida.ag_zonas_desagregado.Split('|');
                string[] zonasHectareasPropiasArray = salida.ag_total_has_propias_desagregado.Split('|');
                string[] zonasHectareasAvalistasArray = salida.ag_total_has_avalistas_desagregado.Split('|');
                string[] zonasHectareasTercerosArray = salida.ag_total_has_terceros_desagregado.Split('|');
                string[] zonasHectareasTotalesArray = salida.ag_total_has_desagregado.Split('|');
                if (zonasNombreArray.Length > 0)
                {
                    for (int i = 0; i < zonasNombreArray.Length - 1; i++)
                    {
                        var zona = new ResponseCalculadoraAgroZona();
                        zona.zona = zonasNombreArray[i];
                        zona.TotalHectareasPropias = decimal.Parse(zonasHectareasPropiasArray[i]);
                        zona.TotalHectareasAvalistas = decimal.Parse(zonasHectareasAvalistasArray[i]);
                        zona.TotalHectareasTerceros = decimal.Parse(zonasHectareasTercerosArray[i]);
                        zona.TotalHectareas = decimal.Parse(zonasHectareasTotalesArray[i]);
                        response.Zonas.Add(zona);
                    }
                }
            }

            if (salida.co_servicios is not null)
            {
                response.Servicios = new List<ResponseCalculadoraAgroServicio>();
                string[] serviciosNombreArray = salida.co_servicios.Split('|');
                string[] serviciosHectareasArray = salida.co_servicios_hectareas.Split('|');
                if (serviciosNombreArray.Length > 0)
                {
                    for (int i = 0; i < serviciosNombreArray.Length - 1; i++)
                    {
                        var servicio = new ResponseCalculadoraAgroServicio();
                        servicio.servicio = serviciosNombreArray[i];
                        servicio.TotalHectareas = decimal.Parse(serviciosHectareasArray[i]);
                        response.Servicios.Add(servicio);
                    }
                }
            }

            return response;
        }
    }
}

public class RequestCalculadoraAgroArray
{
    public RequestCalculadoraAgro element { get; set; }
}

public class RequestCalculadoraAgro
{
    public string? IdCanal { get; set; }
    public String? FechaSolicitud { get; set; }
    public int? Aplicacion { get; set; }
    public int? NumeroCalificacion { get; set; }
    public int? NumeroEvaluacion { get; set; }
    public string? MetodoCalificacion { get; set; }
    public long? CUIT { get; set; }
    public string? Segmento { get; set; }
    public string? Subsegmento { get; set; }
    public string? Sucursal { get; set; }
    public String? FechaHaciendaSementeras { get; set; }
    public RequestCalculadoraAgroHacienda? Hacienda { get; set; }
    public RequestCalculadoraAgroSementeras? Sementeras { get; set; }
    public RequestCalculadoraAgroAgricultura? Agricultura { get; set; }
    public RequestCalculadoraAgroActividadTambo? ActividadTambo { get; set; }
    public RequestCalculadoraAgroActividadGanaderia? ActividadGanaderia { get; set; }
    public RequestCalculadoraAgroActividadContratista? ActividadContratista { get; set; }
    public string? ParametrosModificadosManualmente { get; set; }
    public string? InformacionAdicional { get; set; }
}

public class RequestCalculadoraAgroHacienda
{
    public string? ModalidadCargaHacienda { get; set; }
    public string? PoseeCuentaHacienda { get; set; }
    public RequestCalculadoraAgroDestinoTambo? DestinoTambo { get; set; }
    public RequestCalculadoraAgroDestinoCriaInvernada? DestinoCriaInvernada { get; set; }
    public int? TotalBienesUsoBovinosTamboCriaInvernadaPorTotales { get; set; }
    public decimal? MontoTotalBienesUsoBovinosTamboCriaInvernadaPorTotales { get; set; }
    public RequestCalculadoraAgroDestinoVenta? DestinoVenta { get; set; }
}

public class RequestCalculadoraAgroDestinoTambo
{
    public int? CantidadVacas { get; set; }
    public int? CantidadVaquillonas { get; set; }
    public int? CantidadToros { get; set; }
    public decimal? MontoDeclaradoVacas { get; set; }
    public decimal? MontoDeclaradoVaquillonas { get; set; }
    public decimal? MontoDeclaradoToros { get; set; }
}

public class RequestCalculadoraAgroDestinoCriaInvernada
{
    public int? CantidadVacas { get; set; }
    public int? CantidadToros { get; set; }
    public int? CantidadOtros { get; set; }
    public decimal? MontoDeclaradoVacas { get; set; }
    public decimal? MontoDeclaradoToros { get; set; }
    public decimal? MontoDeclaradoOtros { get; set; }
}

public class RequestCalculadoraAgroDestinoVenta
{
    public RequestCalculadoraAgroTamboCriaInvernada? TamboCriaInvernada { get; set; }
    public RequestCalculadoraAgroFeedLot? FeedLot { get; set; }
    public RequestCalculadoraAgroMontosDeclarados? MontosDeclarados { get; set; }
    public decimal? TotalBienesCambioBovinosTamboCriaInvernadaFeedLotPorTotales { get; set; }
    public int? CantidadCabezasSegunActaVacunacion { get; set; }
}

public class RequestCalculadoraAgroTamboCriaInvernada
{
    public int? CantidadVacas { get; set; }
    public int? CantidadVaquillonas { get; set; }
    public int? CantidadTernerosas { get; set; }
    public int? CantidadNovillos { get; set; }
    public int? CantidadNovillitos { get; set; }
    public int? SumaTotalBienesCambioBovinoPorTotales { get; set; }
    public int? CantidadOtros { get; set; }
}

public class RequestCalculadoraAgroFeedLot
{
    public int? CantidadVacas { get; set; }
    public int? CantidadVaquillonas { get; set; }
    public int? CantidadTernerosas { get; set; }
    public int? CantidadNovillos { get; set; }
    public int? CantidadNovillitos { get; set; }
    public int? SumaTotalBienesCambioBovino { get; set; }
    public int? CantidadOtros { get; set; }
}

public class RequestCalculadoraAgroMontosDeclarados
{
    public decimal? Vacas { get; set; }
    public decimal? Vaquillonas { get; set; }
    public decimal? Terneros { get; set; }
    public decimal? Novillos { get; set; }
    public decimal? Novillitos { get; set; }
    public decimal? Otros { get; set; }
}

public class RequestCalculadoraAgroSementeras
{
    public string? PoseeCuentaSementeras { get; set; }
    public string? ModalidadCargaSementeras { get; set; }
    public RequestCalculadoraAgroRegistrosSementeras[]? RegistrosSementeras { get; set; }
    public decimal? TotalCerealesMontoPorTotales { get; set; }
    public decimal? TotalSementerasMontoPorTotales { get; set; }
    public string? FechaEstructuraProductivaDeclarada { get; set; }
}

public class RequestCalculadoraAgroRegistrosSementeras
{
    public string? CultivoCereal { get; set; }
    public int? CantidadCereal { get; set; }
    public string? MedidaCereal { get; set; }
    public decimal? MontoCereal { get; set; }
}

public class RequestCalculadoraAgroAgricultura
{
    public string? DesarrollaAgricultura { get; set; }
    public RequestCalculadoraAgroRegistrosAgricultura[]? RegistrosAgricultura { get; set; }
    public int? HectareasContratoDestinadasAgriculturaAvalistas { get; set; }
    public int? HectareasContratoDestinadasAgriculturaTerceros { get; set; }
}

public class RequestCalculadoraAgroRegistrosAgricultura
{
    public string? Provincia { get; set; }
    public string? Zona { get; set; }
    public string? PrimerCultivo { get; set; }
    public string? SegundoCultivo { get; set; }
    public int? HectareasPropias { get; set; }
    public int? HectareasAvalistas { get; set; }
    public int? HectareasTerceros { get; set; }
}

public class RequestCalculadoraAgroActividadTambo
{
    public string? PoseeDesarrollaActividadTambo { get; set; }
    public int? CantidadVacasNumeroOrdenie { get; set; }
    public string? UsinaLacteaPyME { get; set; }
    public int? HectareasDestinadasAvalistas { get; set; }
    public int? HectareasDestinadasTerceros { get; set; }
    public int? HectareasContratoDestinadasAvalistas { get; set; }
    public int? HectareasContratoDestinadasTerceros { get; set; }
    public int? HectaresPropiasDestinadas { get; set; }
    public RequestCalculadoraAgroTambos[]? Tambos { get; set; }
}

public class RequestCalculadoraAgroTambos
{
    public String? FechaLiquidacion { get; set; }
    public int? LitrosMensualesSegunLiquidacion { get; set; }
}

public class RequestCalculadoraAgroActividadGanaderia
{
    public string? DesarrollaActividadGanaderia { get; set; }
    public string? DesarrollaActividadGanaderiaCriaCompleto { get; set; }
    public string? DesarrollaActividadGanaderiaInvernada { get; set; }
    public string? DesarrollaActividadGanaderiaFeedLot { get; set; }
    public int? CantidadCabezasCapacidadFeedLot { get; set; }
    public int? HectareasDestinadasAvalistas { get; set; }
    public int? HectareasDestinadasTerceros { get; set; }
    public int? HectareasContratoDestinadasAvalistas { get; set; }
    public int? HectareasContratoDestinadasTerceros { get; set; }
    public int? HectaresPropiasDestinadas { get; set; }
}

public class RequestCalculadoraAgroActividadContratista
{
    public string? DesarrollaActividadComoContratistaRural { get; set; }
    public RequestCalculadoraAgroRegistrosContratista[]? RegistrosContratista { get; set; }

}

public class RequestCalculadoraAgroRegistrosContratista
{
    public string? ServicioComoContratista { get; set; }
    public int? CantidadHectareasToneladasRollos { get; set; }
}


public class ResponseCalculadoraAgroArray
{
    public List<ResponseCalculadoraAgro> elements { get; set; }

    public ResponseCalculadoraAgroArray()
    {
        elements = new List<ResponseCalculadoraAgro>();
    }
}

public class ResponseCalculadoraAgro
{
    public Guid IdInterno { get; set; }
    public string? Aplicacion { get; set; }
    public int? NumeroCalificacion { get; set; }
    public int? NumeroEvaluacion { get; set; }
    public string? EstructuraProductivaCampaniaFecha { get; set; }
    public string? EstructuraProductivaUltimaActualizacionParametros { get; set; }
    public decimal? TipoCambioCOBIS { get; set; }
    public ResponseCalculadoraAgroTotales? Totales { get; set; }
    public ResponseCalculadoraAgroHectareasContratistaRural? HectareasContratistaRural { get; set; }
    public ResponseCalculadoraAgroTambo? Tambo { get; set; }
    public ResponseCalculadoraAgroGanaderia? Ganaderia { get; set; }
    public ResponseCalculadoraAgroEERRGlobalProyectado? EERRGlobalProyectado { get; set; }
    public string? InformacionAdicional { get; set; }
    public string? Version { get; set; }
    public List<ResponseCalculadoraAgroCultivo> Cultivos { get; set; }
    public List<ResponseCalculadoraAgroZona> Zonas { get; set; }
    public List<ResponseCalculadoraAgroServicio> Servicios { get; set; }
}

public class ResponseCalculadoraAgroTotales
{
    public ResponseCalculadoraAgroTotalesMontos? Montos { get; set; }
    public ResponseCalculadoraAgroTotalesHectareasExplotadas? HectareasExplotadas { get; set; }
    public ResponseCalculadoraAgroTotalesHectareasCamposArrendados? HectareasCamposArrendados { get; set; }
    public ResponseCalculadoraAgroTotalesHectareasAgricultura? HectareasAgricultura { get; set; }
}

public class ResponseCalculadoraAgroTotalesMontos
{
    public decimal? Hacienda { get; set; }
    public decimal? Cereales { get; set; }
    public decimal? Sementeras { get; set; }
}

public class ResponseCalculadoraAgroTotalesHectareasExplotadas
{
    public ResponseCalculadoraAgroHectareasAgricultura? HectareasAgricultura { get; set; }
    public ResponseCalculadoraAgroHectareasGanaderia? HectareasGanaderia { get; set; }
    public ResponseCalculadoraAgroHectareasTambo? HectareasTambo { get; set; }
    public ResponseCalculadoraAgroHectareasDeclaradas? HectareasDeclaradas { get; set; }
}

public class ResponseCalculadoraAgroHectareasAgricultura
{
    public int? Propias { get; set; }
    public int? Avalistas { get; set; }
    public int? Terceros { get; set; }
    public int? TotalAgricultura { get; set; }
}

public class ResponseCalculadoraAgroHectareasGanaderia
{
    public int? Propias { get; set; }
    public int? Avalistas { get; set; }
    public int? Terceros { get; set; }
    public int? TotalGanaderia { get; set; }
}

public class ResponseCalculadoraAgroHectareasTambo
{
    public int? Propias { get; set; }
    public int? Avalistas { get; set; }
    public int? Terceros { get; set; }
    public int? TotalTambo { get; set; }
}
public class ResponseCalculadoraAgroHectareasDeclaradas
{
    public int? Propias { get; set; }
    public int? Avalistas { get; set; }
    public int? Terceros { get; set; }
    public int? TotalDeclaradas { get; set; }
}

public class ResponseCalculadoraAgroTotalesHectareasCamposArrendados
{
    public int? ConContratoAvalistas { get; set; }
    public int? ConContratoTerceros { get; set; }
    public int? TotalConContrato { get; set; }
    public int? TotalesDeclaradasAvalistasVinculadosTerceros { get; set; }
}

public class ResponseCalculadoraAgroTotalesHectareasAgricultura
{
    public int? Soja { get; set; }
    public int? SojaSegunda { get; set; }
    public int? Trigo { get; set; }
    public int? Maiz { get; set; }
    public int? Girasol { get; set; }
    public int? Cebada { get; set; }
    public int? Sorgo { get; set; }
    public int? NucleoPropias { get; set; }
    public int? NucleoAvalistas { get; set; }
    public int? NucleoTerceros { get; set; }
    public int? TotalNucleo { get; set; }
    public ResponseCalculadoraAgroTotalesPerimetrales[]? Perimetrales { get; set; }
    public ResponseCalculadoraAgroTotalesSudoesteTriguera? SudoesteTriguera { get; set; }
    public ResponseCalculadoraAgroTotalesMarginal? Marginal { get; set; }
}

public class ResponseCalculadoraAgroTotalesPerimetrales
{
    public int? Propias { get; set; }
    public int? Avalistas { get; set; }
    public int? Terceros { get; set; }
    public int? TotalHectareas { get; set; }
}

public class ResponseCalculadoraAgroTotalesSudoesteTriguera
{
    public int? Propias { get; set; }
    public int? Avalistas { get; set; }
    public int? Terceros { get; set; }
    public int? TotalHectareas { get; set; }
    public int? TotalMarginal { get; set; }
}

public class ResponseCalculadoraAgroTotalesMarginal
{
    public int? Propias { get; set; }
    public int? Avalistas { get; set; }
    public int? Terceros { get; set; }
    public int? TotalMarginal { get; set; }
}

public class ResponseCalculadoraAgroHectareasContratistaRural
{
    public int? Cosecha { get; set; }
    public int? Siembra { get; set; }
    public int? PicadoFino { get; set; }
    public int? PicadoGrueso { get; set; }
    public int? FumigacionAerea { get; set; }
    public int? FumigacionTerrestre { get; set; }
    public int? Laboreo { get; set; }
    public int? EmbolsadoGranos { get; set; }
    public int? Rotoenfardado { get; set; }
}

public class ResponseCalculadoraAgroTambo
{
    public int? TotalVacas { get; set; }
    public int? VacasEnOrdenie { get; set; }
    public decimal? PorcentajeVacasEnOrdenie { get; set; }
    public int? LitrosEstimadosMensuales { get; set; }
    public decimal? LitrosRealDiarioSobreLiquidacion { get; set; }
    public decimal? LitrosRealMensualSobreLiquidacion { get; set; }
    public string? Usina { get; set; }
}

public class ResponseCalculadoraAgroGanaderia
{
    public string? MarcaCria { get; set; }
    public string? MarcaInvernada { get; set; }
    public string? MarcaFeedLot { get; set; }
    public int? CapacidadCabezasFeedLot { get; set; }
    public int? Vientres { get; set; }
    public int? RestoCriaInvernada { get; set; }
    public int? CabezasFeedLot { get; set; }
    public decimal? CabezasSobreHectareas { get; set; }
    public decimal? PorcentajeOcupacion { get; set; }
    public int? TotalCabezas { get; set; }
    public int? TotalSobreActasVacunacion { get; set; }
}

public class ResponseCalculadoraAgroEERRGlobalProyectado
{
    public ResponseCalculadoraAgroProyectadoAgricultura? ProyectadoAgricultura { get; set; }
    public ResponseCalculadoraAgroProyectado? ProyectadoContratistaRural { get; set; }
    public ResponseCalculadoraAgroProyectado? ProyectadoTambo { get; set; }
    public ResponseCalculadoraAgroProyectado? ProyectadoGanaderia { get; set; }
    public ResponseCalculadoraAgroProyectadoTotal? ProyectadoTotal { get; set; }
}

public class ResponseCalculadoraAgroProyectadoAgricultura
{
    public decimal? Ingresos { get; set; }
    public decimal? GastosComercialesConCosecha { get; set; }
    public decimal? IngresosNetos { get; set; }
    public decimal? GastosDirectosConArrendamiento { get; set; }
    public decimal? ResultadosOperacionPotenciales { get; set; }
    public decimal? IngresosParticulares { get; set; }
    public decimal? IngresosPromedio { get; set; }
    public decimal? IngresosDolares { get; set; }
    public decimal? GastosComercialesConCosechaDolares { get; set; }
    public decimal? IngresosNetosDolares { get; set; }
    public decimal? GastosDirectosConArrendamientoDolares { get; set; }
    public decimal? ResultadosOperacionPotencialesDolares { get; set; }
}

public class ResponseCalculadoraAgroProyectado
{
    public decimal? Ingresos { get; set; }
    public decimal? GastosDirectosConArrendamiento { get; set; }
    public decimal? ResultadosOperacionPotenciales { get; set; }
    public decimal? IngresosParticulares { get; set; }
    public decimal? IngresosPromedio { get; set; }
}

public class ResponseCalculadoraAgroProyectadoTotal
{
    public decimal? Ingresos { get; set; }
    public decimal? GastosDirectosConArrendamiento { get; set; }
    public decimal? ResultadosOperacionPotenciales { get; set; }
    public decimal? IngresosParticulares { get; set; }
    public decimal? IngresosPromedio { get; set; }
    public decimal? GastosComercialesConCosecha { get; set; }
    public decimal? IngresosNetos { get; set; }
}

public class ResponseCalculadoraAgroCultivo
{
    public string? cultivo { get; set; }
    public decimal? TotalHectareas { get; set; }
}

public class ResponseCalculadoraAgroZona
{
    public string? zona { get; set; }
    public decimal? TotalHectareas { get; set; }
    public decimal? TotalHectareasPropias { get; set; }
    public decimal? TotalHectareasAvalistas { get; set; }
    public decimal? TotalHectareasTerceros { get; set; }
}

public class ResponseCalculadoraAgroServicio
{
    public string? servicio { get; set; }
    public decimal? TotalHectareas { get; set; }
}