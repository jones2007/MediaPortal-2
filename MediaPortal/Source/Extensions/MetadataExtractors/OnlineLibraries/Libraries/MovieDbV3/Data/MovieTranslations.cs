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
  /// Contains a list of available translation for a specific <see cref="Movie"/>.
  /// http://help.themoviedb.org/kb/api/movie-translations
  /// </summary>
  /// <example>
  /// {
  ///   "id": 11,
  ///   "translations": [
  ///     {
  ///       "english_name": "English",
  ///       "iso_639_1": "en",
  ///       "name": "English"
  ///     },
  ///     {
  ///       "english_name": "German",
  ///       "iso_639_1": "de",
  ///       "name": "Deutsch"
  ///     }
  ///   ]
  /// }
  /// </example>
  [DataContract]
  public class MovieTranslations
  {
    [DataMember(Name = "id")]
    public int Id { get; set; }

    #region Translation class

    /// <example>
    ///     {
    ///       "english_name": "German",
    ///       "iso_639_1": "de",
    ///       "name": "Deutsch"
    ///     }
    /// </example>
    [DataContract]
    public class Translation
    {
      [DataMember(Name = "english_name")]
      public string EnglishName { get; set; }

      [DataMember(Name = "iso_639_1")]
      public string Language { get; set; }

      [DataMember(Name = "name")]
      public string Name { get; set; }

      public override string ToString()
      {
        return Name;
      }
    }

    #endregion

    [DataMember(Name = "translations")]
    public List<Translation> Translations { get; set; }
  }
}