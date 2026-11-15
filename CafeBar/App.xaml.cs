using System.Windows;
using CafeBar.Data;

namespace CafeBar;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            AppDatabase.Initialize();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Не удалось открыть базу данных.\n\nФайл: {AppDatabase.FilePath}\n\n{ex.Message}",
                "Кафе-бар",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown(1);
            return;
        }

        new MainWindow().Show();
    }
}
