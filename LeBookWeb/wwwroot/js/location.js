//Json chua tinh,huyen,xa
const url = '/assets/json/location.json';
//Lay tinh
fetch(url)
    .then(
        function (response) {

            if (response.status !== 200) {
                console.warn('Có lỗi đã xảy ra. Mã lỗi: ' + response.status);
                return;
            }

            response.json().then(function (data) {
                let option;

                for (let i = 0; i < Object.keys(data).length; i++) {
                    if (data[i].name != province.value) {
                        option = document.createElement('option');
                        option.text = data[i].name;
                        option.value = data[i].name;
                        province.add(option);
                    }
                    
                }

            });
        }
    )
    .catch(function (err) {
        console.error('Xảy ra lỗi khi Fecth -', err);
    });

//Lay huyen
province.addEventListener("change", addDistrict);

function addDistrict() {
    district.length = 0;
    fetch(url)
        .then(
            function (response) {

                if (response.status !== 200) {
                    console.warn('Có lỗi đã xảy ra. Mã lỗi: ' + response.status);
                    return;
                }

                let selected = province.value;

                response.json().then(function (data) {
                    let option;

                    let districts = data.find(x => x.name === selected).districts;

                    for (let i = 0; i < Object.keys(districts).length; i++) {
                        option = document.createElement('option');
                        option.text = districts[i].name;
                        option.value = districts[i].name;
                        district.add(option);
                    }
                });
            }
        )
        .catch(function (err) {
            console.error('Xảy ra lỗi khi Fecth -', err);
        });
}

//Lay xa

district.addEventListener("change", addWard);

function addWard() {
    ward.length = 0;
    fetch(url)
        .then(
            function (response) {

                if (response.status !== 200) {
                    console.warn('Có lỗi đã xảy ra. Mã lỗi: ' + response.status);
                    return;
                }

                let selectedprovince = province.value;
                let selecteddistrict = district.value;

                response.json().then(function (data) {
                    let option;

                    let districts = data.find(x => x.name === selectedprovince).districts;
                    console.log(districts);
                    let wards = districts.find(x => x.name === selecteddistrict).wards;
                    console.log(wards);
                    for (let i = 0; i < Object.keys(wards).length; i++) {

                        option = document.createElement('option');
                        option.text = wards[i].name;
                        option.value = wards[i].name;
                        ward.add(option);
                    }
                });
            }
        )
        .catch(function (err) {
            console.error('Xảy ra lỗi khi Fecth -', err);
        });
}
