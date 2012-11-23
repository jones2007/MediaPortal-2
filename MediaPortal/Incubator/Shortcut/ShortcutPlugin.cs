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
using MediaPortal.Common;
using MediaPortal.Common.PluginManager;
using MediaPortal.Common.Settings;
using MediaPortal.UI.Control.InputManager;
using MediaPortal.UI.Presentation.Workflow;
using MediaPortal.UI.SkinEngine.InputManagement;
using MediaPortal.UiComponents.Shortcut.Settings;

namespace MediaPortal.UiComponents.Shortcut
{
  public class ShortcutPlugin : IPluginStateTracker
  {
    #region Protected fields

    protected IDictionary<Key, Guid> _mappedKeyCodes = new Dictionary<Key, Guid>();

    #endregion

    #region Public methods

    public void RegisterShortcuts()
    {
      if (!ReadMappings())
        return;

      InputManager inputManager = InputManager.Instance;
      inputManager.KeyPressed += HandleKeyPress;
    }

    private bool ReadMappings()
    {
      ISettingsManager settings = ServiceRegistration.Get<ISettingsManager>();
      ShortcutSettings shortcutSettings = settings.Load<ShortcutSettings>();
      if (shortcutSettings.EnableShortcuts && shortcutSettings.KeyMappings == null)
      {
        // Setup default mappings
        _mappedKeyCodes[new Key('H')] = _mappedKeyCodes[Key.Start] = new Guid("7F702D9C-F2DD-42da-9ED8-0BA92F07787F"); // Home
        _mappedKeyCodes[new Key('V')] = new Guid("A4DF2DF6-8D66-479a-9930-D7106525EB07"); // <WorkflowContributorAction Id="A4DF2DF6-8D66-479a-9930-D7106525EB07" Name="Home->Videos"...
        _mappedKeyCodes[new Key('T')] = _mappedKeyCodes[Key.LiveTV] = new Guid("C7646667-5E63-48c7-A490-A58AC9518CFA"); // <PushNavigationTransition Id="B4A9199F-6DD4-4bda-A077-DE9C081F7703" Name="Home->SlimTvClient"...TargetState="C7646667-5E63-48c7-A490-A58AC9518CFA"
        _mappedKeyCodes[new Key('G')] = _mappedKeyCodes[Key.Guide] = new Guid("7323BEB9-F7B0-48c8-80FF-8B59A4DB5385"); // <PushNavigationTransition Id="FA056DED-1122-42bd-A3DE-CB6CF2A59C66" Name="SlimTvClient->Guide"...TargetState="7323BEB9-F7B0-48c8-80FF-8B59A4DB5385"
        _mappedKeyCodes[new Key('W')] = new Guid("E34FDB62-1F3E-4aa9-8A61-D143E0AF77B5"); // <PushNavigationTransition Id="E34FDB62-1F3E-4aa9-8A61-D143E0AF77B5" Name="Home->Weather"...TargetState="44E1CF89-66D0-4850-A076-E1B602432983"
        _mappedKeyCodes[new Key('F')] = _mappedKeyCodes[Key.Red] = new Guid("9C3E6701-6856-49ec-A4CD-0CEB15F385F6"); // <WorkflowContributorAction Id="9C3E6701-6856-49ec-A4CD-0CEB15F385F6" Name="*->FullscreenContent"
        _mappedKeyCodes[new Key('C')] = new Guid("D83604C0-0936-4416-9DE8-7B6D7C50023C"); // <WorkflowContributorAction Id="D83604C0-0936-4416-9DE8-7B6D7C50023C" Name="*->CurrentMedia"
        shortcutSettings.KeyMappings = _mappedKeyCodes.ToList();
        settings.Save(shortcutSettings);
      }
      if (shortcutSettings.KeyMappings == null)
        return false;
      
      _mappedKeyCodes = shortcutSettings.KeyMappings.ToDictionary();
      return shortcutSettings.EnableShortcuts && _mappedKeyCodes.Count > 0;
    }

    public void UnregisterShortcuts()
    {
      InputManager inputManager = InputManager.Instance;
      inputManager.KeyPressed -= HandleKeyPress;
    }

    private void HandleKeyPress(ref Key key)
    {
      Guid actionOrTargetState;
      if (_mappedKeyCodes.TryGetValue(key, out actionOrTargetState) ||
        key.RawCode.HasValue && _mappedKeyCodes.TryGetValue(new Key(key.RawCode.Value), out actionOrTargetState))
      {
        IWorkflowManager workflowManager = ServiceRegistration.Get<IWorkflowManager>();
        WorkflowAction action;
        // We have two possibilities here: the mapped GUID might be either a WorkflowAction or TargetState.
        // First we try to find a WorkflowAction:
        if (workflowManager.MenuStateActions.TryGetValue(actionOrTargetState, out action))
        {
          // Check if workflow contributor can handle this action currently
          if (action.IsEnabled(null) && action.IsVisible(null))
            action.Execute();
        }
        else
        {
          // actionOrTargetState did not match a WorkFlowAction, so try to navigate into a new State
          bool isInStack = workflowManager.NavigationContextStack.ToList().FirstOrDefault(n => n.WorkflowState.StateId == actionOrTargetState) != null;
          if (isInStack)
            workflowManager.NavigatePopToState(actionOrTargetState, false);
          else
            workflowManager.NavigatePush(actionOrTargetState);
        }
      }
    }

    #endregion

    #region IPluginStateTracker implementation

    public void Activated(PluginRuntime pluginRuntime)
    {
      RegisterShortcuts();
    }

    public bool RequestEnd()
    {
      UnregisterShortcuts();
      return true;
    }

    public void Stop() { }

    public void Continue() { }

    public void Shutdown()
    {
      UnregisterShortcuts();
    }

    #endregion
  }
}
