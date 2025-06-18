# API Examples - Исправленные примеры

## Важные замечания по использованию API

### Enum значения:
- **IndustryType**: 0=Agriculture, 1=Transport, 2=LightIndustry, 3=HeavyIndustry, 4=Construction, 5=MaterialSupply
- **OwnershipType**: 0=StateFederal, 1=MunicipalCity, 2=LimitedLiabilityPartnership, 3=Private, 4=JointStock

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

### Обновление предприятия
```http
PUT /api/Enterprise/REG001
Content-Type: application/json

{
  "name": "Обновленное название",
  "employeeCount": 1600,
  "totalArea": 26000.0
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

### Обновление поставки
```http
PUT /api/Supply/1
Content-Type: application/json

{
  "productName": "Обновленный товар",
  "quantity": 150,
  "cost": 18000.00
}
```

### Удаление поставки
```http
DELETE /api/Supply/1
```

## Примеры правильных JSON для тестирования

### Предприятие (все обязательные поля)
```json
{
  "registrationNumber": "REG017",
  "name": "Тестовое предприятие",
  "industryType": 3,
  "ownershipType": 4,
  "employeeCount": 200,
  "address": "ул. Тестовая, 10",
  "phone": "+7-111-222-3333",
  "totalArea": 8000.5
}
```

### Поставщик (все обязательные поля)
```json
{
  "name": "Тестовый поставщик",
  "address": "ул. Поставщиков, 15",
  "phone": "+7-444-555-6666"
}
```

### Поставка (все обязательные поля)
```json
{
  "enterpriseRegistrationNumber": "REG001",
  "supplierId": 1,
  "productName": "Тестовый товар",
  "quantity": 50,
  "cost": 25000.00,
  "supplyDate": "2024-01-25T00:00:00"
}
```

## Частые ошибки и их решения

1. **Bad Request при создании предприятия**:
   - Убедитесь, что используете числовые значения для enum (industryType, ownershipType)
   - Удалите поле "district" - его нет в модели
   - Добавьте обязательные поля: totalArea

2. **Bad Request при создании поставщика**:
   - Удалите поля "industryType", "ownershipType", "contactPerson" - их нет в модели
   - Оставьте только: name, address, phone

3. **Bad Request при создании поставки**:
   - Убедитесь, что enterpriseRegistrationNumber существует в базе
   - Убедитесь, что supplierId существует в базе
   - Используйте правильный формат даты: "YYYY-MM-DDTHH:mm:ss"

## HTTP коды ответов
- 200 OK - Успешное выполнение
- 201 Created - Успешное создание
- 400 Bad Request - Неверный запрос
- 404 Not Found - Ресурс не найден
- 500 Internal Server Error - Внутренняя ошибка сервера