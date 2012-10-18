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
  /// Contains all cast & crew information for a specific <see cref="AbstractPerson"/>
  /// http://help.themoviedb.org/kb/api/person-credits
  /// </summary>
  /// <example>
  /// {
  ///   "cast": [
  ///     {
  ///       "character": "Jeffrey Goines",
  ///       "id": 63,
  ///       "original_title": "12 Monkeys",
  ///       "poster_path": "/6Sj9wDu3YugthXsU0Vry5XFAZGg.jpg",
  ///       "release_date": "1995-12-27",
  ///       "title": "12 Monkeys"
  ///     }
  ///   ],
  ///   "crew": [
  ///     {
  ///       "department": "Production",
  ///       "id": 652,
  ///       "job": "Other",
  ///       "original_title": "Troy",
  ///       "poster_path": "/dKQXmxWI9XLAUCIou4hwjuoqFcJ.jpg",
  ///       "release_date": "2004-05-13",
  ///       "title": "Troy"
  ///     }
  ///   ],
  ///   "id": 287
  /// }
  /// </example>
  [DataContract]
  public class PersonCredits
  {
    [DataMember(Name = "id")]
    public int Id { get; set; }

    #region CastItem class

    /// <summary>
    /// Todo: should be inherited from <see cref="AbstractMovie"/>, but here is not implementation of 'backdrop_path'-property allowed
    /// </summary>
    [DataContract]
    public class CastItem
    {
      [DataMember(Name = "character")]
      public string Character { get; set; }

      [DataMember(Name = "id")]
      public int Id { get; set; }

      [DataMember(Name = "title")]
      public string Title { get; set; }

      [DataMember(Name = "original_title")]
      public string OriginalTitle { get; set; }

      [DataMember(Name = "poster_path")]
      public string PosterPath { get; set; }

      [DataMember(Name = "release_date")]
      public DateTime? ReleaseDate { get; set; }

      [DataMember(Name = "adult")]
      public bool Adult { get; set; }
    }

    #endregion

    [DataMember(Name = "cast")]
    public List<CastItem> Cast { get; set; }

    #region CrewItem class

    /// <summary>
    /// Todo: should be inherited from <see cref="AbstractMovie"/>, but here is not implementation of 'backdrop_path'-property allowed
    /// </summary>
    [DataContract]
    public class CrewItem
    {
      [DataMember(Name = "department")]
      public string Department { get; set; }

      [DataMember(Name = "job")]
      public string Job { get; set; }

      [DataMember(Name = "id")]
      public int Id { get; set; }

      [DataMember(Name = "title")]
      public string Title { get; set; }

      [DataMember(Name = "original_title")]
      public string OriginalTitle { get; set; }

      [DataMember(Name = "poster_path")]
      public string PosterPath { get; set; }

      [DataMember(Name = "release_date")]
      public DateTime? ReleaseDate { get; set; }

      [DataMember(Name = "adult")]
      public bool Adult { get; set; }
    }

    #endregion

    [DataMember(Name = "crew")]
    public List<CrewItem> Crew { get; set; }
  }
}