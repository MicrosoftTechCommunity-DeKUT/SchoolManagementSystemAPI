using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Values
{
  public class UpdateClassroom
  {
    public class Command : IRequest
    {
      public int Id { get; set; }
      public string Name { get; set; }
    }
    public class Handler : IRequestHandler<Command>
    {
      private readonly SchoolDbContext _context;

      public Handler(SchoolDbContext context)
      {
        _context = context;
      }

      public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
      {
        var classroom = await _context.Values.FindAsync(request.Id);
        if (classroom == null)
          throw new Exception("could not find the value");
        classroom.Name = request.Name ?? classroom.Name;
        var success = await _context.SaveChangesAsync() > 0;
        if (success) return Unit.Value;
        throw new Exception("Problem with updating record");
      }
    }
  }
}