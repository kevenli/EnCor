using EnCor.ModuleLoader;
namespace EnCor.ModuleLoader
{
    /// <summary>
    /// IEnCorModule, DO NOT use the interface directly, derive from EnCor.ModuleConfig class
    /// </summary>
    public interface IEnCorModule
    {
        /// <summary>
        /// Init module before using, this should be implemented only in system level modules, such as logging
        /// </summary>
        void Init();

        /// <summary>
        /// Start module running.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop module running, recycle all resources.
        /// </summary>
        void Stop();
    }
}
