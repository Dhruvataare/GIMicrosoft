// <copyright file="TeamStorageProvider.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SubmitIdea.Common.Providers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using global::Azure;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SubmitIdea.Common.Interfaces;
    using Microsoft.Teams.Apps.SubmitIdea.Models;
    using Microsoft.Teams.Apps.SubmitIdea.Models.Configuration;

    /// <summary>
    /// Implements storage provider which helps to storage team information in Microsoft Azure Table storage.
    /// </summary>
    public class TeamStorageProvider : BaseStorageProvider, ITeamStorageProvider
    {
        private const string TeamConfigurationTable = "TeamConfiguration";

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamStorageProvider"/> class.
        /// </summary>
        /// <param name="options">A set of key/value application configuration properties for Microsoft Azure Table storage.</param>
        /// <param name="logger">Logger implementation to send logs to the logger service.</param>
        public TeamStorageProvider(
            IOptions<StorageSettings> options,
            ILogger<CategoryStorageProvider> logger)
            : base(options?.Value.ConnectionString, TeamConfigurationTable, logger)
        {
        }

        /// <summary>
        /// Store or update team detail in Azure table storage.
        /// </summary>
        /// <param name="teamEntity">Represents team entity used for storage and retrieval.</param>
        /// <returns><see cref="Task"/> that represents team entity is saved or updated.</returns>
        public async Task<bool> StoreOrUpdateTeamDetailAsync(TeamEntity teamEntity)
        {
            var result = await this.StoreOrUpdateEntityAsync(teamEntity);

            return !result.IsError;
        }

        /// <summary>
        /// Get already team detail from Azure table storage.
        /// </summary>
        /// <param name="teamId">Team Id.</param>
        /// <returns><see cref="Task"/> Already saved team detail.</returns>
        public async Task<TeamEntity> GetTeamDetailAsync(string teamId)
        {
            await this.EnsureInitializedAsync();

            if (string.IsNullOrWhiteSpace(teamId))
            {
                return null;
            }

            var data = await this.Table.GetEntityAsync<TeamEntity>(teamId, teamId);
            return data.Value;
        }

        /// <summary>
        /// This method delete the team detail record from table.
        /// </summary>
        /// <param name="teamEntity">Team configuration table entity.</param>
        /// <returns>A <see cref="Task"/> of type bool where true represents entity record is successfully deleted from table while false indicates failure in deleting data.</returns>
        public async Task<bool> DeleteTeamDetailAsync(TeamEntity teamEntity)
        {
            await this.EnsureInitializedAsync();
            teamEntity = teamEntity ?? throw new ArgumentNullException(nameof(teamEntity));

            var result = await this.Table.DeleteEntityAsync(teamEntity.PartitionKey, teamEntity.RowKey);
            return !result.IsError;
        }

        /// <summary>
        /// Stores or update team details data in Microsoft Azure Table storage.
        /// </summary>
        /// <param name="entity">Holds team idea detail entity data.</param>
        /// <returns>A task that represents idea post entity data is saved or updated.</returns>
        private async Task<Response> StoreOrUpdateEntityAsync(TeamEntity entity)
        {
            await this.EnsureInitializedAsync();
            entity = entity ?? throw new ArgumentNullException(nameof(entity));

            if (string.IsNullOrWhiteSpace(entity.ServiceUrl) || string.IsNullOrWhiteSpace(entity.TeamId))
            {
                return null;
            }

            return await this.Table.UpsertEntityAsync(entity);
        }
    }
}
