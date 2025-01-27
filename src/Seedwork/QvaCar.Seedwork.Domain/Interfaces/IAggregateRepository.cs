﻿using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Seedwork.Domain
{
    public interface IAggregateRepository<T,Tid> where T : AggregateRoot<Tid>
    {
        Task AddAsync(T aggregateRoot, CancellationToken cancellationToken);

        Task UpdateAsync(T aggregateRoot, CancellationToken cancellationToken);
    }
}