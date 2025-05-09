

function bindSelCountry() {
    iCountry.style.display = '';
    selCountry.style.display = 'none';
    wcfAddr.JSON_getCountryListAll(bindSelCountryOnComplete, JSONOnError);
}

function bindSelCountryOnComplete(result) {

    iCountry.style.display = 'none';
    selCountry.style.display = '';
    result = eval(result);
    if (result.length == 0) return 0;
    selCountry.length = 0;

    for (var i = 0; i < result.length  ; i++) {
        var newOpt = document.createElement('option');
        newOpt.text = MyURLDecode(result[i].countryName);
        newOpt.value = result[i].countryCode;


        try {
            selCountry.add(newOpt);
        } catch (e) {
            selCountry.add(newOpt, null);
        }
    }

    if (_selCountry == '') _selCountry = 'INA';
    selCountry.value = _selCountry;

    bindSelProvince(selCountry.value);
}


function bindSelProvince(countryCode) {
    iProvince.style.display = '';
    selProvince.style.display = 'none';
    wcfAddr.JSON_getProvinceListAll(countryCode, bindSelProvinceOnComplete, JSONOnError);
}

function bindSelProvinceOnComplete(result) {
    iProvince.style.display = 'none';
    selProvince.style.display = '';
    result = eval(result);
    if (result.length == 0) return 0;

    selProvince.length = 0;

    for (var i = 0; i < result.length  ; i++) {
        var newOpt = document.createElement('option');
        newOpt.text = MyURLDecode(result[i].provinceName);
        newOpt.value = result[i].provinceCode;
        try {
            selProvince.add(newOpt);
        } catch (e) {
            selProvince.add(newOpt, null);
        }
    }

    if (_selProvince != '' && _selProvince != '-') { selProvince.value = _selProvince; _selProvince = ''; }

    bindSelCity(selCountry.value, selProvince.value);

}

function bindSelCity(countryCode, provinceCode) {
    iCity.style.display = '';
    selCity.display = 'none';
    wcfAddr.JSON_getCityListAll(countryCode, provinceCode, bindSelCityOnComplete, JSONOnError);
}

function bindSelCityOnComplete(result) {
    iCity.style.display = 'none';
    selCity.style.display = '';
    result = eval(result);
    if (result.length == 0) return 0;

    selCity.length = 0;

    for (var i = 0; i < result.length  ; i++) {
        var newOpt = document.createElement('option');
        newOpt.text = MyURLDecode(result[i].cityName);
        newOpt.value = result[i].cityCode;
        try {
            selCity.add(newOpt);
        } catch (e) {
            selCity.add(newOpt, null);
        }
    }

    if (_selCity != '' && _selCity != '-') { selCity.value = _selCity; _selCity = ''; }

}

function JSONOnError(result) {
    alert(result.get_message());
}