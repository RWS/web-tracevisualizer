using System;
using System.Collections.Generic;
using System.Linq;
using Tridion.Logging;

namespace Sdl_Web.TraceVisualizer
{
    internal static class ArgumentParser
    {
        internal static TraceInstruction Parse(string[] args)
        {
            if (args.Length == 0)
            {
                ShowHelp();
                return null;
            }

            try
            {
                var instruction = new TraceInstruction();
                var argsQueue = new Queue<string>(args);

                while (argsQueue.Count > 0)
                {
                    string paramName = GetParameterName(argsQueue);

                    switch (paramName)
                    {
                        case "-filepath":
                            string path = GetParameterValue(argsQueue);
                            instruction.LogPath = path;
                            break;
                        case "-tracelevel":
                            string traceLevel = GetParameterValue(argsQueue);
                            instruction.TraceLevel = (TraceLevels)Enum.Parse(typeof(TraceLevels), traceLevel);
                            break;
                        case "-tracekeywords":
                            string keywords = GetParameterValue(argsQueue);
                            instruction.TraceKeywords = (TraceKeywords)Enum.Parse(typeof(TraceKeywords), keywords);
                            break;
                        case "-processnames":
                            string processNames = GetParameterValue(argsQueue);
                            instruction.ProcessNames = processNames.Split(',').Select(s => s.Trim());
                            break;
                        case "-channelnames":
                            string channelNames = GetParameterValue(argsQueue);
                            IEnumerable<TraceChannels> traceChannels = channelNames.Split(',').Select(name => (TraceChannels)Enum.Parse(typeof(TraceChannels), name.Trim()));
                            instruction.TraceChannels = traceChannels.ToList();
                            break;
                        case "-tracefileformat":
                            string fileFormat = GetParameterValue(argsQueue);
                            instruction.TraceFileFormat = (FileFormat)Enum.Parse(typeof(FileFormat), fileFormat);
                            break;
                        case "-circularsize":
                            instruction.CircularSize = int.Parse(GetParameterValue(argsQueue));
                            break;
                        case "-multifilesize":
                            instruction.MultiFileSize = int.Parse(GetParameterValue(argsQueue));
                            break;
                        case "-buffersizemb":
                            instruction.BufferSizeMb = int.Parse(GetParameterValue(argsQueue));
                            break;
                        case "-bufferquantumkb":
                            instruction.BufferQuantumKB = int.Parse(GetParameterValue(argsQueue));
                            break;
                        default:
                            throw new Exception(string.Format("Unknown parameter name: '{0}'", paramName));
                    }
                }

                return instruction;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();

                ShowHelp();
                return null;
            }
        }

        private static string GetParameterValue(Queue<string> argsQueue)
        {
            string paramValue = argsQueue.Dequeue();
            if (paramValue[0] == '-')
            {
                throw new Exception(string.Format("Unexpected parameter value: '{0}'", paramValue));
            }

            return paramValue;
        }

        private static string GetParameterName(Queue<string> argsQueue)
        {
            string paramName = argsQueue.Dequeue().ToLowerInvariant();
            if (paramName[0] != '-')
            {
                throw new Exception(string.Format("Unknown parameter name: '{0}'", paramName));
            }

            return paramName;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Usage: ");
        }
    }
}