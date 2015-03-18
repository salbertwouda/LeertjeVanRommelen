using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace LeertjeVanRommelen
{
    internal class ProgramOptions
    {
        [Option('s', "source", HelpText = "a path pointing towards the sourcefile")]
        public string SourceFile { get; set; }

        [Option('t', "target", HelpText = "a connectionstring pointing towards the target database")]
        public string ConnectionString { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        public void InterpretArguments(IEnumerable<string> args)
        {
            var fixedArgs = args.Select(FixCusomArgumentFormat)
                                    .ToArray();

            Parser.Default.ParseArguments(fixedArgs, this);
        }

        private string FixCusomArgumentFormat(string arg)
        {
            var prependWithDash = new[] { "-source", "-target" };
            if (prependWithDash.Contains(arg.ToLower()))
            {
                return "-" + arg;
            }
            return arg;
        }
    }
}