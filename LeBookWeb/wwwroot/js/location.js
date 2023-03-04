const url = '/assets/json/location.json';

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