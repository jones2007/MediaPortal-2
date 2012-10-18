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

using System.Runtime.Serialization;

namespace MediaPortal.Extensions.OnlineLibraries.Libraries.MovieDbV3.Data
{
  /// <summary>
  /// Represents a single item in a PagedSearchResult.
  /// http://help.themoviedb.org/kb/api/search-companies
  /// </summary>
  /// <example>
  /// {
  ///   "page": 1,
  ///   "results": [
  ///     {
  ///       "id": 34,
  ///       "logo_path": "/56VlAu08MIE926dQNfBcUwTY8np.png",
  ///       "name": "Sony Pictures"
  ///     },
  ///     {
  ///       "id": 2251,
  ///       "logo_path": null,
  ///       "name": "Sony Pictures Animation"
  ///     }
  ///   ],
  ///   "total_pages": 1,
  ///   "total_results": 2
  /// }
  /// </example>
  [DataContract]
  public class CompanySearchResult : Company
  {
    [DataMember(Name = "logo_path")]
    public string LogoPath { get; set; }
  }
}