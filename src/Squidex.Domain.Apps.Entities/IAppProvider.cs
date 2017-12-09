﻿// ==========================================================================
//  IAppProvider.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squidex.Domain.Apps.Entities.Apps;
using Squidex.Domain.Apps.Entities.Rules;
using Squidex.Domain.Apps.Entities.Schemas;

namespace Squidex.Domain.Apps.Entities
{
    public interface IAppProvider
    {
        Task<(IAppEntity, ISchemaEntity)> GetAppWithSchemaAsync(Guid appId, Guid id);

        Task<IAppEntity> GetAppAsync(string appName);

        Task<ISchemaEntity> GetSchemaAsync(Guid appId, Guid id, bool provideDeleted = false);

        Task<ISchemaEntity> GetSchemaAsync(Guid appId, string name, bool provideDeleted = false);

        Task<List<ISchemaEntity>> GetSchemasAsync(Guid appId);

        Task<List<IRuleEntity>> GetRulesAsync(Guid appId);

        Task<List<IAppEntity>> GetUserApps(string userId);
    }
}
