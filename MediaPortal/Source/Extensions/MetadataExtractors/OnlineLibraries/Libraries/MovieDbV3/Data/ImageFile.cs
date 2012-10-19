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
  /// Represents a single image file.
  /// </summary>
  /// <example>
  /// {
  ///   "file_path":"/zEbgoayf0MfuSznehhXdaP2YkeH.jpg",
  ///   "width":700,
  ///   "height":983,
  ///   "iso_639_1":null,
  ///   "aspect_ratio":0.71
  /// }
  /// </example>
  [DataContract]
  public class ImageFile
  {
    [DataMember(Name = "aspect_ratio")]
    public float AspectRatio { get; set; }

    [DataMember(Name = "file_path")]
    public string FilePath { get; set; }

    [DataMember(Name = "height")]
    public int Height { get; set; }

    [DataMember(Name = "width")]
    public int Width { get; set; }

    [DataMember(Name = "iso_639_1")]
    public string Language { get; set; }

    // Not filled by API!
    public string ParentObject { get; set; }
    public int ParentObjectId { get; set; }
    public string ImageCategory { get; set; }

    public override string ToString()
    {
      return FilePath;
    }
  }

  /// <summary>
  /// Represents a single image file including rating information.
  /// </summary>
  /// <example>
  /// {
  ///   "file_path": "/fdmSovGcTO4qeYH4llwqDsYi5cB.jpg",
  ///   "width": 500,
  ///   "height": 713,
  ///   "iso_639_1": "en",
  ///   "aspect_ratio": 0.7,
  ///   "vote_average": 2.7,
  ///   "vote_count": 5
  /// }
  /// </example>
  [DataContract]
  public class RatedImageFile : ImageFile
  {
    [DataMember(Name = "vote_average")]
    public float VoteAverage { get; set; }

    [DataMember(Name = "vote_count")]
    public int VoteCount { get; set; }
  }
}