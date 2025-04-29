function initSelect2(selector = '.select2') {
    $(selector).select2({
        placeholder: "Оберіть зі списку",
        allowClear: true,
        width: '100%'
    });
}

document.addEventListener('DOMContentLoaded', async function () {
    initSelect2();

    const departmentSelect = document.getElementById('department-select');
    const divisionSelect = document.getElementById('division-select');
    const vacationSelect = document.getElementById('vacation-select');
    const countrySelect = document.getElementById('country-select');
    const citySelect = document.getElementById('city-select');
    const officeSelect = document.getElementById('office-select');
    const placementIdInput = document.getElementById('PlacementId');
    const form = document.querySelector('form');

    // Отримати попередньо встановлені значення
    const selectedDepartmentId = departmentSelect.dataset.selected;
    const selectedDivisionId = divisionSelect.dataset.selected;
    const selectedVacationId = vacationSelect.dataset.selected;
    const selectedCountryId = countrySelect.dataset.selected;
    const selectedCityName = citySelect.dataset.selected;
    const selectedOfficeName = officeSelect.dataset.selected;

    function resetSelect(select, placeholder) {
        $(select).select2('destroy');
        select.innerHTML = `<option value="">${placeholder}</option>`;
        select.disabled = true;
        $(select).select2({
            placeholder: placeholder,
            allowClear: true,
            width: '100%'
        });
    }

    // ============================= Департаменти → Відділи → Вакансії

    async function loadDivisions(departmentId, divisionId = null) {
        resetSelect(divisionSelect, "Оберіть відділ");
        resetSelect(vacationSelect, "Оберіть вакансію");

        if (!departmentId) return;

        const divisions = await fetch(`/api/people/get-divisions/${departmentId}`).then(r => r.json());
        if (divisions.length > 0) {
            divisionSelect.disabled = false;
            divisions.forEach(d => {
                const option = new Option(d.name, d.id, false, divisionId && d.id == divisionId);
                divisionSelect.add(option);
            });
            $(divisionSelect).trigger('change');
        }
    }

    async function loadVacations(divisionId, vacationId = null) {
        resetSelect(vacationSelect, "Оберіть вакансію");

        if (!divisionId) return;

        const vacations = await fetch(`/api/people/get-vacations/${divisionId}`).then(r => r.json());
        if (vacations.length > 0) {
            vacationSelect.disabled = false;
            vacations.forEach(v => {
                const option = new Option(v.name, v.id, false, vacationId && v.id == vacationId);
                vacationSelect.add(option);
            });
            $(vacationSelect).trigger('change');
        }
    }

    $(departmentSelect).on('select2:select', function (e) {
        const departmentId = e.params.data.id;
        loadDivisions(departmentId);
    });

    $(divisionSelect).on('select2:select', function (e) {
        const divisionId = e.params.data.id;
        loadVacations(divisionId);
    });

    // ============================= Країни → Міста → Офіси

    async function loadCities(countryId, cityName = null) {
        resetSelect(citySelect, "Оберіть місто");
        resetSelect(officeSelect, "Оберіть офіс");

        if (!countryId) return;

        const cities = await fetch(`/api/people/get-cities/${countryId}`).then(r => r.json());
        if (cities.length > 0) {
            citySelect.disabled = false;
            cities.forEach(c => {
                const option = new Option(c.name, c.name, false, cityName && c.name === cityName);
                citySelect.add(option);
            });
            $(citySelect).trigger('change');
        }
    }

    async function loadOffices(cityName, officeName = null) {
        resetSelect(officeSelect, "Оберіть офіс");

        if (!cityName) return;

        const offices = await fetch(`/api/people/get-offices/${encodeURIComponent(cityName)}`).then(r => r.json());
        officeSelect.disabled = false;
        $(officeSelect).select2('destroy');
        officeSelect.innerHTML = '';

        officeSelect.add(new Option('(Без офісу)', '', !officeName, !officeName));

        offices.forEach(o => {
            const option = new Option(o.name, o.name, false, officeName && o.name === officeName);
            officeSelect.add(option);
        });

        $(officeSelect).select2({
            placeholder: "Оберіть офіс",
            allowClear: true,
            width: '100%'
        });

        $(officeSelect).trigger('change');
    }

    $(countrySelect).on('select2:select', function (e) {
        const countryId = e.params.data.id;
        loadCities(countryId);
    });

    $(citySelect).on('select2:select', function (e) {
        const cityName = e.params.data.text;
        loadOffices(cityName);
    });

    $(officeSelect).on('select2:select', function (e) {
        const officeName = e.params.data.text || null;
        findPlacement(officeName);
    });

    async function findPlacement(selectedOfficeName) {
        const countryId = $(countrySelect).select2('data')[0]?.id;
        const cityName = $(citySelect).select2('data')[0]?.text.trim();

        if (!countryId || !cityName) {
            placementIdInput.value = '';
            return;
        }

        const params = new URLSearchParams({
            countryId: countryId,
            city: cityName,
            office: selectedOfficeName || ''
        });

        await fetch(`/api/people/get-placement?${params}`)
            .then(response => response.ok ? response.json() : null)
            .then(placement => {
                if (placement) {
                    placementIdInput.value = placement.id;
                }
            })
            .catch(() => {
                placementIdInput.value = '';
            });
    }

    // ============================= Відновлення попередніх значень

    if (selectedDepartmentId) {
        $(departmentSelect).val(selectedDepartmentId).trigger('change');
        await loadDivisions(selectedDepartmentId, selectedDivisionId);
        await loadVacations(selectedDivisionId, selectedVacationId);
    }

    if (selectedCountryId) {
        $(countrySelect).val(selectedCountryId).trigger('change');
        await loadCities(selectedCountryId, selectedCityName);
        await loadOffices(selectedCityName, selectedOfficeName);
    }

    // ============================= Звільнення працівника
    

    // ============================= Валідація перед сабмітом
    form.addEventListener('submit', function (e) {
        if (!placementIdInput.value) {
            e.preventDefault();
            alert('Оберіть розміщення (країну, місто, офіс) перед збереженням!');
        }
    });
});

