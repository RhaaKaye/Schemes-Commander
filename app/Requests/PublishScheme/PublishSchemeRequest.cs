﻿using System;
using MidnightLizard.Schemes.Commander.Requests.Base;

namespace MidnightLizard.Schemes.Commander.Requests.PublishScheme
{
    public class PublishSchemeRequest : Request
    {
        /// <summary>
        /// ID of the color scheme you want to publish
        /// </summary>
        public override Guid AggregateId { get => base.AggregateId; set => base.AggregateId = value; }

        /// <summary>
        /// Optional Request ID
        /// </summary>
        public override Guid Id { get => base.Id; set => base.Id = value; }

        public string Description { get; set; }
        public ColorScheme ColorScheme { get; set; }
    }
}
