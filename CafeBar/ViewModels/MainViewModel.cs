using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CafeBar.Commands;

namespace CafeBar.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private string _sectionTitle = "Заказы";
    private string _sectionDescription = "Здесь будет работа с заказами: создание, позиции и оплата.";

    public string SectionTitle
    {
        get => _sectionTitle;
        private set
        {
            if (_sectionTitle == value) return;
            _sectionTitle = value;
            OnPropertyChanged();
        }
    }

    public string SectionDescription
    {
        get => _sectionDescription;
        private set
        {
            if (_sectionDescription == value) return;
            _sectionDescription = value;
            OnPropertyChanged();
        }
    }

    public ICommand OpenSectionCommand { get; }

    public MainViewModel()
    {
        OpenSectionCommand = new RelayCommand(OpenSection);
    }

    private void OpenSection(object? parameter)
    {
        if (parameter is not string section) return;

        string? description = section switch
        {
            "Заказы" => "Здесь будет работа с заказами: создание, позиции и оплата.",
            "Меню" => "Здесь будут категории, позиции меню и модификаторы.",
            "Склад" => "Здесь будут остатки, поступления и списания ингредиентов.",
            "Техкарты" => "Здесь будет состав блюд и расчёт себестоимости.",
            "Отчёты" => "Здесь будут отчёты о продажах и остатках.",
            "Персонал" => "Здесь будут карточки сотрудников и учёт смен.",
            "Администрирование" => "Здесь будут настройки доступа и журнал действий.",
            _ => null
        };

        if (description is null) return;
        SectionTitle = section;
        SectionDescription = description;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
