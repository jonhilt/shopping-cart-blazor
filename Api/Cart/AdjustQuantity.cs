using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCartWithBackend.Data;

namespace ShoppingCartWithBackend.Api.Cart
{

    public class AdjustQuantity
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
            public int Quantity { get; set; }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly StoreContext _context;

            public CommandHandler(StoreContext context)
            {
                _context = context;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                // find the cart (in reality we'd use the customer's id or similar to do this)
                var cart = await _context.Carts
                    .Include(x => x.LineItems)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                // find the relevant line and adjust its quantity
                var line = cart.LineItems.Single(x => x.Id == request.Id);
                line.Quantity = request.Quantity;

                // save changes
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}