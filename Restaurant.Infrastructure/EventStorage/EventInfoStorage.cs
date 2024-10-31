﻿using application.EventHandlers.Interfaces;
using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using infrastructure.EventStorage.DatabaseModels;
using Microsoft.EntityFrameworkCore;
namespace infrastructure.EventStorage
{
    // TODO: TESTS
    // TODO: retry should be implmeneted here?
    public class EventInfoStorage<TAggregate, TKey>(EventDbContext context) : IEventInfoStorage<TAggregate, TKey>
        where TKey : SimpleValueType<int, TKey>
        where TAggregate : Aggregate<TKey>
    {
        private readonly EventDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        //TODO: store event reference within a scope
        public async Task StoreFailureAsync(int eventId, CancellationToken cancellationToken)
        {
            var @event = await GetEventAsync(eventId, cancellationToken);
            @event.Failed();

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> StorePendingEventAsync(TAggregate notification, CancellationToken cancellationToken)
        {
            var dbSet = _context.GetDbSet<TAggregate, TKey>();
            var @event = DomainEventModel<TAggregate, TKey>.Pending(notification);

            dbSet.Add(@event);

            await _context.SaveChangesAsync(cancellationToken);
            return @event.EventId;
        }

        public async Task StoreSuccessAsyncAsync(int eventId, CancellationToken cancellationToken)
        {
            var @event = await GetEventAsync(eventId, cancellationToken);
            @event.Success();

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<DomainEventModel<TAggregate, TKey>> GetEventAsync(int eventId, CancellationToken cancellationToken)
        {
            var dbSet = _context.GetDbSet<TAggregate, TKey>();
            var @event = await dbSet.FirstOrDefaultAsync(e => e.EventId == eventId, cancellationToken);
            if(@event is null)
                throw new ArgumentException(nameof(@event));

            return @event;
        }
    }
}
