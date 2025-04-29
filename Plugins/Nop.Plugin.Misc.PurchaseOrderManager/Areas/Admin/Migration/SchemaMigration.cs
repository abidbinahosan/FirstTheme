﻿using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.MIgration;
[NopSchemaMigration("2025/04/28 06:39:23", "Misc.PurchaseOrderManager base schema", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<PurchaseOrder>();
        Create.TableFor<PurchaseOrderProduct>();
    }
}