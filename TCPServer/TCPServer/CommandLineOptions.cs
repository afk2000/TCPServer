using CommandLine;
using CommandLine.Text;

namespace TCPServer
{
    public class CommandLineOptions
    {

        [Option('c', "connections", DefaultValue = 5,
            HelpText = "Number of concurrent connections allowed.")]
        public int ConcurrentConnectionsAllowed { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
