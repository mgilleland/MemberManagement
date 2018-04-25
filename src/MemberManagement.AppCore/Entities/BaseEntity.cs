using System;

namespace MemberManagement.AppCore.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Archived { get; set; }
    }
}
