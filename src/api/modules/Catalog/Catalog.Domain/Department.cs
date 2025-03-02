using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;


namespace FSH.Starter.WebApi.Catalog.Domain
{
    public class Department: AuditableEntity, IAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        public static Department Create(string code, string name)
        {
            var department = new Department
            {
                Code = code,
                Name = name
            };
            department.QueueDomainEvent(new DepartmentCreated() { Department = department });
            return department;
        }

        public Department Update(string code, string name)
        {
            if (!string.IsNullOrEmpty(code) && !Code.Equals(code, StringComparison.OrdinalIgnoreCase)) Code = code;
            if (!string.IsNullOrEmpty(name) && !Name.Equals(name, StringComparison.OrdinalIgnoreCase)) Name = name;
            this.QueueDomainEvent(new DepartmentUpdated() { Department = this });
            return this;
        }

        public static Department Update(Guid id, string code, string name)
        {
            var department = new Department
            {
                Id = id,
                Code = code,
                Name = name
            };
            department.QueueDomainEvent(new DepartmentUpdated() { Department = department });
            return department;
        }
    }
}
