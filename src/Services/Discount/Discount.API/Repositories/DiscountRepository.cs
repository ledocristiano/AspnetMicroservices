using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            string sqlCommand = @" SELECT 
                                        Id,
                                        ProductName,
                                        Description,
                                        Amount
                                    FROM
                                        Coupon
                                    WHERE
                                        ProductName = @ProductName";

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                (sqlCommand, new { ProductName = productName });

            if (coupon == null)
            {
                return new Coupon
                {
                    ProductName = "No Discount",
                    Description = "No Discount Desc",
                    Amount = 0
                };
            }

            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
               (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            string sqlCommand = @"INSERT INTO Coupon (
                                      ProductName,
                                      Description,
                                      Amount)
                                  VALUES
                                      (@ProductName,
                                       @Description,
                                       @Amount)";

            var affected = await connection.ExecuteAsync(sqlCommand, coupon);

            return affected != 0;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
              (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            string sqlCommand = @"UPDATE Coupon SET
                                      ProductName = @ProductName,
                                      Description = @Description,
                                      Amount = @Amount
                                  WHERE Id = @Id";

            var affected = await connection.ExecuteAsync(sqlCommand, coupon);

            return affected != 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection
              (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            string sqlCommand = @"DELETE FROM Coupon WHERE ProductName = @ProductName";

            var affected = await connection.ExecuteAsync(sqlCommand, new { ProductName = productName });

            return affected != 0;
        }
    }
}
