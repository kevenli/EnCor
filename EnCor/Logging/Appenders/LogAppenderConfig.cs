using EnCor.Configuration;
using EnCor.ObjectBuilder;

namespace EnCor.Logging.Appenders
{
    [Assembler(typeof(LogAppenderAssembler))]
    public class LogAppenderConfig : NameTypeConfigElement
    {
    }
}
