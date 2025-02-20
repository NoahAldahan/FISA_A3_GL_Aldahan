using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveWPFApp.Utilities
{
    enum ELogFormat
    {
        JSON = 0,
        XML = 1
    }
    internal static class LogUtilities
    {
        static ELogFormat CurrentLogFormat = ELogFormat.JSON;

        internal static void SetLogFormat(ELogFormat logFormat)
        {
            CurrentLogFormat = logFormat;
        }
        internal static ELogFormat GetLogFormat()
        {
            return CurrentLogFormat;
        }
    }
}
