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
using MediaPortal.Common;
using MediaPortal.Common.Runtime;
using MediaPortal.UI.Presentation.DataObjects;
using MediaPortal.UI.Presentation.Models;
using MediaPortal.UI.Presentation.Screens;
using MediaPortal.UI.Presentation.Workflow;

//using MediaPortal.UiComponents.Weather.Settings;


namespace MediaPortal.Plugins.ShutdownManager.Models
{
  /// <summary>
  /// Workflow model for the weather setup.
  /// </summary>
  public class ShutdownTimerDialogModel : IWorkflowModel
  {
    private ItemsList _customTimerActions ;
    public const string SHUTDOWN_TIMER_DIALOG_MODEL_ID_STR = "D5513721-92D8-4E45-B988-2C4DBF055B0F";


    //// Locations that are already in the list
    //private List<CitySetupInfo> _locations = null;
    //// Locations that return as result of searching for a city
    //private List<CitySetupInfo> _locationsSearch = null;

    //// Variants of the above that is exposed to the skin
    //private ItemsList _locationsExposed = null;
    //private ItemsList _locationsSearchExposed = null;

    //private AbstractProperty _searchCityProperty = null;

    //public AbstractProperty SearchCityProperty
    //{
    //  get { return _searchCityProperty; }
    //}

    ///// <summary>
    ///// Exposes the current search string to the skin.
    ///// </summary>
    //public string SearchCity
    //{
    //  get { return (string) _searchCityProperty.GetValue(); }
    //  set { _searchCityProperty.SetValue(value); }
    //}

    protected void UpdateActions()
    {
      //base.UpdateChannels();
      //if (_webChannelGroupIndex < _channelGroups.Count)
      //{
      //  IChannelGroup currentGroup = _channelGroups[_webChannelGroupIndex];
      //  CurrentGroupName = currentGroup.Name;
      //}
      //_channelList.Clear();
      _customTimerActions.Clear();
      //if (_channels == null)
      //  return;

      ListItem newItem;

      newItem = new ListItem("AfterTime","Time");
      _customTimerActions.Add(newItem);

      // check for playback of a file
      if (true)
      {
        newItem = new ListItem("AfterFile", "File");
        _customTimerActions.Add(newItem);
      }

      // check for playback of a playlist
      if (true)
      {
        newItem = new ListItem("AfterPlaylist", "Playlist");
        _customTimerActions.Add(newItem);
      }

      //foreach (IChannel channel in _channels)
      //{
      //  // Use local variable, otherwise delegate argument is not fixed
      //  IChannel currentChannel = channel;

      //  ChannelProgramListItem item = new ChannelProgramListItem(currentChannel, GetNowAndNextProgramsList(currentChannel))
      //  {
      //    Command = new MethodDelegateCommand(() => Tune(currentChannel))
      //  };
      //  item.AdditionalProperties["CHANNEL"] = channel;
      //  _channelList.Add(item);
      //}
      //CurrentGroupChannels.FireChange();
      CustomTimerActions.FireChange();
    }

    public ItemsList CustomTimerActions
    {
      get { return _customTimerActions; }
    }

    ///// <summary>
    ///// Loads all locations from the settings.
    ///// </summary>
    //private void GetLocationsFromSettings()
    //{
    //  WeatherSettings settings = ServiceRegistration.Get<ISettingsManager>().Load<WeatherSettings>();
    //  Locations = settings.LocationsList ?? new List<CitySetupInfo>();
    //}

    ///// <summary>
    ///// Search for a location name and fill up the _locationsSearch list.
    ///// </summary>
    ///// <param name="name"></param>
    //public void SearchLocations(string name)
    //{
    //  LocationsSearch = ServiceRegistration.Get<IWeatherCatcher>().FindLocationsByName(name);
    //}

    ///// <summary>
    ///// Saves the current state to the settings file.
    ///// </summary>
    //public void SaveSettings()
    //{
    //  ISettingsManager settingsManager = ServiceRegistration.Get<ISettingsManager>();
    //  WeatherSettings settings = settingsManager.Load<WeatherSettings>();
    //  // Apply new locations list
    //  settings.LocationsList = Locations;
    //  // Check if preferred location still in list, if not then set the first available
    //  if (settings.LocationsList.Find(loc => loc.Id == settings.LocationCode) == null && settings.LocationsList.Count > 0)
    //    settings.LocationCode = settings.LocationsList[0].Id;
    //  settingsManager.Save(settings);
    //  WeatherMessaging.SendWeatherMessage(WeatherMessaging.MessageType.LocationChanged);
    //}

    ///// <summary>
    ///// this will add a location
    ///// to the _locationsExposed and _locations list
    ///// </summary>
    ///// <param name="item"></param>
    //public void AddLocation(ListItem item)
    //{
    //  // Don't add it if it's already in there
    //  if (_locationsExposed.Any(i => i["Id"].Equals(item["Id"])))
    //    return;

    //  _locationsExposed.Add(item);
    //  // Create a CitySetupObject and add it to the loctions list
    //  CitySetupInfo c = new CitySetupInfo(item["Name"], item["Id"]) { Detail = item["Detail"] };
    //  _locations.Add(c);
    //  _locationsExposed.FireChange();
    //}

    ///// <summary>
    ///// this will delete a location from the _locationsExposed list
    ///// </summary>
    ///// <param name="item"></param>
    //public void Delete(ListItem item)
    //{
    //  if (_locationsExposed.Contains(item))
    //  {
    //    _locationsExposed.Remove(item);
    //    _locationsExposed.FireChange();
    //    string id = item["Id"];
    //    foreach (CitySetupInfo info in _locations)
    //    {
    //      if (info.Id == id)
    //      {
    //        _locations.Remove(info);
    //        return;
    //      }
    //    }
    //  }
    //}

    ///// <summary>
    ///// gets or sets the Locations
    ///// </summary>
    //public List<CitySetupInfo> Locations
    //{
    //  get { return _locations; }
    //  set
    //  {
    //    _locations = value;
    //    if (_locations == null)
    //      return;

    //    _locationsExposed.Clear();

    //    foreach (CitySetupInfo c in _locations)
    //      AddListItem(_locationsExposed, c);

    //    _locationsExposed.FireChange();
    //  }
    //}

    ///// <summary>
    ///// Gets or sets the locations which were found.
    ///// </summary>
    //public List<CitySetupInfo> LocationsSearch
    //{
    //  get { return _locationsSearch; }
    //  set
    //  {
    //    _locationsSearch = value;
    //    if (_locationsSearch == null)
    //      return;

    //    _locationsSearchExposed.Clear();
    //    foreach (CitySetupInfo c in _locationsSearch)
    //      AddListItem(_locationsSearchExposed, c);

    //    _locationsSearchExposed.FireChange();
    //  }
    //}

    //private static void AddListItem(ItemsList list, CitySetupInfo city)
    //{
    //  if (city == null)
    //    return;
    //  ListItem item = new ListItem();
    //  item.SetLabel("Name", city.Name);
    //  item.SetLabel("Id", city.Id);
    //  item.SetLabel("Detail", city.Detail);
    //  list.Add(item);
    //}

    ///// <summary>
    ///// Exposes the available locations.
    ///// </summary>
    //public ItemsList SetupLocations
    //{
    //  get { return _locationsExposed; }
    //}

    ///// <summary>
    ///// Exposes the search result.
    ///// </summary>
    //public ItemsList SetupSearchLocations
    //{
    //  get { return _locationsSearchExposed; }
    //}

    #region IWorkflowModel implementation

    public Guid ModelId
    {
      get { return new Guid(SHUTDOWN_TIMER_DIALOG_MODEL_ID_STR); }
    }

    public bool CanEnterState(NavigationContext oldContext, NavigationContext newContext)
    {
      return true;
    }

    public void EnterModelContext(NavigationContext oldContext, NavigationContext newContext)
    {
      //_searchCityProperty = new WProperty(typeof(string), string.Empty);
      //_locations = new List<CitySetupInfo>();
      //_locationsExposed = new ItemsList();
      //_locationsSearch = new List<CitySetupInfo>();
      //_locationsSearchExposed = new ItemsList();
      _customTimerActions = new ItemsList();
      //// Load settings
      //GetLocationsFromSettings();
      UpdateActions();
    }

    public void ExitModelContext(NavigationContext oldContext, NavigationContext newContext)
    {
      //_locationsExposed.Clear();
      //_locationsExposed = null;
      //_locationsSearchExposed.Clear();
      //_locationsSearchExposed = null;
      //_searchCityProperty = null;
    }

    public void ChangeModelContext(NavigationContext oldContext, NavigationContext newContext, bool push)
    {
      // TODO
    }

    public void Deactivate(NavigationContext oldContext, NavigationContext newContext)
    {
      // Nothing to do here
    }

    public void Reactivate(NavigationContext oldContext, NavigationContext newContext)
    {
      // Nothing to do here
    }

    public void UpdateMenuActions(NavigationContext context, IDictionary<Guid, WorkflowAction> actions)
    {
      // Nothing to do here
    }

    public ScreenUpdateMode UpdateScreen(NavigationContext context, ref string screen)
    {
      return ScreenUpdateMode.AutoWorkflowManager;
    }

    #endregion
  }
}