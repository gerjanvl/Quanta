using System;
using System.Collections.Generic;
using System.Text;

namespace Quanta.DataAccess.Entities
{
    public interface ITrackableEntity
    {
        DateTime CreatedOn { get; set; }

        DateTime LastModified { get; set; }
    }
}
