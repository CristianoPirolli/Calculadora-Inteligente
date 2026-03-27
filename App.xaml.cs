using System.Windows;
using CalculadoraInteligente.Application.Updates;
using CalculadoraInteligente.UI;

namespace CalculadoraInteligente;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = new MainWindow();
        MainWindow = mainWindow;
        mainWindow.Show();

        _ = CheckUpdatesAsync(mainWindow);
    }

    private async Task CheckUpdatesAsync(Window owner)
    {
        try
        {
            await AutoUpdater.TryUpdateAsync(owner);
        }
        catch
        {
            // Falha de atualização não pode impedir o uso da aplicação.
        }
    }
}
