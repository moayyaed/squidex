﻿// ==========================================================================
//  IContentQueryService.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Squidex.Domain.Apps.Entities.Apps;
using Squidex.Domain.Apps.Entities.Schemas;

namespace Squidex.Domain.Apps.Entities.Contents
{
    public interface IContentQueryService
    {
        Task<(ISchemaEntity Schema, long Total, IReadOnlyList<IContentEntity> Items)> QueryWithCountAsync(IAppEntity app, string schemaIdOrName, ClaimsPrincipal user, bool archived, HashSet<Guid> ids);

        Task<(ISchemaEntity Schema, long Total, IReadOnlyList<IContentEntity> Items)> QueryWithCountAsync(IAppEntity app, string schemaIdOrName, ClaimsPrincipal user, bool archived, string query);

        Task<(ISchemaEntity Schema, IContentEntity Content)> FindContentAsync(IAppEntity app, string schemaIdOrName, ClaimsPrincipal user, Guid id, long version = -1);

        Task<ISchemaEntity> FindSchemaAsync(IAppEntity app, string schemaIdOrName);
    }
}
