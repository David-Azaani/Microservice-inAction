﻿using Npgsql;

namespace Discount.Grpc.Extentions
{
    public static class HostExtentions
    {

        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postresql database.");

                    ExecuteMigrations(configuration);
                    logger.LogInformation("Migrated  postresql database.");
                }
                catch (NpgsqlException e)
                {
                    logger.LogInformation(e, "An Error has occurred while migrating!");
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }


            }

            return host;
        }
        private static void ExecuteMigrations(IConfiguration configuration)
        {
            using var connection = new NpgsqlConnection
                (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            connection.Open();

            using var command = new NpgsqlCommand
            {
                Connection = connection
            };

            command.CommandText = "DROP TABLE IF EXISTS Coupon";
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
            command.ExecuteNonQuery();


            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 160);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 200);";
            command.ExecuteNonQuery();
        }
    }
}
