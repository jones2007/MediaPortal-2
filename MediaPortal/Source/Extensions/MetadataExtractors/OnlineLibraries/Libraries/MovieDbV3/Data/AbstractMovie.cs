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
  /// Contains the basic information for a specific Movie.
  /// todo: Renamed AbstractMovie to something else, because it is not abstract anymore. Maybe 'Movie' -> 'MovieInfo' and 'AbstractMovie' to 'Movie' or 'MovieBase'
  /// </summary>
  [DataContract]
  public class AbstractMovie
  {
    [DataMember(Name = "id")]
    public int Id { get; set; }

    [DataMember(Name = "title")]
    public string Title { get; set; }

    [DataMember(Name = "release_date")]
    public DateTime? ReleaseDate { get; set; }

    [DataMember(Name = "poster_path")]
    public string PosterPath { get; set; }

    [DataMember(Name = "backdrop_path")]
    public string BackdropPath { get; set; }

    public override string ToString()
    {
      return Title;
    }
  }
}