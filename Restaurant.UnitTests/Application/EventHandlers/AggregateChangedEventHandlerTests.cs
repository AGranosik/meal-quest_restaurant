using application.EventHandlers;
using application.EventHandlers.Interfaces;
using domain.Common.BaseTypes;
using Moq;

namespace unitTests.Application.EventHandlers
{
    [TestFixture]
    public abstract class AggregateChangedEventHandlerTests<TAggregate, TKey>
        where TKey : ValueObject<TKey>
        where TAggregate : Aggregate<TKey>
    {
        private Mock<IEventInfoStorage<TAggregate, TKey>> _eventInforStorage;

        [SetUp]
        public virtual void SetUp()
        {
            _eventInforStorage = new Mock<IEventInfoStorage<TAggregate, TKey>>();
        }

        [Test]
        public void Handle_EventCannotBeNull_ThrowsException()
        {
            var handelr = CreateHandler();

        }

        [Test]
        public async Task PendingState_Success()
        {
            var handler = CreateHandler();
            
        }

        protected AggregateChangedEventHandler<TAggregate, TKey> CreateHandler()
            => new AggregateChangedEventHandler<TAggregate, TKey>(_eventInforStorage.Object);
    }
}
