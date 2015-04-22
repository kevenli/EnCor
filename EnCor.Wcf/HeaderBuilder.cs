using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;

namespace EnCor.Wcf
{
    public abstract class HeaderBuilder
    {
        public abstract IList<MessageHeader> BuildHeaders();
    }
}
