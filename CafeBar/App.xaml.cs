using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using CafeBar.Data;
using CafeBar.Modules.StaffManagement.Logic;
using CafeBar.Modules.StaffManagement.UI;

namespace CafeBar;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Числа и даты в привязках показываются в формате системы (по умолчанию WPF берёт en-US).
        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

        // Окно входа и главное окно сменяют друг друга, поэтому приложение завершается только явно.
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

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

        StartSession();
    }

    /// <summary>Вход в систему и открытие главного окна. После кнопки «Выйти» выполняется заново.</summary>
    private void StartSession()
    {
        if (!new EmployeeService().HasEmployees())
        {
            // Первый запуск: сотрудников ещё нет, войти не под кем.
            var card = new EmployeeCardWindow(new EmployeeCardViewModel(null, firstAdministrator: true))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            if (card.ShowDialog() != true)
            {
                Shutdown();
                return;
            }
        }

        if (new LoginWindow().ShowDialog() != true)
        {
            Shutdown();
            return;
        }

        var window = new MainWindow();
        bool signOutRequested = false;
        window.SignOutRequested += (_, _) =>
        {
            signOutRequested = true;
            window.Close();
        };
        window.Closed += (_, _) =>
        {
            Session.Current.SignOut();
            if (signOutRequested) Dispatcher.BeginInvoke(new Action(StartSession));
            else Shutdown();
        };
        MainWindow = window;
        window.Show();
    }
}
