# Лабораторная работа №3: Миграция на базу данных с использованием EF Core

## Описание предметной области

Статистическое управление района хранит информацию о предприятиях, их поставщиках и поставках. Система включает:

- **Предприятия** с информацией о регистрационном номере, типе отрасли, наименовании, адресе, телефоне, форме собственности, количестве работающих и общей площади
- **Поставщики** с данными о наименовании, адресе и контактной информации
- **Поставки** с информацией о товаре, количестве, дате поставки и стоимости

## Структура проекта

```
SGAY1/
├── Models/
│   ├── Enterprise.cs          # Модель предприятия
│   ├── Supplier.cs           # Модель поставщика
│   ├── Supply.cs             # Модель поставки
│   ├── IndustryType.cs       # Перечисление типов отраслей
│   └── OwnershipType.cs      # Перечисление форм собственности
├── Data/
│   ├── AppDbContext.cs       # Контекст базы данных
│   └── DistrictStatisticsRepository.cs  # Репозиторий для работы с данными
├── Controllers/
│   ├── AnalyticsController.cs        # Контроллер аналитики
│   ├── DataInitializationController.cs  # Контроллер инициализации данных
│   └── LabQueriesController.cs       # Контроллер запросов для лабораторной
├── SQL_Scripts/
│   ├── 01_InitializeData.sql # Скрипт инициализации данных
│   ├── 02_Queries.sql        # SQL запросы для лабораторной
│   └── 03_CreateTables.sql   # Скрипт создания таблиц
└── appsettings.json          # Настройки подключения к БД
```

## Модель данных

### Сущности и их атрибуты

#### 1. Enterprise (Предприятие)
- **RegistrationNumber** (VARCHAR(50), PK) - Регистрационный номер
- **IndustryType** (INTEGER) - Тип отрасли
- **Name** (VARCHAR(200)) - Наименование
- **Address** (VARCHAR(300)) - Адрес
- **Phone** (VARCHAR(20)) - Телефон
- **OwnershipType** (INTEGER) - Форма собственности
- **EmployeeCount** (INTEGER) - Количество работающих
- **TotalArea** (DECIMAL(18,2)) - Общая площадь

#### 2. Supplier (Поставщик)
- **Id** (SERIAL, PK) - Идентификатор
- **Name** (VARCHAR(200)) - Наименование
- **Address** (VARCHAR(300)) - Адрес
- **Phone** (VARCHAR(20)) - Телефон

#### 3. Supply (Поставка)
- **Id** (SERIAL, PK) - Идентификатор
- **EnterpriseRegistrationNumber** (VARCHAR(50), FK) - Ссылка на предприятие
- **SupplierId** (INTEGER, FK) - Ссылка на поставщика
- **ProductName** (VARCHAR(200)) - Наименование товара
- **Quantity** (INTEGER) - Количество
- **SupplyDate** (DATE) - Дата поставки
- **Cost** (DECIMAL(18,2)) - Стоимость

### Связи между сущностями
- **Enterprise ↔ Supply**: Один ко многим (одно предприятие может иметь много поставок)
- **Supplier ↔ Supply**: Один ко многим (один поставщик может делать много поставок)
- **Enterprise ↔ Supplier**: Многие ко многим (через таблицу Supply)

### Нормализация (3НФ)
Модель приведена к третьей нормальной форме:
- Все атрибуты атомарны (1НФ)
- Все неключевые атрибуты полностью зависят от первичного ключа (2НФ)
- Отсутствуют транзитивные зависимости (3НФ)

## Инструкция по выполнению

### Шаг 1: Настройка окружения

1. Убедитесь, что PostgreSQL установлен и запущен
2. Проверьте строку подключения в `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=DistrictStatistics;Username=postgres;Password=your_password"
  }
}
```

### Шаг 2: Создание базы данных

1. Выполните скрипт создания таблиц:
```bash
psql -U postgres -d DistrictStatistics -f SQL_Scripts/03_CreateTables.sql
```

2. Или используйте миграции EF Core:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Шаг 3: Инициализация данных

1. Выполните SQL скрипт:
```bash
psql -U postgres -d DistrictStatistics -f SQL_Scripts/01_InitializeData.sql
```

2. Или используйте API endpoint:
```bash
POST /api/DataInitialization/initialize
```

### Шаг 4: Запуск приложения

```bash
dotnet run
```

Приложение будет доступно по адресу: `https://localhost:7000` или `http://localhost:5000`

## API Endpoints для лабораторных запросов

### 1. Получение сведений о предприятии
```
GET /api/LabQueries/enterprise/{registrationNumber}
```

### 2. Поставщики за период
```
GET /api/LabQueries/suppliers-by-period?startDate=2024-01-01&endDate=2024-12-31
```

### 3. Количество предприятий по поставщикам
```
GET /api/LabQueries/enterprise-count-by-supplier
```

### 4. Количество поставщиков по отрасли и форме собственности
```
GET /api/LabQueries/supplier-count-by-industry-ownership
```

### 5. Топ 5 предприятий по количеству поставок
```
GET /api/LabQueries/top-enterprises-by-supply-count
```

### 6. Поставщики с максимальным количеством товара за период
```
GET /api/LabQueries/suppliers-max-quantity?startDate=2024-01-01&endDate=2024-12-31
```

### 7. Общая статистика предприятий
```
GET /api/LabQueries/enterprise-statistics
```

## SQL Запросы

Все необходимые SQL запросы находятся в файле `SQL_Scripts/02_Queries.sql`:

1. **Запросы SELECT** для получения данных согласно заданию
2. **Запросы INSERT** для добавления данных
3. **Запросы UPDATE** для изменения данных
4. **Запросы DELETE** для удаления данных

## CRUD операции

### Добавление данных
```sql
-- Добавление предприятия
INSERT INTO "Enterprises" ("RegistrationNumber", "IndustryType", "Name", "Address", "Phone", "OwnershipType", "EmployeeCount", "TotalArea")
VALUES ('REG001', 0, 'Агрофирма Заря', 'ул. Полевая, 15', '+7-123-456-7890', 0, 150, 5000.00);
```

### Изменение данных
```sql
-- Изменение по идентификатору
UPDATE "Enterprises" 
SET "EmployeeCount" = 200 
WHERE "RegistrationNumber" = 'REG001';

-- Изменение по условию
UPDATE "Enterprises" 
SET "EmployeeCount" = "EmployeeCount" + 10 
WHERE "IndustryType" = 0;
```

### Удаление данных
```sql
-- Удаление по идентификатору
DELETE FROM "Enterprises" WHERE "RegistrationNumber" = 'REG001';

-- Удаление по условию
DELETE FROM "Supplies" WHERE "SupplyDate" < '2024-01-01';

-- Удаление всех записей
DELETE FROM "Supplies";
```

## Проверка выполнения

1. **Модель данных**: Проверьте файлы в папке `Models/`
2. **База данных**: Убедитесь, что таблицы созданы и заполнены данными
3. **API**: Протестируйте все endpoints через Swagger UI (`/swagger`)
4. **SQL запросы**: Выполните запросы из файла `02_Queries.sql`

## Технические детали

- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core
- **База данных**: PostgreSQL
- **Архитектура**: Repository Pattern
- **API**: RESTful Web API
- **Документация**: Swagger/OpenAPI

## Результаты лабораторной работы

✅ Созданы модели сущностей с аннотациями EF Core  
✅ Настроен контекст базы данных с конфигурацией связей  
✅ Реализован репозиторий для работы с данными  
✅ Созданы API контроллеры для всех требуемых запросов  
✅ Подготовлены SQL скрипты для создания таблиц и инициализации данных  
✅ Реализованы все CRUD операции  
✅ Выполнены все запросы согласно варианту задания  
✅ Модель приведена к 3НФ  
✅ Настроены индексы для оптимизации производительности  

## Дополнительные возможности

- Автоматическое применение миграций при запуске
- Валидация данных на уровне модели
- Обработка ошибок и исключений
- Логирование операций
- Swagger документация API
- Оптимизация запросов с помощью индексов