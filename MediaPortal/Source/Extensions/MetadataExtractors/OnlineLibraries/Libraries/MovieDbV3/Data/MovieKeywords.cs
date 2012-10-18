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
  /// Contains all keywords that have been added to a specific <see cref="Movie"/>.
  /// http://help.themoviedb.org/kb/api/movie-keywords
  /// </summary>
  /// <example>
  /// {
  ///   "id": 11,
  ///   "keywords": [
  ///     {
  ///       "id": 378,
  ///       "name": "prison"
  ///     },
  ///     {
  ///       "id": 803,
  ///       "name": "android"
  ///     }
  ///   ]
  /// }
  /// </example>
  [DataContract]
  public class MovieKeywords
  {
    [DataMember(Name = "id")]
    public int Id { get; set; }

    #region Keyword class

    /// <remarks>
    ///   {
    ///   "id": 33851,
    ///   "name": "capitalism"
    ///   }
    /// </remarks>
    [DataContract]
    public class Keyword
    {
      [DataMember(Name = "id")]
      public int Id { get; set; }

      [DataMember(Name = "name")]
      public string Name { get; set; }

      public override string ToString()
      {
        return Name;
      }
    }

    #endregion

    [DataMember(Name = "keywords")]
    public List<Keyword> Keywords { get; set; }
  }
}