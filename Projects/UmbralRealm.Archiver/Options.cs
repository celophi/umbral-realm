using CommandLine;

namespace UmbralRealm.Archiver
{
    /// <summary>
    /// Declares command line options that the user may specify.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Directory path to the location where packages are.
        /// </summary>
        [Option('s', "source", Required = true, HelpText = "Package source directory.")]
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Directory path to write results.
        /// </summary>
        [Option('d', "destination", Required = true, HelpText = "Destination output directory")]
        public string Destination { get; set; } = string.Empty;
    }
}
