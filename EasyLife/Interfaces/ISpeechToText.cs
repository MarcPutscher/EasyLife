using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EasyLife.Interfaces
{
    public interface ISpeechToText
    {
        Task<bool> RequestPermissions();

        Task<string> Listen(CultureInfo culture,
            IProgress<string> recognitionResult,
            CancellationToken cancellationToken);
    }
}
