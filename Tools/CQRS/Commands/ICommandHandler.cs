using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.CQRS.Commands
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public Result Execute(TCommand command);
    }

    public interface ICommandHandler<TCommand, T>
        where TCommand : ICommand<T>
    {
        public Result<T> Execute(TCommand command);
    }
}
