using System;

namespace Quanta.Domain
{
    public interface ITrackableEntity
    {
        DateTime CreatedOn { get; set; }

        DateTime LastModified { get; set; }
    }
}
