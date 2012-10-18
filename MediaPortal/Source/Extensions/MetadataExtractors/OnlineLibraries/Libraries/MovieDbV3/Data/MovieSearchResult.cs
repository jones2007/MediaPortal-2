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
using System.Runtime.Serialization;

namespace MediaPortal.Extensions.OnlineLibraries.Libraries.MovieDbV3.Data
{
  /// <summary>
  /// Represents a single item in a PagedSearchResult.
  /// http://help.themoviedb.org/kb/api/search-movies
  /// </summary>
  /// <example>
  ///     {
  ///       "adult": false,
  ///       "backdrop_path": "/8uO0gUM8aNqYLs1OsTBQiXu0fEv.jpg",
  ///       "id": 550,
  ///       "original_title": "Fight Club",
  ///       "release_date": "1999-10-15",
  ///       "poster_path": "/2lECpi35Hnbpa4y46JX0aY3AWTy.jpg",
  ///       "popularity": 61151.745000000003,
  ///       "title": "Fight Club",
  ///       "vote_average": 9.0999999999999996,
  ///       "vote_count": 174
  ///     }
  /// </example>
  [DataContract]
  public class MovieSearchResult : AbstractMovie
  {
    [DataMember(Name = "original_title")]
    public string OriginalTitle { get; set; }

    [DataMember(Name = "adult")]
    public bool Adult { get; set; }

    [DataMember(Name = "popularity")]
    public float? Popularity { get; set; }

    [DataMember(Name = "vote_average")]
    public float? VoteAverage { get; set; }

    [DataMember(Name = "vote_count")]
    public int? VoteCount { get; set; }
  }
}