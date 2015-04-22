using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.Configuration
{
    public interface IPolyMorphismConfig<TConfig> 
    {
        TConfig ActualConfig { get; }

        void Verify();
    }
}
