using Application.Interfaces;
using Application.Specifications;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.StockS
{
    public class StockService
    {
        private readonly IReadOnlyRepositoryAsync<Stock> _stockRepository;

        public StockService(IReadOnlyRepositoryAsync<Stock> stockRepository)
        {
            _stockRepository = stockRepository;
        }

       public async void RestarStock(int cantidad, long idProducto)
       {
            var stock = await _stockRepository.FirstOrDefaultAsync(new StockSpecification(idProducto)) 
                ?? throw new Exception("No se encontró el stock del producto en el proceso de substracción de stock");
            stock.Cantidad -= cantidad;
            await _stockRepository.UpdateAsync(stock);
        }
    }
}
