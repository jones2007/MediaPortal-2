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
  /// Represents a list of available backdrop and poster <see cref="ImageFile" />s for a specific <see cref="MovieCollection" />.
  /// http://help.themoviedb.org/kb/api/collection-images
  /// </summary>
  /// <example>
  /// {
  ///   "backdrops": [{
  ///     "aspect_ratio": 1.78,
  ///     "file_path": "/mOTtuakUTb1qY6jG6lzMfjdhLwc.jpg",
  ///     "height": 1080,
  ///     "iso_639_1": null,
  ///     "width": 1920
  ///     }
  ///   ],
  ///   "id": 11,
  ///   "posters": [{
  ///     "aspect_ratio": 0.67,
  ///     "file_path": "/qoETrQ73Jbd2LDN8EUfNgUerhzG.jpg",
  ///     "height": 1500,
  ///     "iso_639_1": "en",
  ///     "width": 1000
  ///     }
  ///   ]
  /// }
  /// </example>
  [DataContract]
  public class MovieCollectionImages
  {
    [DataMember(Name = "id")]
    public int Id { get; set; }

    [DataMember(Name = "backdrops")]
    public List<ImageFile> Backdrops { get; set; }

    [DataMember(Name = "posters")]
    public List<ImageFile> Posters { get; set; }

    public void SetCollectionIds()
    {
      if (Backdrops != null)
        Backdrops.ForEach(c =>
        {
          c.ParentObject = "Collections";
          c.ParentObjectId = Id;
          c.ImageCategory = "Backdrops";
        });
      if (Posters != null)
        Posters.ForEach(c =>
        {
          c.ParentObject = "Collections";
          c.ParentObjectId = Id;
          c.ImageCategory = "Posters";
        });
    }
  }
}