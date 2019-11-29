using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCartWithBackend.Data;

namespace ShoppingCartWithBackend.Api.Cart
{
    public class Get
    {
        public class Query : IRequest<Model>
        {
        }

        public class Model
        {
            public List<Item> Items { get; set; } = new List<Item>();

            public class Item
            {
                public int Id { get; set; }
                public int Quantity { get; set; }
                public string Name { get; set; }
                public decimal Price { get; set; }
                public string Image { get; set; }
            }
        }

        public class QueryHandler : IRequestHandler<Query, Model>
        {
            private readonly StoreContext _storeContext;

            public QueryHandler(StoreContext storeContext)
            {
                _storeContext = storeContext;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                // in reality, we'd take something like a customer id and retrieve their specific cart...
                var cart = await _storeContext.Carts
                    .Include(x => x.LineItems)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                // map to the model we're returning (for this specific query)
                var items = cart.LineItems.Select(x => new Model.Item
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Image = x.Image
                });

                // return an instance of the model
                return new Model
                {
                    Items = items.ToList()
                };
            }
        }
    }
}