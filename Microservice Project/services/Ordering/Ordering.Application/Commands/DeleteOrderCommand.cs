using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Ordering.Application.Commands
{
    public class DeleteOrderCommand:IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
