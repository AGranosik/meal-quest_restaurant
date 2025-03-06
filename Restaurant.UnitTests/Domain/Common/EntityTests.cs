using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using FluentAssertions;

namespace unitTests.Domain.Common;

[TestFixture]
internal class EntityTests
{
    [Test]
    public void Creation_WithoutId_Success()
    {
        var creation = () => new EntityUnderTests();
        creation.Should().NotThrow();
    }

    [Test]
    public void AssignId_IdCannotBeNull_ThrowsException()
    {
        var entity = new EntityUnderTests();
        var action = () => entity.SetId(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AssignId_Success()
    {
        var entity = new EntityUnderTests();
        var action = () => entity.SetId(new EntityIdUnderTests(2));
        action.Should().NotThrow();
    }

    [Test]
    public void Equality_ReferenceEquality_True()
    {
        var entity = new EntityUnderTests();
        (entity == entity).Should().BeTrue();
    }

    [Test]
    public void Equality_BothIdNotAssigned_False()
    {
        var entity = new EntityUnderTests();
        var entity2 = new EntityUnderTests();
        (entity == entity2).Should().BeFalse();
    }

    [Test]
    public void Equality_OneIdNotAssigned_False()
    {
        var entity = new EntityUnderTests();
        entity.SetId(new EntityIdUnderTests(2));

        var entity2 = new EntityUnderTests();
        (entity == entity2).Should().BeFalse();
    }


    [Test]
    public void Equality_DifferentId_False()
    {
        var entity = new EntityUnderTests();
        entity.SetId(new EntityIdUnderTests(2));

        var entity2 = new EntityUnderTests();
        entity2.SetId(new EntityIdUnderTests(3));
        (entity == entity2).Should().BeFalse();
    }

    [Test]
    public void Equality_SameId_True()
    {
        var entity = new EntityUnderTests();
        entity.SetId(new EntityIdUnderTests(2));

        var entity2 = new EntityUnderTests();
        entity2.SetId(new EntityIdUnderTests(2));
        (entity == entity2).Should().BeTrue();
    }
}

internal class EntityIdUnderTests(int value) : SimpleValueType<int, EntityIdUnderTests>(value)
{
}
internal class EntityUnderTests : Entity<EntityIdUnderTests>
{
}