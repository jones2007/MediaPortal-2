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
using System.Threading;

namespace Ui.Players.BassPlayer.Utils
{
  /// <summary>
  /// Represents a single workitem to be executed on a different thread.
  /// </summary>
  public class WorkItem : IDisposable
  {
    #region Fields

    private readonly Delegate _Method;
    private readonly object[] _Args = null;
    private object _Result = null;
    private readonly ManualResetEvent _Event = new ManualResetEvent(false);

    #endregion

    #region Public members

    /// <summary>
    /// Gets a waithandle that can be used to wait for a command to be served by the controller.
    /// </summary>
    public WaitHandle WaitHandle
    {
      get { return _Event; }
    }

    public void Dispose()
    {
      _Event.Close();
    }

    /// <summary>
    /// Gets the result of the operation.
    /// </summary>
    public object Result
    {
      get { return _Result; }
    }

    /// <summary>
    /// Gets the result of the operation as a boolean value.
    /// </summary>
    public bool ResultAsBool
    {
      get { return _Result == null ? false : (bool)_Result; }
    }

    /// <summary>
    /// Gets the result of the operation as a integer value.
    /// </summary>
    public int ResultAsInt
    {
      get { return _Result == null ? 0 : (int)_Result; }
    }

    /// <summary>
    /// Creates a workitem object.
    /// </summary>
    /// <param name="method">A delegate representing the method to execute.</param>
    /// <param name="args">Optional parameters for the method.</param>
    public WorkItem(Delegate method, params object[] args)
    {
      _Method = method;
      _Args = args;
    }

    /// <summary>
    /// Executes the method associated with the workitem.
    /// </summary>
    public void Invoke()
    {
      _Result = _Method.DynamicInvoke(_Args);
      _Event.Set();
    }

    #endregion
  }
}