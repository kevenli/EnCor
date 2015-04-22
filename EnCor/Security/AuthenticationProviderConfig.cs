using System.Configuration;
using EnCor.Configuration;

namespace EnCor.Security
{
    public class AuthenticationProviderConfig : NameTypeConfigElement
    {
        public const string TagString = "tag";
        private const string StrMapper = "mapper";

        /// <summary>
        /// Tag of the AuthenticationProvider
        /// </summary>
        [ConfigurationProperty(TagString)]
        public string Tag
        {
            get
            {
                return (string)this[TagString];
            }
        }

        [ConfigurationProperty(StrMapper)]
        public string Mapper
        {
            get
            {
                return (string)this[StrMapper];
            }
        }
    }
}
