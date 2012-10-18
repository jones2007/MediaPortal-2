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
  /// Represents a list of available backdrop and poster <see cref="ImageFile" />s for a specific <see cref="Movie" />.
  /// http://help.themoviedb.org/kb/api/movie-images
  /// </summary>
  /// <example>
  /// {
  ///   "backdrops": [
  ///     {
  ///       "file_path": "/r0v9dayXd1IH5WPWFBWv52tGHkB.jpg",
  ///       "width": 1920,
  ///       "height": 1080,
  ///       "iso_639_1": null,
  ///       "aspect_ratio": 1.78,
  ///       "vote_average": 7.65,
  ///       "vote_count": 10
  ///     }
  ///   ],
  ///   "id": 11,
  ///   "posters": [
  ///     {
  ///       "file_path": "/tvSlBzAdRE29bZe5yYWrJ2ds137.jpg",
  ///       "width": 1000,
  ///       "height": 1500,
  ///       "iso_639_1": "en",
  ///       "aspect_ratio": 0.67,
  ///       "vote_average": 6.385245901639344,
  ///       "vote_count": 61
  ///     }
  ///   ]
  /// }
  /// </example>
  [DataContract]
  public class MovieImages
  {
    [DataMember(Name = "id")]
    public int Id { get; set; }

    [DataMember(Name = "backdrops")]
    public List<RatedImageFile> Backdrops { get; set; }

    [DataMember(Name = "posters")]
    public List<RatedImageFile> Posters { get; set; }

    public void SetMovieIds()
    {
      if (Backdrops != null) Backdrops.ForEach(c => c.ParentObjectId = Id);
      if (Posters != null) Posters.ForEach(c => c.ParentObjectId = Id);
    }
  }
}