function initSelect2(selector = '.select2', placeholder = 'Оберіть зі списку') {
    $(selector).select2({
        placeholder: placeholder,
        allowClear: true,
        width: '100%' // Щоб було як в Bootstrap
    });
}

document.addEventListener('DOMContentLoaded', function () {
    initSelect2();

    const departmentSelect = document.getElementById('department-select');
    const divisionSelect = document.getElementById('division-select');
    const nameInput = document.getElementById('Name');

    if (!departmentSelect || !divisionSelect || !nameInput) return;

    function resetDivision() {
        divisionSelect.innerHTML = '<option value="">Оберіть відділ</option>';
        divisionSelect.value = "";
        divisionSelect.disabled = true;
        if ($(divisionSelect).hasClass("select2")) {
            $(divisionSelect).trigger("change");
        }
    }

    function loadDivisions(departmentId, selectedDivisionId = null) {
        fetch(`/api/vacations/divisions/${departmentId}`)
            .then(res => res.json())
            .then(data => {
                divisionSelect.innerHTML = '<option value="">Оберіть відділ</option>';
                data.forEach(div => {
                    const option = document.createElement('option');
                    option.value = div.id;
                    option.text = div.name;
                    if (selectedDivisionId && div.id === +selectedDivisionId) {
                        option.selected = true;
                    }
                    divisionSelect.appendChild(option);
                });

                divisionSelect.disabled = false;
                if ($(divisionSelect).hasClass("select2")) {
                    $(divisionSelect).trigger("change");
                }

                nameInput.disabled = false;
            });
    }

    departmentSelect.addEventListener('change', function () {
        const departmentId = this.value;
        resetDivision();
        nameInput.disabled = true;

        if (!departmentId) return;

        loadDivisions(departmentId);
    });

    const preselectedDepartmentId = departmentSelect.value;
    const preselectedDivisionId = divisionSelect.value;

    if (preselectedDepartmentId) {
        loadDivisions(preselectedDepartmentId, preselectedDivisionId);
    } else {
        resetDivision();
        nameInput.disabled = true;
    }
});


