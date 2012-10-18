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
  /// Contains all cast information for a specific <see cref="Movie"/>.
  /// http://help.themoviedb.org/kb/api/movie-casts
  /// </summary>
  /// <example>
  /// {
  ///   "cast": [
  ///     {
  ///       "character": "Luke Skywalker",
  ///       "id": 2,
  ///       "name": "Mark Hamill",
  ///       "order": 3,
  ///       "profile_path": "/a85lLkADqD2Ab03cfMyJVQaE1UR.jpg"
  ///     }
  ///   ],
  ///   "crew": [
  ///     {
  ///       "department": "Directing",
  ///       "id": 1,
  ///       "job": "Director",
  ///       "name": "George Lucas",
  ///       "profile_path": "/7Q5FVw6RhI1gsr1QHmJZuwxshRF.jpg"
  ///     }
  ///   ],
  ///   "id": 11
  /// }
  /// </example>
  [DataContract]
  public class MovieCasts
  {
    [DataMember(Name = "id")]
    public int Id { get; set; }

    #region CastItem class

    /// <example>
    ///     {
    ///       "character": "Luke Skywalker",
    ///       "id": 2,
    ///       "name": "Mark Hamill",
    ///       "order": 3,
    ///       "profile_path": "/a85lLkADqD2Ab03cfMyJVQaE1UR.jpg"
    ///     }
    /// </example>
    [DataContract]
    public class CastItem : AbstractPerson
    {
      [DataMember(Name = "character")]
      public string Character { get; set; }

      [DataMember(Name = "order")]
      public string Order { get; set; }
    }

    #endregion

    [DataMember(Name = "cast")]
    public List<CastItem> Cast { get; set; }

    #region CrewItem class

    /// <example>
    ///     {
    ///       "department": "Directing",
    ///       "id": 1,
    ///       "job": "Director",
    ///       "name": "George Lucas",
    ///       "profile_path": "/7Q5FVw6RhI1gsr1QHmJZuwxshRF.jpg"
    ///     }
    /// </example>
    [DataContract]
    public class CrewItem : AbstractPerson
    {
      [DataMember(Name = "department")]
      public string Department { get; set; }

      [DataMember(Name = "job")]
      public string Job { get; set; }
    }

    #endregion

    [DataMember(Name = "crew")]
    public List<CrewItem> Crew { get; set; }
  }
}