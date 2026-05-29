# KG.MES — Manufacturing Execution System

Система управления производством окон. Состоит из нескольких Blazor-приложений, общего API-сервера и общей библиотеки компонентов.

## 🏗️ Архитектура
KG.MES.Apps/
├── KG.MES.sln
├── KG.MES.Shared/ # Общие модели, DTO, сервисы, хелперы
├── KG.MES.UI.Shared/ # Общие Blazor-компоненты, CSS, JS
├── KG.MES.Main/ # Основное приложение (производство)
├── KG.MES.Masters/ # Приложение для мастеров
├── KG.MES.Supply/ # Приложение для снабжения
└── KG.MES.Commerce/ # Приложение для коммерческого отдела (в будущем)


## 🚀 Запуск приложений

Все команды выполняются из корневой папки `KG.MES.Apps`.

### Основное приложение (производство)
```bash
dotnet run --project KG.MES.Main
```
Открыть: http://localhost:5007

### Мастера
```bash
dotnet run --project KG.MES.Masters
```
Открыть: http://localhost:5148

### Снабжение
```bash
dotnet run --project KG.MES.Supply
```
Открыть: http://localhost:5162

### Отдел продаж
```bash
dotnet run --project KG.MES.Sales
```

### API-сервер (Node.js)
```bash
cd ../krg.mes.server.nodejs
npm run dev
```
API: http://192.168.0.179:3000/api

## ⚙️ Конфигурация
Каждое приложение имеет свой appsettings.json и Config/orderViewSettings.json. Общие настройки стилей в Config/BadgeStyles.json.

### Основные параметры orderViewSettings.json:
listEndpoint — эндпоинт для списка заказов

cardEndpoint — эндпоинт для карточки заказа

showActions — показывать кнопки действий

canEdit / canExport / canDelete — права

showTrace — показывать историю прохождения

showSupply — показывать снабжение

editSupply — разрешить редактирование снабжения

## 🔧 Сборка
```bash
dotnet build
```

## Очистка и пересборка:
```bash
dotnet clean
dotnet build
```

## 🧪 Технологии
.NET 9 / 10 (Blazor Server)

Bootstrap 5.3

Bootstrap Icons

Socket.IO (WebSocket)

Node.js API server

PostgreSQL

## 📦 Зависимости
Общие библиотеки автоматически подключаются через Project Reference:

```xml
<ProjectReference Include="..\KG.MES.Shared\KG.MES.Shared.csproj" />
<ProjectReference Include="..\KG.MES.UI.Shared\KG.MES.UI.Shared.csproj" />
```

## 📄 Лицензия
Внутренний проект компании. Распространение запрещено.