# OrderonTailoringCostCalculation

Приложение для расчёта стоимости заказа на индивидуальный пошив одежды.

Платформа: .NET MAUI  
Язык: C#  
Хранилище данных: SQLite

## Описание

OrderonTailoringCostCalculation — это desktop/mobile приложение, предназначенное для автоматизации расчёта стоимости пошива одежды по индивидуальным параметрам заказа. Программа позволяет учитывать:

- тип изделия/минимальную стоимость пошива;
- выбранную группу материалов;
- сложные элементы пошива;
- дополнительные наценки и скидки;
- итоговую стоимость заказа;
- сохранение и редактирование чеков/расчётов.

Приложение подходит для ателье, швейных мастерских и специалистов, которым необходимо быстро оценивать стоимость индивидуального пошива.

## Основные возможности

- создание и сохранение расчётов заказов;
- просмотр списка ранее созданных чеков;
- редактирование существующих расчётов;
- выбор основы изделия;
- выбор категории материалов;
- учёт сложных элементов (например: кокетки, карманы, застёжки, декоративные детали и т.д.);
- применение скидок и надбавок;
- автоматический расчёт общей стоимости;
- хранение данных в локальной SQLite-базе.

## Технологический стек

- .NET MAUI
- C#
- SQLite
- MVVM
- CommunityToolkit.Mvvm

## Структура проекта

```text
OrderonTailoringCostCalculation/
├── Models/                     # Модели данных
│   ├── Receipt.cs
│   ├── MaterialGroup.cs
│   ├── MinValueGarment.cs
│   ├── Discount.cs
│   ├── ComplicatedElement.cs
│   ├── ReceiptDiscount.cs
│   ├── ReceiptComplicatedElement.cs
│   └── ...
├── ViewModels/                 # ViewModel для экранов
│   ├── ReceiptsViewModel.cs
│   ├── ReceiptDetailsViewModel.cs
│   ├── MaterialGroupsViewModel.cs
│   ├── DiscountsViewModel.cs
│   ├── ComplicatedElementsViewModel.cs
│   └── ...
├── Views/                     # UI-экраны MAUI
│   ├── MainPage.xaml
│   ├── ReceiptPage.xaml
│   ├── MaterialGroupsPage.xaml
│   ├── DiscountsPage.xaml
│   ├── MinValueGarmentsPage.xaml
│   ├── ComplicatedElementsPage.xaml
│   └── ...
├── Services/                  # Сервисы и логика доступа к данным
│   ├── ReceiptService.cs
│   ├── MaterialGroupService.cs
│   ├── DiscountService.cs
│   ├── ComplicatedElementService.cs
│   ├── MauiDialogService.cs
│   ├── MauiNavigationService.cs
│   └── ...
├── App.xaml
├── AppShell.xaml
├── MauiProgram.cs
├── Constants.cs
├── OrderonTailoringCostCalculation.csproj
├── .gitignore
├── LICENSE
├── OrderonTailoringCostCalculation.slnx
└── OTCCData.db3              # база данных приложения
```

## Как работает приложение

Приложение использует локальную базу данных SQLite для хранения данных о:

- изделиях;
- материалах;
- усложняющих элементах;
- скидках;
- расчётных чеках.

На главном экране отображается список созданных расчётов. При создании или редактировании чека пользователь выбирает:

1. изделие/тип пошива;
2. группу материалов;
3. сложные элементы;
4. скидки/надбавки;
5. итоговую стоимость.

После этого приложение автоматически пересчитывает итоговую сумму и сохраняет результат.

## Требования

Для запуска проекта потребуется:

- .NET SDK 10
- workload MAUI
- Visual Studio 2022 или VS Code с поддержкой MAUI
- ОС: Windows / Android / iOS / macOS (в зависимости от целевой платформы)

## Установка и запуск

1. Клонируйте репозиторий:
   ```bash
   git clone https://github.com/NikitaD156/OrderonTailoringCostCalculation.git
   ```

2. Откройте проект в Visual Studio.

3. Убедитесь, что установлены необходимые workloads .NET MAUI.

4. Сборка проекта:
   ```bash
   dotnet restore
   dotnet build
   ```

5. Запустите приложение на выбранной платформе.

## Примечание по данным

Приложение использует локальный SQLite-файл `OTCCData.db3`, который сохраняется в папке данных приложения (`AppDataDirectory`). При первом запуске база автоматически инициализируется, если она ещё не существует.

## Лицензия

Этот проект распространяется под лицензией MIT. Подробности см. в файле [LICENSE](LICENSE).

## Автор

NikitaD156

## Краткая цель проекта

Проект создан для упрощения и автоматизации расчёта стоимости пошива одежды на заказ, чтобы сократить ручной подсчёт и уменьшить вероятность ошибок в коммерческих расчётах.
