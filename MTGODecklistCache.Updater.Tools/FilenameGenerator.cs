using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGODecklistCache.Updater.Tools
{
    public static class FilenameGenerator
    {
        public static string GenerateFileName(string tournamentId, string tournamentName, DateTime tournamentDate, string tournamentFormat, string[] validFormats, int seat)
        {
            if (!tournamentName.Contains(tournamentFormat, StringComparison.InvariantCultureIgnoreCase)) tournamentName += $" ({tournamentFormat})";

            foreach (var otherFormat in validFormats.Where(f => !f.Equals(tournamentFormat, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (tournamentName.Contains(otherFormat, StringComparison.InvariantCultureIgnoreCase)) tournamentName = tournamentName.Replace(otherFormat, otherFormat.Substring(0, 3), StringComparison.InvariantCultureIgnoreCase);
            }

            if (seat >= 0) tournamentName += $" (Seat {seat + 1})";

            return $"{SlugGenerator.SlugGenerator.GenerateSlug(tournamentName.Trim())}-{tournamentId}-{tournamentDate.ToString("yyyy-MM-dd")}.json";
        }
    }
}
