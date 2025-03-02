using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain.Events
{
    public sealed record DepartmentUpdated : DomainEvent
    {
        public Department? Department { get; set; }
    }
}
