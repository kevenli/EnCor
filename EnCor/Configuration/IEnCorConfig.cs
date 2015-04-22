using System.Collections.Generic;

namespace EnCor.Configuration
{
    public interface IEnCorConfig
    {
        void Verify();

        IList<IModuleConfig> Modules { get; }
    }
}
