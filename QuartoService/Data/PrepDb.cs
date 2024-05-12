using QuartoService.Models;
using Microsoft.EntityFrameworkCore;

namespace QuartoService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {
                try
                {
                    context.Database.Migrate();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"--> Erro na migração: {ex.Message}");
                }
            }

            if(!context.Quartos.Any())
            {
                Console.WriteLine("Inserindo Dados no Banco");

                for (int i = 0; i < 20; i++)
                {
                    context.Quartos.Add(new Quarto
                    {
                        NumeroQuarto = GetRandomInt(1, 100), 
                        QuantidadeCamas = GetRandomInt(1, 4),
                        IdHotel = GetRandomInt(1, 6)
                    });

                    context.SaveChanges();

                }

            }
            else
            {
                Console.WriteLine("Já tem Dados no Banco!");
            }
        }

        static int GetRandomInt(int min, int max)
        {
        return new Random().Next(min, max + 1);
        }
    }
}