﻿using core.SimpleTypes;
using domain.Menus.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations;

internal class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        const string idName = "IngredientID";
        builder.ToTable(MenuDatabaseConstants.Ingredients, MenuDatabaseConstants.Schema);
        builder.Property<int>(idName)
            .ValueGeneratedOnAdd();

        builder.HasKey(idName);

        builder.Property(i => i.Name)
            .HasConversion(name => name.Value, db => new NotEmptyString(db));
    }
}