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
  /// Contains all available information about a specific <see cref="MovieCollection"/>.
  /// http://help.themoviedb.org/kb/api/collection-info
  /// </summary>
  /// <example>
  /// {
  ///   "backdrop_path": "/mOTtuakUTb1qY6jG6lzMfjdhLwc.jpg",
  ///   "id": 10,
  ///   "name": "Star Wars Collection",
  ///   "parts": [{
  ///     "backdrop_path": "/mOTtuakUTb1qY6jG6lzMfjdhLwc.jpg",
  ///     "id": 11,
  ///     "poster_path": "/qoETrQ73Jbd2LDN8EUfNgUerhzG.jpg",
  ///     "release_date": "1977-12-27",
  ///     "title": "Star Wars: Episode IV: A New Hope"
  ///     }
  ///   ],
  ///   "poster_path": "/6rddZZpxMQkGlpQYVVxb2LdQRI3.jpg"
  /// }
  /// </example>
  [DataContract]
  public class MovieCollectionInfo : MovieCollection
  {
    [DataMember(Name = "parts")]
    public List<AbstractMovie> Parts { get; set; }
  }
}