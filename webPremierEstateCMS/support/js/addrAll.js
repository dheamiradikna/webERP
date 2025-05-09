function bindSelCountry(tipe) {
    if (tipe == '1') {
        iCountryHO.style.display = '';
        selCountryHO.style.display = 'none';
    } else if (tipe == '2') {
        iCountryBranch.style.display = '';
        selCountryBranch.style.display = 'none';
    } else if (tipe == '3') {
        iCountryPlant.style.display = '';
        selCountryPlant.style.display = 'none';
    } else if (tipe == '4') {
        iCountryWH.style.display = '';
        selCountryWH.style.display = 'none';
    } else if (tipe == '5') {
        iCountryWorkshop.style.display = '';
        selCountryWorkshop.style.display = 'none';
    } else if (tipe == '6') {
        iCountryFarm.style.display = '';
        selCountryFarm.style.display = 'none';
    } else if (tipe == '7') {
        iCountryStockBreeding.style.display = '';
        selCountryStockBreeding.style.display = 'none';
    } else if (tipe == '8') {
        iCountryOther.style.display = '';
        selCountryOther.style.display = 'none';
    } else if (tipe == '9') {
        iCountrySubsidiary.style.display = '';
        selCountrySubsidiary.style.display = 'none';
    }
    wcfAddr.JSON_getCountryList(bindSelCountryOnComplete, JSONOnError, tipe);
}

function bindSelCountryOnComplete(result, tipe) {

    result = eval(result);
    if (result.length == 0) return 0;

    if (tipe == '1') {
        iCountryHO.style.display = 'none';
        selCountryHO.style.display = '';
        selCountryHO.length = 0;
    } else if (tipe == '2') {
        iCountryBranch.style.display = 'none';
        selCountryBranch.style.display = '';
        selCountryBranch.length = 0;
    } else if (tipe == '3') {
        iCountryPlant.style.display = 'none';
        selCountryPlant.style.display = '';
        selCountryPlant.length = 0;
    } else if (tipe == '4') {
        iCountryWH.style.display = 'none';
        selCountryWH.style.display = '';
        selCountryWH.length = 0;
    } else if (tipe == '5') {
        iCountryWorkshop.style.display = 'none';
        selCountryWorkshop.style.display = '';
        selCountryWorkshop.length = 0;
    } else if (tipe == '6') {
        iCountryFarm.style.display = 'none';
        selCountryFarm.style.display = '';
        selCountryFarm.length = 0;
    } else if (tipe == '7') {
        iCountryStockBreeding.style.display = 'none';
        selCountryStockBreeding.style.display = '';
        selCountryStockBreeding.length = 0;
    } else if (tipe == '8') {
        iCountryOther.style.display = 'none';
        selCountryOther.style.display = '';
        selCountryOther.length = 0;
    } else if (tipe == '9') {
        iCountrySubsidiary.style.display = 'none';
        selCountrySubsidiary.style.display = '';
        selCountrySubsidiary.length = 0;
    }
    
    for (var i = 0; i < result.length  ; i++) {
        var newOpt = document.createElement('option');
        newOpt.text = MyURLDecode(result[i].countryName);
        newOpt.value = result[i].countryCode;


        try {
            if (tipe == '1') {
                selCountryHO.add(newOpt);
            } else if (tipe == '2') {
                selCountryBranch.add(newOpt);
            } else if (tipe == '3') {
                selCountryPlant.add(newOpt);
            } else if (tipe == '4') {
                selCountryWH.add(newOpt);
            } else if (tipe == '5') {
                selCountryWorkshop.add(newOpt);
            } else if (tipe == '6') {
                selCountryFarm.add(newOpt);
            } else if (tipe == '7') {
                selCountryStockBreeding.add(newOpt);
            } else if (tipe == '8') {
                selCountryOther.add(newOpt);
            } else if (tipe == '9') {
                selCountrySubsidiary.add(newOpt);
            }
        } catch (e) {
            if (tipe == '1') {
                selCountryHO.add(newOpt, null);
            } else if (tipe == '2') {
                selCountryBranch.add(newOpt, null);
            } else if (tipe == '3') {
                selCountryPlant.add(newOpt, null);
            } else if (tipe == '4') {
                selCountryWH.add(newOpt, null);
            } else if (tipe == '5') {
                selCountryWorkshop.add(newOpt, null);
            } else if (tipe == '6') {
                selCountryFarm.add(newOpt, null);
            } else if (tipe == '7') {
                selCountryStockBreeding.add(newOpt, null);
            } else if (tipe == '8') {
                selCountryOther.add(newOpt, null);
            } else if (tipe == '9') {
                selCountrySubsidiary.add(newOpt, null);
            }
        }
    }

    if (tipe == '1') {
        if (_selCountryHO == '') _selCountryHO = 'INA';
        selCountryHO.value = _selCountryHO;

        bindSelProvince(selCountryHO.value, tipe);
    } else if (tipe == '2') {
        if (_selCountryBranch == '') _selCountryBranch = 'INA';
        selCountryBranch.value = _selCountryBranch;

        bindSelProvince(selCountryBranch.value, tipe);
    } else if (tipe == '3') {
        if (_selCountryPlant == '') _selCountryPlant = 'INA';
        selCountryPlant.value = _selCountryPlant;

        bindSelProvince(selCountryPlant.value, tipe);
    } else if (tipe == '4') {
        if (_selCountryWH == '') _selCountryWH = 'INA';
        selCountryWH.value = _selCountryWH;

        bindSelProvince(selCountryWH.value, tipe);
    } else if (tipe == '5') {
        if (_selCountryWorkshop == '') _selCountryWorkshop = 'INA';
        selCountryWorkshop.value = _selCountryWorkshop;

        bindSelProvince(selCountryWorkshop.value, tipe);
    } else if (tipe == '6') {
        if (_selCountryFarm == '') _selCountryFarm = 'INA';
        selCountryFarm.value = _selCountryFarm;

        bindSelProvince(selCountryFarm.value, tipe);
    } else if (tipe == '7') {
        if (_selCountryStockBreeding == '') _selCountryStockBreeding = 'INA';
        selCountryStockBreeding.value = _selCountryStockBreeding;

        bindSelProvince(selCountryStockBreeding.value, tipe);
    } else if (tipe == '8') {
        if (_selCountryOther == '') _selCountryOther = 'INA';
        selCountryOther.value = _selCountryOther;

        bindSelProvince(selCountryOther.value, tipe);
    } else if (tipe == '9') {
        if (_selCountrySubsidiary == '') _selCountrySubsidiary = 'INA';
        selCountrySubsidiary.value = _selCountrySubsidiary;

        bindSelProvince(selCountrySubsidiary.value, tipe);
    }
    
}


function bindSelProvince(countryCode, tipe) {
    if (tipe == '1') {
        iProvinceHO.style.display = '';
        selProvinceHO.style.display = 'none';
    } else if (tipe == '2') {
        iProvinceBranch.style.display = '';
        selProvinceBranch.style.display = 'none';
    } else if (tipe == '3') {
        iProvincePlant.style.display = '';
        selProvincePlant.style.display = 'none';
    } else if (tipe == '4') {
        iProvinceWH.style.display = '';
        selProvinceWH.style.display = 'none';
    } else if (tipe == '5') {
        iProvinceWorkshop.style.display = '';
        selProvinceWorkshop.style.display = 'none';
    } else if (tipe == '6') {
        iProvinceFarm.style.display = '';
        selProvinceFarm.style.display = 'none';
    } else if (tipe == '7') {
        iProvinceStockBreeding.style.display = '';
        selProvinceStockBreeding.style.display = 'none';
    } else if (tipe == '8') {
        iProvinceOther.style.display = '';
        selProvinceOther.style.display = 'none';
    } else if (tipe == '9') {
        iProvinceSubsidiary.style.display = '';
        selProvinceSubsidiary.style.display = 'none';
    }
    wcfAddr.JSON_getProvinceList(countryCode, bindSelProvinceOnComplete, JSONOnError, tipe);
}

function bindSelProvinceOnComplete(result, tipe) {
    result = eval(result);
    if (result.length == 0) return 0;

    if (tipe == '1') {
        iProvinceHO.style.display = 'none';
        selProvinceHO.style.display = '';
        selProvinceHO.length = 0;
    } else if (tipe == '2') {
        iProvinceBranch.style.display = 'none';
        selProvinceBranch.style.display = '';
        selProvinceBranch.length = 0;
    } else if (tipe == '3') {
        iProvincePlant.style.display = 'none';
        selProvincePlant.style.display = '';
        selProvincePlant.length = 0;
    } else if (tipe == '4') {
        iProvinceWH.style.display = 'none';
        selProvinceWH.style.display = '';
        selProvinceWH.length = 0;
    } else if (tipe == '5') {
        iProvinceWorkshop.style.display = 'none';
        selProvinceWorkshop.style.display = '';
        selProvinceWorkshop.length = 0;
    } else if (tipe == '6') {
        iProvinceFarm.style.display = 'none';
        selProvinceFarm.style.display = '';
        selProvinceFarm.length = 0;
    } else if (tipe == '7') {
        iProvinceStockBreeding.style.display = 'none';
        selProvinceStockBreeding.style.display = '';
        selProvinceStockBreeding.length = 0;
    } else if (tipe == '8') {
        iProvinceOther.style.display = 'none';
        selProvinceOther.style.display = '';
        selProvinceOther.length = 0;
    } else if (tipe == '9') {
        iProvinceSubsidiary.style.display = 'none';
        selProvinceSubsidiary.style.display = '';
        selProvinceSubsidiary.length = 0;
    }
    
    for (var i = 0; i < result.length  ; i++) {
        var newOpt = document.createElement('option');
        newOpt.text = MyURLDecode(result[i].provinceName);
        newOpt.value = result[i].provinceCode;
        try {
            if (tipe == '1') {
                selProvinceHO.add(newOpt);
            } else if (tipe == '2') {
                selProvinceBranch.add(newOpt);
            } else if (tipe == '3') {
                selProvincePlant.add(newOpt);
            } else if (tipe == '4') {
                selProvinceWH.add(newOpt);
            } else if (tipe == '5') {
                selProvinceWorkshop.add(newOpt);
            } else if (tipe == '6') {
                selProvinceFarm.add(newOpt);
            } else if (tipe == '7') {
                selProvinceStockBreeding.add(newOpt);
            } else if (tipe == '8') {
                selProvinceOther.add(newOpt);
            } else if (tipe == '9') {
                selProvinceSubsidiary.add(newOpt);
            }
        } catch (e) {
            if (tipe == '1') {
                selProvinceHO.add(newOpt, null);
            } else if (tipe == '2') {
                selProvinceBranch.add(newOpt, null);
            } else if (tipe == '3') {
                selProvincePlant.add(newOpt, null);
            } else if (tipe == '4') {
                selProvinceWH.add(newOpt, null);
            } else if (tipe == '5') {
                selProvinceWorkshop.add(newOpt, null);
            } else if (tipe == '6') {
                selProvinceFarm.add(newOpt, null);
            } else if (tipe == '7') {
                selProvinceStockBreeding.add(newOpt, null);
            } else if (tipe == '8') {
                selProvinceOther.add(newOpt, null);
            } else if (tipe == '9') {
                selProvinceSubsidiary.add(newOpt, null);
            }
        }
    }

    if (tipe == '1') {
        if (_selProvinceHO != '' && _selProvinceHO != '-') { selProvinceHO.value = _selProvinceHO; _selProvinceHO = ''; }

        bindSelCity(selCountryHO.value, selProvinceHO.value, tipe);
    } else if (tipe == '2') {
        if (_selProvinceBranch != '' && _selProvinceBranch != '-') { selProvinceBranch.value = _selProvinceBranch; _selProvinceBranch = ''; }

        bindSelCity(selCountryBranch.value, selProvinceBranch.value, tipe);
    } else if (tipe == '3') {
        if (_selProvincePlant != '' && _selProvincePlant != '-') { selProvincePlant.value = _selProvincePlant; _selProvincePlant = ''; }

        bindSelCity(selCountryPlant.value, selProvincePlant.value, tipe);
    } else if (tipe == '4') {
        if (_selProvinceWH != '' && _selProvinceWH != '-') { selProvinceWH.value = _selProvinceWH; _selProvinceWH = ''; }

        bindSelCity(selCountryWH.value, selProvinceWH.value, tipe);
    } else if (tipe == '5') {
        if (_selProvinceWorkshop != '' && _selProvinceWorkshop != '-') { selProvinceWorkshop.value = _selProvinceWorkshop; _selProvinceWorkshop = ''; }

        bindSelCity(selCountryWorkshop.value, selProvinceWorkshop.value, tipe);
    } else if (tipe == '6') {
        if (_selProvinceFarm != '' && _selProvinceFarm != '-') { selProvinceFarm.value = _selProvinceFarm; _selProvinceFarm = ''; }

        bindSelCity(selCountryFarm.value, selProvinceFarm.value, tipe);
    } else if (tipe == '7') {
        if (_selProvinceStockBreeding != '' && _selProvinceStockBreeding != '-') { selProvinceStockBreeding.value = _selProvinceStockBreeding; _selProvinceStockBreeding = ''; }

        bindSelCity(selCountryStockBreeding.value, selProvinceStockBreeding.value, tipe);
    } else if (tipe == '8') {
        if (_selProvinceOther != '' && _selProvinceOther != '-') { selProvinceOther.value = _selProvinceOther; _selProvinceOther = ''; }

        bindSelCity(selCountryOther.value, selProvinceOther.value, tipe);
    } else if (tipe == '9') {
        if (_selProvinceSubsidiary != '' && _selProvinceSubsidiary != '-') { selProvinceSubsidiary.value = _selProvinceSubsidiary; _selProvinceSubsidiary = ''; }

        bindSelCity(selCountrySubsidiary.value, selProvinceSubsidiary.value, tipe);
    }

}

function bindSelCity(countryCode, provinceCode, tipe) {
    if (tipe == '1') {
        iCityHO.style.display = '';
        selCityHO.display = 'none';
    } else if (tipe == '2') {
        iCityBranch.style.display = '';
        selCityBranch.display = 'none';
    } else if (tipe == '3') {
        iCityPlant.style.display = '';
        selCityPlant.display = 'none';
    } else if (tipe == '4') {
        iCityWH.style.display = '';
        selCityWH.display = 'none';
    } else if (tipe == '5') {
        iCityWorkshop.style.display = '';
        selCityWorkshop.display = 'none';
    } else if (tipe == '6') {
        iCityFarm.style.display = '';
        selCityFarm.display = 'none';
    } else if (tipe == '7') {
        iCityStockBreeding.style.display = '';
        selCityStockBreeding.display = 'none';
    } else if (tipe == '8') {
        iCityOther.style.display = '';
        selCityOther.display = 'none';
    } else if (tipe == '9') {
        iCitySubsidiary.style.display = '';
        selCitySubsidiary.display = 'none';
    }
   
    wcfAddr.JSON_getCityList(countryCode, provinceCode, bindSelCityOnComplete, JSONOnError, tipe);
}

function bindSelCityOnComplete(result, tipe) {
    result = eval(result);
    if (result.length == 0) return 0;

    if (tipe == '1') {
        iCityHO.style.display = 'none';
        selCityHO.style.display = '';
        selCityHO.length = 0;
    } else if (tipe == '2') {
        iCityBranch.style.display = 'none';
        selCityBranch.style.display = '';
        selCityBranch.length = 0;
    } else if (tipe == '3') {
        iCityPlant.style.display = 'none';
        selCityPlant.style.display = '';
        selCityPlant.length = 0;
    } else if (tipe == '4') {
        iCityWH.style.display = 'none';
        selCityWH.style.display = '';
        selCityWH.length = 0;
    } else if (tipe == '5') {
        iCityWorkshop.style.display = 'none';
        selCityWorkshop.style.display = '';
        selCityWorkshop.length = 0;
    } else if (tipe == '6') {
        iCityFarm.style.display = 'none';
        selCityFarm.style.display = '';
        selCityFarm.length = 0;
    } else if (tipe == '7') {
        iCityStockBreeding.style.display = 'none';
        selCityStockBreeding.style.display = '';
        selCityStockBreeding.length = 0;
    } else if (tipe == '8') {
        iCityOther.style.display = 'none';
        selCityOther.style.display = '';
        selCityOther.length = 0;
    } else if (tipe == '9') {
        iCitySubsidiary.style.display = 'none';
        selCitySubsidiary.style.display = '';
        selCitySubsidiary.length = 0;
    }
    
    for (var i = 0; i < result.length  ; i++) {
        var newOpt = document.createElement('option');
        newOpt.text = MyURLDecode(result[i].cityName);
        newOpt.value = result[i].cityCode;
        try {
            if (tipe == '1') {
                selCityHO.add(newOpt);
            } else if (tipe == '2') {
                selCityBranch.add(newOpt);
            } else if (tipe == '3') {
                selCityPlant.add(newOpt);
            } else if (tipe == '4') {
                selCityWH.add(newOpt);
            } else if (tipe == '5') {
                selCityWorkshop.add(newOpt);
            } else if (tipe == '6') {
                selCityFarm.add(newOpt);
            } else if (tipe == '7') {
                selCityStockBreeding.add(newOpt);
            } else if (tipe == '8') {
                selCityOther.add(newOpt);
            } else if (tipe == '9') {
                selCitySubsidiary.add(newOpt);
            }
            
        } catch (e) {
            if (tipe == '1') {
                selCityHO.add(newOpt, null);
            } else if (tipe == '2') {
                selCityBranch.add(newOpt, null);
            } else if (tipe == '3') {
                selCityPlant.add(newOpt, null);
            } else if (tipe == '4') {
                selCityWH.add(newOpt, null);
            } else if (tipe == '5') {
                selCityWorkshop.add(newOpt, null);
            } else if (tipe == '6') {
                selCityFarm.add(newOpt, null);
            } else if (tipe == '7') {
                selCityStockBreeding.add(newOpt, null);
            } else if (tipe == '8') {
                selCityOther.add(newOpt, null);
            } else if (tipe == '9') {
                selCitySubsidiary.add(newOpt, null);
            }
            
        }
    }

    if (tipe == '1') {
        if (_selCityHO != '' && _selCityHO != '-') { selCityHO.value = _selCityHO; _selCityHO = ''; }
    } else if (tipe == '2') {
        if (_selCityBranch != '' && _selCityBranch != '-') { selCityBranch.value = _selCityBranch; _selCityBranch = ''; }
    } else if (tipe == '3') {
        if (_selCityPlant != '' && _selCityPlant != '-') { selCityPlant.value = _selCityPlant; _selCityPlant = ''; }
    } else if (tipe == '4') {
        if (_selCityWH != '' && _selCityWH != '-') { selCityWH.value = _selCityWH; _selCityWH = ''; }
    } else if (tipe == '5') {
        if (_selCityWorkshop != '' && _selCityWorkshop != '-') { selCityWorkshop.value = _selCityWorkshop; _selCityWorkshop = ''; }
    } else if (tipe == '6') {
        if (_selCityFarm != '' && _selCityFarm != '-') { selCityFarm.value = _selCityFarm; _selCityFarm = ''; }
    } else if (tipe == '7') {
        if (_selCityStockBreeding != '' && _selCityStockBreeding != '-') { selCityStockBreeding.value = _selCityStockBreeding; _selCityStockBreeding = ''; }
    } else if (tipe == '8') {
        if (_selCityOther != '' && _selCityOther != '-') { selCityOther.value = _selCityOther; _selCityOther = ''; }
    } else if (tipe == '9') {
        if (_selCitySubsidiary != '' && _selCitySubsidiary != '-') { selCitySubsidiary.value = _selCitySubsidiary; _selCitySubsidiary = ''; }
    }
   
}


////////////////////////////////////////////////////////////////////

function bindSelCountryAll() {
    iCountry.style.display = '';
    selCountry.style.display = 'none';
    wcfAddr.JSON_getCountryListAll(bindSelCountryAllOnComplete, JSONOnError);
}

function bindSelCountryAllOnComplete(result) {

    iCountry.style.display = 'none';
    selCountry.style.display = '';
    result = eval(result);
    if (result.length == 0) return 0;
    selCountry.length = 0;

    for (var i = 0; i < result.length; i++) {
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

    bindSelProvinceAll(selCountry.value);
}


function bindSelProvinceAll(countryCode) {
    iProvince.style.display = '';
    selProvince.style.display = 'none';
    wcfAddr.JSON_getProvinceListAll(countryCode, bindSelProvinceAllOnComplete, JSONOnError);
}

function bindSelProvinceAllOnComplete(result) {
    iProvince.style.display = 'none';
    selProvince.style.display = '';
    result = eval(result);
    if (result.length == 0) return 0;

    selProvince.length = 0;

    for (var i = 0; i < result.length; i++) {
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

    bindSelCityAll(selCountry.value, selProvince.value);
}

function bindSelCityAll(countryCode, provinceCode) {
    iCity.style.display = '';
    selCity.display = 'none';
    wcfAddr.JSON_getCityListAll(countryCode, provinceCode, bindSelCityAllOnComplete, JSONOnError);
}

function bindSelCityAllOnComplete(result) {
    iCity.style.display = 'none';
    selCity.style.display = '';
    result = eval(result);
    if (result.length == 0) return 0;

    selCity.length = 0;

    for (var i = 0; i < result.length; i++) {
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