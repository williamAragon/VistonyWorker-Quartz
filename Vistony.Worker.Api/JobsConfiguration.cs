using Hangfire;
using Vistony.Worker.Application.Almacen.UseCases;
using Vistony.Worker.Application.Articulo.UseCases;
using Vistony.Worker.Application.CalificacionRiesgo.UseCases;
using Vistony.Worker.Application.CategoriaCliente.UseCases;
using Vistony.Worker.Application.Cliente.UseCases;
using Vistony.Worker.Application.Comisiones.UseCases;
using Vistony.Worker.Application.CostoEstandar.UseCases;
using Vistony.Worker.Application.CuentaSalesForce.UseCases;
using Vistony.Worker.Application.ETL.UseCases;
using Vistony.Worker.Application.ETLCreditosCobranza.UseCases;
using Vistony.Worker.Application.MigracionCliente.UseCases;
using Vistony.Worker.Application.OrdenFabricacion.UseCases;
using Vistony.Worker.Application.PagoComisiones.UseCases;
using Vistony.Worker.Application.PCP.UseCase;
using Vistony.Worker.Application.PromoRangos.UseCases;
using Vistony.Worker.Application.RutaHistorico.UseCases;
using Vistony.Worker.Application.StockCierre.UseCases;
using Vistony.Worker.Application.TipoCambio.UseCases;
using Vistony.Worker.Application.UpdateWorkPath.UseCases;
using Vistony.Worker.Application.WMS.UseCases;

namespace Vistony.Worker.Api
{
    public static class JobsConfiguration
    {
        public static void RegisterJobs(IConfiguration configuration)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(
                configuration["Jobs:TimeZone"] ?? "SA Pacific Standard Time");

            RecurringJob.AddOrUpdate<ObtenerTipoCambioSunatJob>(
                "tipo-cambio-sunat-sap",
                job => job.ExecuteAsync(),
                configuration["Jobs:TipoCambioCron"] ?? "0 20 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerOrdenFabricacionJob>(
                "orden-fabricacion",
                job => job.ExecuteAsync(),
                configuration["Jobs:OrdenFabricacion"] ?? "02 15 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerAlmacenJob>(
                "almacen",
                job => job.ExecuteAsync(),
                configuration["Jobs:Almacen"] ?? "30 21 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerClienteJob>(
                "cliente",
                job => job.ExecuteAsync(),
                configuration["Jobs:Cliente"] ?? "0 6 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerArticuloJob>(
                "articulo",
                job => job.ExecuteAsync(),
                configuration["Jobs:Articulo"] ?? "0 6 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerCuentaSalesForceJob>(
                "cuenta-salesforce",
                job => job.ExecuteAsync(),
                configuration["Jobs:CuentaSalesForce"] ?? "30 12 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerCostoEstandarJob>(
                "costo-estandar",
                job => job.ExecuteAsync(),
                configuration["Jobs:CostoEstandar"] ?? "0 2 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerPagoComisionesJob>(
                "pago-comisiones",
                job => job.ExecuteAsync(),
                configuration["Jobs:PagoComisiones"] ?? "21 11 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerStockCierreJob>(
                "stock-cierre",
                job => job.ExecuteAsync(),
                configuration["Jobs:StockCierre"] ?? "0 6 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerCalificacionRiesgoJob>(
                "calificacion-riesgo",
                job => job.ExecuteAsync(),
                configuration["Jobs:CalificacionRiesgo"] ?? "0 12 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerCategoriaClienteJob>(
                "categoria-cliente",
                job => job.ExecuteAsync(),
                configuration["Jobs:CategoriaCliente"] ?? "0 5 15 * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerRutaHistoricoJob>(
                "ruta-historico",
                job => job.ExecuteAsync(),
                configuration["Jobs:RutaHistorico"] ?? "45 4/8 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerUpdateWorkPathJob>(
                "update-workpath",
                job => job.ExecuteAsync(),
                configuration["Jobs:UpdateWorkPath"] ?? "45 5 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            //RecurringJob.AddOrUpdate<ObtenerComisionesJob>(
            //    "comisiones",
            //    job => job.ExecuteAsync(),
            //    configuration["Jobs:Comisiones"] ?? "15 6/4 * * *",
            //    new RecurringJobOptions
            //    {
            //        TimeZone = timeZone
            //    });

            RecurringJob.AddOrUpdate<ObtenerETLInduvisJob>(
                "etl-induvis",
                job => job.ExecuteAsync(),
                configuration["Jobs:ETLInduvis"] ?? "0 * * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerETLPeruJob>(
                "etl-peru",
                job => job.ExecuteAsync(),
                configuration["Jobs:ETLPeru"] ?? "0 * * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<ObtenerETLCreditosCobranzaJob>(
                "etl-creditos-cobranza",
                job => job.ExecuteAsync(),
                configuration["Jobs:ETLCreditosCobranza"] ?? "30 6 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<CostosComercialJob>(
            "CostosComercial",
            job => job.ExecuteAsync(),
            configuration["Jobs:CostosComercial"] ?? "11 15 * * *",
            new RecurringJobOptions
            {
                TimeZone = timeZone
            });

            RecurringJob.AddOrUpdate<PCPJob>(
                "pcp",
                job => job.ExecuteAsync(),
                configuration["Jobs:PCP"] ?? "55 * * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<MigrarClienteJob>(
                "migracion-clienteGeneral",
                job => job.ExecuteAsync(),
                configuration["Jobs:MigracionCliente"] ?? "0 6 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            
            RecurringJob.AddOrUpdate<WMSJob>(
                "wms",
                job => job.ExecuteAsync(),
                configuration["Jobs:WMS"] ?? "5 * * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });

            RecurringJob.AddOrUpdate<CalcularPromoRangosJob>(
                "promo-rangos",
                job => job.ExecuteAsync(),
                configuration["Jobs:PromoRangos"] ?? "0 8-17 * * *",
                new RecurringJobOptions
                {
                    TimeZone = timeZone
                });
        }
    }
}

