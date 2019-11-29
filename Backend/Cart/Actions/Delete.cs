using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCartWithBackend.Backend.Domain;

namespace ShoppingCartWithBackend.Backend.Cart.Actions
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
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
                var cart = await _context.Carts.Include(x => x.LineItems).FirstOrDefaultAsync();
                var toDelete = cart.LineItems.Single(x => x.Id == request.Id);
                cart.LineItems.Remove(toDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}