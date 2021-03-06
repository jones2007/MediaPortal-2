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
using MediaPortal.UI.Presentation.Players;

namespace MediaPortal.UI.Services.Players.PCMOpenPlayerStrategy
{
  /// <summary>
  /// Player open strategy which works the same as the <see cref="Default"/> strategy except for the case when at least one video player is active and
  /// another video player is requested in concurrency mode <see cref="PlayerContextConcurrencyMode.ConcurrentVideo"/>. In this case,
  /// the new video will replace the primary slot, PiP remains untouched playing in background.
  /// If we have only one video playing, the old video is be moved to PiP while the new video gets the primary (fullscreen) video.
  /// </summary>
  public class PreservePiP : Default
  {
    public override void PrepareVideoPlayer(IPlayerManager playerManager, IList<IPlayerContext> playerContexts, PlayerContextConcurrencyMode concurrencyMode, Guid mediaModuleId,
        out IPlayerSlotController slotController, ref int audioSlotIndex, ref int currentPlayerIndex)
    {
        int numActive = playerContexts.Count;
        switch (concurrencyMode)
        {
          case PlayerContextConcurrencyMode.ConcurrentVideo:
            if (numActive >= 1 && playerContexts[0].AVType == AVType.Video)
            { // The primary slot is a video player slot
              if (numActive == 1)
              {
                int slotIndex;
                playerManager.OpenSlot(out slotIndex, out slotController);
                playerManager.SwitchSlots();
              }
              else // numActive > 1
              {
                IPlayerContext pc = playerContexts[0];
                pc.Reset(); // Necessary to reset the player context to disable the auto close function (pc.CloseWhenFinished)
                playerManager.ResetSlot(PlayerManagerConsts.PRIMARY_SLOT, out slotController);
              }

              audioSlotIndex = PlayerManagerConsts.PRIMARY_SLOT;
              currentPlayerIndex = PlayerManagerConsts.PRIMARY_SLOT;
              return;
            }
            break;
        }
      // All other cases are the same as in the default player open strategy
      base.PrepareVideoPlayer(playerManager, playerContexts, concurrencyMode, mediaModuleId, out slotController, ref audioSlotIndex, ref currentPlayerIndex);
    }
  }
}