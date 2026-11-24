using Dapper;
using EvaluaIndividuos.Model;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace EvaluaIndividuos.Database.MEBI.Model
{
    [Table("cache_trazas")]
    public class CacheTrazas
    {
        public string salida { get; }
    

    public static async Task<string> GetCache(string servicio, long numeroTributario, int? vigencia) { 

        using var connection = new SqlConnection(MEBIContext.GetConnectionString());
        var parameters = new DynamicParameters();
        parameters.Add("@servicio", servicio, DbType.String, ParameterDirection.Input, 50);
        parameters.Add("@numero_tributario", numeroTributario, DbType.Int64, ParameterDirection.Input);
        if (vigencia.HasValue) parameters.Add("@vigencia", vigencia.Value, DbType.Int32, ParameterDirection.Input);
        parameters.Add("@salida", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

        await connection.ExecuteAsync(
            "[dbo].[get_cache]",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<string>("@salida");
        }
    }
}