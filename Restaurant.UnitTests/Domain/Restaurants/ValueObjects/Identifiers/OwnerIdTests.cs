﻿using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;

namespace unitTests.Domain.Restaurants.ValueObjects.Identifiers;

[TestFixture]
public class OwnerIdTests
{
    [Test]
    public void OwnerId_CanBe0_Success()
    {
        var creation = () => new OwnerId(0);
        creation.Should().NotThrow();
    }

    [Test]
    public void OwnerId_CanBeNegative_Success()
    {
        var creation = () => new OwnerId(-1);
        creation.Should().NotThrow();
    }

    [Test]
    public void OwnerId_CanBePositive_Success()
    {
        var creation = () => new OwnerId(1);
        creation.Should().NotThrow();
    }
}