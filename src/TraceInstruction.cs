using System;
using System.Collections.Generic;
using System.Linq;
using Tridion.Logging;

namespace Sdl_Web.TraceVisualizer
{
    internal class TraceInstruction
    {
        private List<TraceChannels> _traceChannels = null;
        private TraceKeywords? _traceKeywords;
        private TraceLevels? _traceLevel;
        
        internal string LogPath { get; set; }

        internal IEnumerable<string> ProcessNames { get; set; }

        internal List<TraceChannels> TraceChannels
        {
            get
            {
                if (_traceChannels == null)
                {
                    List<TraceChannels> channels = new List<TraceChannels>();
                    foreach (string channelName in Enum.GetNames(typeof(TraceChannels)))
                    {
                        Enum.TryParse(channelName, out TraceChannels channel);
                        channels.Add(channel);
                    }
                    _traceChannels = channels;
                }
                return _traceChannels;
            }
            set
            {
                _traceChannels = value;
            }
        }

        internal TraceKeywords TraceKeywords
        {
            get
            {
                return _traceKeywords ?? TraceKeywords.All;
            }
            set
            {
                _traceKeywords = value;
            }
        }

        internal TraceLevels TraceLevel
        {
            get
            {
                return _traceLevel ?? TraceLevels.Verbose;
            }
            set
            {
                _traceLevel = value;
            }
        }

        internal Action OnComplete { get; set; }

        internal Action<Exception> OnError { get; set; }

        internal FileFormat TraceFileFormat { get; set; }

        internal int? CircularSize { get; set; }

        internal int? MultiFileSize { get; set; }

        internal int? BufferSizeMb { get; set; }

        internal int? BufferQuantumKB { get; set; }

        internal bool IsSingleProcessTrace
        {
            get
            {
                return ProcessNames != null && ProcessNames.Count() == 1;
            }
        }
    }

    public enum FileFormat
    {
        Text,
        Etl
    }
}