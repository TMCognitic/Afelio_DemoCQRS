using Afelio_DemoCQRS.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Connections.Databases;
using Tools.CQRS.Queries;

namespace Afelio_DemoCQRS.Dal.Queries
{
    public class GetAllUserQuery : IQuery<IEnumerable<Utilisateur>>
    {
    }

    public class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, IEnumerable<Utilisateur>>
    {
        private readonly DbConnection _dbConnection;

        public GetAllUserQueryHandler(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IEnumerable<Utilisateur>? Execute(GetAllUserQuery query)
        {
            using(_dbConnection)
            {
                return _dbConnection.ExecuteReader<Utilisateur>("SELECT Id, Nom, Prenom, Email, Anniversaire FROM Utilisateur;", false, query, true);
            }
        }
    }
}
