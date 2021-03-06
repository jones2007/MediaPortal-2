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
using System.Linq;
using MediaPortal.Common;
using MediaPortal.Common.General;
using MediaPortal.Common.Logging;
using MediaPortal.Common.MediaManagement;
using MediaPortal.Common.MediaManagement.Helpers;
using MediaPortal.Common.ResourceAccess;
using MediaPortal.Common.Messaging;
using MediaPortal.Common.Settings;
using MediaPortal.Common.SystemResolver;
using MediaPortal.Common.Threading;
using MediaPortal.UI.ServerCommunication;
using MediaPortal.UI.ServerCommunication.Settings;
using MediaPortal.UI.Shares;
using UPnP.Infrastructure.CP;
using RelocationMode=MediaPortal.Common.MediaManagement.RelocationMode;

namespace MediaPortal.UI.Services.ServerCommunication
{
  public class ServerConnectionManager : IServerConnectionManager
  {
    /// <summary>
    /// Callback instance for the importer worker, implementing the callback interfaces
    /// <see cref="IMediaBrowsing"/> and <see cref="IImportResultHandler"/>.
    /// </summary>
    internal class ImporterCallback : IMediaBrowsing, IImportResultHandler
    {
      protected IContentDirectory _contentDirectory;
      protected string _localSystemId;

      public ImporterCallback(IContentDirectory contentDirectory)
      {
        _contentDirectory = contentDirectory;
        _localSystemId = ServiceRegistration.Get<ISystemResolver>().LocalSystemId;
      }

      #region IMediaBrowsing implementation

      public MediaItem LoadLocalItem(ResourcePath path,
          IEnumerable<Guid> necessaryRequestedMIATypeIDs, IEnumerable<Guid> optionalRequestedMIATypeIDs)
      {
        try
        {
          return _contentDirectory.LoadItem(_localSystemId, path, necessaryRequestedMIATypeIDs, optionalRequestedMIATypeIDs);
        }
        catch (Exception)
        {
          throw new DisconnectedException();
        }
      }

      public ICollection<MediaItem> Browse(Guid parentDirectoryId,
          IEnumerable<Guid> necessaryRequestedMIATypeIDs, IEnumerable<Guid> optionalRequestedMIATypeIDs)
      {
        try
        {
          return _contentDirectory.Browse(parentDirectoryId, necessaryRequestedMIATypeIDs, optionalRequestedMIATypeIDs);
        }
        catch (Exception)
        {
          throw new DisconnectedException();
        }
      }

      #endregion

      #region IImportResultHandler implementation

      public Guid UpdateMediaItem(Guid parentDirectoryId, ResourcePath path, IEnumerable<MediaItemAspect> updatedAspects)
      {
        try
        {
          return _contentDirectory.AddOrUpdateMediaItem(parentDirectoryId, _localSystemId, path, updatedAspects);
        }
        catch (Exception)
        {
          throw new DisconnectedException();
        }
      }

      public void DeleteMediaItem(ResourcePath path)
      {
        try
        {
          _contentDirectory.DeleteMediaItemOrPath(_localSystemId, path, true);
        }
        catch (Exception)
        {
          throw new DisconnectedException();
        }
      }

      public void DeleteUnderPath(ResourcePath path)
      {
        try
        {
          _contentDirectory.DeleteMediaItemOrPath(_localSystemId, path, false);
        }
        catch (Exception)
        {
          throw new DisconnectedException();
        }
      }

      #endregion
    }

    protected AsynchronousMessageQueue _messageQueue;
    protected UPnPServerWatcher _serverWatcher = null;
    protected UPnPClientControlPoint _controlPoint = null;
    protected bool _isHomeServerConnected = false;
    protected ICollection<Guid> _currentlyImportingSharesProxy = new List<Guid>();
    protected object _syncObj = new object();

    public ServerConnectionManager()
    {
      _messageQueue = new AsynchronousMessageQueue(this, new string[]
          {
            SharesMessaging.CHANNEL,
            ImporterWorkerMessaging.CHANNEL,
          });
      _messageQueue.MessageReceived += OnMessageReceived;
      _messageQueue.Start();
      string homeServerSystemId = HomeServerSystemId;
      if (string.IsNullOrEmpty(homeServerSystemId))
        // Watch for all MP 2 media servers, if we don't have a homeserver yet
        _serverWatcher = BuildServerWatcher();
      else
        // If we have a homeserver set, we'll try to connect to it
        _controlPoint = BuildClientControlPoint(homeServerSystemId);
    }

    void OnMessageReceived(AsynchronousMessageQueue queue, SystemMessage message)
    {
      if (message.ChannelName == SharesMessaging.CHANNEL)
      {
        IContentDirectory cd = ContentDirectory;
        SharesMessaging.MessageType messageType =
            (SharesMessaging.MessageType) message.MessageType;
        IImporterWorker importerWorker = ServiceRegistration.Get<IImporterWorker>();
        Share share;
        switch (messageType)
        {
          case SharesMessaging.MessageType.ShareAdded:
            share = (Share) message.MessageData[SharesMessaging.SHARE];
            if (cd != null)
              cd.RegisterShare(share);
            importerWorker.ScheduleImport(share.BaseResourcePath, share.MediaCategories, true);
            break;
          case SharesMessaging.MessageType.ShareRemoved:
            share = (Share) message.MessageData[SharesMessaging.SHARE];
            importerWorker.CancelJobsForPath(share.BaseResourcePath);
            if (cd != null)
              cd.RemoveShare(share.ShareId);
            break;
          case SharesMessaging.MessageType.ShareChanged:
            RelocationMode relocationMode = (RelocationMode) message.MessageData[SharesMessaging.RELOCATION_MODE];
            share = (Share) message.MessageData[SharesMessaging.SHARE];
            importerWorker.CancelJobsForPath(share.BaseResourcePath);
            if (cd == null)
            {
              ISettingsManager settingsManager = ServiceRegistration.Get<ISettingsManager>();
              ServerConnectionSettings settings = settingsManager.Load<ServerConnectionSettings>();
              RelocationMode oldMode;
              if (settings.CachedSharesUpdates.TryGetValue(share.ShareId, out oldMode) && oldMode == RelocationMode.ClearAndReImport)
                // ClearAndReimport is stronger than Relocate, use ClearAndReImport
                relocationMode = oldMode;
              settings.CachedSharesUpdates[share.ShareId] = relocationMode;
              settingsManager.Save(settings);
            }
            else
            {
              cd.UpdateShare(share.ShareId, share.BaseResourcePath, share.Name, share.MediaCategories, relocationMode);
              switch (relocationMode)
              {
                case RelocationMode.ClearAndReImport:
                  importerWorker.ScheduleImport(share.BaseResourcePath, share.MediaCategories, true);
                  break;
                case RelocationMode.Relocate:
                  importerWorker.ScheduleRefresh(share.BaseResourcePath, share.MediaCategories, true);
                  break;
              }
            }
            break;
          case SharesMessaging.MessageType.ReImportShare:
            share = (Share) message.MessageData[SharesMessaging.SHARE];
            importerWorker.ScheduleRefresh(share.BaseResourcePath, share.MediaCategories, true);
            break;
        }
      }
      else if (message.ChannelName == ImporterWorkerMessaging.CHANNEL)
      {
        IContentDirectory cd = ContentDirectory;
        ImporterWorkerMessaging.MessageType messageType = (ImporterWorkerMessaging.MessageType) message.MessageType;
        switch (messageType)
        {
          case ImporterWorkerMessaging.MessageType.ImportStarted:
          case ImporterWorkerMessaging.MessageType.ImportCompleted:
            if (cd == null)
              break;
            ResourcePath path = (ResourcePath) message.MessageData[ImporterWorkerMessaging.RESOURCE_PATH];
            ILocalSharesManagement lsm = ServiceRegistration.Get<ILocalSharesManagement>();
            ICollection<Share> shares = lsm.Shares.Values;
            Share share = shares.BestContainingPath(path);
            if (share == null)
              break;
            if (messageType == ImporterWorkerMessaging.MessageType.ImportStarted)
              cd.ClientStartedShareImport(share.ShareId);
            else
              cd.ClientCompletedShareImport(share.ShareId);
            break;
        }
      }
    }

    static void OnAvailableBackendServersChanged(ICollection<ServerDescriptor> allAvailableServers, bool serversWereAdded)
    {
      ServerConnectionMessaging.SendAvailableServersChangedMessage(allAvailableServers, serversWereAdded);
    }

    void OnBackendServerConnected(DeviceConnection connection)
    {
      ServerDescriptor serverDescriptor = ServerDescriptor.GetMPBackendServerDescriptor(connection.RootDescriptor);
      if (serverDescriptor == null)
      {
        ServiceRegistration.Get<ILogger>().Warn("ServerConnectionManager: Could not connect to home server - Unable to verify UPnP root descriptor");
        return;
      }
      SystemName preferredLink = serverDescriptor.GetPreferredLink();
      ServiceRegistration.Get<ILogger>().Info("ServerConnectionManager: Connected to home server '{0}' at host '{1}' (IP address: '{2}')", serverDescriptor.MPBackendServerUUID, preferredLink.HostName, preferredLink.Address);
      lock (_syncObj)
      {
        _isHomeServerConnected = true;
        SaveLastHomeServerData(serverDescriptor);
      }

      ServerConnectionMessaging.SendServerConnectionStateChangedMessage(ServerConnectionMessaging.MessageType.HomeServerConnected);
      ServiceRegistration.Get<IThreadPool>().Add(CompleteServerConnection);
    }

    void OnBackendServerDisconnected(DeviceConnection connection)
    {
      lock (_syncObj)
        _isHomeServerConnected = false;
      IImporterWorker importerWorker = ServiceRegistration.Get<IImporterWorker>();
      importerWorker.Suspend();
      UpdateCurrentlyImportingShares(null); // Mark all shares as not being imported
      ServerConnectionMessaging.SendServerConnectionStateChangedMessage(ServerConnectionMessaging.MessageType.HomeServerDisconnected);
    }

    protected internal UPnPContentDirectoryServiceProxy ContentDirectoryServiceProxy
    {
      get
      {
        UPnPClientControlPoint cp;
        lock (_syncObj)
          cp = _controlPoint;
        return cp == null ? null : cp.ContentDirectoryService;
      }
    }

    protected internal UPnPServerControllerServiceProxy ServerControllerServiceProxy
    {
      get
      {
        UPnPClientControlPoint cp;
        lock (_syncObj)
          cp = _controlPoint;
        return cp == null ? null : cp.ServerControllerService;
      }
    }

    /// <summary>
    /// When a home server is connected, we store the connection data of the server to be able to
    /// provide the home server's data also when the connection is down. We'll refresh the data each time
    /// the server is connected to track changes in the server's location, name, ...
    /// </summary>
    protected static void SaveLastHomeServerData(ServerDescriptor serverDescriptor)
    {
      ISettingsManager settingsManager = ServiceRegistration.Get<ISettingsManager>();
      ServerConnectionSettings settings = settingsManager.Load<ServerConnectionSettings>();
      settings.LastHomeServerName = serverDescriptor.ServerName;
      settings.LastHomeServerSystem = serverDescriptor.GetPreferredLink();
      settingsManager.Save(settings);
    }

    protected UPnPServerWatcher BuildServerWatcher()
    {
      UPnPServerWatcher result = new UPnPServerWatcher();
      result.AvailableBackendServersChanged += OnAvailableBackendServersChanged;
      return result;
    }

    protected UPnPClientControlPoint BuildClientControlPoint(string homeServerSystemId)
    {
      UPnPClientControlPoint result = new UPnPClientControlPoint(homeServerSystemId);
      result.BackendServerConnected += OnBackendServerConnected;
      result.BackendServerDisconnected += OnBackendServerDisconnected;
      return result;
    }

    /// <summary>
    /// Synchronously synchronizes all local shares and media item aspect types with the MediaPortal server.
    /// </summary>
    protected void CompleteServerConnection()
    {
      UPnPServerControllerServiceProxy sc = ServerControllerServiceProxy;
      ISystemResolver systemResolver = ServiceRegistration.Get<ISystemResolver>();
      if (sc != null)
      {
        try
        {
          // Check if we're attached to the server. If the server lost its state, it might have forgotten us.
          if (!sc.GetAttachedClients().Select(clientMetadata => clientMetadata.SystemId).Contains(systemResolver.LocalSystemId))
            sc.AttachClient(systemResolver.LocalSystemId);
        }
        catch (Exception e)
        {
          ServiceRegistration.Get<ILogger>().Warn("ServerConnectionManager: Error checking attachment state at home server '{0}'", e, HomeServerSystemId);
          return; // This is a real error case, we don't need to try any other service calls
        }

        // Register state variables change events
        sc.AttachedClientsChanged += OnAttachedClientsChanged;
        sc.ConnectedClientsChanged += OnConnectedClientsChanged;
      }
      IImporterWorker importerWorker = ServiceRegistration.Get<IImporterWorker>();
      ICollection<Share> newShares = new List<Share>();
      UPnPContentDirectoryServiceProxy cd = ContentDirectoryServiceProxy;
      if (cd != null)
      {
        // Update shares registration
        try
        {
          ISettingsManager settingsManager = ServiceRegistration.Get<ISettingsManager>();
          ServerConnectionSettings settings = settingsManager.Load<ServerConnectionSettings>();
          ServiceRegistration.Get<ILogger>().Info("ServerConnectionManager: Synchronizing shares with home server");
          IDictionary<Guid, Share> serverShares = new Dictionary<Guid, Share>();
          foreach (Share share in cd.GetShares(systemResolver.LocalSystemId, SharesFilter.All))
            serverShares.Add(share.ShareId, share);
          IDictionary<Guid, Share> localShares = ServiceRegistration.Get<ILocalSharesManagement>().Shares;
          // First remove shares - if the client lost its configuration and re-registers an already present share, the server's method will throw an exception
          foreach (Guid serverShareId in serverShares.Keys)
            if (!localShares.ContainsKey(serverShareId))
              cd.RemoveShare(serverShareId);
          foreach (Share localShare in localShares.Values)
          {
            RelocationMode relocationMode;
            if (!serverShares.ContainsKey(localShare.ShareId))
            {
              cd.RegisterShare(localShare);
              newShares.Add(localShare);
            }
            else if (settings.CachedSharesUpdates.TryGetValue(localShare.ShareId, out relocationMode))
            {
              cd.UpdateShare(localShare.ShareId, localShare.BaseResourcePath, localShare.Name, localShare.MediaCategories,
                  relocationMode);
              switch (relocationMode)
              {
                case RelocationMode.ClearAndReImport:
                  importerWorker.ScheduleImport(localShare.BaseResourcePath, localShare.MediaCategories, true);
                  break;
                case RelocationMode.Relocate:
                  importerWorker.ScheduleRefresh(localShare.BaseResourcePath, localShare.MediaCategories, true);
                  break;
              }
            }
          }
          settings.CachedSharesUpdates.Clear();
          settingsManager.Save(settings);
        }
        catch (Exception e)
        {
          ServiceRegistration.Get<ILogger>().Warn("ServerConnectionManager: Could not synchronize local shares with server", e);
        }

        // Update media item aspect type registration
        try
        {
          IMediaItemAspectTypeRegistration miatr = ServiceRegistration.Get<IMediaItemAspectTypeRegistration>();
          ServiceRegistration.Get<ILogger>().Info("ServerConnectionManager: Checking for unregistered media item aspect types at home server");
          ICollection<Guid> serverMIATypes = cd.GetAllManagedMediaItemAspectTypes();
          foreach (KeyValuePair<Guid, MediaItemAspectMetadata> localMiaType in miatr.LocallyKnownMediaItemAspectTypes)
            if (!serverMIATypes.Contains(localMiaType.Key))
            {
              ServiceRegistration.Get<ILogger>().Info("ServerConnectionManager: Adding unregistered media item aspect type '{0}' (ID '{1}') at home server",
                  localMiaType.Value.Name, localMiaType.Key);
              cd.AddMediaItemAspectStorage(localMiaType.Value);
            }
        }
        catch (Exception e)
        {
          ServiceRegistration.Get<ILogger>().Warn("ServerConnectionManager: Could not synchronize local media item aspect types with server", e);
        }

        // Register state variables change events
        cd.PlaylistsChanged += OnContentDirectoryPlaylistsChanged;
        cd.MIATypeRegistrationsChanged += OnContentDirectoryMIATypeRegistrationsChanged;
        cd.RegisteredSharesChangeCounterChanged += OnRegisteredSharesChangeCounterChanged;
        cd.CurrentlyImportingSharesChanged += OnCurrentlyImportingSharesChanged;

        // Activate importer worker
        ServiceRegistration.Get<ILogger>().Debug("ServerConnectionManager: Activating importer worker");
        ImporterCallback ic = new ImporterCallback(cd);
        importerWorker.Activate(ic, ic);
        foreach (Share share in newShares)
          importerWorker.ScheduleImport(share.BaseResourcePath, share.MediaCategories, true);
      }
    }

    static void OnContentDirectoryPlaylistsChanged()
    {
      ContentDirectoryMessaging.SendPlaylistsChangedMessage();
    }

    static void OnContentDirectoryMIATypeRegistrationsChanged()
    {
      ContentDirectoryMessaging.SendMIATypesChangedMessage();
    }

    static void OnRegisteredSharesChangeCounterChanged()
    {
      ContentDirectoryMessaging.SendRegisteredSharesChangedMessage();
    }

    static void OnAttachedClientsChanged()
    {
      // Not implemented because this event isn't really interesting for the MP2 client yet
    }

    static void OnConnectedClientsChanged()
    {
      ServerConnectionMessaging.SendClientConnectionStateChangedMessage();
    }

    /// <summary>
    /// This method maintains the content directory's last importing shares state and generates the appropriate content directory messages.
    /// </summary>
    void UpdateCurrentlyImportingShares(ICollection<Guid> currentlyImportingShares)
    {
      if (currentlyImportingShares == null)
        currentlyImportingShares = new List<Guid>();
      ICollection<Guid> oldImportingShares = _currentlyImportingSharesProxy;
      ICollection<Guid> newImportingShares = currentlyImportingShares;
      _currentlyImportingSharesProxy = newImportingShares;
      foreach (Guid importingShare in
          newImportingShares.Where(importingShare => !oldImportingShares.Contains(importingShare)))
        ContentDirectoryMessaging.SendShareImportMessage(ContentDirectoryMessaging.MessageType.ShareImportStarted, importingShare);
      foreach (Guid oldShare in
          oldImportingShares.Where(oldShare => !newImportingShares.Contains(oldShare)))
        ContentDirectoryMessaging.SendShareImportMessage(ContentDirectoryMessaging.MessageType.ShareImportCompleted, oldShare);
    }

    void OnCurrentlyImportingSharesChanged()
    {
      ServiceRegistration.Get<IThreadPool>().Add(() =>
        {
          ICollection<Guid> currentlyImportingShares = null;
          try
          {
            IContentDirectory cd = ContentDirectory;
            currentlyImportingShares = cd == null ? null : cd.GetCurrentlyImportingShares();
          }
          catch (Exception)
          {
            ServiceRegistration.Get<ILogger>().Warn("ServerConnectionManager.OnCurrentlyImportingSharesChanged: Failed to update currently importing shares.");
          }
          UpdateCurrentlyImportingShares(currentlyImportingShares);
        });
    }

    #region IServerCommunicationManager implementation

    public ICollection<ServerDescriptor> AvailableServers
    {
      get
      {
        lock (_syncObj)
          if (_serverWatcher != null)
            return new List<ServerDescriptor>(_serverWatcher.AvailableServers);
        return null;
      }
    }

    public bool IsHomeServerConnected
    {
      get
      {
        lock (_syncObj)
          return _isHomeServerConnected;
      }
    }

    public string HomeServerSystemId
    {
      get
      {
        ServerConnectionSettings settings = ServiceRegistration.Get<ISettingsManager>().Load<ServerConnectionSettings>();
        return settings.HomeServerSystemId;
      }
    }

    public string LastHomeServerName
    {
      get
      {
        ServerConnectionSettings settings = ServiceRegistration.Get<ISettingsManager>().Load<ServerConnectionSettings>();
        return settings.LastHomeServerName;
      }
    }

    public SystemName LastHomeServerSystem
    {
      get
      {
        ServerConnectionSettings settings = ServiceRegistration.Get<ISettingsManager>().Load<ServerConnectionSettings>();
        return settings.LastHomeServerSystem;
      }
    }

    public IContentDirectory ContentDirectory
    {
      get { return ContentDirectoryServiceProxy; }
    }

    public IResourceInformationService ResourceInformationService
    {
      get
      {
        UPnPClientControlPoint cp;
        lock (_syncObj)
          cp = _controlPoint;
        return cp == null ? null : cp.ResourceInformationService;
      }
    }

    public IServerController ServerController
    {
      get
      {
        UPnPClientControlPoint cp;
        lock (_syncObj)
          cp = _controlPoint;
        return cp == null ? null : cp.ServerControllerService;
      }
    }

    public UPnPClientControlPoint ControlPoint
    {
      get { return _controlPoint; }
    }

    public void Startup()
    {
      UPnPServerWatcher watcher;
      UPnPClientControlPoint cp;
      lock (_syncObj)
      {
        watcher = _serverWatcher;
        cp = _controlPoint;
      }
      if (watcher != null)
        watcher.Start();
      if (cp != null)
        cp.Start();
    }

    public void Shutdown()
    {
      UPnPServerWatcher watcher;
      UPnPClientControlPoint cp;
      lock (_syncObj)
      {
        watcher = _serverWatcher;
        cp = _controlPoint;
      }
      if (watcher != null)
        watcher.Stop();
      if (cp != null)
        cp.Stop();
      _messageQueue.Shutdown();
    }

    public void DetachFromHomeServer()
    {
      ISettingsManager settingsManager = ServiceRegistration.Get<ISettingsManager>();
      ServerConnectionSettings settings = settingsManager.Load<ServerConnectionSettings>();
      ServiceRegistration.Get<ILogger>().Info("ServerConnectionManager: Detaching from home server '{0}'", settings.HomeServerSystemId);

      ServiceRegistration.Get<ILogger>().Debug("ServerConnectionManager: Clearing pending import jobs and suspending importer worker");
      IImporterWorker importerWorker = ServiceRegistration.Get<IImporterWorker>();
      importerWorker.Suspend();
      importerWorker.CancelPendingJobs();

      ServiceRegistration.Get<ILogger>().Debug("ServerConnectionManager: Notifying the MediaPortal server about the detachment");
      UPnPServerControllerServiceProxy sc = ServerControllerServiceProxy;
      ISystemResolver systemResolver = ServiceRegistration.Get<ISystemResolver>();
      if (sc != null)
        try
        {
          sc.DetachClient(systemResolver.LocalSystemId);
          sc.AttachedClientsChanged -= OnAttachedClientsChanged;
          sc.ConnectedClientsChanged -= OnConnectedClientsChanged;
        }
        catch (Exception e)
        {
          ServiceRegistration.Get<ILogger>().Warn("ServerConnectionManager: Error detaching from home server '{0}'", e, HomeServerSystemId);
        }
      UPnPContentDirectoryServiceProxy cd = ContentDirectoryServiceProxy;
      if (cd != null)
        try
        {
          cd.PlaylistsChanged -= OnContentDirectoryPlaylistsChanged;
          cd.MIATypeRegistrationsChanged -= OnContentDirectoryMIATypeRegistrationsChanged;
          cd.RegisteredSharesChangeCounterChanged -= OnRegisteredSharesChangeCounterChanged;
          cd.CurrentlyImportingSharesChanged -= OnCurrentlyImportingSharesChanged;
        }
        catch (Exception e)
        {
          ServiceRegistration.Get<ILogger>().Warn("ServerConnectionManager: Error unregistering from state variable change events", e);
        }

      ServiceRegistration.Get<ILogger>().Debug("ServerConnectionManager: Closing server connection");
      UPnPClientControlPoint cp;
      lock (_syncObj)
        cp = _controlPoint;
      if (cp != null)
        cp.Stop(); // Must be outside the lock - sends messages
      lock (_syncObj)
      {
        settings.HomeServerSystemId = null;
        settings.LastHomeServerName = null;
        settings.LastHomeServerSystem = null;
        settingsManager.Save(settings);
        _controlPoint = null;
      }
      UpdateCurrentlyImportingShares(null); // Mark all shares as not being imported
      ServerConnectionMessaging.SendServerConnectionStateChangedMessage(ServerConnectionMessaging.MessageType.HomeServerDetached);

      ServiceRegistration.Get<ILogger>().Debug("ServerConnectionManager: Starting to watch for MediaPortal servers");
      if (_serverWatcher == null)
      {
        lock (_syncObj)
          _serverWatcher = BuildServerWatcher();
        _serverWatcher.Start(); // Outside the lock
      }
    }

    public void SetNewHomeServer(string backendServerSystemId)
    {
      ServiceRegistration.Get<ILogger>().Info("ServerConnectionManager: Attaching to MediaPortal backend server '{0}'", backendServerSystemId);

      ServiceRegistration.Get<ILogger>().Debug("ServerConnectionManager: Stopping to watch for MediaPortal servers");
      lock (_syncObj)
        if (_serverWatcher != null)
        {
          _serverWatcher.Stop();
          _serverWatcher = null;
        }

      ServiceRegistration.Get<ILogger>().Debug("ServerConnectionManager: Building UPnP control point for communication with the new home server");
      UPnPClientControlPoint cp;
      lock (_syncObj)
        cp = _controlPoint;
      if (cp != null)
        cp.Stop(); // Must be outside the lock - sends messages
      lock (_syncObj)
      {
        ISettingsManager settingsManager = ServiceRegistration.Get<ISettingsManager>();
        ServerConnectionSettings settings = settingsManager.Load<ServerConnectionSettings>();
        // Here, we only set the system ID of the new home server. The server's system ID will remain in the settings
        // until method SetNewHomeServer is called again.
        settings.HomeServerSystemId = backendServerSystemId;
        settingsManager.Save(settings);
        _controlPoint = BuildClientControlPoint(backendServerSystemId);
      }
      _controlPoint.Start(); // Outside the lock
      ServerConnectionMessaging.SendServerConnectionStateChangedMessage(ServerConnectionMessaging.MessageType.HomeServerAttached);
    }

    #endregion
  }
}
