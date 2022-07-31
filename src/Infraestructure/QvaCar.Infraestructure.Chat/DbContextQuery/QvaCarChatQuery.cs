using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Data.DbContextQuery
{
    public class QvaCarChatQuery : DbConnectionManagerBase, IQvaCarChatQuery
    {
        public QvaCarChatQuery(DbConnection connection, bool closeConnection = true) : base(connection, closeConnection) { }

        public async Task<IEnumerable<T>> QueryListAsync<T>(string sql, object? parameters = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<T>? response = null;
            await ExecuteInConnectionAsync(async connection =>
            {
                response = await connection.QueryAsync<T>(sql, parameters);
            }, cancellationToken);

            return response ?? throw new Exception("Not gonna happen");
        }

        public async Task<(T1, IEnumerable<T2>)> QueryMultipleAsync<T1, T2>(string sqlQuery, object? param = null, CancellationToken cancellationToken = default)
        {
            T1? item1 = default;
            IEnumerable<T2>? item2 = null;

            await ExecuteInConnectionAsync(async connection =>
            {
                var queryResult = await Connection.QueryMultipleAsync(sqlQuery, param);
                (item1, item2) = (queryResult.Read<T1>().First(), queryResult.Read<T2>());
            }, cancellationToken);

            return (item1 ?? throw new Exception("Not gonna happen"), item2 ?? throw new Exception("Not gonna happen"));
        }
    }
}
