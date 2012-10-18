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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MediaPortal.Extensions.OnlineLibraries.Libraries.MovieDbV3.Data
{
  /// <summary>
  /// Contains all available release and certification data for a specific <see cref="Movie"/>.
  /// http://help.themoviedb.org/kb/api/movie-release-info
  /// </summary>
  /// <example>
  /// {
  ///   "countries": [
  ///     {
  ///       "certification": "PG",
  ///       "iso_3166_1": "US",
  ///       "release_date": "1977-05-25"
  ///     }
  ///   ],
  ///   "id": 11
  /// }
  /// </example>
  [DataContract]
  public class MovieReleases
  {
    [DataMember(Name = "id")]
    public int Id { get; set; }

    #region Release class

    /// <example>
    ///     {
    ///       "certification": "PG",
    ///       "iso_3166_1": "US",
    ///       "release_date": "1977-05-25"
    ///     }
    /// </example>
    [DataContract]
    public class Release
    {
      [DataMember(Name = "certification")]
      public string Certification { get; set; }

      [DataMember(Name = "iso_3166_1")]
      public string CountryCode { get; set; }

      [DataMember(Name = "release_date")]
      public DateTime? ReleaseDate { get; set; }
    }

    #endregion

    [DataMember(Name = "countries")]
    public List<Release> Releases { get; set; }
  }
}