﻿using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.Supplier.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.Supplier.Areas.Admin.Mapping
{
    public class SupplierEntityBuilder : NopEntityBuilder<SupplierEntity>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(SupplierEntity.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(SupplierEntity.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(SupplierEntity.ContactPerson)).AsString(200).Nullable()
                .WithColumn(nameof(SupplierEntity.Phone)).AsString(100).Nullable()
                .WithColumn(nameof(SupplierEntity.Email)).AsString(200).Nullable()
                .WithColumn(nameof(SupplierEntity.Address)).AsString(1000).Nullable()
                .WithColumn(nameof(SupplierEntity.Description)).AsString(250).Nullable()
                 .WithColumn(nameof(SupplierEntity.IsActive)).AsBoolean().NotNullable().WithDefaultValue(true);
        }
    }
}