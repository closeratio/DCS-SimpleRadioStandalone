﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Ciribob.DCS.SimpleRadio.Standalone.Common;
using Ciribob.DCS.SimpleRadio.Standalone.Server.Network;
using Ciribob.DCS.SimpleRadio.Standalone.Server.UI.ClientAdmin;
using NLog;
using LogManager = NLog.LogManager;

namespace Ciribob.DCS.SimpleRadio.Standalone.Server.UI.MainWindow
{
    public sealed class MainViewModel : Screen, IHandle<ServerStateMessage>
    {
        private readonly ClientAdminViewModel _clientAdminViewModel;
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public MainViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            ClientAdminViewModel clientAdminViewModel)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _clientAdminViewModel = clientAdminViewModel;
            _eventAggregator.Subscribe(this);

            DisplayName = "DCS-SRS Server - " + UpdaterChecker.VERSION;

            Logger.Info("DCS-SRS Server Running - " + UpdaterChecker.VERSION);
        }

        public bool IsServerRunning { get; private set; } = true;

        public string ServerButtonText => IsServerRunning ? "Stop Server" : "Start Server";

        public int ClientsCount { get; private set; }

        public string RadioSecurityText
            =>
            ServerSettings.Instance.ServerSetting[(int) ServerSettingType.COALITION_AUDIO_SECURITY]
                ? "ON"
                : "OFF";

        public string SpectatorAudioText
            =>
            ServerSettings.Instance.ServerSetting[(int) ServerSettingType.SPECTATORS_AUDIO_DISABLED]
                ? "DISABLED"
                : "ENABLED";

        public string ExportListText
            =>
            ServerSettings.Instance.ServerSetting[(int) ServerSettingType.CLIENT_EXPORT_ENABLED]
                ? "ON"
                : "OFF";

        public string LOSText
            => ServerSettings.Instance.ServerSetting[(int) ServerSettingType.LOS_ENABLED] ? "ON" : "OFF";

        public string DistanceLimitText
            => ServerSettings.Instance.ServerSetting[(int) ServerSettingType.DISTANCE_ENABLED] ? "ON" : "OFF";

        public string RealRadioText
            => ServerSettings.Instance.ServerSetting[(int) ServerSettingType.IRL_RADIO_TX] ? "ON" : "OFF";

        public string IRLRadioRxText
            => ServerSettings.Instance.ServerSetting[(int) ServerSettingType.IRL_RADIO_RX_INTERFERENCE] ? "ON" : "OFF";

        public string RadioExpansion
            => ServerSettings.Instance.ServerSetting[(int) ServerSettingType.RADIO_EXPANSION] ? "ON" : "OFF";

        public string ListeningPort
            => ServerSettings.Instance.ServerListeningPort() + "";

        public void Handle(ServerStateMessage message)
        {
            IsServerRunning = message.IsRunning;
            ClientsCount = message.Count;
        }

        public void ServerStartStop()
        {
            if (IsServerRunning)
            {
                _eventAggregator.PublishOnBackgroundThread(new StopServerMessage());
            }
            else
            {
                _eventAggregator.PublishOnBackgroundThread(new StartServerMessage());
            }
        }

        public void ShowClientList()
        {
            IDictionary<string, object> settings = new Dictionary<string, object>
            {
                {"Icon", new BitmapImage(new Uri("pack://application:,,,/SR-Server;component/server-10.ico"))},
                {"ResizeMode", ResizeMode.CanMinimize}
            };
            _windowManager.ShowWindow(_clientAdminViewModel, null, settings);
        }

        public void RadioSecurityToggle()
        {
            var newSetting = RadioSecurityText != "ON";
            ServerSettings.Instance.WriteSetting(ServerSettingType.COALITION_AUDIO_SECURITY, newSetting);
            NotifyOfPropertyChange(() => RadioSecurityText);

            _eventAggregator.PublishOnBackgroundThread(new ServerSettingsChangedMessage());
        }

        public void SpectatorAudioToggle()
        {
            var newSetting = SpectatorAudioText != "DISABLED";
            ServerSettings.Instance.WriteSetting(ServerSettingType.SPECTATORS_AUDIO_DISABLED, newSetting);
            NotifyOfPropertyChange(() => SpectatorAudioText);

            _eventAggregator.PublishOnBackgroundThread(new ServerSettingsChangedMessage());
        }

        public void ExportListToggle()
        {
            var newSetting = ExportListText != "ON";
            ServerSettings.Instance.WriteSetting(ServerSettingType.CLIENT_EXPORT_ENABLED, newSetting);
            NotifyOfPropertyChange(() => ExportListText);

            _eventAggregator.PublishOnBackgroundThread(new ServerSettingsChangedMessage());
        }

        public void LOSToggle()
        {
            var newSetting = LOSText != "ON";
            ServerSettings.Instance.WriteSetting(ServerSettingType.LOS_ENABLED, newSetting);
            NotifyOfPropertyChange(() => LOSText);

            _eventAggregator.PublishOnBackgroundThread(new ServerSettingsChangedMessage());
        }

        public void DistanceLimitToggle()
        {
            var newSetting = DistanceLimitText != "ON";
            ServerSettings.Instance.WriteSetting(ServerSettingType.DISTANCE_ENABLED, newSetting);
            NotifyOfPropertyChange(() => DistanceLimitText);

            _eventAggregator.PublishOnBackgroundThread(new ServerSettingsChangedMessage());
        }

        public void RealRadioToggle()
        {
            var newSetting = RealRadioText != "ON";
            ServerSettings.Instance.WriteSetting(ServerSettingType.IRL_RADIO_TX, newSetting);
            NotifyOfPropertyChange(() => RealRadioText);

            _eventAggregator.PublishOnBackgroundThread(new ServerSettingsChangedMessage());
        }

        public void IRLRadioRxBehaviourToggle()
        {
            var newSetting = IRLRadioRxText != "ON";
            ServerSettings.Instance.WriteSetting(ServerSettingType.IRL_RADIO_RX_INTERFERENCE, newSetting);
            NotifyOfPropertyChange(() => IRLRadioRxText);

            _eventAggregator.PublishOnBackgroundThread(new ServerSettingsChangedMessage());
        }

        public void RadioExpansionToggle()
        {
            var newSetting = RadioExpansion != "ON";
            ServerSettings.Instance.WriteSetting(ServerSettingType.RADIO_EXPANSION, newSetting);
            NotifyOfPropertyChange(() => RadioExpansion);

            _eventAggregator.PublishOnBackgroundThread(new ServerSettingsChangedMessage());
        }
    }
}