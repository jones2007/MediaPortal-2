﻿#region Copyright (C) 2007-2008 Team MediaPortal

/*
    Copyright (C) 2007-2008 Team MediaPortal
    http://www.team-mediaportal.com
 
    This file is part of MediaPortal II

    MediaPortal II is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal II is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal II.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

using System;
using System.IO;
using MediaPortal.Core.PluginManager;

namespace MediaPortal.Core.Services.PluginManager.Builders
{
  /// <summary>
  /// Builds an item of type "Resource". The resource item type provides access to a resource
  /// directory which is provided by a plugin.
  /// </summary>
  /// <remarks>
  /// The item registration has to provide the parameters "Type" and "Directory":
  /// <example>
  /// &lt;Resource Type="Skin" Directory="Skin"/&gt;
  /// </example>
  /// The values for the "Type" parameter come from the type <see cref="PluginResourceType"/>.
  /// </remarks>
  public class ResourceBuilder : IPluginItemBuilder
  {
    public object BuildItem(PluginItemMetadata itemData, PluginRuntime plugin)
    {
      BuilderHelper.CheckParameter("Type", itemData);
      BuilderHelper.CheckParameter("Directory", itemData);
      return new PluginResource(
          (PluginResourceType) Enum.Parse(typeof (PluginResourceType), itemData.Attributes["Type"]),
          new DirectoryInfo(plugin.Metadata.GetAbsolutePath(itemData.Attributes["Directory"])));
    }

    public bool NeedsPluginActive
    {
      get { return false; }
    }
  }
}
