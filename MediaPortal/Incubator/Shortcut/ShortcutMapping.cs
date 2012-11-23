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
using System.Linq;
using System.Collections.Generic;
using MediaPortal.UI.Control.InputManager;

namespace MediaPortal.UiComponents.Shortcut
{
  public class ShortcutMapping
  {
    public Key Key { get; set; }
    public Guid Action { get; set; }
  }

  public static class ShortcutMappingExtensions
  {
    public static IDictionary<Key, Guid> ToDictionary(this IList<ShortcutMapping> mappings)
    {
      Dictionary<Key, Guid> result = new Dictionary<Key, Guid>();
      mappings.ToList().ForEach(m => result[m.Key] = m.Action);
      return result;
    }
    public static List<ShortcutMapping> ToList(this IDictionary<Key, Guid> mappings)
    {
      return mappings.Select(mkc => new ShortcutMapping { Key = mkc.Key, Action = mkc.Value }).ToList();
    }
  }
}
