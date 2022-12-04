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
    public class LoginQuery : IQuery<Utilisateur>
    {
        public string Email { get; init; }
        public string Passwd { get; init; }

        public LoginQuery(string email, string passwd)
        {
            Email = email;
            Passwd = passwd;
        }
    }

    public class LoginQueryHandler : IQueryHandler<LoginQuery, Utilisateur>
    {
        private readonly DbConnection _dbConnection;

        public LoginQueryHandler(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Utilisateur? Execute(LoginQuery query)
        {
            using(_dbConnection)
            {
                return _dbConnection.ExecuteReader<Utilisateur>("CSP_Login", true, query).SingleOrDefault();
            }
        }
    }
}
