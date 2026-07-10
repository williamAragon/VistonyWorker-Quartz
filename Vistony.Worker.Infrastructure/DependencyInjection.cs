using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vistony.Worker.Application.Almacen.Interfaces;
using Vistony.Worker.Application.Almacen.UseCases;
using Vistony.Worker.Application.Articulo.Interfaces;
using Vistony.Worker.Application.Articulo.UseCases;
using Vistony.Worker.Application.CalificacionRiesgo.Interfaces;
using Vistony.Worker.Application.CalificacionRiesgo.UseCases;
using Vistony.Worker.Application.CategoriaCliente.Interfaces;
using Vistony.Worker.Application.CategoriaCliente.UseCases;
using Vistony.Worker.Application.Cliente.Interfaces;
using Vistony.Worker.Application.Cliente.UseCases;
using Vistony.Worker.Application.Comisiones.Interfaces;
using Vistony.Worker.Application.Comisiones.UseCases;
using Vistony.Worker.Application.CostoEstandar.Interfaces;
using Vistony.Worker.Application.CostoEstandar.UseCases;
using Vistony.Worker.Application.CuentaSalesForce.Interfaces;
using Vistony.Worker.Application.CuentaSalesForce.UseCases;
using Vistony.Worker.Application.ETL.Interfaces;
using Vistony.Worker.Application.ETL.UseCases;
using Vistony.Worker.Application.ETLCreditosCobranza.Interfaces;
using Vistony.Worker.Application.ETLCreditosCobranza.UseCases;
using Vistony.Worker.Application.OrdenFabricacion.Interfaces;
using Vistony.Worker.Application.OrdenFabricacion.UseCases;
using Vistony.Worker.Application.PagoComisiones.Interfaces;
using Vistony.Worker.Application.PagoComisiones.UseCases;
using Vistony.Worker.Application.PCP.Interfaces;
using Vistony.Worker.Application.PCP.UseCase;
using Vistony.Worker.Application.RutaHistorico.Interfaces;
using Vistony.Worker.Application.RutaHistorico.UseCases;
using Vistony.Worker.Application.StockCierre.Interfaces;
using Vistony.Worker.Application.StockCierre.UseCases;
using Vistony.Worker.Application.TipoCambio.Interfaces;  
using Vistony.Worker.Application.TipoCambio.UseCases;
using Vistony.Worker.Application.UpdateWorkPath.Interfaces;
using Vistony.Worker.Application.UpdateWorkPath.UseCases;
using Vistony.Worker.Application.WMS.Interfaces;
using Vistony.Worker.Application.WMS.UseCases;
using Vistony.Worker.Infrastructure.Correo;
using Vistony.Worker.Infrastructure.Firestore;
using Vistony.Worker.Infrastructure.Hana;
using Vistony.Worker.Infrastructure.SalesForce;
using Vistony.Worker.Infrastructure.ServiceLayer;
using Vistony.Worker.Infrastructure.Sunat;
using Vistony.Worker.Infrastructure.TipoCambio;

namespace Vistony.Worker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // USE CASES - TipoCambio
        services.AddScoped<SincronizarTipoCambioUseCase>();
        services.AddScoped<ObtenerTipoCambioSunatJob>();

        // REPOSITORIES - TipoCambio
        services.AddScoped<ITipoCambioFechaRepository, TipoCambioFechaHanaRepository>();

        // SUNAT
        services.AddHttpClient<ISunatTipoCambioClient, SunatTipoCambioClient>();

        // SAP SERVICE LAYER
        services.AddScoped<IServiceLayerTipoCambioClient, ServiceLayerTipoCambioClient>();

        services.AddScoped<IOrdenFabricacionRepository, OrdenFabricacionHanaRepository>();
        services.AddSingleton<IFirestoreOrdenFabricacionClient, FirestoreOrdenFabricacionClient>();
        services.AddScoped<ObtenerOrdenFabricacionUseCase>();
        services.AddScoped<ObtenerOrdenFabricacionJob>();

        services.AddScoped<IAlmacenRepository, AlmacenHanaRepository>();
        services.AddScoped<IFirestoreAlmacenClient, FirestoreAlmacenClient>();
        services.AddScoped<ObtenerAlmacenUseCase>();
        services.AddScoped<ObtenerAlmacenJob>();

        services.AddScoped<IClienteRepository, ClienteHanaRepository>();
        services.AddSingleton<IFirestoreClienteClient, FirestoreClienteClient>();
        services.AddScoped<ObtenerClienteUseCase>();
        services.AddScoped<ObtenerClienteJob>();

        services.AddScoped<IArticuloRepository, ArticuloHanaRepository>();
        services.AddSingleton<IFirestoreArticuloClient, FirestoreArticuloClient>();
        services.AddScoped<ObtenerArticuloUseCase>();
        services.AddScoped<ObtenerArticuloJob>();

        services.AddScoped<ICuentaSalesForceRepository, CuentaSalesForceHanaRepository>();
        services.AddHttpClient<ISalesForceCuentaClient, SalesForceCuentaClient>();
        services.AddScoped<ObtenerCuentaSalesForceUseCase>();
        services.AddScoped<ObtenerCuentaSalesForceJob>();

        services.AddScoped<ICostoEstandarRepository, CostoEstandarHanaRepository>();
        services.AddScoped<ObtenerCostoEstandarUseCase>();
        services.AddScoped<ObtenerCostoEstandarJob>();

        services.AddScoped<IPagoComisionesRepository, PagoComisionesHanaRepository>();
        services.AddScoped<ICorreoPagoComisionesClient, CorreoPagoComisionesClient>();
        services.AddScoped<ObtenerPagoComisionesUseCase>();
        services.AddScoped<ObtenerPagoComisionesJob>();

        services.AddScoped<IStockCierreRepository,StockCierreHanaRepository>();
        services.AddScoped<ObtenerStockCierreUseCase>();
        services.AddScoped<ObtenerStockCierreJob>();

        services.AddScoped<ICalificacionRiesgoRepository, CalificacionRiesgoHanaRepository>();
        services.AddScoped<ObtenerCalificacionRiesgoUseCase>();
        services.AddScoped<ObtenerCalificacionRiesgoJob>();

        services.AddScoped<ICategoriaClienteRepository, CategoriaClienteHanaRepository>();
        services.AddScoped<ObtenerCategoriaClienteUseCase>();
        services.AddScoped<ObtenerCategoriaClienteJob>();

        services.AddScoped<IRutaHistoricoRepository, RutaHistoricoHanaRepository>();
        services.AddScoped<ObtenerRutaHistoricoUseCase>();
        services.AddScoped<ObtenerRutaHistoricoJob>();

        services.AddScoped<IUpdateWorkPathRepository, UpdateWorkPathHanaRepository>();
        services.AddScoped<ObtenerUpdateWorkPathUseCase>();
        services.AddScoped<ObtenerUpdateWorkPathJob>();

        services.AddScoped<IComisionesRepository, ComisionesHanaRepository>();
        services.AddScoped<ObtenerComisionesUseCase>();
        services.AddScoped<ObtenerComisionesJob>();

        services.AddScoped<IETLRepository, ETLHanaRepository>();
        services.AddScoped<IETLInduvisRepository, ETLInduvisHanaRepository>();
        services.AddScoped<ObtenerETLInduvisUseCase>();
        services.AddScoped<ObtenerETLInduvisJob>();

        services.AddScoped<ObtenerETLPeruUseCase>();
        services.AddScoped<ObtenerETLPeruJob>();

        services.AddScoped<IETLCreditosCobranzaRepository, ETLCreditosCobranzaHanaRepository>();
        services.AddScoped<ObtenerETLCreditosCobranzaUseCase>();
        services.AddScoped<ObtenerETLCreditosCobranzaJob>();

        services.AddScoped<IPCPRepository, PCPHanaRepository>();
        services.AddScoped<PCPUseCase>();
        services.AddScoped<PCPJob>();

        services.AddScoped<IWMSRepository, WMSRepository>();
        services.AddScoped<WMSUseCase>();
        services.AddScoped<WMSJob>();

        return services;
    }
}