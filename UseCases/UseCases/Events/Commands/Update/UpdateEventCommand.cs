using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Commands.Update;

public class UpdateEventCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateTimeEvent { get; set; }
    public string Location { get; set; }
    public string Category { get; set; }
    public int MaxCountMembers { get; set; }
}
