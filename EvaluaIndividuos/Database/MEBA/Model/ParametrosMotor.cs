namespace EvaluaIndividuos.Database.MEBA.Model
    {
    using EvaluaIndividuos.Model;
    using Microsoft.EntityFrameworkCore;
        using System.ComponentModel.DataAnnotations;
        using System.ComponentModel.DataAnnotations.Schema;

        [Table("PARAMETROS_MOTOR")]
        public class ParametrosMotor
        {
            [Column("VAR_ID", TypeName = "varchar(50)")]
            public string? Parametro { get; set; }

            [Column("GRUPO", TypeName = "varchar(50)")]
            public string? Grupo { get; set; }

            [Column("VALOR_VARCHAR", TypeName = "varchar(2000)")]
            public string? Valor { get; set; }


            public static async Task<Parametros> GetAsync(MEBAFactory meba)
            {
                using var db = await meba._dbContextFactory.CreateDbContextAsync();
                var registros = await db.Parametros
                    .AsNoTracking()
                    .ToListAsync();

                var registrosMap = registros
                        .Where(r => r.Parametro != null)
                        .ToDictionary(
                            r => r.Parametro!,
                            r => r.Valor ?? string.Empty
                        );

            return new Parametros(registrosMap);
            }
        }
    }