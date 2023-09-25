using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasyLife.Interfaces
{
    /// <summary>
    /// Interface zur erstellung von Dateipfaden.
    /// </summary>
    public interface IAccessFile
    {
        /// <summary>
        /// Erstellt einen Dateipfad zu dem Dakumentenordner in dem Internen Speicher im Handy. 
        /// </summary>
        /// <param name="FileName">Dateiname der für den vollständigen Pfad bnötigt werden.</param>
        /// <returns></returns>
        string CreateFile(string FileName);
    }
}
