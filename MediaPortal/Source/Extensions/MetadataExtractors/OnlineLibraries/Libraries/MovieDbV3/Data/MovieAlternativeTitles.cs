#region Copyright (C) 2007-2012 Team MediaPortal

/*
    Copyright (C) 2007-2012 Team MediaPortal
    http://www.team-mediaportal.com

    This file is part of MediaPortal 2

    MediaPortal 2 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal 2 is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal 2. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MediaPortal.Extensions.OnlineLibraries.Libraries.MovieDbV3.Data
{
  /// <summary>
  /// Contains all available alternative titles for a specific <see cref="Movie"/>.
  /// http://help.themoviedb.org/kb/api/movie-alternative-titles
  /// </summary>
  /// <example>
  /// {
  ///   "id": 11,
  ///   "titles": [
  ///     {
  ///       "iso_3166_1": "ES",
  ///       "title": "La guerra de las galaxias. Episodio IV: Una nueva esperanza"
  ///     }
  ///   ]
  /// }
  /// </example>
  [DataContract]
  public class MovieAlternativeTitles
  {
    [DataMember(Name = "id")]
    public int Id { get; set; }

    #region AlternativeTitle class

    /// <example>
    ///     {
    ///       "iso_3166_1": "ES",
    ///       "title": "La guerra de las galaxias. Episodio IV: Una nueva esperanza"
    ///     }
    /// </example>
    [DataContract]
    public class AlternativeTitle
    {
      [DataMember(Name = "iso_3166_1")]
      public string CountryCode { get; set; }

      [DataMember(Name = "title")]
      public string Title { get; set; }

      public override string ToString()
      {
        return Title;
      }
    }

    #endregion

    [DataMember(Name = "titles")]
    public List<AlternativeTitle> Titles { get; set; }
  }
}