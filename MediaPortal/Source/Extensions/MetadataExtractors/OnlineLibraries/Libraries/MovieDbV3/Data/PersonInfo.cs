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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MediaPortal.Extensions.OnlineLibraries.Libraries.MovieDbV3.Data
{
  /// <summary>
  /// Contains all available information about a specific <see cref="AbstractPerson"/>.
  /// http://help.themoviedb.org/kb/api/person-info
  /// </summary>
  /// <example>
  /// {
  ///   "adult": false, 
  ///   "also_known_as": [],
  ///   "biography": "From Wikipedia, the free encyclopedia.\n\nWilliam Bradley \"Brad\" Pitt (born December 18, 1963) is an American actor and film producer. Pitt has received two Academy Award nominations and four Golden Globe Award nominations, winning one. He has been described as one of the world's most attractive men, a label for which he has received substantial media attention.\n\nPitt began his acting career with television guest appearances, including a role on the CBS prime-time soap opera Dallas in 1987. He later gained recognition as the cowboy hitchhiker who seduces Geena Davis's character in the 1991 road movie Thelma &amp; Louise. Pitt's first leading roles in big-budget productions came with A River Runs Through It (1992) and Interview with the Vampire (1994). He was cast opposite Anthony Hopkins in the 1994 drama Legends of the Fall, which earned him his first Golden Globe nomination. In 1995 he gave critically acclaimed performances in the crime thriller Seven and the science fiction film 12 Monkeys, the latter securing him a Golden Globe Award for Best Supporting Actor and an Academy Award nomination. Four years later, in 1999, Pitt starred in the cult hit Fight Club. He then starred in the major international hit as Rusty Ryan in Ocean's Eleven (2001) and its sequels, Ocean's Twelve (2004) and Ocean's Thirteen (2007). His greatest commercial successes have been Troy (2004) and Mr. &amp; Mrs. Smith (2005). Pitt received his second Academy Award nomination for his title role performance in the 2008 film The Curious Case of Benjamin Button.\n\nFollowing a high-profile relationship with actress Gwyneth Paltrow, Pitt was married to actress Jennifer Aniston for five years. Pitt lives with actress Angelina Jolie in a relationship that has generated wide publicity. He and Jolie have six children\u2014Maddox, Pax, Zahara, Shiloh, Knox, and Vivienne. Since beginning his relationship with Jolie, he has become increasingly involved in social issues both in the United States and internationally. Pitt owns a production company named Plan B Entertainment, whose productions include the 2007 Academy Award winning Best Picture, The Departed.\n\nDescription above from the Wikipedia article Brad Pitt, licensed under CC-BY-SA, full list of contributors on Wikipedia.",
  ///   "birthday": "1963-12-18",
  ///   "deathday": "",
  ///   "homepage": "http://simplybrad.com/",
  ///   "id": 287,
  ///   "name": "Brad Pitt",
  ///   "place_of_birth": "Shawnee, Oklahoma, United States",
  ///   "profile_path": "/w8zJQuN7tzlm6FY9mfGKihxp3Cb.jpg"
  /// }
  /// </example>
  [DataContract]
  public class PersonInfo : AbstractPerson
  {
    [DataMember(Name = "adult")]
    public bool Adult { get; set; }

    [DataMember(Name = "also_known_as")]
    public List<string> AlsoKnownAs { get; set; }

    [DataMember(Name = "biography")]
    public string Biography { get; set; }

    [DataMember(Name = "birthday")]
    public DateTime? Birthday { get; set; }

    [DataMember(Name = "deathday")]
    public DateTime? Deathday { get; set; }

    [DataMember(Name = "homepage")]
    public string Homepage { get; set; }

    [DataMember(Name = "place_of_birth")]
    public string PlaceOfBirth { get; set; }
  }
}