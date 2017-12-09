﻿// ==========================================================================
//  MongoRuleRepository.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Squidex.Domain.Apps.Entities.Rules.Repositories;
using Squidex.Domain.Apps.Entities.Rules.State;
using Squidex.Infrastructure.MongoDb;
using Squidex.Infrastructure.States;

namespace Squidex.Domain.Apps.Entities.MongoDb.Rules
{
    public sealed class MongoRuleRepository : MongoRepositoryBase<MongoRuleEntity>, IRuleRepository, ISnapshotStore<RuleState>
    {
        public MongoRuleRepository(IMongoDatabase database)
            : base(database)
        {
        }

        protected override string CollectionName()
        {
            return "States_Rules";
        }

        protected override async Task SetupCollectionAsync(IMongoCollection<MongoRuleEntity> collection)
        {
            await collection.Indexes.CreateOneAsync(Index.Ascending(x => x.State.AppId));
            await collection.Indexes.CreateOneAsync(Index.Ascending(x => x.State.IsDeleted));
        }

        public async Task<(RuleState Value, long Version)> ReadAsync(string key)
        {
            var existing =
                await Collection.Find(x => x.Id == key)
                    .FirstOrDefaultAsync();

            if (existing != null)
            {
                return (existing.State, existing.Version);
            }

            return (null, -1);
        }

        public async Task<IReadOnlyList<Guid>> QueryRuleIdsAsync(Guid appId)
        {
            var ruleEntities =
                await Collection.Find(x => x.State.AppId == appId && !x.State.IsDeleted).Only(x => x.Id)
                    .ToListAsync();

            return ruleEntities.Select(x => Guid.Parse(x.Id)).ToList();
        }

        public async Task WriteAsync(string key, RuleState value, long oldVersion, long newVersion)
        {
            try
            {
                await Collection.UpdateOneAsync(x => x.Id == key && x.Version == oldVersion,
                    Update
                        .Set(x => x.State, value)
                        .Set(x => x.Version, newVersion),
                    Upsert);
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    var existingVersion =
                        await Collection.Find(x => x.Id == key).Only(x => x.Id, x => x.Version)
                            .FirstOrDefaultAsync();

                    if (existingVersion != null)
                    {
                        throw new InconsistentStateException(existingVersion.Version, oldVersion, ex);
                    }
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
