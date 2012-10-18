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
  /// A generic paged list of search results.
  /// </summary>
  /// <example>
  /// {
  ///   "page": 1,
  ///   "results": [
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
  ///   ],
  ///   "total_pages": 1,
  ///   "total_results": 5
  /// }
  /// </example>
  [DataContract]
  internal class PagedSearchResult<T>
  {
    [DataMember(Name = "page")]
    public int Page { get; set; }

    [DataMember(Name = "total_pages")]
    public int TotalPages { get; set; }

    [DataMember(Name = "total_results")]
    public int TotalResults { get; set; }

    [DataMember(Name = "results")]
    public List<T> Results { get; set; }
  }
}