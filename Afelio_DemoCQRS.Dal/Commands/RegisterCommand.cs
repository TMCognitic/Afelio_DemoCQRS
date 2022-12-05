using Afelio_DemoCQRS.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Connections.Databases;
using Tools.CQRS;
using Tools.CQRS.Commands;

namespace Afelio_DemoCQRS.Dal.Commands
{
    public class RegisterCommand : ICommand<int>
    {
        public string Nom { get; init; }
        public string Prenom { get; init; }
        public string Email { get; init; }
        public DateOnly Anniversaire { get; init; }
        public string Passwd { get; init; }

        public RegisterCommand(string nom, string prenom, string email, DateOnly anniversaire, string passwd)
        {
            Nom = nom;
            Prenom = prenom;
            Email = email;
            Anniversaire = anniversaire;
            Passwd = passwd;
        }
    }

    [InjectionMode(InjectionMode.Scoped)]
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, int>
    {
        private readonly DbConnection _dbConnection;

        public RegisterCommandHandler(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Result<int> Execute(RegisterCommand command)
        {
            using (_dbConnection)
            {
                try
                {
                    int? id = (int?)_dbConnection.ExecuteScalar("CSP_Register", true, command);
                    
                    if(!id.HasValue)
                    {
                        return Result<int>.Failure("Erreur avec la db...");
                    }

                    return Result<int>.Success(id.Value);
                }
                catch (Exception ex)
                {
                    return Result<int>.Failure(ex.Message);
                }
            }
        }
    }
}
