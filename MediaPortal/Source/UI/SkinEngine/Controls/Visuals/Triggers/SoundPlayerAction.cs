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

using System.Media;
using MediaPortal.Common;
using MediaPortal.Common.General;
using MediaPortal.UI.Presentation.Players;
using MediaPortal.UI.SkinEngine.SkinManagement;
using MediaPortal.Utilities.DeepCopy;

namespace MediaPortal.UI.SkinEngine.Controls.Visuals.Triggers
{
  public class SoundPlayerAction : TriggerAction
  {
    #region Protected fields

    protected AbstractProperty _sourceProperty;
    protected bool _disableOnAudioOutput = true;

    #endregion

    #region Ctor

    public SoundPlayerAction()
    {
      Init();
    }

    void Init()
    {
      _sourceProperty = new SProperty(typeof(string), null);
    }

    public override void DeepCopy(IDeepCopyable source, ICopyManager copyManager)
    {
      base.DeepCopy(source, copyManager);
      SoundPlayerAction s = (SoundPlayerAction) source;
      Source = copyManager.GetCopy(s.Source);
      DisableOnAudioOutput = s.DisableOnAudioOutput;
    }

    #endregion

    #region Public properties

    public AbstractProperty SourceProperty
    {
      get { return _sourceProperty; }
    }

    public string Source
    {
      get { return (string) _sourceProperty.GetValue(); }
      set { _sourceProperty.SetValue(value); }
    }

    public bool DisableOnAudioOutput
    {
      get { return _disableOnAudioOutput; }
      set { _disableOnAudioOutput = value; }
    }

    #endregion

    public override void Execute(UIElement element)
    {
      string source = Source;
      if (string.IsNullOrEmpty(source))
        return;
      if (_disableOnAudioOutput)
      {
        IPlayerContextManager playerContextManager = ServiceRegistration.Get<IPlayerContextManager>();
        IPlayer player1 = playerContextManager[PlayerContextIndex.PRIMARY];
        IPlayer player2 = playerContextManager[PlayerContextIndex.SECONDARY];
        if (player1 is IAudioPlayer || player1 is IVideoPlayer || player2 is IAudioPlayer || player2 is IVideoPlayer)
          return;
      }
      using (SoundPlayer simpleSound = new SoundPlayer(SkinContext.SkinResources.GetResourceFilePath(
          SkinResources.SOUNDS_DIRECTORY + "\\" + source)))
        simpleSound.Play();
    }
  }
}
