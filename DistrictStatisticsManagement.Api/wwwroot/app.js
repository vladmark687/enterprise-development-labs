// Конфигурация API
const API_BASE_URL = '/api';

// Утилиты для работы с API
class ApiClient {
    static async get(endpoint) {
        try {
            const response = await fetch(`${API_BASE_URL}${endpoint}`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return await response.json();
        } catch (error) {
            console.error('GET request failed:', error);
            throw error;
        }
    }

    static async post(endpoint, data) {
        try {
            const response = await fetch(`${API_BASE_URL}${endpoint}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data)
            });
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return await response.json();
        } catch (error) {
            console.error('POST request failed:', error);
            throw error;
        }
    }

    static async put(endpoint, data) {
        try {
            const response = await fetch(`${API_BASE_URL}${endpoint}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data)
            });
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.status === 204 ? null : await response.json();
        } catch (error) {
            console.error('PUT request failed:', error);
            throw error;
        }
    }

    static async delete(endpoint) {
        try {
            const response = await fetch(`${API_BASE_URL}${endpoint}`, {
                method: 'DELETE'
            });
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.status === 204 ? null : await response.json();
        } catch (error) {
            console.error('DELETE request failed:', error);
            throw error;
        }
    }
}

// Утилиты для UI
class UIUtils {
    static showAlert(message, type = 'info') {
        const alertDiv = document.createElement('div');
        alertDiv.className = `alert alert-${type} alert-dismissible fade show`;
        alertDiv.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;
        
        const container = document.querySelector('.container');
        container.insertBefore(alertDiv, container.firstChild);
        
        setTimeout(() => {
            alertDiv.remove();
        }, 5000);
    }

    static setLoading(element, isLoading) {
        if (isLoading) {
            element.disabled = true;
            element.innerHTML = '<span class="loading"></span> Загрузка...';
        } else {
            element.disabled = false;
        }
    }

    static formatDate(dateString) {
        return new Date(dateString).toLocaleDateString('ru-RU');
    }

    static formatCurrency(amount) {
        return new Intl.NumberFormat('ru-RU', {
            style: 'currency',
            currency: 'RUB'
        }).format(amount);
    }
}

// Перечисления
const IndustryTypes = {
    0: 'Сельское хозяйство',
    1: 'Транспорт',
    2: 'Легкая промышленность',
    3: 'Тяжелая промышленность',
    4: 'Строительство',
    5: 'Материально-техническое снабжение'
};

const OwnershipTypes = {
    0: 'Государственно-федеральная',
    1: 'Муниципально-городская',
    2: 'ТОО',
    3: 'Частная',
    4: 'Акционерная'
};

// Управление предприятиями
class EnterpriseManager {
    static async loadAll() {
        try {
            const enterprises = await ApiClient.get('/Enterprise');
            this.renderTable(enterprises);
        } catch (error) {
            UIUtils.showAlert('Ошибка загрузки предприятий: ' + error.message, 'danger');
        }
    }

    static renderTable(enterprises) {
        const tbody = document.getElementById('enterprisesTable');
        
        if (enterprises.length === 0) {
            tbody.innerHTML = `
                <tr>
                    <td colspan="5" class="text-center text-muted py-4">
                        <i class="fas fa-building fa-3x mb-3 d-block"></i>
                        <h5>Нет данных</h5>
                        <p>Предприятия не найдены</p>
                    </td>
                </tr>
            `;
            return;
        }

        tbody.innerHTML = enterprises.map(enterprise => `
            <tr>
                <td><strong>${enterprise.registrationNumber}</strong></td>
                <td>${enterprise.name}</td>
                <td><span class="badge bg-primary">${IndustryTypes[enterprise.industryType]}</span></td>
                <td><span class="badge bg-secondary">${OwnershipTypes[enterprise.ownershipType]}</span></td>
                <td>
                    <button class="btn btn-sm btn-outline-primary me-1" onclick="EnterpriseManager.viewDetails('${enterprise.registrationNumber}')">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-warning" onclick="EnterpriseManager.edit('${enterprise.registrationNumber}')">
                        <i class="fas fa-edit"></i>
                    </button>
                </td>
            </tr>
        `).join('');
    }

    static async create(formData) {
        try {
            const enterprise = {
                registrationNumber: formData.get('regNumber'),
                name: formData.get('enterpriseName'),
                industryType: parseInt(formData.get('industryType')),
                address: formData.get('enterpriseAddress'),
                phone: formData.get('enterprisePhone'),
                ownershipType: parseInt(formData.get('ownershipType')),
                employeeCount: parseInt(formData.get('employeeCount')),
                totalArea: parseFloat(formData.get('totalArea'))
            };

            await ApiClient.post('/Enterprise', enterprise);
            UIUtils.showAlert('Предприятие успешно создано!', 'success');
            document.getElementById('enterpriseForm').reset();
            this.loadAll();
        } catch (error) {
            UIUtils.showAlert('Ошибка создания предприятия: ' + error.message, 'danger');
        }
    }

    static async viewDetails(registrationNumber) {
        try {
            const enterprise = await ApiClient.get(`/Enterprise/${registrationNumber}/details`);
            
            const modalBody = document.getElementById('editModalBody');
            modalBody.innerHTML = `
                <div class="data-card">
                    <h6><i class="fas fa-building me-2"></i>Информация о предприятии</h6>
                    <p><strong>Регистрационный номер:</strong> ${enterprise.registrationNumber}</p>
                    <p><strong>Наименование:</strong> ${enterprise.name}</p>
                    <p><strong>Тип отрасли:</strong> ${enterprise.industryType}</p>
                    <p><strong>Адрес:</strong> ${enterprise.address}</p>
                    <p><strong>Телефон:</strong> ${enterprise.phone}</p>
                    <p><strong>Форма собственности:</strong> ${enterprise.ownershipType}</p>
                    <p><strong>Количество работающих:</strong> ${enterprise.employeeCount}</p>
                    <p><strong>Общая площадь:</strong> ${enterprise.totalArea} м²</p>
                    <p><strong>Количество поставок:</strong> ${enterprise.suppliesCount}</p>
                    <p><strong>Общая стоимость поставок:</strong> ${UIUtils.formatCurrency(enterprise.totalSupplyCost)}</p>
                </div>
            `;
            
            document.getElementById('editModalTitle').textContent = 'Детали предприятия';
            new bootstrap.Modal(document.getElementById('editModal')).show();
        } catch (error) {
            UIUtils.showAlert('Ошибка загрузки деталей предприятия: ' + error.message, 'danger');
        }
    }

    static async edit(registrationNumber) {
        try {
            const enterprise = await ApiClient.get(`/Enterprise/${registrationNumber}`);
            
            const modalBody = document.getElementById('editModalBody');
            modalBody.innerHTML = `
                <form id="editEnterpriseForm">
                    <div class="mb-3">
                        <label class="form-label">Регистрационный номер</label>
                        <input type="text" class="form-control" name="registrationNumber" value="${enterprise.registrationNumber}" readonly>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Наименование *</label>
                        <input type="text" class="form-control" name="name" value="${enterprise.name}" required>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Тип отрасли *</label>
                        <select class="form-select" name="industryType" required>
                            ${Object.entries(IndustryTypes).map(([key, value]) => 
                                `<option value="${key}" ${enterprise.industryType == key ? 'selected' : ''}>${value}</option>`
                            ).join('')}
                        </select>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Адрес *</label>
                        <input type="text" class="form-control" name="address" value="${enterprise.address}" required>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Телефон</label>
                        <input type="tel" class="form-control" name="phone" value="${enterprise.phone || ''}">
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Форма собственности *</label>
                        <select class="form-select" name="ownershipType" required>
                            ${Object.entries(OwnershipTypes).map(([key, value]) => 
                                `<option value="${key}" ${enterprise.ownershipType == key ? 'selected' : ''}>${value}</option>`
                            ).join('')}
                        </select>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Количество работающих *</label>
                        <input type="number" class="form-control" name="employeeCount" value="${enterprise.employeeCount}" min="1" required>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Общая площадь (м²) *</label>
                        <input type="number" class="form-control" name="totalArea" value="${enterprise.totalArea}" step="0.01" min="0" required>
                    </div>
                    <div class="d-flex justify-content-between">
                        <button type="button" class="btn btn-danger" onclick="EnterpriseManager.delete('${registrationNumber}')">
                            <i class="fas fa-trash"></i> Удалить
                        </button>
                        <div>
                            <button type="button" class="btn btn-secondary me-2" data-bs-dismiss="modal">Отмена</button>
                            <button type="submit" class="btn btn-primary">
                                <i class="fas fa-save"></i> Сохранить
                            </button>
                        </div>
                    </div>
                </form>
            `;
            
            document.getElementById('editModalTitle').textContent = 'Редактирование предприятия';
            const modal = new bootstrap.Modal(document.getElementById('editModal'));
            modal.show();
            
            // Обработчик формы редактирования
            document.getElementById('editEnterpriseForm').addEventListener('submit', async function(e) {
                e.preventDefault();
                const formData = new FormData(this);
                await EnterpriseManager.update(registrationNumber, formData);
                modal.hide();
            });
        } catch (error) {
            UIUtils.showAlert('Ошибка загрузки данных предприятия: ' + error.message, 'danger');
        }
    }

    static async update(registrationNumber, formData) {
        try {
            const enterprise = {
                registrationNumber: formData.get('registrationNumber'),
                name: formData.get('name'),
                industryType: parseInt(formData.get('industryType')),
                address: formData.get('address'),
                phone: formData.get('phone'),
                ownershipType: parseInt(formData.get('ownershipType')),
                employeeCount: parseInt(formData.get('employeeCount')),
                totalArea: parseFloat(formData.get('totalArea'))
            };

            await ApiClient.put(`/Enterprise/${registrationNumber}`, enterprise);
            UIUtils.showAlert('Предприятие успешно обновлено!', 'success');
            this.loadAll();
        } catch (error) {
            UIUtils.showAlert('Ошибка обновления предприятия: ' + error.message, 'danger');
        }
    }

    static async delete(registrationNumber) {
        if (!confirm('Вы уверены, что хотите удалить это предприятие? Это действие нельзя отменить.')) {
            return;
        }

        try {
            await ApiClient.delete(`/Enterprise/${registrationNumber}`);
            UIUtils.showAlert('Предприятие успешно удалено!', 'success');
            this.loadAll();
            bootstrap.Modal.getInstance(document.getElementById('editModal')).hide();
        } catch (error) {
            UIUtils.showAlert('Ошибка удаления предприятия: ' + error.message, 'danger');
        }
    }
}

// Управление поставщиками
class SupplierManager {
    static async loadAll() {
        try {
            const suppliers = await ApiClient.get('/Supplier');
            this.renderTable(suppliers);
            this.populateSelects(suppliers);
        } catch (error) {
            UIUtils.showAlert('Ошибка загрузки поставщиков: ' + error.message, 'danger');
        }
    }

    static renderTable(suppliers) {
        const tbody = document.getElementById('suppliersTable');
        
        if (suppliers.length === 0) {
            tbody.innerHTML = `
                <tr>
                    <td colspan="5" class="text-center text-muted py-4">
                        <i class="fas fa-truck fa-3x mb-3 d-block"></i>
                        <h5>Нет данных</h5>
                        <p>Поставщики не найдены</p>
                    </td>
                </tr>
            `;
            return;
        }

        tbody.innerHTML = suppliers.map(supplier => `
            <tr>
                <td><strong>${supplier.id}</strong></td>
                <td>${supplier.name}</td>
                <td>${supplier.address}</td>
                <td>${supplier.phone || '-'}</td>
                <td>
                    <button class="btn btn-sm btn-outline-primary me-1" onclick="SupplierManager.viewDetails(${supplier.id})">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-warning" onclick="SupplierManager.edit(${supplier.id})">
                        <i class="fas fa-edit"></i>
                    </button>
                </td>
            </tr>
        `).join('');
    }

    static populateSelects(suppliers) {
        const select = document.getElementById('supplierSelect');
        select.innerHTML = '<option value="">Выберите поставщика...</option>' +
            suppliers.map(supplier => `<option value="${supplier.id}">${supplier.name}</option>`).join('');
    }

    static async create(formData) {
        try {
            const supplier = {
                name: formData.get('supplierName'),
                address: formData.get('supplierAddress'),
                phone: formData.get('supplierPhone')
            };

            await ApiClient.post('/Supplier', supplier);
            UIUtils.showAlert('Поставщик успешно создан!', 'success');
            document.getElementById('supplierForm').reset();
            this.loadAll();
        } catch (error) {
            UIUtils.showAlert('Ошибка создания поставщика: ' + error.message, 'danger');
        }
    }

    static async viewDetails(id) {
        try {
            const supplier = await ApiClient.get(`/Supplier/${id}/details`);
            
            const modalBody = document.getElementById('editModalBody');
            modalBody.innerHTML = `
                <div class="data-card">
                    <h6><i class="fas fa-truck me-2"></i>Информация о поставщике</h6>
                    <p><strong>ID:</strong> ${supplier.id}</p>
                    <p><strong>Наименование:</strong> ${supplier.name}</p>
                    <p><strong>Адрес:</strong> ${supplier.address}</p>
                    <p><strong>Телефон:</strong> ${supplier.phone || 'Не указан'}</p>
                    <p><strong>Количество поставок:</strong> ${supplier.suppliesCount}</p>
                    <p><strong>Общая стоимость поставок:</strong> ${UIUtils.formatCurrency(supplier.totalSupplyCost)}</p>
                </div>
            `;
            
            document.getElementById('editModalTitle').textContent = 'Детали поставщика';
            new bootstrap.Modal(document.getElementById('editModal')).show();
        } catch (error) {
            UIUtils.showAlert('Ошибка загрузки деталей поставщика: ' + error.message, 'danger');
        }
    }

    static async edit(id) {
        try {
            const supplier = await ApiClient.get(`/Supplier/${id}`);
            
            const modalBody = document.getElementById('editModalBody');
            modalBody.innerHTML = `
                <form id="editSupplierForm">
                    <div class="mb-3">
                        <label class="form-label">ID</label>
                        <input type="text" class="form-control" value="${supplier.id}" readonly>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Наименование *</label>
                        <input type="text" class="form-control" name="name" value="${supplier.name}" required>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Адрес *</label>
                        <input type="text" class="form-control" name="address" value="${supplier.address}" required>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Телефон</label>
                        <input type="tel" class="form-control" name="phone" value="${supplier.phone || ''}">
                    </div>
                    <div class="d-flex justify-content-between">
                        <button type="button" class="btn btn-danger" onclick="SupplierManager.delete(${id})">
                            <i class="fas fa-trash"></i> Удалить
                        </button>
                        <div>
                            <button type="button" class="btn btn-secondary me-2" data-bs-dismiss="modal">Отмена</button>
                            <button type="submit" class="btn btn-primary">
                                <i class="fas fa-save"></i> Сохранить
                            </button>
                        </div>
                    </div>
                </form>
            `;
            
            document.getElementById('editModalTitle').textContent = 'Редактирование поставщика';
            const modal = new bootstrap.Modal(document.getElementById('editModal'));
            modal.show();
            
            // Обработчик формы редактирования
            document.getElementById('editSupplierForm').addEventListener('submit', async function(e) {
                e.preventDefault();
                const formData = new FormData(this);
                await SupplierManager.update(id, formData);
                modal.hide();
            });
        } catch (error) {
            UIUtils.showAlert('Ошибка загрузки данных поставщика: ' + error.message, 'danger');
        }
    }

    static async update(id, formData) {
        try {
            const supplier = {
                id: id,
                name: formData.get('name'),
                address: formData.get('address'),
                phone: formData.get('phone')
            };

            await ApiClient.put(`/Supplier/${id}`, supplier);
            UIUtils.showAlert('Поставщик успешно обновлен!', 'success');
            this.loadAll();
        } catch (error) {
            UIUtils.showAlert('Ошибка обновления поставщика: ' + error.message, 'danger');
        }
    }

    static async delete(id) {
        if (!confirm('Вы уверены, что хотите удалить этого поставщика? Это действие нельзя отменить.')) {
            return;
        }

        try {
            await ApiClient.delete(`/Supplier/${id}`);
            UIUtils.showAlert('Поставщик успешно удален!', 'success');
            this.loadAll();
            bootstrap.Modal.getInstance(document.getElementById('editModal')).hide();
        } catch (error) {
            UIUtils.showAlert('Ошибка удаления поставщика: ' + error.message, 'danger');
        }
    }
}

// Управление поставками
class SupplyManager {
    static async loadAll() {
        try {
            const supplies = await ApiClient.get('/Supply');
            this.renderTable(supplies);
        } catch (error) {
            UIUtils.showAlert('Ошибка загрузки поставок: ' + error.message, 'danger');
        }
    }

    static renderTable(supplies) {
        const tbody = document.getElementById('suppliesTable');
        
        if (supplies.length === 0) {
            tbody.innerHTML = `
                <tr>
                    <td colspan="8" class="text-center text-muted py-4">
                        <i class="fas fa-boxes fa-3x mb-3 d-block"></i>
                        <h5>Нет данных</h5>
                        <p>Поставки не найдены</p>
                    </td>
                </tr>
            `;
            return;
        }

        tbody.innerHTML = supplies.map(supply => `
            <tr>
                <td><strong>${supply.id}</strong></td>
                <td>${supply.enterprise?.name || supply.enterpriseRegistrationNumber}</td>
                <td>${supply.supplier?.name || supply.supplierId}</td>
                <td>${supply.productName}</td>
                <td>${supply.quantity}</td>
                <td>${UIUtils.formatCurrency(supply.cost)}</td>
                <td>${UIUtils.formatDate(supply.supplyDate)}</td>
                <td>
                    <button class="btn btn-sm btn-outline-primary me-1" onclick="SupplyManager.viewDetails(${supply.id})">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-warning" onclick="SupplyManager.edit(${supply.id})">
                        <i class="fas fa-edit"></i>
                    </button>
                </td>
            </tr>
        `).join('');
    }

    static async create(formData) {
        try {
            const supply = {
                enterpriseRegistrationNumber: formData.get('enterpriseSelect'),
                supplierId: parseInt(formData.get('supplierSelect')),
                productName: formData.get('productName'),
                quantity: parseInt(formData.get('quantity')),
                cost: parseFloat(formData.get('cost')),
                supplyDate: formData.get('supplyDate')
            };

            await ApiClient.post('/Supply', supply);
            UIUtils.showAlert('Поставка успешно создана!', 'success');
            document.getElementById('supplyForm').reset();
            this.loadAll();
        } catch (error) {
            UIUtils.showAlert('Ошибка создания поставки: ' + error.message, 'danger');
        }
    }

    static async viewDetails(id) {
        try {
            const supply = await ApiClient.get(`/Supply/${id}/details`);
            
            const modalBody = document.getElementById('editModalBody');
            modalBody.innerHTML = `
                <div class="data-card">
                    <h6><i class="fas fa-boxes me-2"></i>Информация о поставке</h6>
                    <p><strong>ID:</strong> ${supply.id}</p>
                    <p><strong>Предприятие:</strong> ${supply.enterpriseName} (${supply.enterpriseRegistrationNumber})</p>
                    <p><strong>Поставщик:</strong> ${supply.supplierName} (ID: ${supply.supplierId})</p>
                    <p><strong>Товар:</strong> ${supply.productName}</p>
                    <p><strong>Количество:</strong> ${supply.quantity}</p>
                    <p><strong>Стоимость за единицу:</strong> ${UIUtils.formatCurrency(supply.cost)}</p>
                    <p><strong>Общая стоимость:</strong> ${UIUtils.formatCurrency(supply.totalValue)}</p>
                    <p><strong>Дата поставки:</strong> ${UIUtils.formatDate(supply.supplyDate)}</p>
                </div>
            `;
            
            document.getElementById('editModalTitle').textContent = 'Детали поставки';
            new bootstrap.Modal(document.getElementById('editModal')).show();
        } catch (error) {
            UIUtils.showAlert('Ошибка загрузки деталей поставки: ' + error.message, 'danger');
        }
    }

    static async edit(id) {
        try {
            const supply = await ApiClient.get(`/Supply/${id}`);
            const enterprises = await ApiClient.get('/Enterprise');
            const suppliers = await ApiClient.get('/Supplier');
            
            const modalBody = document.getElementById('editModalBody');
            modalBody.innerHTML = `
                <form id="editSupplyForm">
                    <div class="mb-3">
                        <label class="form-label">ID</label>
                        <input type="text" class="form-control" value="${supply.id}" readonly>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Предприятие *</label>
                        <select class="form-select" name="enterpriseRegistrationNumber" required>
                            <option value="">Выберите предприятие</option>
                            ${enterprises.map(e => `<option value="${e.registrationNumber}" ${e.registrationNumber === supply.enterpriseRegistrationNumber ? 'selected' : ''}>${e.name}</option>`).join('')}
                        </select>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Поставщик *</label>
                        <select class="form-select" name="supplierId" required>
                            <option value="">Выберите поставщика</option>
                            ${suppliers.map(s => `<option value="${s.id}" ${s.id === supply.supplierId ? 'selected' : ''}>${s.name}</option>`).join('')}
                        </select>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Наименование товара *</label>
                        <input type="text" class="form-control" name="productName" value="${supply.productName}" required>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Дата поставки *</label>
                        <input type="date" class="form-control" name="supplyDate" value="${supply.supplyDate.split('T')[0]}" required>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Количество *</label>
                        <input type="number" class="form-control" name="quantity" value="${supply.quantity}" min="1" required>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Стоимость *</label>
                        <input type="number" class="form-control" name="cost" value="${supply.cost}" step="0.01" min="0" required>
                    </div>
                    <div class="d-flex justify-content-between">
                        <button type="button" class="btn btn-danger" onclick="SupplyManager.delete(${id})">
                            <i class="fas fa-trash"></i> Удалить
                        </button>
                        <div>
                            <button type="button" class="btn btn-secondary me-2" data-bs-dismiss="modal">Отмена</button>
                            <button type="submit" class="btn btn-primary">
                                <i class="fas fa-save"></i> Сохранить
                            </button>
                        </div>
                    </div>
                </form>
            `;
            
            document.getElementById('editModalTitle').textContent = 'Редактирование поставки';
            const modal = new bootstrap.Modal(document.getElementById('editModal'));
            modal.show();
            
            // Обработчик формы редактирования
            document.getElementById('editSupplyForm').addEventListener('submit', async function(e) {
                e.preventDefault();
                const formData = new FormData(this);
                await SupplyManager.update(id, formData);
                modal.hide();
            });
        } catch (error) {
            UIUtils.showAlert('Ошибка загрузки данных поставки: ' + error.message, 'danger');
        }
    }

    static async update(id, formData) {
        try {
            const supply = {
                id: id,
                enterpriseRegistrationNumber: formData.get('enterpriseRegistrationNumber'),
                supplierId: parseInt(formData.get('supplierId')),
                productName: formData.get('productName'),
                supplyDate: formData.get('supplyDate'),
                quantity: parseInt(formData.get('quantity')),
                cost: parseFloat(formData.get('cost'))
            };

            await ApiClient.put(`/Supply/${id}`, supply);
            UIUtils.showAlert('Поставка успешно обновлена!', 'success');
            this.loadAll();
        } catch (error) {
            UIUtils.showAlert('Ошибка обновления поставки: ' + error.message, 'danger');
        }
    }

    static async delete(id) {
        if (!confirm('Вы уверены, что хотите удалить эту поставку? Это действие нельзя отменить.')) {
            return;
        }

        try {
            await ApiClient.delete(`/Supply/${id}`);
            UIUtils.showAlert('Поставка успешно удалена!', 'success');
            this.loadAll();
            bootstrap.Modal.getInstance(document.getElementById('editModal')).hide();
        } catch (error) {
            UIUtils.showAlert('Ошибка удаления поставки: ' + error.message, 'danger');
        }
    }
}

// Аналитические функции
class AnalyticsManager {
    static async searchEnterprise() {
        const regNumber = document.getElementById('searchRegNumber').value.trim();
        if (!regNumber) {
            UIUtils.showAlert('Введите регистрационный номер', 'warning');
            return;
        }

        try {
            const enterprise = await ApiClient.get(`/Enterprise/${regNumber}/details`);
            
            const container = document.getElementById('enterpriseDetails');
            container.innerHTML = `
                <div class="data-card fade-in">
                    <h6><i class="fas fa-building me-2"></i>${enterprise.name}</h6>
                    <p><strong>Регистрационный номер:</strong> ${enterprise.registrationNumber}</p>
                    <p><strong>Тип отрасли:</strong> ${enterprise.industryType}</p>
                    <p><strong>Адрес:</strong> ${enterprise.address}</p>
                    <p><strong>Телефон:</strong> ${enterprise.phone}</p>
                    <p><strong>Форма собственности:</strong> ${enterprise.ownershipType}</p>
                    <p><strong>Количество работающих:</strong> ${enterprise.employeeCount}</p>
                    <p><strong>Общая площадь:</strong> ${enterprise.totalArea} м²</p>
                    <p><strong>Количество поставок:</strong> ${enterprise.suppliesCount}</p>
                    <p><strong>Общая стоимость поставок:</strong> ${UIUtils.formatCurrency(enterprise.totalSupplyCost)}</p>
                </div>
            `;
        } catch (error) {
            document.getElementById('enterpriseDetails').innerHTML = `
                <div class="alert alert-danger">
                    <i class="fas fa-exclamation-triangle me-2"></i>
                    Предприятие с номером "${regNumber}" не найдено
                </div>
            `;
        }
    }

    static async getSuppliersByPeriod() {
        const startDate = document.getElementById('startDate').value;
        const endDate = document.getElementById('endDate').value;
        
        if (!startDate || !endDate) {
            UIUtils.showAlert('Выберите период', 'warning');
            return;
        }

        try {
            const suppliers = await ApiClient.get(`/Supplier/byPeriod?startDate=${startDate}&endDate=${endDate}`);
            
            const container = document.getElementById('suppliersByPeriod');
            if (suppliers.length === 0) {
                container.innerHTML = `
                    <div class="alert alert-info">
                        <i class="fas fa-info-circle me-2"></i>
                        За указанный период поставщики не найдены
                    </div>
                `;
                return;
            }

            container.innerHTML = `
                <h6>Поставщики за период с ${UIUtils.formatDate(startDate)} по ${UIUtils.formatDate(endDate)}:</h6>
                <div class="list-group">
                    ${suppliers.map(supplier => `
                        <div class="list-group-item">
                            <strong>${supplier.name}</strong><br>
                            <small class="text-muted">${supplier.address}</small>
                        </div>
                    `).join('')}
                </div>
            `;
        } catch (error) {
            UIUtils.showAlert('Ошибка получения поставщиков: ' + error.message, 'danger');
        }
    }

    static async getTop5Enterprises() {
        try {
            const enterprises = await ApiClient.get('/Enterprise/top5BySupplyCount');
            
            const container = document.getElementById('top5Enterprises');
            if (enterprises.length === 0) {
                container.innerHTML = `
                    <div class="alert alert-info">
                        <i class="fas fa-info-circle me-2"></i>
                        Данные не найдены
                    </div>
                `;
                return;
            }

            container.innerHTML = `
                <div class="row">
                    ${enterprises.map((enterprise, index) => `
                        <div class="col-md-6 mb-3">
                            <div class="stat-card">
                                <h3>#${index + 1}</h3>
                                <p><strong>${enterprise.name}</strong></p>
                                <p>Поставок: ${enterprise.suppliesCount || 0}</p>
                            </div>
                        </div>
                    `).join('')}
                </div>
            `;
        } catch (error) {
            UIUtils.showAlert('Ошибка получения топ-5 предприятий: ' + error.message, 'danger');
        }
    }
}

// Глобальные функции для вызова из HTML
function loadEnterprises() {
    EnterpriseManager.loadAll();
}

function loadSuppliers() {
    SupplierManager.loadAll();
}

function loadSupplies() {
    SupplyManager.loadAll();
}

function searchEnterprise() {
    AnalyticsManager.searchEnterprise();
}

function getSuppliersByPeriod() {
    AnalyticsManager.getSuppliersByPeriod();
}

function getTop5Enterprises() {
    AnalyticsManager.getTop5Enterprises();
}

// Инициализация приложения
document.addEventListener('DOMContentLoaded', function() {
    // Загрузка начальных данных
    EnterpriseManager.loadAll();
    SupplierManager.loadAll();
    SupplyManager.loadAll();

    // Загрузка предприятий для селекта
    ApiClient.get('/Enterprise').then(enterprises => {
        const select = document.getElementById('enterpriseSelect');
        select.innerHTML = '<option value="">Выберите предприятие...</option>' +
            enterprises.map(enterprise => 
                `<option value="${enterprise.registrationNumber}">${enterprise.name}</option>`
            ).join('');
    });

    // Обработчики форм
    document.getElementById('enterpriseForm').addEventListener('submit', function(e) {
        e.preventDefault();
        const formData = new FormData(this);
        EnterpriseManager.create(formData);
    });

    document.getElementById('supplierForm').addEventListener('submit', function(e) {
        e.preventDefault();
        const formData = new FormData(this);
        SupplierManager.create(formData);
    });

    document.getElementById('supplyForm').addEventListener('submit', function(e) {
        e.preventDefault();
        const formData = new FormData(this);
        SupplyManager.create(formData);
    });

    // Обработчик поиска предприятия по Enter
    document.getElementById('searchRegNumber').addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            AnalyticsManager.searchEnterprise();
        }
    });

    // Установка текущей даты по умолчанию
    const today = new Date().toISOString().split('T')[0];
    document.getElementById('supplyDate').value = today;
    document.getElementById('startDate').value = today;
    document.getElementById('endDate').value = today;
});