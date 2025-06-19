# API Примеры для CRUD операций

## Инициализация данных

### Инициализация тестовых данных
```http
POST /api/DataInitialization/initialize
```
Ответ:
```json
{
  "message": "База данных успешно инициализирована",
  "enterprises_count": 15,
  "suppliers_count": 12,
  "supplies_count": 20,
  "total_records": 47
}
```

### Получение статистики
```http
GET /api/DataInitialization/statistics
```
Ответ:
```json
{
  "enterprises_count": 15,
  "suppliers_count": 12,
  "supplies_count": 20,
  "total_records": 47,
  "last_updated": "2024-01-15T10:30:00"
}
```

## Предприятия (Enterprises)

### Создание предприятия
```http
POST /api/Enterprise
Content-Type: application/json

{
  "registrationNumber": "REG016",
  "name": "Новое предприятие",
  "industryType": 2,
  "ownershipType": 3,
  "employeeCount": 150,
  "address": "ул. Новая, 1",
  "phone": "+7-123-456-7890",
  "totalArea": 5000.0
}
```

### Получение всех предприятий
```http
GET /api/Enterprise
```

### Получение предприятия по номеру регистрации
```http
GET /api/Enterprise/REG001
```

### Получение детальной информации о предприятии
```http
GET /api/Enterprise/REG001/details
```
Ответ:
```json
{
  "registrationNumber": "REG001",
  "name": "Металлургический завод",
  "industryType": "Металлургия",
  "ownershipType": "Государственная",
  "employeeCount": 1500,
  "address": "ул. Заводская, 1",
  "district": "Промышленный",
  "supplyCount": 2,
  "totalSupplyCost": 75000.00
}
```

### Обновление предприятия
```http
PUT /api/Enterprise/REG001
Content-Type: application/json

{
  "name": "Обновленное название",
  "employeeCount": 1600
}
```

### Удаление предприятия
```http
DELETE /api/Enterprise/REG001
```

## Поставщики (Suppliers)

### Создание поставщика
```http
POST /api/Supplier
Content-Type: application/json

{
  "name": "Новый поставщик",
  "address": "ул. Торговая, 5",
  "phone": "+7-123-456-7890"
}
```

### Получение всех поставщиков
```http
GET /api/Supplier
```

### Получение поставщика по ID
```http
GET /api/Supplier/1
```

### Получение детальной информации о поставщике
```http
GET /api/Supplier/1/details
```
Ответ:
```json
{
  "id": 1,
  "name": "МеталлСнаб",
  "industryType": "Металлургия",
  "ownershipType": "Частная",
  "address": "ул. Металлургов, 15",
  "contactPerson": "Петров П.П.",
  "phone": "+7-111-222-3333",
  "supplyCount": 3,
  "totalSupplyCost": 275000.00,
  "totalQuantitySupplied": 1700
}
```

### Обновление поставщика
```http
PUT /api/Supplier/1
Content-Type: application/json

{
  "name": "Обновленное название поставщика",
  "phone": "+7-999-888-7777"
}
```

### Удаление поставщика
```http
DELETE /api/Supplier/1
```

## Поставки (Supplies)

### Создание поставки
```http
POST /api/Supply
Content-Type: application/json

{
  "enterpriseRegistrationNumber": "REG001",
  "supplierId": 1,
  "productName": "Новый товар",
  "quantity": 100,
  "cost": 15000.00,
  "supplyDate": "2024-01-20T00:00:00"
}
```

### Получение всех поставок
```http
GET /api/Supply
```

### Получение поставки по ID
```http
GET /api/Supply/1
```

### Получение детальной информации о поставке
```http
GET /api/Supply/1/details
```
Ответ:
```json
{
  "id": 1,
  "enterpriseRegistrationNumber": "REG001",
  "supplierId": 1,
  "productName": "Сталь листовая",
  "quantity": 1000,
  "cost": 50000.00,
  "supplyDate": "2024-01-15T00:00:00",
  "enterpriseName": "Металлургический завод",
  "supplierName": "МеталлСнаб",
  "totalValue": 50000000.00
}
```

### Обновление поставки
```http
PUT /api/Supply/1
Content-Type: application/json

{
  "quantity": 1200,
  "cost": 60000.00
}
```

### Удаление поставки
```http
DELETE /api/Supply/1
```

## Массовые операции

### Удаление всех предприятий
```http
DELETE /api/CrudOperations/enterprises/all
```

### Удаление всех поставщиков
```http
DELETE /api/CrudOperations/suppliers/all
```

### Удаление всех поставок
```http
DELETE /api/CrudOperations/supplies/all
```

### Удаление предприятий по типу отрасли
```http
DELETE /api/CrudOperations/enterprises/by-industry/Металлургия
```

### Удаление поставок старше определенной даты
```http
DELETE /api/CrudOperations/supplies/older-than/2024-01-01
```

### Обновление количества сотрудников по типу отрасли
```http
PUT /api/CrudOperations/enterprises/update-employees
Content-Type: application/json

{
  "industryType": "Металлургия",
  "newEmployeeCount": 2000
}
```

### Обновление стоимости поставок в диапазоне дат
```http
PUT /api/CrudOperations/supplies/update-cost
Content-Type: application/json

{
  "startDate": "2024-01-01",
  "endDate": "2024-01-31",
  "multiplier": 1.1
}
```

### Получение примеров структур данных

#### Пример структуры предприятия
```http
GET /api/CrudOperations/examples/enterprise
```
Ответ:
```json
{
  "registrationNumber": "REG999",
  "name": "Пример предприятия",
  "industryType": "Производство",
  "ownershipType": "Частная",
  "employeeCount": 100,
  "address": "ул. Примерная, 1",
  "district": "Примерный район"
}
```

#### Пример структуры поставщика
```http
GET /api/CrudOperations/examples/supplier
```
Ответ:
```json
{
  "name": "Пример поставщика",
  "industryType": "Торговля",
  "ownershipType": "Частная",
  "address": "ул. Поставщиков, 1",
  "contactPerson": "Примеров П.П.",
  "phone": "+7-000-000-0000"
}
```

#### Пример структуры поставки
```http
GET /api/CrudOperations/examples/supply
```
Ответ:
```json
{
  "enterpriseRegistrationNumber": "REG001",
  "supplierId": 1,
  "productName": "Пример товара",
  "quantity": 100,
  "cost": 10000.00,
  "supplyDate": "2024-01-15T00:00:00"
}
```

## Очистка всех данных
```http
DELETE /api/DataInitialization/clear-all
```
Ответ:
```json
{
  "message": "Все данные успешно удалены"
}
```

## Коды ответов

- `200 OK` - Успешная операция
- `201 Created` - Ресурс создан
- `400 Bad Request` - Ошибка в запросе
- `404 Not Found` - Ресурс не найден
- `500 Internal Server Error` - Внутренняя ошибка сервера

## Примечания

1. Все даты должны быть в формате ISO 8601: `YYYY-MM-DDTHH:mm:ss`
2. Денежные значения передаются как числа с плавающей точкой
3. При создании поставки убедитесь, что предприятие и поставщик существуют
4. При удалении предприятия или поставщика связанные поставки также удаляются
5. Все строковые поля поддерживают кириллицу