﻿// ==========================================================================
//  DomainObjectState.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System;
using Newtonsoft.Json;
using NodaTime;
using Squidex.Infrastructure;

namespace Squidex.Domain.Apps.Entities
{
    public abstract class DomainObjectState<T> : Cloneable<T>,
        IEntity,
        IEntityWithCreatedBy,
        IEntityWithLastModifiedBy,
        IEntityWithVersion,
        IUpdateableEntity,
        IUpdateableEntityWithCreatedBy,
        IUpdateableEntityWithLastModifiedBy,
        IUpdateableEntityWithVersion
        where T : Cloneable
    {
        [JsonProperty]
        public Guid Id { get; set; }

        [JsonProperty]
        public RefToken CreatedBy { get; set; }

        [JsonProperty]
        public RefToken LastModifiedBy { get; set; }

        [JsonProperty]
        public Instant Created { get; set; }

        [JsonProperty]
        public Instant LastModified { get; set; }

        [JsonProperty]
        public long Version { get; set; }

        public T Clone()
        {
            return Clone(x => { });
        }
    }
}
