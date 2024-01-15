using CommunityToolkit.Mvvm.Input;
using Onova;
using Onova.Services;
using System;
using System.Windows.Input;
using ReactiveUI;
namespace OnovaHyper1.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string greeting;
        private string upMg;
        public string Greeting
        {
            get => greeting;
            set => this.RaiseAndSetIfChanged(ref greeting, value);
        }
        public string UpMg
        {
            get => upMg;
            set => this.RaiseAndSetIfChanged(ref upMg, value);
        }
        public ICommand VerificarUpdate { get; }

        private readonly UpdateManager _updateManager;
        public MainWindowViewModel()
        {
            upMg = "OnovaHyperCube 1";
            greeting = "Aperte no botão";
            VerificarUpdate = new RelayCommand(InitializeAsync);
            _updateManager = new UpdateManager(
            new LocalPackageResolver("c:\\test\\packages", "hyper-v*.zip"),
            new ZipPackageExtractor());
        }
        private async void InitializeAsync()
        {
            try
            {
                var result = await _updateManager.CheckForUpdatesAsync();
                if (result.CanUpdate)
                {
                    Greeting = "Atualizando, espere...";
                    await _updateManager.PrepareUpdateAsync(result.LastVersion);
                    _updateManager.LaunchUpdater(result.LastVersion);
                    Environment.Exit(0);
                }
                else
                {
                    Greeting = "O programa já está atualizado!!";// Código para lidar com a ausência de atualizações
                }
            }
            catch (Exception ex)
            {
                // Código para lidar com qualquer exceção que possa ocorrer durante a verificação de atualização
            }
        }
    }
}
