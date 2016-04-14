using System;
using System.Collections.Generic;
using System.Linq;
using Tridion.Logging;

namespace SdlWeb_TraceVisualizer
{
    public class StructuredLogEntry
    {
        private List<StructuredLogEntry> _nestedCalls;
        private List<LogEntry> _info;

        public StructuredLogEntry(TraceEntryBase startEntry, StructuredLogEntry parent = null)
        {
            if (startEntry == null)
            {
                throw new ArgumentNullException("startEntry");
            }

            ExtensionTraceEntry extensionLoggingEntry = startEntry as ExtensionTraceEntry;
            if (extensionLoggingEntry != null)
            {
                OperationName = extensionLoggingEntry.ExtensionName ?? string.Empty;
            }
            else
            {
                OperationName = startEntry.DeclaringType.Split('.').LastOrDefault() ?? string.Empty;
            }
            OperationName += "." + startEntry.MethodName;
            Start = startEntry;
            Parent = parent;
        }

        public string OperationName { get; set; }
        public TraceEntryBase Start { get; set; }
        public TraceEntryBase Stop { get; set; }

        public string ParameterValueDetailed
        {
            get
            {
                if (!string.IsNullOrEmpty(Start.MethodParameters))
                {
                    string value = string.Empty;
                    string[] paramTypes = Start.MethodSignature.Split(',');
                    string[] paramValues = Start.MethodParameters.Split(new[] { "#END#" }, StringSplitOptions.None);
                    for (int i = 0; i < paramTypes.Length; i++)
                    {
                        if (i > 0)
                        {
                            value += ", ";
                        }
                        value += paramTypes[i] + (i < paramValues.Length ? ": " + paramValues[i] : String.Empty);
                    }

                    return value;
                }

                return Start.MethodSignature ?? string.Empty;
            }
        }

        public StructuredLogEntry Parent { get; set; }

        public IList<StructuredLogEntry> NestedCalls
        {
            get
            {
                return _nestedCalls ?? (_nestedCalls = new List<StructuredLogEntry>());
            }
        }

        public IList<LogEntry> Info
        {
            get
            {
                return _info ?? (_info = new List<LogEntry>());
            }
        }

        public override string ToString()
        {
            string stopIntervalInMillisecond = Stop != null ? Stop.TimeInterval.TotalMilliseconds.ToString() : "...";

            string signature = Start.MethodSignature ?? string.Empty;

            return string.Format("{0}({1}) [{2}ms] [{3} nested calls]", OperationName, signature, stopIntervalInMillisecond, NestedCalls.Count);
        }
    }
}
