using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Configuration;

namespace EnCor.Wcf
{
    public class AspnetFormsTokenExtension : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(WcfClientMessageInspector); }
        }

        protected override object CreateBehavior()
        {
            return new WcfClientMessageInspector(new AspnetFormsTokenHeaderBuilder());
        }
    }
}
