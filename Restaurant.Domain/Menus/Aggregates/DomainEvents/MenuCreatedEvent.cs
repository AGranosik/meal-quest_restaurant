﻿using domain.Common.DomainImplementationTypes;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects.Identifiers;

namespace domain.Menus.Aggregates.DomainEvents
{
    public sealed record MenuCreatedEvent(Menu Menu, MenuId Id) : DomainEvent<MenuId>(Id);
}
