using System.Configuration;
using System.Xml;

namespace EnCor.Configuration
{
    /// <summary>
    /// Represents a named <see cref="ConfigurationElement"/> wher the name is the key to a collection.
    /// </summary>
    /// <remarks>
    /// This class is used in conjunction with a <see cref="NamedElementCollection{T}"/>.
    /// </remarks>
    public abstract class NamedConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// Name of the property that holds the name of <see cref="NamedConfigurationElement"/>.
        /// </summary>
        public const string NameProperty = "name";

        /// <summary>
        /// Initialize a new instance of a <see cref="NamedConfigurationElement"/> class.
        /// </summary>
        public NamedConfigurationElement()
        {
        }

        /// <summary>
        /// Intialize a new instance of a <see cref="NamedConfigurationElement"/> class with a name.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        public NamedConfigurationElement(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        /// <value>
        /// The name of the element.
        /// </value>
        [ConfigurationProperty(NameProperty, IsKey = true, DefaultValue = "Name", IsRequired = true)]
        [StringValidator(MinLength = 1)]
        public string Name
        {
            get
            {
                return (string)this[NameProperty];
            }
            set
            {
                this[NameProperty] = value;
            }
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Updates the configuration properties of the receiver with the information in the current element in the <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The reader over the configuration file.</param>
        public void DeserializeElement(XmlReader reader)
        {
            base.DeserializeElement(reader, false);
        }
    }
}
