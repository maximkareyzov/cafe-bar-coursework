using System.Windows.Input;
using CafeBar.Commands;
using CafeBar.Modules.StaffManagement.Logic;
using CafeBar.Modules.StaffManagement.UI;
using CafeBar.Modules.MenuManagement.UI;
using CafeBar.Modules.Inventory.UI;

namespace CafeBar.ViewModels;

public class MainViewModel : ViewModelBase
{
    private string _sectionTitle = "Заказы";
    private string _sectionDescription = "Здесь будет работа с заказами: создание, позиции и оплата.";
    private object? _sectionContent;

    public string SectionTitle
    {
        get => _sectionTitle;
        private set => Set(ref _sectionTitle, value);
    }

    public string SectionDescription
    {
        get => _sectionDescription;
        private set => Set(ref _sectionDescription, value);
    }

    //Модель представления открытого раздела; null, пока раздел в разработке.
    public object? SectionContent
    {
        get => _sectionContent;
        private set
        {
            if (Set(ref _sectionContent, value)) OnPropertyChanged(nameof(IsSectionInDevelopment));
        }
    }

    public bool IsSectionInDevelopment => _sectionContent == null;

    //Сеанс работы: в окне показывается вошедший сотрудник.
    public Session Session => Session.Current;

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
            "Персонал" => "Карточки сотрудников.",
            "Администрирование" => "Здесь будут настройки доступа и журнал действий.",

            _ => null
        };

        if (description is null) return;
        if (!AccessPolicy.CanOpen(Session.Current.Employee?.Role, section)) return;
        SectionTitle = section;
        SectionDescription = description;
        SectionContent = section switch
        {
            "Персонал" => new EmployeesViewModel(),
            "Меню" => new MenuViewModel(),
            "Склад" => new IngredientsViewModel(),
            _ => null
        };
    }
}

