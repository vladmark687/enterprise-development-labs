-- Скрипт создания таблиц для лабораторной работы
-- Статистическое управление района
-- PostgreSQL

-- Удаление таблиц если они существуют (в правильном порядке из-за внешних ключей)
DROP TABLE IF EXISTS "Supplies" CASCADE;
DROP TABLE IF EXISTS "Suppliers" CASCADE;
DROP TABLE IF EXISTS "Enterprises" CASCADE;

-- Создание таблицы Enterprises (Предприятия)
CREATE TABLE "Enterprises" (
    "RegistrationNumber" VARCHAR(50) PRIMARY KEY,
    "IndustryType" INTEGER NOT NULL,
    "Name" VARCHAR(200) NOT NULL,
    "Address" VARCHAR(300) NOT NULL,
    "Phone" VARCHAR(20),
    "OwnershipType" INTEGER NOT NULL,
    "EmployeeCount" INTEGER NOT NULL CHECK ("EmployeeCount" >= 0),
    "TotalArea" DECIMAL(18,2) NOT NULL CHECK ("TotalArea" >= 0)
);

-- Создание индексов для таблицы Enterprises
CREATE INDEX "IX_Enterprises_IndustryType" ON "Enterprises" ("IndustryType");
CREATE INDEX "IX_Enterprises_OwnershipType" ON "Enterprises" ("OwnershipType");
CREATE INDEX "IX_Enterprises_Name" ON "Enterprises" ("Name");

-- Создание таблицы Suppliers (Поставщики)
CREATE TABLE "Suppliers" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(200) NOT NULL,
    "Address" VARCHAR(300) NOT NULL,
    "Phone" VARCHAR(20)
);

-- Создание индексов для таблицы Suppliers
CREATE INDEX "IX_Suppliers_Name" ON "Suppliers" ("Name");

-- Создание таблицы Supplies (Поставки)
CREATE TABLE "Supplies" (
    "Id" SERIAL PRIMARY KEY,
    "EnterpriseRegistrationNumber" VARCHAR(50) NOT NULL,
    "SupplierId" INTEGER NOT NULL,
    "ProductName" VARCHAR(200) NOT NULL,
    "Quantity" INTEGER NOT NULL CHECK ("Quantity" > 0),
    "SupplyDate" DATE NOT NULL,
    "Cost" DECIMAL(18,2) NOT NULL CHECK ("Cost" > 0),
    
    -- Внешние ключи
    CONSTRAINT "FK_Supplies_Enterprises" 
        FOREIGN KEY ("EnterpriseRegistrationNumber") 
        REFERENCES "Enterprises"("RegistrationNumber") 
        ON DELETE CASCADE,
    
    CONSTRAINT "FK_Supplies_Suppliers" 
        FOREIGN KEY ("SupplierId") 
        REFERENCES "Suppliers"("Id") 
        ON DELETE CASCADE
);

-- Создание индексов для таблицы Supplies
CREATE INDEX "IX_Supplies_SupplyDate" ON "Supplies" ("SupplyDate");
CREATE INDEX "IX_Supplies_EnterpriseRegistrationNumber" ON "Supplies" ("EnterpriseRegistrationNumber");
CREATE INDEX "IX_Supplies_SupplierId" ON "Supplies" ("SupplierId");
CREATE INDEX "IX_Supplies_SupplyDate_SupplierId" ON "Supplies" ("SupplyDate", "SupplierId");

-- Комментарии к таблицам и столбцам
COMMENT ON TABLE "Enterprises" IS 'Таблица предприятий';
COMMENT ON COLUMN "Enterprises"."RegistrationNumber" IS 'Регистрационный номер предприятия (первичный ключ)';
COMMENT ON COLUMN "Enterprises"."IndustryType" IS 'Тип отрасли: 0-Сельское хозяйство, 1-Транспорт, 2-Легкая промышленность, 3-Тяжелая промышленность, 4-Строительство, 5-Материально-техническое снабжение';
COMMENT ON COLUMN "Enterprises"."Name" IS 'Наименование предприятия';
COMMENT ON COLUMN "Enterprises"."Address" IS 'Адрес предприятия';
COMMENT ON COLUMN "Enterprises"."Phone" IS 'Телефон предприятия';
COMMENT ON COLUMN "Enterprises"."OwnershipType" IS 'Форма собственности: 0-Государственно-федеральная, 1-Муниципально-городская, 2-ТОО, 3-Частная, 4-Акционерная';
COMMENT ON COLUMN "Enterprises"."EmployeeCount" IS 'Количество работающих';
COMMENT ON COLUMN "Enterprises"."TotalArea" IS 'Общая площадь в квадратных метрах';

COMMENT ON TABLE "Suppliers" IS 'Таблица поставщиков';
COMMENT ON COLUMN "Suppliers"."Id" IS 'Идентификатор поставщика (первичный ключ)';
COMMENT ON COLUMN "Suppliers"."Name" IS 'Наименование поставщика';
COMMENT ON COLUMN "Suppliers"."Address" IS 'Адрес поставщика';
COMMENT ON COLUMN "Suppliers"."Phone" IS 'Контактный телефон';

COMMENT ON TABLE "Supplies" IS 'Таблица поставок';
COMMENT ON COLUMN "Supplies"."Id" IS 'Идентификатор поставки (первичный ключ)';
COMMENT ON COLUMN "Supplies"."EnterpriseRegistrationNumber" IS 'Регистрационный номер предприятия-получателя (внешний ключ)';
COMMENT ON COLUMN "Supplies"."SupplierId" IS 'Идентификатор поставщика (внешний ключ)';
COMMENT ON COLUMN "Supplies"."ProductName" IS 'Наименование товара/сырья';
COMMENT ON COLUMN "Supplies"."Quantity" IS 'Количество единиц товара/сырья';
COMMENT ON COLUMN "Supplies"."SupplyDate" IS 'Дата поставки';
COMMENT ON COLUMN "Supplies"."Cost" IS 'Стоимость поставки';

SELECT 'Таблицы успешно созданы!' as message;