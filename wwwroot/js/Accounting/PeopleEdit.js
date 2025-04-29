function initSelect2(selector = '.select2') {
    $(selector).select2({
        placeholder: "Оберіть зі списку",
        allowClear: true,
        width: '100%'
    });
}

document.addEventListener('DOMContentLoaded', function () {
    initSelect2();

    const departmentSelect = document.getElementById('department-select');
    const divisionSelect = document.getElementById('division-select');
    const vacationSelect = document.getElementById('vacation-select');

    const countrySelect = document.getElementById('country-select');
    const citySelect = document.getElementById('city-select');
    const officeSelect = document.getElementById('office-select');

    const dismissalButton = document.getElementById('dismiss-button');
    const dismissalDateGroup = document.getElementById('dismissal-group');
    const dismissalDateInput = document.getElementById('DismissalDate');

    const isActiveInput = document.getElementById('IsActive');

    function resetSelect(select, defaultText) {
        select.innerHTML = `<option value="">${defaultText}</option>`;
        select.disabled = true;
        $(select).trigger('change');
    }

    // --- Посада ---
    departmentSelect.addEventListener('change', function () {
        const id = this.value;
        resetSelect(divisionSelect, "Оберіть відділ");
        resetSelect(vacationSelect, "Оберіть вакансію");

        if (!id) return;

        fetch(`/api/people/get-divisions/${id}`)
            .then(res => res.json())
            .then(data => {
                divisionSelect.disabled = false;
                data.forEach(item => {
                    const option = new Option(item.name, item.id);
                    divisionSelect.appendChild(option);
                });
                $(divisionSelect).trigger('change');
            });
    });

    divisionSelect.addEventListener('change', function () {
        const id = this.value;
        resetSelect(vacationSelect, "Оберіть вакансію");

        if (!id) return;

        fetch(`/api/people/get-vacations/${id}`)
            .then(res => res.json())
            .then(data => {
                vacationSelect.disabled = false;
                data.forEach(item => {
                    const option = new Option(item.name, item.id);
                    vacationSelect.appendChild(option);
                });
                $(vacationSelect).trigger('change');
            });
    });

    // --- Розміщення ---
    countrySelect.addEventListener('change', function () {
        const id = this.value;
        resetSelect(citySelect, "Оберіть місто");
        resetSelect(officeSelect, "Оберіть офіс");

        if (!id) return;

        fetch(`/api/countries/${id}/cities`)
            .then(res => res.json())
            .then(data => {
                citySelect.disabled = false;
                data.forEach(item => {
                    const option = new Option(item.name, item.id);
                    citySelect.appendChild(option);
                });
                $(citySelect).trigger('change');
            });
    });

    citySelect.addEventListener('change', function () {
        const id = this.value;
        resetSelect(officeSelect, "Оберіть офіс");

        if (!id) return;

        fetch(`/api/cities/${id}/offices`)
            .then(res => res.json())
            .then(data => {
                officeSelect.disabled = false;
                data.forEach(item => {
                    const option = new Option(item.name, item.id);
                    officeSelect.appendChild(option);
                });
                $(officeSelect).trigger('change');
            });
    });

    // --- Звільнення ---
    dismissalButton?.addEventListener('click', function (e) {
        e.preventDefault();
        dismissalDateGroup.classList.remove('d-none');
        dismissalDateInput.disabled = false;
        isActiveInput.value = 'false';
    });
});
