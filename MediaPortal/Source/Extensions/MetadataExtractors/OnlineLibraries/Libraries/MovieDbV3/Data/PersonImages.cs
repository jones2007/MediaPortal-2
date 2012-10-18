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
  /// Represents a list of available profile <see cref="ImageFile" />s for a specific <see cref="PersonInfo" />.
  /// http://docs.themoviedb.apiary.io/#get-%2F3%2Fperson%2F%7Bid%7D%2Fimages
  /// </summary>
  /// <example>
  /// {
  ///   "id": 287,
  ///   "profiles": [
  ///     {
  ///       "aspect_ratio": 0.66,
  ///       "file_path": "/w8zJQuN7tzlm6FY9mfGKihxp3Cb.jpg",
  ///       "height": 1969,
  ///       "iso_639_1": null,
  ///       "width": 1295
  ///     }
  ///   ]
  /// }
  /// </example>
  [DataContract]
  public class PersonImages
  {
    [DataMember(Name = "id")]
    public int Id { get; set; }

    [DataMember(Name = "profiles")]
    public List<ImageFile> Profiles { get; set; }

    public void SetPersonIds()
    {
      if (Profiles != null) Profiles.ForEach(c => c.ParentObjectId = Id);
    }
  }
}