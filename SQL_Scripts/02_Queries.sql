-- Запросы для лабораторной работы
-- Статистическое управление района

-- ========================================
-- 1) Вывести все сведения о конкретном предприятии
-- ========================================

-- Запрос 1: Информация о предприятии с регистрационным номером 'E001'
SELECT 
    e."RegistrationNumber" as "Регистрационный номер",
    e."Name" as "Наименование",
    e."Address" as "Адрес",
    e."Phone" as "Телефон",
    CASE e."IndustryType"
        WHEN 0 THEN 'Сельское хозяйство'
        WHEN 1 THEN 'Транспорт'
        WHEN 2 THEN 'Легкая промышленность'
        WHEN 3 THEN 'Тяжелая промышленность'
        WHEN 4 THEN 'Строительство'
        WHEN 5 THEN 'Материально-техническое снабжение'
    END as "Тип отрасли",
    CASE e."OwnershipType"
        WHEN 0 THEN 'Государственно-федеральная'
        WHEN 1 THEN 'Муниципально-городская'
        WHEN 2 THEN 'ТОО'
        WHEN 3 THEN 'Частная'
        WHEN 4 THEN 'Акционерная'
    END as "Форма собственности",
    e."EmployeeCount" as "Количество работающих",
    e."TotalArea" as "Общая площадь"
FROM "Enterprises" e
WHERE e."RegistrationNumber" = 'E001';

-- ========================================
-- 2) Вывести всех поставщиков, поставивших сырье за заданный период, упорядочить по названию
-- ========================================

-- Запрос 2: Поставщики за период с 01.01.2024 по 28.02.2024
SELECT DISTINCT
    s."Id" as "ID поставщика",
    s."Name" as "Наименование поставщика",
    s."Address" as "Адрес",
    s."Phone" as "Телефон"
FROM "Suppliers" s
INNER JOIN "Supplies" sup ON s."Id" = sup."SupplierId"
WHERE sup."SupplyDate" BETWEEN '2024-01-01' AND '2024-02-28'
ORDER BY s."Name";

-- ========================================
-- 3) Вывести количество предприятий, с которыми работает каждый поставщик
-- ========================================

-- Запрос 3: Количество предприятий для каждого поставщика
SELECT 
    s."Id" as "ID поставщика",
    s."Name" as "Наименование поставщика",
    COUNT(DISTINCT sup."EnterpriseRegistrationNumber") as "Количество предприятий"
FROM "Suppliers" s
LEFT JOIN "Supplies" sup ON s."Id" = sup."SupplierId"
GROUP BY s."Id", s."Name"
ORDER BY "Количество предприятий" DESC, s."Name";

-- ========================================
-- 4) Вывести информацию о количестве поставщиков для каждого типа отрасли и форме собственности
-- ========================================

-- Запрос 4: Количество поставщиков по типу отрасли и форме собственности
SELECT 
    CASE e."IndustryType"
        WHEN 0 THEN 'Сельское хозяйство'
        WHEN 1 THEN 'Транспорт'
        WHEN 2 THEN 'Легкая промышленность'
        WHEN 3 THEN 'Тяжелая промышленность'
        WHEN 4 THEN 'Строительство'
        WHEN 5 THEN 'Материально-техническое снабжение'
    END as "Тип отрасли",
    CASE e."OwnershipType"
        WHEN 0 THEN 'Государственно-федеральная'
        WHEN 1 THEN 'Муниципально-городская'
        WHEN 2 THEN 'ТОО'
        WHEN 3 THEN 'Частная'
        WHEN 4 THEN 'Акционерная'
    END as "Форма собственности",
    COUNT(DISTINCT sup."SupplierId") as "Количество поставщиков"
FROM "Enterprises" e
LEFT JOIN "Supplies" sup ON e."RegistrationNumber" = sup."EnterpriseRegistrationNumber"
GROUP BY e."IndustryType", e."OwnershipType"
ORDER BY "Тип отрасли", "Форма собственности";

-- ========================================
-- 5) Вывести топ 5 предприятий по количеству поставок
-- ========================================

-- Запрос 5: Топ 5 предприятий по количеству поставок
SELECT 
    e."RegistrationNumber" as "Регистрационный номер",
    e."Name" as "Наименование предприятия",
    COUNT(sup."Id") as "Количество поставок"
FROM "Enterprises" e
LEFT JOIN "Supplies" sup ON e."RegistrationNumber" = sup."EnterpriseRegistrationNumber"
GROUP BY e."RegistrationNumber", e."Name"
ORDER BY "Количество поставок" DESC
LIMIT 5;

-- ========================================
-- 6) Вывести информацию о поставщиках, поставивших максимальное количество товара за указанный период
-- ========================================

-- Запрос 6: Поставщики с максимальным количеством товара за период январь-март 2024
WITH SupplierTotals AS (
    SELECT 
        s."Id",
        s."Name",
        s."Address",
        s."Phone",
        SUM(sup."Quantity") as total_quantity
    FROM "Suppliers" s
    INNER JOIN "Supplies" sup ON s."Id" = sup."SupplierId"
    WHERE sup."SupplyDate" BETWEEN '2024-01-01' AND '2024-03-31'
    GROUP BY s."Id", s."Name", s."Address", s."Phone"
),
MaxQuantity AS (
    SELECT MAX(total_quantity) as max_qty
    FROM SupplierTotals
)
SELECT 
    st."Id" as "ID поставщика",
    st."Name" as "Наименование поставщика",
    st."Address" as "Адрес",
    st."Phone" as "Телефон",
    st.total_quantity as "Общее количество товара"
FROM SupplierTotals st
CROSS JOIN MaxQuantity mq
WHERE st.total_quantity = mq.max_qty
ORDER BY st."Name";

-- ========================================
-- ДОПОЛНИТЕЛЬНЫЕ ЗАПРОСЫ ДЛЯ ДЕМОНСТРАЦИИ CRUD ОПЕРАЦИЙ
-- ========================================

-- Запрос добавления нового предприятия
-- INSERT INTO "Enterprises" ("RegistrationNumber", "IndustryType", "Name", "Address", "Phone", "OwnershipType", "EmployeeCount", "TotalArea") 
-- VALUES ('E013', 2, 'ООО НовТекс', 'г. Москва, ул. Новая, 1', '+7-495-999-88-77', 2, 80, 1500.0);

-- Запрос изменения данных предприятия по идентификатору
-- UPDATE "Enterprises" 
-- SET "Phone" = '+7-495-999-88-88', "EmployeeCount" = 85
-- WHERE "RegistrationNumber" = 'E013';

-- Запрос изменения данных по условию (увеличить количество сотрудников на 10% для предприятий легкой промышленности)
-- UPDATE "Enterprises" 
-- SET "EmployeeCount" = ROUND("EmployeeCount" * 1.1)
-- WHERE "IndustryType" = 2;

-- Запрос удаления конкретной записи
-- DELETE FROM "Enterprises" WHERE "RegistrationNumber" = 'E013';

-- Запрос удаления по условию (удалить предприятия с количеством сотрудников менее 30)
-- DELETE FROM "Enterprises" WHERE "EmployeeCount" < 30;

-- Запрос удаления всех записей из таблицы (ОСТОРОЖНО!)
-- DELETE FROM "Supplies";
-- DELETE FROM "Suppliers";
-- DELETE FROM "Enterprises";