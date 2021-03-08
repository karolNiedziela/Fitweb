using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Entities
{
    public interface IBaseEntity
    {
        int Id { get; set; }

        DateTime DateCreated { get; set; }

        DateTime DateUpdated { get; set; }
    }
}
