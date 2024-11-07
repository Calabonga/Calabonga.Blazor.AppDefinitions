# Calabonga.Blazor.AppDefinitions

## Описание

Модульный монолит на базе Blazor? Легко! Эта сборка позволяет выделить модули (Components) в отдельные сборки (в отдельные проекты), чтобы предоставить возможность разным командам разрабатывать разные модули одного приложения.

Помимо всего, эта сборка позволяет навести порядок в вашем `Program.cs`. Можно всё разложить "по полочкам". А еще можно реализовать систему плагинов. Чтобы воспользоваться сборкой надо просто установить nuget-пакет [Calabonga.Blazor.AppDefinitions](https://www.nuget.org/packages/Calabonga.Blazor.AppDefinitions/).

## Nuget-пакет

* [Calabonga.Blazor.AppDefinitions](https://www.nuget.org/packages/Calabonga.Blazor.AppDefinitions/) - nuget-пакет (этот репозиторий), который собой представляет набор контактов для использования в модулях и в основном приложении.

* [Calabonga.Blazor.AppDefinitions.Template](https://www.nuget.org/packages/Calabonga.Blazor.AppDefinitions.Template/) - nuget-пакет, установка которого добавит в список шаблонов новый шаблон `Calabonga.Blazor.Shell`. Этот шаблон создает проект Blazor Server приложения как основа для модульного монолита (Shell). Шаблон работает и для Visual Studio, и для JetBrains Rider, и для dotnet CLI.

## Description
 
Application Definitions for Blazor application. The small but very helpful package that can help you to organize modules in Blazor application.

## История версий

### Версия 1.0.0

* Первый релиз сборки.