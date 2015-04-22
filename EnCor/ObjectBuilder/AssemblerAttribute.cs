using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ObjectBuilder
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class AssemblerAttribute : Attribute
    {
        private Type assemblerType;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblerAttribute"/> class.
        /// </summary>
        /// <param name="assemblerType">The type that implements the <see cref="IAssembler{TObject, TConfiguration}"/> interface.</param>
        public AssemblerAttribute(Type assemblerType)
        {
            if (assemblerType == null)
                throw new ArgumentNullException("assemblerType");
            // can't test for compatibility since IAssembler is generic

            this.assemblerType = assemblerType;
        }

        /// <summary>
        /// Returns the assembler type.
        /// </summary>
        public Type AssemblerType
        {
            get
            {
                return assemblerType;
            }
        }
    }
}
