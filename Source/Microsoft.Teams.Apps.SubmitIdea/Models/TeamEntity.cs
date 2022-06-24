﻿// <copyright file="TeamEntity.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SubmitIdea.Models
{
    using System;

    /// <summary>
    /// Class contains team details where application is installed.
    /// </summary>
    public class TeamEntity : ATableEntity
    {
        /// <summary>
        /// Gets or sets team id where application is installed.
        /// </summary>
        public string TeamId
        {
            get => this.PartitionKey;

            set
            {
                this.PartitionKey = value;
                this.RowKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the date time when the application is installed.
        /// </summary>
        public DateTime BotInstalledOn { get; set; }

        /// <summary>
        /// Gets or sets service URL.
        /// </summary>
        public string ServiceUrl { get; set; }
    }
}
