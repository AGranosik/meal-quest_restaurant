﻿using FluentAssertions;
using Restaurant.Domain.Common.ValueTypes.Strings;
using Restaurant.Domain.Restaurants.Entities;
using Restaurant.Domain.Restaurants.ValueObjects;
using Restaurant.Domain.Restaurants.ValueObjects.Identifiers;

namespace Restaurant.UnitTests.Restaurants.Entities
{
    [TestFixture]
    public class OwnerTests
    {
        private readonly Address _validAddress = new(new Street("street"), new City("city"), new Coordinates(3, 3));
        private readonly Name _validName = new("name");
        private readonly Name _validSurname = new("surname");
        private readonly OwnerId _validOwnerId = new OwnerId(1);

        [Test]
        public void Creation_OwnerIdCannotBeNull_ThrowsException()
        {
            var creation = () => new Owner(null, new Name("name"), new Name("sss"), _validAddress);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_OwnerIdCanBeNegative_Success()
        {
            var creation = () => new Owner(new OwnerId(-3), new Name("name"), new Name("sss"), _validAddress);
            creation.Should().NotThrow();
        }

        [Test]
        public void Creation_OwnerIdCanBe0_Success()
        {
            var creation = () => new Owner(new OwnerId(0), new Name("name"), new Name("sss"), _validAddress);
            creation.Should().NotThrow();
        }

        [Test]
        public void Creation_OwnerIdCanBePositive_Success()
        {
            var creation = () => new Owner(new OwnerId(3), new Name("name"), new Name("sss"), _validAddress);
            creation.Should().NotThrow();
        }

        [Test]
        public void Creation_NameCannotBeNull_Success()
        {
            var creation = () => new Owner(_validOwnerId, null, new Name("sss"), _validAddress);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_SurnameCannotBeNull_Success()
        {
            var creation = () => new Owner(_validOwnerId, new Name("name"), null, _validAddress);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_AddressCannotBeNull_Success()
        {
            var creation = () => new Owner(_validOwnerId, new Name("name"), _validSurname, null);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Equality_SameReference_True()
        {
            var owner = new Owner(_validOwnerId, new Name("name"), _validSurname, _validAddress);
            (owner == owner).Should().BeTrue();
        }

        [Test]
        public void Equality_AlleTheSame_True()
        {
            var owner = new Owner(_validOwnerId, new Name("name"), _validSurname, _validAddress);
            var owner2 = new Owner(_validOwnerId, new Name("name"), _validSurname, _validAddress);
            (owner == owner2).Should().BeTrue();
        }

        [Test]
        public void Equality_DifferentOWnerId_False()
        {
            var owner = new Owner(_validOwnerId, new Name("name"), _validSurname, _validAddress);
            var owner2 = new Owner(new OwnerId(34), new Name("name"), _validSurname, _validAddress);
            (owner == owner2).Should().BeFalse();
        }
    }
}