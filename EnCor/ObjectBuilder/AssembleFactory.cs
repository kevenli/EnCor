using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ObjectBuilder
{
    public class AssembleFactory<TObject, TConfig>
        where TObject: class
        where TConfig: class
    {
        public virtual TObject Build(TConfig config, IBuilderContext context)
        {
            // get actual config instance's assembler attribute
            IAssembler<TObject, TConfig> assembler = GetAssembler(config);
            if (assembler != null)
            {
                return assembler.Assemble(context, config);
            }

            throw new EnCorException(string.Format("Cannot build module for config:{0}", config));
        }

        protected IAssembler<TObject, TConfig> GetAssembler(TConfig actualInstance)
        {
            if (actualInstance is IAssembler<TObject, TConfig>)
            {
                return actualInstance as IAssembler<TObject, TConfig>;
            }

            object[] attributes = actualInstance.GetType().GetCustomAttributes(typeof(AssemblerAttribute), true);
            if (attributes.Length == 0)
            {
                return null;
            }
            var attribute = attributes[0] as AssemblerAttribute;
            if (attribute == null)
            {
                return null;
            }
            var assembler = (IAssembler<TObject, TConfig>)Activator.CreateInstance(attribute.AssemblerType);
            return assembler;
        }
    }
}
